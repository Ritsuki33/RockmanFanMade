using System.Collections;
using UnityEngine;

public class GreenMan : StageEnemy, IDirect, IRbVisitor, IHitEvent
{
    enum StateId
    {
        Idle,
        Float,
        Shoot,
        Shooting,
        Jump
    }

    [SerializeField] Transform launcher = default;
    [SerializeField] Gravity gravity = default;
    [SerializeField] Jump jump = default;
    [SerializeField] ExpandRigidBody exRb = default;
    [SerializeField] Direct direct;
    AmbiguousTimer timer = new AmbiguousTimer();

    private StagePlayer Player => WorldManager.Instance.Player;
    Coroutine defense = null;


    ExRbStateMachine<GreenMan> stateMachine = new ExRbStateMachine<GreenMan>();
    CachedCollide rbCollide = new CachedCollide();
    CachedHit exRbHit = new CachedHit();
    public bool IsRight => direct.IsRight;
    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
    public void TurnFace() => TurnFace();

    protected override void Awake()
    {
        stateMachine.AddState((int)StateId.Idle, new Idle());
        stateMachine.AddState((int)StateId.Float, new Float());
        stateMachine.AddState((int)StateId.Shoot, new Shoot());
        stateMachine.AddState((int)StateId.Shooting, new Shooting());
        stateMachine.AddState((int)StateId.Jump, new Jumping());

        exRb.Init(this);

        rbCollide.CacheClear();
        exRbHit.CacheClear();
    }


    protected override void Init()
    {
        base.Init();
        gravity.Reset();
        jump.SetSpeed(0);
        stateMachine.TransitReady((int)StateId.Idle);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        stateMachine.FixedUpdate(this);
    }

    protected override void OnLateFixedUpdate()
    {
        exRb.FixedUpdate();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(this, collision);
    }

    void IHitEvent.OnTopHitEnter(RaycastHit2D hit)
    {
        stateMachine.OnTopHitEnter(this, hit);
    }

    void IHitEvent.OnBottomHitStay(RaycastHit2D hit)
    {
        stateMachine.OnBottomHitStay(this, hit);
    }

    void IHitEvent.OnBottomHitExit(RaycastHit2D hit)
    {
        stateMachine.OnBottomHitExit(this, hit);
    }

    void IRbVisitor.OnTriggerEnter(PlayerAttack damage)
    {
        stateMachine.OnTriggerEnter(this, damage);
    }

    void IRbVisitor.OnTriggerEnter(RockBuster damage)
    {
        stateMachine.OnTriggerEnter(this, damage);
    }

    class Idle : ExRbState<GreenMan, Idle>
    {
        static int animationHash = Animator.StringToHash("Idle");

        protected override void Enter(GreenMan greenMan, int preId, int subId)
        {
            greenMan.MainAnimator.Play(animationHash);
            greenMan.timer.Start(1, 2);
        }

