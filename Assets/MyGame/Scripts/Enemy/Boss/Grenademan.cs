using System;
using UnityEngine;

public class Grenademan : StageBoss, IDirect, IHitEvent, IRbVisitor, IExRbVisitor
{
    public Action<float> hpParamIncrementAnimation = default;

    [SerializeField] Transform buster;
    [SerializeField] private Gravity _gravity;
    [SerializeField] private Move _move;
    [SerializeField] private Jump _jump;
    [SerializeField] private Direct _direct;
    [SerializeField] private ExpandRigidBody _exRb;

    private AmbiguousTimer _timer = new AmbiguousTimer();
    private Action _finishActionCallback;

    bool existBomb = false;

    [SerializeField] Transform[] _placeBombPosArray = null;

    ExRbStateMachine<Grenademan> stateMachine = new ExRbStateMachine<Grenademan>();

    CachedCollide rbCollide = new CachedCollide();
    CachedHit exRbHit = new CachedHit();
    enum StateId
    {
        Idle,
        Run,
        Jump,
        PlaceBomb,
        Shoot,
        Appearance,
    }

    protected override void Awake()
    {
        base.Awake();
        _exRb.Init(this);

        stateMachine.AddState((int)StateId.Idle, new Idle());
        stateMachine.AddState((int)StateId.Run, new Run());
        stateMachine.AddState((int)StateId.Jump, new Jumping());
        stateMachine.AddState((int)StateId.PlaceBomb, new PlaceBomb());
        stateMachine.AddState((int)StateId.Shoot, new Shoot());
        stateMachine.AddState((int)StateId.Appearance, new Appearance());
        rbCollide.CacheClear();
        exRbHit.CacheClear();
    }

    protected override void Init()
    {
        base.Init();
        stateMachine.TransitReady((int)StateId.Appearance);
    }

    protected override void OnReset()
    {
        Delete();
    }

    protected override void OnFixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    protected override void OnLateFixedUpdate()
    {
        _exRb.FixedUpdate();
    }

    protected override void OnUpdate()
    {
        stateMachine.Update(this);
    }

    public void Setup(Transform[] placeBombPosArray)
    {
        _placeBombPosArray = placeBombPosArray;
    }

    public override void Appeare(Action finishCallback)
    {
        _finishActionCallback = finishCallback;
        this.stateMachine.TransitReady((int)StateId.Appearance);
    }

    public override void ToBattleState()
    {
        this.stateMachine.TransitReady((int)StateId.Idle);
    }

    void IHitEvent.OnBottomHitEnter(RaycastHit2D hit)
    {
        stateMachine.OnBottomHitEnter(this, hit);
    }

    void IHitEvent.OnBottomHitStay(RaycastHit2D hit)
    {
        stateMachine.OnBottomHitStay(this, hit);
    }

    void IHitEvent.OnLeftHitStay(RaycastHit2D hit)
    {
        stateMachine.OnLeftHitStay(this, hit);
    }

    void IHitEvent.OnRightHitStay(RaycastHit2D hit)
    {
        stateMachine.OnRightHitStay(this, hit);
    }

    void IRbVisitor.OnTriggerEnter(RockBuster collision)
    {
        stateMachine.OnTriggerEnter(this, collision);
    }

