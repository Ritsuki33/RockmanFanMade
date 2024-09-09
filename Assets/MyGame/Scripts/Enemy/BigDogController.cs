using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class BigDogController : RbStateMachine<BigDogController>
{
    [SerializeField] Animator _animator;
    AmbiguousTimer timer = new AmbiguousTimer();

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
