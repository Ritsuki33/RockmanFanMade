using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class BossController : ExRbStateMachine<BossController>
{
    [SerializeField] Boss boss;
    [SerializeField] private Animator _animator;
    private Gravity _gravity;
    private Move _move;
    private Jump jump;
    private ExpandRigidBody _exRb;

    private AmbiguousTimer _timer=new AmbiguousTimer();
    private Action finishActionCallback;

    bool IsRight => boss.IsRight;

    enum StateId
    {
        Idle,
        Run,
        Jump,
        Appearance,
    }


    private void Awake()
    {
        _gravity = GetComponent<Gravity>();
        _exRb = GetComponent<ExpandRigidBody>();
        _move = GetComponent<Move>();
        jump = GetComponent<Jump>();

        AddState((int)StateId.Idle, new Idle());
        AddState((int)StateId.Run, new Run());
        AddState((int)StateId.Jump, new Jumping());
        //AddState((int)StateId.Float, new Float());
        AddState((int)StateId.Appearance, new Appearance());

        TransitReady((int)StateId.Appearance);
    }

    class Appearance : ExRbState<BossController>
    {
        enum SubStateId
        {
            Float,
            Pause,
            Wait,
        }

        public Appearance()
        {
            this.AddSubState((int)SubStateId.Float, new Float());
            this.AddSubState((int)SubStateId.Pause, new Pause());
            this.AddSubState((int)SubStateId.Wait, new Wait());
        }

        protected override void Enter(BossController ctr, int preId, int subId)
        {
            this.TransitSubReady((int)SubStateId.Float, preId);
        }

        protected override void FixedUpdate(BossController ctr, IParentState parent)
        {
            ctr._gravity.UpdateVelocity();
            ctr._exRb.velocity = ctr._gravity.CurrentVelocity;
        }

        protected override void OnBottomHitStay(BossController ctr, RaycastHit2D hit, IParentState parent)
        {
            ctr._gravity.Reset();
        }

        class Float : ExRbState<BossController>
        {
            protected override void Enter(BossController ctr, int preId, int subId)
            {
                ctr._animator.Play(AnimationNameHash.Float);
            }

            protected override void OnBottomHitEnter(BossController ctr, RaycastHit2D hit, IParentState parent)
            {
                parent.TransitSubReady((int)SubStateId.Pause);
            }
        }

        class Pause : ExRbState<BossController>
        {
            protected override void Enter(BossController ctr, int preId, int subId)
            {
                ctr._animator.Play(AnimationNameHash.Pause);
            }

            protected override void OnBottomHitEnter(BossController ctr, RaycastHit2D hit, IParentState parent)
            {
                parent.TransitSubReady((int)SubStateId.Pause);
            }

            protected override void Update(BossController ctr, IParentState parent)
            {
                if (!ctr._animator.IsPlayingCurrentAnimation(AnimationNameHash.Pause))
                {
                    parent.TransitSubReady((int)SubStateId.Wait);
                }
            }

            protected override void Exit(BossController ctr, int nextId)
            {
                // 通知を飛ばす
                ctr.finishActionCallback?.Invoke();
            }
        }

        class Wait : ExRbState<BossController> { }
    }

    class Idle : ExRbState<BossController>
    {
        protected override void Enter(BossController ctr, int preId, int subId)
        {
            ctr._animator.Play(AnimationNameHash.Idle);
            ctr._timer.Start(0.2f, 0.5f);
        }

        protected override void FixedUpdate(BossController ctr, IParentState parent)
        {

            ctr._gravity.UpdateVelocity();
            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
        }

        protected override void Update(BossController ctr, IParentState parent)
        {
            ctr._timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                    (50, () =>
                    {
                        ctr.TransitReady((int)StateId.Run);
                    }
                ),
                   (50, () =>
                   {
                       ctr.TransitReady((int)StateId.Jump);
                   }
                )
                );
            });
        }

        protected override void OnBottomHitStay(BossController ctr, RaycastHit2D hit, IParentState parent)
        {
            ctr._gravity.Reset();
        }
    }

    class Jumping : ExRbState<BossController>
    {
        float jump_vel =30f;
        float vel_x;
        int layerMask = LayerMask.GetMask("Ground");
        protected override void Enter(BossController ctr, int preId, int subId)
        {
            ctr.boss.TurnToTarget(GameManager.Instance.Player.transform.position);
            ctr._animator.Play(AnimationNameHash.Float);

            ctr.jump.Init(jump_vel);

            RaycastHit2D left = Physics2D.Raycast(ctr.transform.position, Vector2.left, Mathf.Infinity, layerMask);
            RaycastHit2D right = Physics2D.Raycast(ctr.transform.position, Vector2.right, Mathf.Infinity, layerMask);


            Probability.BranchMethods((50, () =>
            {
                vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, GameManager.Instance.Player.transform.position.x, jump_vel, ctr._gravity.GravityScale);
            }
            ),
            (50, () =>
            {
                float targetPosX = UnityEngine.Random.Range((left) ? left.point.x : ctr.transform.position.x, (right) ? right.point.x : ctr.transform.position.x);
                vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, targetPosX, jump_vel, ctr._gravity.GravityScale);
            }
            ));

        }

        protected override void FixedUpdate(BossController ctr, IParentState parent)
        {
            if (ctr.jump.CurrentSpeed > 0)
            {
                ctr.jump.UpdateVelocity(ctr._gravity.GravityScale);
                ctr._exRb.velocity += ctr.jump.CurrentVelocity;
            }
            else
            {
                ctr._gravity.UpdateVelocity();
                ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
            }

            ctr._exRb.velocity += new Vector2(vel_x, 0);
        }

        protected override void Update(BossController ctr, IParentState parent)
        {
            ctr._timer.MoveAheadTime(Time.deltaTime, () =>
            {
                ctr.TransitReady((int)StateId.Run);
            });
        }

        protected override void OnBottomHitStay(BossController ctr, RaycastHit2D hit, IParentState parent)
        {
            ctr.TransitReady((int)StateId.Idle);
        }
    }

    class Run : ExRbState<BossController>
    {
        public Run()
        {
            this.AddSubState(0, new Start());
            this.AddSubState(1, new Running());
        }

        protected override void Enter(BossController ctr, int preId, int subId)
        {
            this.TransitSubReady(0);
        }

        protected override void FixedUpdate(BossController ctr, IParentState parent)
        {
            ctr._gravity.UpdateVelocity();
            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
        }

        protected override void OnBottomHitStay(BossController ctr, RaycastHit2D hit, IParentState parent)
        {
            ctr._gravity.Reset();
        }

        class Start : ExRbState<BossController>
        {
            int animationHash = Animator.StringToHash("RunStart");
            protected override void Enter(BossController ctr, int preId, int subId)
            {
                ctr.boss.TurnToTarget(GameManager.Instance.Player.transform.position);
                ctr._animator.Play(animationHash);
            }

            protected override void Update(BossController ctr, IParentState parent)
            {
                if (!ctr._animator.IsPlayingCurrentAnimation(animationHash))
                {
                    parent.TransitSubReady(1);
                }
            }
        }


        class Running : ExRbState<BossController>
        {
            bool move_Right = false;
            Vector2 targetPos = default;
            Vector2 prePos= default;
            protected override void Enter(BossController ctr, int preId, int subId)
            {
                ctr._animator.Play(AnimationNameHash.Run);
                targetPos = GameManager.Instance.Player.transform.position;
                prePos = ctr.transform.position;
            }

            protected override void FixedUpdate(BossController ctr, IParentState parent)
            {

                if (MoveAI.IsPassedParam(prePos.x, ctr.transform.position.x, targetPos.x))
                {
                    ctr.TransitReady((int)StateId.Idle);
                    prePos = ctr.transform.position;
                }

                ctr._move.UpdateVelocity(Vector2.right, (ctr.IsRight) ? Move.InputType.Right : Move.InputType.Left);
                ctr._exRb.velocity += ctr._move.CurrentVelocity;

            }
            protected override void Update(BossController ctr, IParentState parent)
            {
                if (!ctr._animator.IsPlayingCurrentAnimation(AnimationNameHash.Run))
                {
                    parent.TransitSubReady(1);
                }
            }

            protected override void OnLeftHitStay(BossController ctr, RaycastHit2D hit, IParentState parent)
            {
                ctr.TransitReady((int)StateId.Idle);
            }

            protected override void OnRightHitStay(BossController ctr, RaycastHit2D hit, IParentState parent)
            {
                ctr.TransitReady((int)StateId.Idle);
            }
        }
    }

    public void Appeare(Action finishCallback)
    {
        gameObject.SetActive(true);
        finishActionCallback = finishCallback;
        this.TransitReady((int)StateId.Appearance);
    }

    public void ToBattleState()
    {
        this.TransitReady((int)StateId.Idle);
    }
}