    void IRbVisitor.OnTriggerEnter(PlayerAttack collision)
    {
        stateMachine.OnTriggerEnter(this, collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateMachine.OnTriggerEnter(this, collision);
        rbCollide.OnTriggerEnter(this, collision);
    }

    public override void OnDead()
    {
        var deathEffect = ObjectManager.Instance.OnGet<PsObject>(PoolType.PlayerDeathEffect);
        deathEffect.Setup(this.transform.position);

        Delete();
        AudioManager.Instance.PlaySe(SECueIDs.thiun);
    }

    class Appearance : ExRbState<Grenademan, Appearance>
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

        protected override void Enter(Grenademan ctr, int preId, int subId)
        {
            ctr.existBomb = false;
            ctr._exRb.BoxCollider.enabled = false;
            this.TransitSubReady((int)SubStateId.Float);
        }

        protected override void FixedUpdate(Grenademan ctr)
        {
            ctr._gravity.OnUpdate();
            ctr._exRb.velocity = ctr._gravity.CurrentVelocity;
        }

        protected override void OnBottomHitStay(Grenademan ctr, RaycastHit2D hit)
        {
            ctr._gravity.Reset();
        }

        class Float : ExRbSubState<Grenademan, Float, Appearance>
        {
            protected override void Enter(Grenademan ctr, Appearance parent, int preId, int subId)
            {
                ctr.MainAnimator.Play(AnimationNameHash.Float);
            }

            protected override void OnBottomHitEnter(Grenademan ctr, Appearance parent, RaycastHit2D hit)
            {
                parent.TransitSubReady((int)SubStateId.Pause);
            }

            protected override void OnTriggerEnter(Grenademan ctr, Appearance parent, Collider2D collision)
            {
                ctr._exRb.BoxCollider.enabled = true;
            }
        }

        class Pause : ExRbSubState<Grenademan, Pause, Appearance>
        {
            protected override void Enter(Grenademan ctr, Appearance parent, int preId, int subId)
            {
                ctr.MainAnimator.Play(AnimationNameHash.Pause);
            }

            protected override void Update(Grenademan ctr, Appearance parent)
            {
                if (!ctr.MainAnimator.IsPlayingCurrentAnimation(AnimationNameHash.Pause))
                {
                    parent.TransitSubReady((int)SubStateId.Wait);
                }
            }

            protected override void Exit(Grenademan ctr, Appearance parent, int nextId)
            {
                // 通知を飛ばす
                ctr._finishActionCallback?.Invoke();
            }
        }

        class Wait : ExRbSubState<Grenademan, Wait, Appearance> { }
    }

    class Idle : ExRbState<Grenademan, Idle>
    {
        protected override void Enter(Grenademan ctr, int preId, int subId)
        {
            ctr.MainAnimator.Play(AnimationNameHash.Idle);
            ctr._timer.Start(0.2f, 0.5f);
        }

        protected override void FixedUpdate(Grenademan ctr)
        {

            ctr._gravity.OnUpdate();
            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
        }

