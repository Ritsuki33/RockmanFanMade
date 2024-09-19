using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.Tilemaps;
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

    PlayerController Player => GameManager.Instance.Player;
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

    class Idle :RbState<BigDogController>
    {
        static int animationHash = Animator.StringToHash("Idle");
        protected override void Enter(BigDogController ctr, int preId)
        {
            ctr._animator.Play(animationHash);
            ctr.timer.Start(1, 3);
        }

        protected override void Update(BigDogController ctr, IParentState parent)
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

        protected override void OnTriggerEnter2D(BigDogController ctr, Collider2D collision, IParentState parent)
        {
            ctr.bigDog.Attacked(collision);
        }
    }

    class Fire : RbState<BigDogController>
    {
        static int animationHash = Animator.StringToHash("Fire");

        bool finished = false;
        protected override void Enter(BigDogController ctr, int preId)
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

        protected override void Update(BigDogController ctr, IParentState parent)
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

        protected override void OnTriggerEnter2D(BigDogController ctr, Collider2D collision, IParentState parent)
        {
            ctr.bigDog.Attacked(collision);
        }
    }


    class TailFire : RbState<BigDogController>
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

        protected override void Enter(BigDogController ctr, int preId)
        {
            TransitSubReady((int)SubStateId.Stance, preId);
            ctr._animator.Play(animationHash);
        }

        class Stance : RbState<BigDogController>
        {
            protected override void Update(BigDogController ctr, IParentState parent)
            {
                if (!ctr._animator.IsPlayingCurrentAnimation(animationHash))
                {
                    parent.TransitSubReady((int)SubStateId.Fire);
                }
            }
        }

        class Fire : RbState<BigDogController>
        {
            protected override void Enter(BigDogController ctr, int preId)
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
                        projectile.Delete();
                        var explode = ctr.ExplodePool.Pool.Get();
                        explode.transform.position= projectile.transform.position;
                    }
                    );
                ctr.timer.Start(1, 1);
            }

            protected override void Update(BigDogController ctr, IParentState parent)
            {
                ctr.timer.MoveAheadTime(Time.deltaTime,
                () =>
                {
                    ctr.TransitReady((int)StateId.Idle);
                }
                );
            }
        }

        protected override void OnTriggerEnter2D(BigDogController ctr, Collider2D collision, IParentState parent)
        {
            ctr.bigDog.Attacked(collision);
        }
    }
}
