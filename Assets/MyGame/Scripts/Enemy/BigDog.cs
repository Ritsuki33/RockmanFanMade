using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigDog : StageEnemy
{
    [SerializeField] Transform _mouth;

    [Header("しっぽ攻撃")]
    [SerializeField] Transform _tale;

    [Header("ファイア攻撃")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    AmbiguousTimer timer = new AmbiguousTimer();

    RbCollide rbCollide = new RbCollide();
    enum StateId
    {
        Idle,
        Fire,
        TailFire,
    }

    RbStateMachine<BigDog> stateMachine=new RbStateMachine<BigDog>();
    StagePlayer Player => WorldManager.Instance.Player;

    protected override void Awake()
    {
        stateMachine.AddState((int)StateId.Idle, new Idle());
        stateMachine.AddState((int)StateId.Fire, new Fire());
        stateMachine.AddState((int)StateId.TailFire, new TailFire());

        rbCollide.Init();
        rbCollide.onTriggerEnterRockBusterDamage += OnTriggerEnterDamagedBase;
    }

    protected override void Init()
    {
        base.Init();
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

    private void OnTriggerEnterDamagedBase(RockBusterDamage damage)
    {
        stateMachine.OnTriggerEnter(this, damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(collision);    
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

        protected override void OnTriggerEnter(BigDog ctr, RockBusterDamage collision)
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
                    ObjectManager.Instance.Create(
                        ProjectileType.Fire,
                         new Vector3(ctr._mouth.position.x, ctr._mouth.position.y, -1),
                         3,
                         false,
                         null,
                         (rb) =>
                         {
                             rb.SetVelocty(BezierCurveBeheviorHelper.GetStrobe(rb.transform.position, ctr._mouth, ctr.pointA, ctr.pointB, time));
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

        protected override void OnTriggerEnter(BigDog ctr, RockBusterDamage collision)
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

                ObjectManager.Instance.Create(
                    ProjectileType.Bom,
                    new Vector3(ctr._tale.position.x, ctr._tale.position.y, -1),
                    3,
                    false,
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
                            ObjectManager.Instance.Create(ExplodeType.Explode1, Explode.Layer.EnemyAttack, 3, projectile.transform.position);

                            yield return PauseManager.Instance.PausableWaitForSeconds(0.1f);
                            ObjectManager.Instance.Create(ExplodeType.Explode1, Explode.Layer.EnemyAttack, 3, projectile.transform.position + Vector3.up);

                            ObjectManager.Instance.Create(ExplodeType.Explode1, Explode.Layer.EnemyAttack, 3, projectile.transform.position + Vector3.down);

                            ObjectManager.Instance.Create(ExplodeType.Explode1, Explode.Layer.EnemyAttack, 3, projectile.transform.position + Vector3.right);

                            ObjectManager.Instance.Create(ExplodeType.Explode1, Explode.Layer.EnemyAttack, 3, projectile.transform.position + Vector3.left);
                        }
                    });
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

        protected override void OnTriggerEnter(BigDog ctr, RockBusterDamage collision)
        {
            ctr.Damaged(collision);
        }
    }
}
