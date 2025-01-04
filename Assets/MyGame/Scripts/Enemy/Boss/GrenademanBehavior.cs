using System;
using UnityEngine;

public class GrenademanBehavior : MonoBehaviour
{
    [SerializeField] Grenademan boss;
    [SerializeField] Transform buster;
    [SerializeField] private Animator _animator;
    private Gravity _gravity;
    private Move _move;
    private Jump jump;
    private ExpandRigidBody _exRb;

    private AmbiguousTimer _timer=new AmbiguousTimer();
    private Action finishActionCallback;

    bool IsRight => boss.IsRight;

    bool existBomb = false;
    public int CurrentHp => boss.CurrentHp;
    public int MaxHp => boss.MaxHp;

    [SerializeField] Transform[] placeBombPosArray = null;
    BaseObjectPool ExplodePool=>EffectManager.Instance.ExplodePool;

    public Action<float> HpChangeTrigger { get {return  boss.hpChangeTrigger; } set { boss.hpChangeTrigger = value; } }

    enum StateId
    {
        Idle,
        Run,
        Jump,
        PlaceBomb,
        Shoot,
        Appearance,
    }


    //private void Awake()
    //{
    //    _gravity = GetComponent<Gravity>();
    //    _exRb = GetComponent<ExpandRigidBody>();
    //    _move = GetComponent<Move>();
    //    jump = GetComponent<Jump>();

    //    AddState((int)StateId.Idle, new Idle());
    //    AddState((int)StateId.Run, new Run());
    //    AddState((int)StateId.Jump, new Jumping());
    //    AddState((int)StateId.PlaceBomb, new PlaceBomb());
    //    AddState((int)StateId.Shoot, new Shoot());
    //    //AddState((int)StateId.Float, new Float());
    //    AddState((int)StateId.Appearance, new Appearance());

    //    TransitReady((int)StateId.Appearance);
    //}

    //class Appearance : ExRbState<GrenademanBehavior, Appearance>
    //{
    //    enum SubStateId
    //    {
    //        Float,
    //        Pause,
    //        Wait,
    //    }

    //    public Appearance()
    //    {
    //        this.AddSubState((int)SubStateId.Float, new Float());
    //        this.AddSubState((int)SubStateId.Pause, new Pause());
    //        this.AddSubState((int)SubStateId.Wait, new Wait());
    //    }

    //    protected override void Enter(GrenademanBehavior ctr, int preId, int subId)
    //    {
    //        ctr.existBomb = false;
    //        ctr._exRb.BoxCollider.enabled = false;
    //        this.TransitSubReady((int)SubStateId.Float);
    //    }

    //    protected override void FixedUpdate(GrenademanBehavior ctr)
    //    {
    //        ctr._gravity.UpdateVelocity();
    //        ctr._exRb.velocity = ctr._gravity.CurrentVelocity;
    //    }

    //    protected override void OnBottomHitStay(GrenademanBehavior ctr, RaycastHit2D hit)
    //    {
    //        ctr._gravity.Reset();
    //    }

    //    class Float : ExRbSubState<GrenademanBehavior, Float, Appearance>
    //    {
    //        protected override void Enter(GrenademanBehavior ctr, Appearance parent, int preId, int subId)
    //        {
    //            ctr._animator.Play(AnimationNameHash.Float);
    //        }

    //        protected override void OnBottomHitEnter(GrenademanBehavior ctr, Appearance parent, RaycastHit2D hit)
    //        {
    //            parent.TransitSubReady((int)SubStateId.Pause);
    //        }

    //        protected override void OnTriggerEnter(GrenademanBehavior ctr, Appearance parent, Collider2D collision)
    //        {
    //            ctr._exRb.BoxCollider.enabled = true;
    //        }
    //    }

    //    class Pause : ExRbSubState<GrenademanBehavior, Pause, Appearance>
    //    {
    //        protected override void Enter(GrenademanBehavior ctr, Appearance parent, int preId, int subId)
    //        {
    //            ctr._animator.Play(AnimationNameHash.Pause);
    //        }

    //        protected override void Update(GrenademanBehavior ctr, Appearance parent)
    //        {
    //            if (!ctr._animator.IsPlayingCurrentAnimation(AnimationNameHash.Pause))
    //            {
    //                parent.TransitSubReady((int)SubStateId.Wait);
    //            }
    //        }

    //        protected override void Exit(GrenademanBehavior ctr, Appearance parent, int nextId)
    //        {
    //            // 通知を飛ばす
    //            ctr.finishActionCallback?.Invoke();
    //        }
    //    }

    //    class Wait : ExRbSubState<GrenademanBehavior, Wait, Appearance> { }
    //}

