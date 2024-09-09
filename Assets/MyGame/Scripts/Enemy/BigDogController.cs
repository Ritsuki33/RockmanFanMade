using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BigDogController : RbStateMachine<BigDogController>
{
    [SerializeField] Animator _animator;
    [SerializeField] Transform _mouth;
    [SerializeField] Transform _tale;
    [SerializeField] Transform _target;
    AmbiguousTimer timer = new AmbiguousTimer();


    BaseObjectPool BomPool=>EffectManager.Instance.BomPool;
    BaseObjectPool ExplodePool=>EffectManager.Instance.ExplodePool;
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
                              (0, () =>
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
    }

    class Fire : RbState<BigDogController>
    {
        static int animationHash = Animator.StringToHash("Fire");
        protected override void Enter(BigDogController ctr, int preId)
        {
            ctr._animator.Play(animationHash);
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
            TransitSubReady((int)SubStateId.Stance);
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
                        Vector2 startVec = ParabolicBehavior.Init(ctr._target.position, ctr._tale.position, 60, gravityScale, () => { Debug.Log("発射失敗"); });
                        rb.velocity = startVec;
                    },
                    (rb) =>
                    {
                        ParabolicBehavior.FixedUpdate(rb, gravityScale);
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
    }
}