        protected override void FixedUpdate(GreenMan greenMan)
        {
            greenMan.gravity.OnUpdate();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenMan greenMan)
        {
            greenMan.TurnToTarget(greenMan.Player.transform.position);
            greenMan.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                              (50, () =>
                              {
                                  greenMan.stateMachine.TransitReady((int)StateId.Shoot);
                              }
                ),
                              (25, () =>
                              {
                                  greenMan.stateMachine.TransitReady((int)StateId.Jump);
                              }
                ));
            });
        }


        protected override void OnTriggerEnter(GreenMan greenMan, RockBuster collision)
        {
            greenMan.Defense(collision);
        }


        protected override void OnBottomHitStay(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.stateMachine.TransitReady((int)StateId.Float);
        }
    }
    class Float : ExRbState<GreenMan, Float>
    {
        static int animationHash = Animator.StringToHash("Float");
        protected override void Enter(GreenMan greenMan, int preId, int subId)
        {
            greenMan.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(GreenMan greenMan)
        {
            greenMan.gravity.OnUpdate();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void OnTriggerEnter(GreenMan greenMan, RockBuster collision)
        {
            greenMan.Defense(collision);
        }

        protected override void OnBottomHitStay(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.stateMachine.TransitReady((int)StateId.Idle);
        }
    }

    class Shoot : ExRbState<GreenMan, Shoot>
    {
        static int animationHash = Animator.StringToHash("Shoot");
        protected override void Enter(GreenMan greenMan, int preId, int subId)
        {
            greenMan.MainAnimator.Play(animationHash);
        }
        protected override void FixedUpdate(GreenMan greenMan)
        {
            greenMan.gravity.OnUpdate();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenMan greenMan)
        {
            if (!greenMan.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                greenMan.stateMachine.TransitReady((int)StateId.Shooting);
            }
        }

        protected override void OnTriggerEnter(GreenMan greenMan, PlayerAttack collision)
        {
            greenMan.Damaged(collision);
        }

        protected override void OnTriggerEnter(GreenMan greenMan, RockBuster collision)
        {
            greenMan.Damaged(collision);
        }

        protected override void OnBottomHitStay(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.stateMachine.TransitReady((int)StateId.Float);
        }
    }
    class Shooting : ExRbState<GreenMan, Shooting>
    {
        static int animationHash = Animator.StringToHash("Shooting");

        protected override void Enter(GreenMan greenMan, int preId, int subId)
        {
            greenMan.MainAnimator.Play(animationHash);
            greenMan.timer.Start(1, 2);
            greenMan.Atack();
        }

        protected override void FixedUpdate(GreenMan greenMan)
        {
            greenMan.gravity.OnUpdate();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenMan greenMan)
        {
            greenMan.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                              (50, () =>
                              {
                                  greenMan.stateMachine.TransitReady((int)StateId.Idle);
                              }
                ),
                              (25, () =>
                              {
                                  greenMan.stateMachine.TransitReady((int)StateId.Shooting, true);
                              }
                ));
            });
        }

        protected override void OnTriggerEnter(GreenMan greenMan, PlayerAttack collision)
        {
            greenMan.Damaged(collision);
        }

        protected override void OnTriggerEnter(GreenMan greenMan, RockBuster collision)
        {
            greenMan.Damaged(collision);
        }

        protected override void OnBottomHitStay(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.stateMachine.TransitReady((int)StateId.Float);
        }
    }

    class Jumping : ExRbState<GreenMan, Jumping>
    {
        static int animationHash = Animator.StringToHash("Float");
        protected override void Enter(GreenMan greenMan, int preId, int subId)
        {
            greenMan.jump.SetSpeed(20);
            greenMan.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(GreenMan greenMan)
        {
            greenMan.jump.OnUpdate(greenMan.gravity.GravityScale);
            greenMan.exRb.velocity = greenMan.jump.CurrentVelocity;

            if (greenMan.jump.CurrentSpeed == 0)
            {
                greenMan.stateMachine.TransitReady((int)StateId.Float);
            }
        }

        protected override void OnTriggerEnter(GreenMan greenMan, RockBuster collision)
        {
            greenMan.Defense(collision);
        }

        protected override void OnTopHitEnter(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(GreenMan greenMan, RaycastHit2D hit)
        {
            greenMan.stateMachine.TransitReady((int)StateId.Idle);
        }
    }

    private void Defense(RockBuster rockBuster)
    {
        AudioManager.Instance.PlaySe(SECueIDs.kin);
        if (rockBuster.Type == RockBuster.BusterType.Mame)
        {
            ReflectBuster(rockBuster);
        }
        else
        {
            rockBuster.Delete();
        }
    }

    public void ReflectBuster(RockBuster projectile)
    {
        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster());
        IEnumerator DefenseRockBuster()
        {
            Vector2 reflection = projectile.CurVelocity;
            float speed = projectile.CurSpeed;
            reflection.x *= -1;
            reflection = new Vector2(reflection.x, 0).normalized;
            reflection += Vector2.up;
            reflection = reflection.normalized;
            projectile.ChangeBehavior(
                0,
                null,
                (rb) =>
                {
                    rb.velocity = reflection * speed;
                });
            yield return PauseManager.Instance.PausableWaitForSeconds(1f);

            defense = null;
        }
    }



    void Atack()
    {
        Vector2 direction = IsRight ? Vector2.right : Vector2.left;
        float speed = 10;

        var projectile = ObjectManager.Instance.OnGet<SimpleProjectileComponent>(PoolType.MettoruFire);

        projectile.Setup(
            launcher.transform.position,
            IsRight,
            3,
            null,
            (rb) =>
            {
                rb.velocity = direction * speed;
            }
            );
    }

}