    //class Idle : ExRbState<GrenademanBehavior, Idle>
    //{
    //    protected override void Enter(GrenademanBehavior ctr, int preId, int subId)
    //    {
    //        ctr._animator.Play(AnimationNameHash.Idle);
    //        ctr._timer.Start(0.2f, 0.5f);
    //    }

    //    protected override void FixedUpdate(GrenademanBehavior ctr)
    //    {

    //        ctr._gravity.UpdateVelocity();
    //        ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
    //    }

    //    protected override void Update(GrenademanBehavior ctr)
    //    {
    //        ctr._timer.MoveAheadTime(Time.deltaTime, () =>
    //        {
    //            Probability.BranchMethods(
    //                (20, () =>
    //                {
    //                    ctr.TransitReady((int)StateId.Run);
    //                }
    //            ),
    //               (30, () =>
    //               {
    //                   ctr.TransitReady((int)StateId.Jump);
    //               }
    //            ),
    //               ((!ctr.existBomb) ? 40 : 0, () =>
    //               {
    //                   ctr.TransitReady((int)StateId.PlaceBomb);
    //               }
    //            ),
    //               (25, () =>
    //               {
    //                   ctr.TransitReady((int)StateId.Shoot);
    //               }
    //            )
    //            );
    //        }, true);
    //    }

    //    protected override void OnBottomHitStay(GrenademanBehavior ctr, RaycastHit2D hit)
    //    {
    //        ctr._gravity.Reset();
    //    }

    //    protected override void OnTriggerEnter(GrenademanBehavior ctr, RockBusterDamage collision)
    //    {
    //        ctr.boss.Damaged(collision);
    //    }
    //}

    //class Jumping : ExRbState<GrenademanBehavior, Jumping>
    //{
    //    float jump_vel =30f;
    //    float vel_x;
    //    int layerMask = LayerMask.GetMask("Ground");
    //    protected override void Enter(GrenademanBehavior ctr, int preId, int subId)
    //    {
    //        ctr.boss.TurnToTarget(WorldManager.Instance.PlayerController.transform.position);
    //        ctr._animator.Play(AnimationNameHash.Float);

    //        ctr.jump.Init(jump_vel);

    //        RaycastHit2D left = Physics2D.Raycast(ctr.transform.position, Vector2.left, Mathf.Infinity, layerMask);
    //        RaycastHit2D right = Physics2D.Raycast(ctr.transform.position, Vector2.right, Mathf.Infinity, layerMask);


    //        Probability.BranchMethods((50, () =>
    //        {
    //            vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, WorldManager.Instance.PlayerController.transform.position.x, jump_vel, ctr._gravity.GravityScale);
    //        }
    //        ),
    //        (50, () =>
    //        {
    //            float targetPosX = UnityEngine.Random.Range((left) ? left.point.x : ctr.transform.position.x, (right) ? right.point.x : ctr.transform.position.x);
    //            vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, targetPosX, jump_vel, ctr._gravity.GravityScale);
    //        }
    //        ));

    //    }

    //    protected override void FixedUpdate(GrenademanBehavior ctr)
    //    {
    //        if (ctr.jump.CurrentSpeed > 0)
    //        {
    //            ctr.jump.UpdateVelocity(ctr._gravity.GravityScale);
    //            ctr._exRb.velocity += ctr.jump.CurrentVelocity;
    //        }
    //        else
    //        {
    //            ctr._gravity.UpdateVelocity();
    //            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
    //        }

    //        ctr._exRb.velocity += new Vector2(vel_x, 0);
    //    }

    //    protected override void OnBottomHitStay(GrenademanBehavior ctr, RaycastHit2D hit)
    //    {
    //        ctr.TransitReady((int)StateId.Idle);
    //    }

    //    protected override void OnTriggerEnter(GrenademanBehavior ctr, RockBusterDamage collision)
    //    {
    //        ctr.boss.Damaged(collision);
    //    }
    //}

    //class PlaceBomb : ExRbState<GrenademanBehavior, PlaceBomb>
    //{
    //    float jump_vel = 30f;
    //    float vel_x;
    //    int layerMask = LayerMask.GetMask("Ground");
    //    int animationHash = Animator.StringToHash("PlaceBomb");
    //    bool isFire = false;

    //    BaseObjectPool PlacedBombPool => EffectManager.Instance.PlacedBombPool;
    //    protected override void Enter(GrenademanBehavior ctr, int preId, int subId)
    //    {
    //        ctr.boss.TurnToTarget(WorldManager.Instance.PlayerController.transform.position);
    //        ctr._animator.Play(animationHash);

    //        ctr.jump.Init(jump_vel);

