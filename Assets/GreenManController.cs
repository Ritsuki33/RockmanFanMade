using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class GreenManController : ExRbStateMachine<GreenManController>
{
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
    private PlayerController Player => GameManager.Instance.Player;
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
        TransitReady((int)StateId.Float);
    }

    class Idle: ExRbState<GreenManController> {
        static int animationHash = Animator.StringToHash("Idle");

        protected override void Enter(GreenManController greenMan, int preId)
        {
            greenMan._animator.Play(animationHash);
            greenMan.timer.Start(1, 2);
        }

        protected override void FixedUpdate(GreenManController greenMan, IParentState parent)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenManController greenMan, IParentState parent)
        {
            greenMan.TurnToTarget(greenMan.Player.transform.position);
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

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision, IParentState parent)
        {
            greenMan.Defense(collision);
        }
      
        protected override void OnBottomHitStay(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.TransitReady((int)StateId.Float);
        }
    }
    class Float : ExRbState<GreenManController>
    {
        static int animationHash = Animator.StringToHash("Float");
        protected override void Enter(GreenManController greenMan, int preId)
        {
            greenMan._animator.Play(animationHash);
        }

        protected override void FixedUpdate(GreenManController greenMan, IParentState parent)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision, IParentState parent)
        {
            greenMan.Defense(collision);
        }

        protected override void OnBottomHitEnter(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.TransitReady((int)StateId.Idle);
        }
    }

    class Shoot : ExRbState<GreenManController>
    {
        static int animationHash = Animator.StringToHash("Shoot");
        protected override void Enter(GreenManController greenMan, int preId)
        {
            greenMan._animator.Play(animationHash);
        }
        protected override void FixedUpdate(GreenManController greenMan, IParentState parent)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenManController greenMan, IParentState parent)
        {
            if (!greenMan._animator.IsPlayingCurrentAnimation(animationHash))
            {
                greenMan.TransitReady((int)StateId.Shooting);
            }
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision, IParentState parent)
        {
            greenMan.Dead(collision);
        }

        protected override void OnBottomHitStay(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.TransitReady((int)StateId.Float);
        }
    }
    class Shooting : ExRbState<GreenManController>
    {
        static int animationHash = Animator.StringToHash("Shooting");

        protected override void Enter(GreenManController greenMan, int preId)
        {
            greenMan._animator.Play(animationHash);
            greenMan.timer.Start(1, 2);
            greenMan.Atack();
        }

        protected override void FixedUpdate(GreenManController greenMan, IParentState parent)
        {
            greenMan.gravity.UpdateVelocity();
            greenMan.exRb.velocity = greenMan.gravity.CurrentVelocity;
        }

        protected override void Update(GreenManController greenMan, IParentState parent)
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

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision, IParentState parent)
        {
            greenMan.Dead(collision);
        }

        protected override void OnBottomHitStay(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.gravity.Reset();
        }

        protected override void OnBottomHitExit(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.TransitReady((int)StateId.Float);
        }
    }

    class Jumping : ExRbState<GreenManController>
    {
        static int animationHash = Animator.StringToHash("Float");
        protected override void Enter(GreenManController greenMan, int preId)
        {
            greenMan.jump.SetSpeed(15);
            greenMan._animator.Play(animationHash);
        }

        protected override void FixedUpdate(GreenManController greenMan, IParentState parent)
        {
            greenMan.jump.UpdateVelocity(greenMan.gravity.GravityScale);
            greenMan.exRb.velocity = greenMan.jump.CurrentVelocity;

            if (greenMan.jump.CurrentSpeed == 0)
            {
                greenMan.TransitReady((int)StateId.Float);
            }
        }

        protected override void OnTriggerEnter2D(GreenManController greenMan, Collider2D collision, IParentState parent)
        {
            greenMan.Defense(collision);
        }

        protected override void OnTopHitEnter(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.TransitReady((int)StateId.Idle);
        }
    }

    public void Defense(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster"))
        {
            ReflectBuster(collision);
        }
        else if (collision.gameObject.CompareTag("ChargeShot"))
        {
            var rockBuster = collision.gameObject.GetComponent<Projectile>();
            rockBuster.Delete();
        }
    }
    public void Dead(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster") || collision.gameObject.CompareTag("ChargeShot"))
        {
            var projectile = collision.gameObject.GetComponent<Projectile>();

            projectile?.Delete();

            var explode = ExplodePool.Pool.Get();
            explode.transform.position = this.transform.position;

            this.gameObject.SetActive(false);
        }
    }


    public void ReflectBuster(Collider2D collision)
    {
        var rockBuster = collision.gameObject.GetComponent<Projectile>();

        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(rockBuster));
    }

    IEnumerator DefenseRockBuster(Projectile projectile)
    {
        projectile.DisableDamageDetection();
        Vector2 reflection = projectile.Direction;
        reflection.x *= -1;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        projectile.ChangeDirection(reflection);
        yield return new WaitForSeconds(1f);

        defense = null;
    }

    void Atack()
    {
        var buster = Buster.Pool.Get();

        var projectile=buster.GetComponent<Projectile>();

        projectile.Init((IsRight) ? Vector2.right : Vector2.left, launcher.position, 5);
    }


    /// <summary>
    /// キャラクターの振り向き
    /// </summary>
    private void TurnToTarget(Vector2 targetPos)
    {
        if (transform.position.x > targetPos.x)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1;
            transform.localScale = localScale;
        }
        else
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1;
            transform.localScale = localScale;
        }
    }

}
