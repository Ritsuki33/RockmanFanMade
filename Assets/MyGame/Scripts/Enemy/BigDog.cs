using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BigDog : StageEnemy, IRbVisitor
{
    [SerializeField] Transform _mouth;
    [SerializeField] BoxCollider2D _boxPhysicalCollider;
    [Header("しっぽ攻撃")]
    [SerializeField] Transform _tale;

    [Header("ファイア攻撃")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    AmbiguousTimer timer = new AmbiguousTimer();

    CachedCollide rbCollide = new CachedCollide();
    enum StateId
    {
        Idle,
        Fire,
        TailFire,
        Death,
    }

    RbStateMachine<BigDog> stateMachine = new RbStateMachine<BigDog>();
    StagePlayer Player => WorldManager.Instance.Player;

    protected override void Awake()
    {
        stateMachine.AddState((int)StateId.Idle, new Idle());
        stateMachine.AddState((int)StateId.Fire, new Fire());
        stateMachine.AddState((int)StateId.TailFire, new TailFire());
        stateMachine.AddState((int)StateId.Death, new Death());

        rbCollide.CacheClear();
    }

    protected override void Init()
    {
        base.Init();
        _boxPhysicalCollider.enabled = true;
        stateMachine.TransitReady((int)StateId.Idle);
    }

    protected override void OnFixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        stateMachine.Update(this);
    }

    public override void OnDead()
    {
        stateMachine.TransitReady((int)StateId.Death);
    }

    void IRbVisitor.OnTriggerEnter(PlayerAttack damage)
    {
        stateMachine.OnTriggerEnter(this, damage);
    }

    void IRbVisitor.OnTriggerEnter(RockBuster damage)
    {
        stateMachine.OnTriggerEnter(this, damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(this, collision);
    }

    class Idle : RbState<BigDog, Idle>
    {
        static int animationHash = Animator.StringToHash("Idle");
        protected override void Enter(BigDog ctr, int preId, int subId)
        {
            ctr.MainAnimator.Play(animationHash);
            ctr.timer.Start(1, 3);
        }

        protected override void Update(BigDog ctr)
        {
            ctr.timer.MoveAheadTime(Time.deltaTime,
                () =>
                {
                    Probability.BranchMethods(
                              (50, () =>
                              {
                                  ctr.stateMachine.TransitReady((int)StateId.Fire);
                              }
                    ),
                              (50, () =>
                              {
                                  ctr.stateMachine.TransitReady((int)StateId.TailFire);
                              }
                    ));
                }
                );
        }

        protected override void OnTriggerEnter(BigDog ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(BigDog ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }

    }

    class Fire : RbState<BigDog, Fire>
    {
        static int animationHash = Animator.StringToHash("Fire");

        bool finished = false;
        protected override void Enter(BigDog ctr, int preId, int subId)
        {
            ctr.MainAnimator.Play(animationHash);
            ctr.timer.Start(1, 1);
            finished = false;

            ctr.StartCoroutine(StartFire());

            IEnumerator StartFire()
            {
                int count = 0;
                while (count < 7)
                {
                    float time = 0;
                    var projectile = ObjectManager.Instance.OnGet<SimpleProjectileComponent>(PoolType.Fire);
                    Vector2 mouthPos = ctr._mouth.position;
                    Vector2 pointA = ctr.pointA.position;
                    Vector2 pointB = WorldManager.Instance.Player.transform.position;
                    projectile.Setup(
                        new Vector3(ctr._mouth.position.x, ctr._mouth.position.y, -1),
                         false,
                         3,
                         (rb) => { AudioManager.Instance.PlaySe(SECueIDs.boostmini); },
                         (rb) =>
                         {
                             rb.SetVelocty(BezierCurveBeheviorHelper.GetStrobe(rb.transform.position, mouthPos, pointA, pointB, time));
                             time += Time.fixedDeltaTime;
                         },
                         null
                        );
                    yield return PauseManager.Instance.PausableWaitForSeconds(0.07f);
                    count++;
                }

                finished = true;
            }
        }

        protected override void Update(BigDog ctr)
        {
            if (finished)
            {
                ctr.timer.MoveAheadTime(Time.deltaTime,
                    () =>
                    {
                        ctr.stateMachine.TransitReady((int)StateId.Idle);
                    }
                    );
            }
        }

        protected override void OnTriggerEnter(BigDog ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(BigDog ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }


    class TailFire : RbState<BigDog, TailFire>
    {
        static int animationHash = Animator.StringToHash("TailFire");

        enum SubStateId
        {
            Stance,
            Fire,
        }
        public TailFire()
        {
            AddSubState((int)SubStateId.Stance, new Stance());
            AddSubState((int)SubStateId.Fire, new Fire());
        }

        protected override void Enter(BigDog ctr, int preId, int subId)
        {
            TransitSubReady((int)SubStateId.Stance);
            ctr.MainAnimator.Play(animationHash);
        }

        class Stance : RbSubState<BigDog, Stance, TailFire>
        {
            protected override void Update(BigDog ctr, TailFire parent)
            {
                if (!ctr.MainAnimator.IsPlayingCurrentAnimation(animationHash))
                {
                    parent.TransitSubReady((int)SubStateId.Fire);
                }
            }
        }

        class Fire : RbSubState<BigDog, Fire, TailFire>
        {
            protected override void Enter(BigDog ctr, TailFire parent, int preId, int subId)
            {
                float gravityScale = 1;

                AudioManager.Instance.PlaySe(SECueIDs.explosion);

                var bom = ObjectManager.Instance.OnGet<SimpleProjectileComponent>(PoolType.Bom);
                bom.Setup(
                    new Vector3(ctr._tale.position.x, ctr._tale.position.y, -1),
                    false,
                    3,
                    (rb) =>
                    {
                        Vector2 startVec = ParabolicBehaviorHelper.Start(ctr.Player.transform.position, ctr._tale.position, 60, gravityScale, () => { Debug.Log("発射失敗"); });
                        rb.velocity = startVec;
                    },
                    (rb) =>
                    {
                        rb.velocity += Vector2.down * gravityScale;
                    },
                    (projectile) =>
                    {

                        projectile.Delete();
                        GameMainManager.Instance.StartCoroutine(ExplodeCo());
                        IEnumerator ExplodeCo()
                        {
                            var explode = ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
                            explode.Setup(Explode.Layer.EnemyAttack, projectile.transform.position, 3);

                            yield return PauseManager.Instance.PausableWaitForSeconds(0.1f);
                            var explode1 = ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
                            explode1.Setup(Explode.Layer.EnemyAttack, projectile.transform.position + Vector3.up, 3);
                            var explode2 = ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
                            explode2.Setup(Explode.Layer.EnemyAttack, projectile.transform.position + Vector3.down, 3);
                            var explode3 = ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
                            explode3.Setup(Explode.Layer.EnemyAttack, projectile.transform.position + Vector3.right, 3);
                            var explode4 = ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
                            explode4.Setup(Explode.Layer.EnemyAttack, projectile.transform.position + Vector3.left, 3);
                        }
                    }
                    );
                ctr.timer.Start(1, 1);
            }

            protected override void Update(BigDog ctr, TailFire parent)
            {
                ctr.timer.MoveAheadTime(Time.deltaTime,
                () =>
                {
                    ctr.stateMachine.TransitReady((int)StateId.Idle);
                }
                );
            }

        }

        protected override void OnTriggerEnter(BigDog ctr, PlayerAttack collision)
        {
            ctr.Damaged(collision);
        }

        protected override void OnTriggerEnter(BigDog ctr, RockBuster collision)
        {
            ctr.Damaged(collision);
        }
    }

    class Death : RbState<BigDog, Death>
    {
        protected override void Enter(BigDog ctr, int preId, int subId)
        {
            ctr.SetAnimSpeed(0);
            ctr._boxPhysicalCollider.enabled = false;
            var effect = ObjectManager.Instance.OnGet<ExplodeParticleSystem>(PoolType.ExplodeParticleSystem);
            effect.transform.position = new Vector3(ctr.transform.position.x, ctr.transform.position.y, -1) + (Vector3)ctr._boxPhysicalCollider.offset;
            effect.Play(ctr._boxPhysicalCollider.size, 3, 7, () =>
            {
                ctr.Delete();
            });
        }
    }
}