    //        RaycastHit2D left = Physics2D.Raycast(ctr.transform.position, Vector2.left, Mathf.Infinity, layerMask);
    //        RaycastHit2D right = Physics2D.Raycast(ctr.transform.position, Vector2.right, Mathf.Infinity, layerMask);


    //        vel_x = ParabolaCalc.GetHorizonVelocity(ctr.transform.position.x, WorldManager.Instance.PlayerController.transform.position.x, jump_vel, ctr._gravity.GravityScale);
    //        isFire = false;
    //    }

    //    protected override void FixedUpdate(GrenademanBehavior ctr)
    //    {
    //        if (ctr.jump.CurrentSpeed > 0)
    //        {
    //            ctr.jump.UpdateVelocity(ctr._gravity.GravityScale);
    //            ctr._exRb.velocity += ctr.jump.CurrentVelocity;
    //        }
    //        else
    //        {
    //            ctr._gravity.UpdateVelocity();
    //            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
    //        }

    //        ctr._exRb.velocity += new Vector2(vel_x, 0);
    //    }

    //    protected override void Update(GrenademanBehavior ctr)
    //    {
    //        if (!isFire&&!ctr._animator.IsPlayingCurrentAnimation(animationHash))
    //        {
    //            isFire = true;
    //            ctr.existBomb = true;

    //            foreach (var t in ctr.placeBombPosArray)
    //            {
    //                var bomb = PlacedBombPool.Pool.Get().GetComponent<PlacedBombBehavior>();
    //                bomb.transform.position = new Vector3(ctr.transform.position.x, ctr.transform.position.y, -2);
    //                Vector2 dir = t.position - ctr.transform.position;
    //                dir = dir.normalized;
    //                bomb.Init(
    //                    (exRb) =>
    //                    {
    //                        exRb.velocity += dir * 20;
    //                    },
    //                    ()=>{ 
    //                        ctr.existBomb = false;
    //                    }
    //                    );

    //            }
    //        }
    //    }

    //    protected override void OnBottomHitStay(GrenademanBehavior ctr, RaycastHit2D hit)
    //    {
    //        ctr.TransitReady((int)StateId.Idle);
    //    }

    //    protected override void OnTriggerEnter(GrenademanBehavior ctr, RockBusterDamage collision)
    //    {
    //        ctr.boss.Damaged(collision);
    //    }
    //}

    //class Run : ExRbState<GrenademanBehavior, Run>
    //{
    //    public Run()
    //    {
    //        this.AddSubState(0, new Start());
    //        this.AddSubState(1, new Running());
    //    }

    //    protected override void Enter(GrenademanBehavior ctr, int preId, int subId)
    //    {
    //        this.TransitSubReady(0);
    //    }

    //    protected override void FixedUpdate(GrenademanBehavior ctr)
    //    {
    //        ctr._gravity.UpdateVelocity();
    //        ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
    //    }

    //    protected override void OnBottomHitStay(GrenademanBehavior ctr, RaycastHit2D hit)
    //    {
    //        ctr._gravity.Reset();
    //    }

    //    class Start : ExRbSubState<GrenademanBehavior, Start, Run>
    //    {
    //        int animationHash = Animator.StringToHash("RunStart");

    //        protected override void Enter(GrenademanBehavior ctr, Run parent, int preId, int subId)
    //        {
    //            ctr.boss.TurnToTarget(WorldManager.Instance.PlayerController.transform.position);
    //            ctr._animator.Play(animationHash);
    //        }

    //        protected override void Update(GrenademanBehavior ctr, Run parent)
    //        {
    //            if (!ctr._animator.IsPlayingCurrentAnimation(animationHash))
    //            {
    //                parent.TransitSubReady(1);
    //            }
    //        }
    //    }


    //    class Running : ExRbSubState<GrenademanBehavior, Running, Run>
    //    {
    //        Vector2 targetPos = default;
    //        Vector2 prePos= default;

    //        protected override void Enter(GrenademanBehavior ctr, Run parent, int preId, int subId)
    //        {
    //            ctr._animator.Play(AnimationNameHash.Run);
    //            targetPos = WorldManager.Instance.PlayerController.transform.position;
    //            prePos = ctr.transform.position;
    //        }
    //        protected override void FixedUpdate(GrenademanBehavior ctr, Run parent)
    //        {
    //            if (MoveAI.IsPassedParam(prePos.x, ctr.transform.position.x, targetPos.x))
    //            {
    //                ctr.TransitReady((int)StateId.Idle);
    //                prePos = ctr.transform.position;
    //            }

    //            ctr._move.UpdateVelocity(Vector2.right, (ctr.IsRight) ? Move.InputType.Right : Move.InputType.Left);
    //            ctr._exRb.velocity += ctr._move.CurrentVelocity;
    //        }