        protected override void Update(Grenademan ctr)
        {
            ctr._timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                    (20, () =>
                    {
                        ctr.stateMachine.TransitReady((int)StateId.Run);
                    }
                ),
                   (30, () =>
                   {
                       ctr.stateMachine.TransitReady((int)StateId.Jump);
                   }
                ),
                   ((!ctr.existBomb) ? 40 : 0, () =>
                   {
                       ctr.stateMachine.TransitReady((int)StateId.PlaceBomb);
                   }
                ),
                   (25, () =>
                   {
                       ctr.stateMachine.TransitReady((int)StateId.Shoot);
                   }
                )
                );
            }, true);
        }

        protected override void OnBottomHitStay(Grenademan ctr, RaycastHit2D hit)
        {
            ctr._gravity.Reset();
        }

        protected override void OnTriggerEnter(Grenademan ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(Grenademan ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }

    class Jumping : ExRbState<Grenademan, Jumping>
    {
        float jump_vel = 30f;
        float vel_x;
        int layerMask = LayerMask.GetMask("Ground");
        protected override void Enter(Grenademan ctr, int preId, int subId)
        {
            ctr.TurnToTarget(WorldManager.Instance.Player.transform.position);
            ctr.MainAnimator.Play(AnimationNameHash.Float);

            ctr._jump.Init(jump_vel);

            RaycastHit2D left = Physics2D.Raycast(ctr.transform.position, Vector2.left, Mathf.Infinity, layerMask);
            RaycastHit2D right = Physics2D.Raycast(ctr.transform.position, Vector2.right, Mathf.Infinity, layerMask);


            Probability.BranchMethods((50, () =>
            {
                vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, WorldManager.Instance.Player.transform.position.x, jump_vel, ctr._gravity.GravityScale);
            }
            ),
            (50, () =>
            {
                float targetPosX = UnityEngine.Random.Range((left) ? left.point.x : ctr.transform.position.x, (right) ? right.point.x : ctr.transform.position.x);
                vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, targetPosX, jump_vel, ctr._gravity.GravityScale);
            }
            ));

        }

        protected override void FixedUpdate(Grenademan ctr)
        {
            if (ctr._jump.CurrentSpeed > 0)
            {
                ctr._jump.OnUpdate(ctr._gravity.GravityScale);
                ctr._exRb.velocity += ctr._jump.CurrentVelocity;
            }
            else
            {
                ctr._gravity.OnUpdate();
                ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
            }

            ctr._exRb.velocity += new Vector2(vel_x, 0);
        }

        protected override void OnBottomHitStay(Grenademan ctr, RaycastHit2D hit)
        {
            ctr.stateMachine.TransitReady((int)StateId.Idle);
        }

        protected override void OnTriggerEnter(Grenademan ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(Grenademan ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }

    class PlaceBomb : ExRbState<Grenademan, PlaceBomb>
    {
        float jump_vel = 30f;
        float vel_x;
        int layerMask = LayerMask.GetMask("Ground");
        int animationHash = Animator.StringToHash("PlaceBomb");
        bool isFire = false;

        ObjectManager ObjectManager => ObjectManager.Instance;
        protected override void Enter(Grenademan ctr, int preId, int subId)
        {
            ctr.TurnToTarget(WorldManager.Instance.Player.transform.position);
            ctr.MainAnimator.Play(animationHash);

            ctr._jump.Init(jump_vel);

            RaycastHit2D left = Physics2D.Raycast(ctr.transform.position, Vector2.left, Mathf.Infinity, layerMask);
            RaycastHit2D right = Physics2D.Raycast(ctr.transform.position, Vector2.right, Mathf.Infinity, layerMask);


            vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, WorldManager.Instance.Player.transform.position.x, jump_vel, ctr._gravity.GravityScale);
            isFire = false;
        }

        protected override void FixedUpdate(Grenademan ctr)
        {
            if (ctr._jump.CurrentSpeed > 0)
            {
                ctr._jump.OnUpdate(ctr._gravity.GravityScale);
                ctr._exRb.velocity += ctr._jump.CurrentVelocity;
            }
            else
            {
                ctr._gravity.OnUpdate();
                ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
            }

            ctr._exRb.velocity += new Vector2(vel_x, 0);
        }

        protected override void Update(Grenademan ctr)
        {
            if (!isFire && !ctr.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                isFire = true;
                ctr.existBomb = true;

                foreach (var t in ctr._placeBombPosArray)
                {
                    Vector2 dir = t.position - ctr.transform.position;
                    dir = dir.normalized;
                    var bomb = ObjectManager.Instance.OnGet<PlacedBomb>(PoolType.PlacedBomb, (bomb) =>
                    {
                        ctr.existBomb = false;
                    });

                    bomb.Setup(
                        ctr.transform.position,
                        (exRb) =>
                        {
                            exRb.velocity += dir * 20;
                        }
                        );
                }

                AudioManager.Instance.PlaySe(SECueIDs.tstrike);

            }
        }

        protected override void OnBottomHitStay(Grenademan ctr, RaycastHit2D hit)
        {
            ctr.stateMachine.TransitReady((int)StateId.Idle);
        }

        protected override void OnTriggerEnter(Grenademan ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(Grenademan ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }

    class Run : ExRbState<Grenademan, Run>
    {
        public Run()
        {
            this.AddSubState(0, new Start());
            this.AddSubState(1, new Running());
        }

        protected override void Enter(Grenademan ctr, int preId, int subId)
        {
            this.TransitSubReady(0);
        }

        protected override void FixedUpdate(Grenademan ctr)
        {
            ctr._gravity.OnUpdate();
            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
        }

        protected override void OnBottomHitStay(Grenademan ctr, RaycastHit2D hit)
        {
            ctr._gravity.Reset();
        }

        class Start : ExRbSubState<Grenademan, Start, Run>
        {
            int animationHash = Animator.StringToHash("RunStart");

            protected override void Enter(Grenademan ctr, Run parent, int preId, int subId)
            {
                ctr.TurnToTarget(WorldManager.Instance.Player.transform.position);
                ctr.MainAnimator.Play(animationHash);
            }

            protected override void Update(Grenademan ctr, Run parent)
            {
                if (!ctr.MainAnimator.IsPlayingCurrentAnimation(animationHash))
                {
                    parent.TransitSubReady(1);
                }
            }
        }


        class Running : ExRbSubState<Grenademan, Running, Run>
        {
            Vector2 targetPos = default;
            Vector2 prePos = default;

            protected override void Enter(Grenademan ctr, Run parent, int preId, int subId)
            {
                ctr.MainAnimator.Play(AnimationNameHash.Run);
                targetPos = WorldManager.Instance.Player.transform.position;
                prePos = ctr.transform.position;

                AudioManager.Instance.PlaySe(SECueIDs.block);
            }
            protected override void FixedUpdate(Grenademan ctr, Run parent)
            {
                if (MoveAI.IsPassedParam(prePos.x, ctr.transform.position.x, targetPos.x))
                {
                    ctr.stateMachine.TransitReady((int)StateId.Idle);
                    prePos = ctr.transform.position;
                }

                ctr._move.OnUpdate(Vector2.right, (ctr.IsRight) ? Move.InputType.Right : Move.InputType.Left);
                ctr._exRb.velocity += ctr._move.CurrentVelocity;
            }

            protected override void Update(Grenademan ctr, Run parent)
            {
                if (!ctr.MainAnimator.IsPlayingCurrentAnimation(AnimationNameHash.Run))
                {
                    parent.TransitSubReady(1);
                }
            }

            protected override void OnLeftHitStay(Grenademan ctr, Run parent, RaycastHit2D hit)
            {
                ctr.stateMachine.TransitReady((int)StateId.Idle);
            }
            protected override void OnRightHitStay(Grenademan ctr, Run parent, RaycastHit2D hit)
            {
                ctr.stateMachine.TransitReady((int)StateId.Idle);
            }
        }

        protected override void OnTriggerEnter(Grenademan ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(Grenademan ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }

    class Shoot : ExRbState<Grenademan, Shoot>
    {
        public Shoot()
        {
            this.AddSubState(0, new Hold());
            this.AddSubState(1, new Fire());
            this.AddSubState(2, new Stiffness());

        }

        protected override void Enter(Grenademan ctr, int preId, int subId)
        {
            this.TransitSubReady(0);
            ctr.TurnToTarget(WorldManager.Instance.Player.transform.position);
            ctr.MainAnimator.Play(AnimationNameHash.Shoot);
        }


        class Hold : ExRbSubState<Grenademan, Hold, Shoot>
        {
            protected override void Enter(Grenademan ctr, Shoot parent, int preId, int subId)
            {
                if (preId != 2) ctr._timer.Start(0.4f, 0.6f);
                else ctr._timer.Start(0, 0);
            }

            protected override void Update(Grenademan ctr, Shoot parent)
            {
                ctr._timer.MoveAheadTime(Time.deltaTime, () =>
                {
                    parent.TransitSubReady(1);
                });
            }
        }

        class Fire : ExRbSubState<Grenademan, Fire, Shoot>
        {
            ObjectManager ObjectManager => ObjectManager.Instance;

            protected override void Enter(Grenademan ctr, Shoot parent, int preId, int subId)
            {
                Vector2 dir = (ctr.IsRight) ? Vector2.right : Vector2.left;
                dir = dir.normalized;

                var projectile = ObjectManager.Instance.OnGet<SimpleProjectileComponent>(PoolType.CrashBomb);
                projectile.Setup(
                    new Vector3(ctr.buster.transform.position.x, ctr.buster.transform.position.y, -2),
                    ctr.IsRight,
                    5,
                    null,
                    (rb) =>
                    {
                        rb.velocity = dir * 8f;
                    },
                    (bomb) =>
                    {
                        bomb.Delete();
                        var explode = ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
                        explode.Setup(Explode.Layer.EnemyAttack, bomb.transform.position, 3);
                    }
                    );
            }

            protected override void Update(Grenademan octrbj, Shoot parent)
            {
                parent.TransitSubReady(2);
            }
        }

        class Stiffness : ExRbSubState<Grenademan, Stiffness, Shoot>
        {
            protected override void Enter(Grenademan ctr, Shoot parent, int preId, int subId)
            {
                ctr._timer.Start(0.5f, 0.5f);
            }

            protected override void Update(Grenademan ctr, Shoot parent)
            {
                ctr._timer.MoveAheadTime(Time.deltaTime, () =>
                {
                    Probability.BranchMethods(
                        (50, () =>
                        {
                            parent.TransitSubReady(0);
                        }
                    ),
                        (50, () =>
                        {
                            ctr.stateMachine.TransitReady((int)StateId.Idle);
                        }
                    )
                    );
                });
            }
        }

        protected override void OnTriggerEnter(Grenademan ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(Grenademan ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }



    private void OnDrawGizmos()
    {
        _exRb.OnDrawGizmos();
    }
    public bool IsRight => _direct.IsRight;
    public void TurnTo(bool isRight) => _direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => _direct.TurnToTarget(targetPos);
    public void TurnFace() => _direct.TurnFace();
}
