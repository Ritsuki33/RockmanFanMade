using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class GreenManController : EnemyController<GreenManController,GreenMan>,IDamageController
{
    //[SerializeField] GreenMan greenMan = default;
    [SerializeField] Animator _animator = default;
    [SerializeField] Transform launcher = default;
    enum StateId
    {
        Idle,
        Float,
        Shoot,
        Shooting,
        Jump
    }

    Gravity gravity = default;
    Jump jump = default;
    ExpandRigidBody exRb = default;
    AmbiguousTimer timer = new AmbiguousTimer();

    BaseObjectPool Buster => EffectManager.Instance.MettoruFirePool;
    private PlayerController Player => WorldManager.Instance.PlayerController;
    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;
    Coroutine defense = null;

    public bool IsRight => this.transform.localScale.x < 0;
    private void Awake()
    {
        gravity = GetComponent<Gravity>();
        jump= GetComponent<Jump>();
        exRb = GetComponent<ExpandRigidBody>();

        AddState((int)StateId.Idle, new Idle());
        AddState((int)StateId.Float, new Float());
        AddState((int)StateId.Shoot, new Shoot());
        AddState((int)StateId.Shooting, new Shooting());
        AddState((int)StateId.Jump, new Jumping());

        Init();
    }

    public void Init()
    {
        gravity.Reset();
        jump.SetSpeed(0);
        TransitReady((int)StateId.Idle);
    }

    class Idle: ExRbState<GreenManController, Idle> {
        static int animationHash = Animator.StringToHash("Idle");

        protected override void Enter(GreenManController greenMan, int preId, int subId)
        {
            greenMan._animator.Play(animationHash);
            greenMan.timer.Start(1, 2);
        }

        protected override void FixedUpdate(GreenManController greenMan)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenManController greenMan)
        {
            greenMan.enemy.TurnToTarget(greenMan.Player.transform.position);
            greenMan.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                              (50, () =>
                              {
                                  greenMan.TransitReady((int)StateId.Shoot);
                              }
                ),
                              (25, () =>
                              {
                                  greenMan.TransitReady((int)StateId.Jump);
                              }
                ));
            });
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision)
        {
            var damage = collision.GetComponent<DamageBase>();
            damage?.Accept(greenMan);
        }
      
        protected override void OnBottomHitStay(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.TransitReady((int)StateId.Float);
        }
    }
    class Float : ExRbState<GreenManController, Float>
    {
        static int animationHash = Animator.StringToHash("Float");
        protected override void Enter(GreenManController greenMan, int preId, int subId)
        {
            greenMan._animator.Play(animationHash);
        }

        protected override void FixedUpdate(GreenManController greenMan)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision)
        {
            var damage = collision.GetComponent<DamageBase>();
            damage?.Accept(greenMan);
        }

        protected override void OnBottomHitEnter(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.TransitReady((int)StateId.Idle);
        }
    }

    class Shoot : ExRbState<GreenManController, Shoot>
    {
        static int animationHash = Animator.StringToHash("Shoot");
        protected override void Enter(GreenManController greenMan, int preId, int subId)
        {
            greenMan._animator.Play(animationHash);
        }
        protected override void FixedUpdate(GreenManController greenMan)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenManController greenMan)
        {
            if (!greenMan._animator.IsPlayingCurrentAnimation(animationHash))
            {
                greenMan.TransitReady((int)StateId.Shooting);
            }
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision)
        {
            var damage = collision.GetComponent<DamageBase>();
            damage?.Accept(greenMan);
        }

        protected override void OnBottomHitStay(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.TransitReady((int)StateId.Float);
        }
    }
    class Shooting : ExRbState<GreenManController, Shooting>
    {
        static int animationHash = Animator.StringToHash("Shooting");

        protected override void Enter(GreenManController greenMan, int preId, int subId)
        {
            greenMan._animator.Play(animationHash);
            greenMan.timer.Start(1, 2);
            greenMan.Atack();
        }

        protected override void FixedUpdate(GreenManController greenMan)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenManController greenMan)
        {
            greenMan.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                              (50, () =>
                              {
                                  greenMan.TransitReady((int)StateId.Idle);
                              }
                ),
                              (25, () =>
                              {
                                  greenMan.TransitReady((int)StateId.Shooting, true);
                              }
                ));
            });
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision)
        {
            //greenMan.enemy.Attacked(collision);
            var damage = collision.GetComponent<DamageBase>();
            damage?.Accept(greenMan);
        }

        protected override void OnBottomHitStay(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.TransitReady((int)StateId.Float);
        }
    }

    class Jumping : ExRbState<GreenManController, Jumping>
    {
        static int animationHash = Animator.StringToHash("Float");
        protected override void Enter(GreenManController greenMan, int preId, int subId)
        {
            greenMan.jump.SetSpeed(15);
            greenMan._animator.Play(animationHash);
        }

        protected override void FixedUpdate(GreenManController greenMan)
        {
            greenMan.jump.UpdateVelocity(greenMan.gravity.GravityScale);
            greenMan.exRb.velocity = greenMan.jump.CurrentVelocity;

            if (greenMan.jump.CurrentSpeed == 0)
            {
                greenMan.TransitReady((int)StateId.Float);
            }
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision)
        {
            var damage = collision.GetComponent<DamageBase>();
            damage?.Accept(greenMan);
        }

        protected override void OnTopHitEnter(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(GreenManController greenMan, RaycastHit2D hit)
        {
            greenMan.TransitReady((int)StateId.Idle);
        }
    }

    private void Defense(RockBusterDamage damage)
    {
        if (damage.baseDamageValue == 1)
        {
            ReflectBuster(damage.projectile);
        }
        else
        {
            damage.DeleteBuster();
        }
    }

    public void ReflectBuster(Projectile projectile)
    {
        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(projectile));
    }

    IEnumerator DefenseRockBuster(Projectile projectile)
    {
        Vector2 reflection = projectile.CurVelocity;
        float speed = projectile.CurSpeed;
        reflection.x *= -1;
        reflection = new Vector2(reflection.x, 0).normalized;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        projectile.Init(
            0,
            null
,
            (rb) =>
            {
                rb.velocity = reflection * speed;
            });
        yield return new WaitForSeconds(1f);

        defense = null;
    }

    void Atack()
    {
        var buster = Buster.Pool.Get();

        var projectile=buster.GetComponent<Projectile>();

        Vector2 direction = IsRight ? Vector2.right : Vector2.left;
        float speed = 5;
        projectile.transform.position = launcher.transform.position;
        projectile.GetComponent<Projectile>().Init(1,
            null,
            (rb) =>
            {
                rb.velocity = direction * speed;
            });
    }


    protected override void TakeDamage(RockBusterDamage damage)
    {
        if (stateMachine.CurId == (int)StateId.Shooting)
        {
            base.TakeDamage(damage);
        }
        else
        {
            Defense(damage);
        }
    }
}