    //        protected override void Update(GrenademanBehavior ctr, Run parent)
    //        {
    //            if (!ctr._animator.IsPlayingCurrentAnimation(AnimationNameHash.Run))
    //            {
    //                parent.TransitSubReady(1);
    //            }
    //        }

    //        protected override void OnLeftHitStay(GrenademanBehavior ctr, Run parent, RaycastHit2D hit)
    //        {
    //            ctr.TransitReady((int)StateId.Idle);
    //        }
    //        protected override void OnRightHitStay(GrenademanBehavior ctr, Run parent, RaycastHit2D hit)
    //        {
    //            ctr.TransitReady((int)StateId.Idle);
    //        }
    //    }

    //    protected override void OnTriggerEnter(GrenademanBehavior ctr, RockBusterDamage collision)
    //    {
    //        ctr.boss.Damaged(collision);
    //    }
    //}

    //class Shoot : ExRbState<GrenademanBehavior, Shoot>
    //{
    //    public Shoot() {
    //        this.AddSubState(0, new Hold());
    //        this.AddSubState(1, new Fire());
    //        this.AddSubState(2, new Stiffness());

    //    }

    //    protected override void Enter(GrenademanBehavior ctr, int preId, int subId)
    //    {
    //        this.TransitSubReady(0);
    //        ctr.boss.TurnToTarget(WorldManager.Instance.PlayerController.transform.position);
    //        ctr._animator.Play(AnimationNameHash.Shoot);
    //    }


    //    class Hold: ExRbSubState<GrenademanBehavior, Hold, Shoot>
    //    {
    //        protected override void Enter(GrenademanBehavior ctr, Shoot parent, int preId, int subId)
    //        {
    //            if (preId != 2) ctr._timer.Start(0.4f, 0.6f);
    //            else ctr._timer.Start(0, 0);
    //        }

    //        protected override void Update(GrenademanBehavior ctr, Shoot parent)
    //        {
    //            ctr._timer.MoveAheadTime(Time.deltaTime, () =>
    //            {
    //                parent.TransitSubReady(1);
    //            });
    //        }
    //    }

    //    class Fire: ExRbSubState<GrenademanBehavior, Fire, Shoot>
    //    {
    //        protected override void Enter(GrenademanBehavior ctr, Shoot parent, int preId, int subId)
    //        {
    //            var bomb = EffectManager.Instance.CrashBombPool.Pool.Get().GetComponent<ProjectileReusable>();
    //            bomb.transform.position = new Vector3(ctr.buster.transform.position.x, ctr.buster.transform.position.y, -2);
    //            Vector2 dir = (ctr.IsRight) ? Vector2.right : Vector2.left;
    //            dir = dir.normalized;
    //            bomb.Init(5, null,
    //                (exRb) =>
    //                {
    //                    exRb.velocity = dir * 8f;
    //                },
    //                (projectile) =>
    //                {
    //                    bomb.Delete();
    //                    var explode = ctr.ExplodePool.Pool.Get() as ExplodeController;
    //                    explode.Init(ExplodeController.Layer.EnemyAttack, 3);
    //                    explode.transform.position = bomb.transform.position;
    //                }
    //               );
    //        }

    //        protected override void Update(GrenademanBehavior octrbj, Shoot parent)
    //        {
    //            parent.TransitSubReady(2);
    //        }
    //    }

    //    class Stiffness : ExRbSubState<GrenademanBehavior, Stiffness, Shoot>
    //    {
    //        protected override void Enter(GrenademanBehavior ctr, Shoot parent, int preId, int subId)
    //        {
    //            ctr._timer.Start(0.5f, 0.5f);
    //        }

    //        protected override void Update(GrenademanBehavior ctr, Shoot parent)
    //        {
    //            ctr._timer.MoveAheadTime(Time.deltaTime, () =>
    //            {
    //                Probability.BranchMethods(
    //                    (50, () =>
    //                    {
    //                        parent.TransitSubReady(0);
    //                    }
    //                ),
    //                    (50, () =>
    //                    {
    //                        ctr.TransitReady((int)StateId.Idle);
    //                    }
    //                )
    //                );
    //            });
    //        }
    //    }

    //    protected override void OnTriggerEnter(GrenademanBehavior ctr, RockBusterDamage collision)
    //    {
    //        ctr.boss.Damaged(collision);
    //    }
    //}

    //public void Appeare(Action finishCallback)
    //{
    //    boss.Init();
    //    gameObject.SetActive(true);
    //    finishActionCallback = finishCallback;
    //    this.TransitReady((int)StateId.Appearance);
    //}

    //public void ToBattleState()
    //{
    //    this.TransitReady((int)StateId.Idle);
    //}

}
