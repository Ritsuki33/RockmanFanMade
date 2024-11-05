using System.Collections;
using UnityEngine;

public class BigDogController : RbStateMachine<BigDogController>
{
    [SerializeField] BigDog bigDog;
    [SerializeField] Animator _animator;
    [SerializeField] Transform _mouth;

    [Header("しっぽ攻撃")]
    [SerializeField] Transform _tale;

    [Header("ファイア攻撃")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    AmbiguousTimer timer = new AmbiguousTimer();


    BaseObjectPool BomPool=>EffectManager.Instance.BomPool;
    BaseObjectPool FirePool=>EffectManager.Instance.FirePool;
    BaseObjectPool ExplodePool=>EffectManager.Instance.ExplodePool;

    PlayerController Player => WorldManager.Instance.PlayerController;
    enum StateId
    {
        Idle,
        Fire,
        TailFire,
    }

    private void Awake()
    {
        AddState((int)StateId.Idle,new Idle());
        AddState((int)StateId.Fire, new Fire());
        AddState((int)StateId.TailFire, new TailFire());
        Init();
    }

    public void Init()
    {
        TransitReady((int)StateId.Idle);
    }

    class Idle :RbState<BigDogController, Idle>
    {
        static int animationHash = Animator.StringToHash("Idle");
        protected override void Enter(BigDogController ctr, int preId, int subId)
        {
            ctr._animator.Play(animationHash);
            ctr.timer.Start(1, 3);
        }

        protected override void Update(BigDogController ctr)
        {
            ctr.timer.MoveAheadTime(Time.deltaTime,
                () =>
                {
                    Probability.BranchMethods(
                              (50, () =>
                              {
                                  ctr.TransitReady((int)StateId.Fire);
                              }
                    ),
                              (50, () =>
                              {
                                  ctr.TransitReady((int)StateId.TailFire);
                              }
                    ));
                }
                );
        }

        protected override void OnTriggerEnter(BigDogController ctr, RockBusterDamage collision)
        {
            ctr.bigDog.Damaged(collision);
        }
      
    }

    class Fire : RbState<BigDogController, Fire>
    {
        static int animationHash = Animator.StringToHash("Fire");

        bool finished = false;
        protected override void Enter(BigDogController ctr, int preId, int subId)
        {
            ctr._animator.Play(animationHash);
            ctr.timer.Start(1, 1);
            finished = false;

            ctr.StartCoroutine(StartFire());

            IEnumerator StartFire()
            {
                int count = 0;
                while (count < 7)
                {
                    var fire = ctr.FirePool.Pool.Get() as Projectile;
                    fire.transform.position = new Vector3(ctr._mouth.position.x, ctr._mouth.position.y, -1);
                    float time = 0;
                    fire.Init(3, null,
                        (rb) =>
                        {
                            rb.velocity = BezierCurveBehevior.GetVelocity(fire.transform.position, time, 1, ctr._mouth, ctr.pointA, ctr.pointB);
                            time += Time.fixedDeltaTime;
                        }
                        );

                    yield return new WaitForSeconds(0.07f);
                    count++;
                }

                finished = true;
            }
        }

        protected override void Update(BigDogController ctr)
        {
            if (finished)
            {
                ctr.timer.MoveAheadTime(Time.deltaTime,
                    () =>
                    {
                        ctr.TransitReady((int)StateId.Idle);
                    }
                    );
            }
        }

        protected override void OnTriggerEnter(BigDogController ctr, RockBusterDamage collision)
        {
            ctr.bigDog.Damaged(collision);
        }
    }


    class TailFire : RbState<BigDogController, TailFire>
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

        protected override void Enter(BigDogController ctr, int preId, int subId)
        {
            TransitSubReady((int)SubStateId.Stance);
            ctr._animator.Play(animationHash);
        }

        class Stance : RbSubState<BigDogController, Stance, TailFire>
        {
            protected override void Update(BigDogController ctr, TailFire parent)
            {
                if (!ctr._animator.IsPlayingCurrentAnimation(animationHash))
                {
                    parent.TransitSubReady((int)SubStateId.Fire);
                }
            }
        }

        class Fire : RbSubState<BigDogController, Fire, TailFire>
        {
            protected override void Enter(BigDogController ctr, TailFire parent, int preId, int subId)
            {
                var bom = ctr.BomPool.Pool.Get() as Projectile;
                var explode = ctr.ExplodePool.Pool.Get();
                bom.transform.position = new Vector3(ctr._tale.position.x, ctr._tale.position.y, -1);
                explode.transform.position = new Vector3(ctr._tale.position.x, ctr._tale.position.y, -1);
                float gravityScale = 1;
                bom.Init(
                    3,
                    (rb) =>
                    {
                        Vector2 startVec = ParabolicBehavior.Init(ctr.Player.transform.position, ctr._tale.position, 60, gravityScale, () => { Debug.Log("発射失敗"); });
                        rb.velocity = startVec;
                    },
                    (rb) =>
                    {
                        ParabolicBehavior.FixedUpdate(rb, gravityScale);
                    },
                    (projectile) =>
                    {

                        bom.Delete();
                        ctr.StartCoroutine(ExplodeCo());
                        IEnumerator ExplodeCo()
                        {
                            var explode = ctr.ExplodePool.Pool.Get() as ExplodeController;
                            explode.transform.position = projectile.transform.position;

                            yield return new WaitForSeconds(0.1f);
                            explode = ctr.ExplodePool.Pool.Get() as ExplodeController;
                            explode.transform.position = projectile.transform.position + Vector3.up;

                            explode = ctr.ExplodePool.Pool.Get() as ExplodeController;
                            explode.transform.position = projectile.transform.position + Vector3.down;

                            explode = ctr.ExplodePool.Pool.Get() as ExplodeController;
                            explode.transform.position = projectile.transform.position + Vector3.right;

                            explode = ctr.ExplodePool.Pool.Get() as ExplodeController;
                            explode.transform.position = projectile.transform.position + Vector3.left;
                        }
                    }
                    );
                ctr.timer.Start(1, 1);
            }

            protected override void Update(BigDogController ctr, TailFire parent)
            {
                ctr.timer.MoveAheadTime(Time.deltaTime,
                () =>
                {
                    ctr.TransitReady((int)StateId.Idle);
                }
                );
            }
          
        }

        protected override void OnTriggerEnter(BigDogController ctr, RockBusterDamage collision)
        {
            ctr.bigDog.Damaged(collision);
        }
    }
}
