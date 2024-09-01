using System;
using System.Threading;
using UnityEngine;

public class GreenManController : ExRbStateMachine<GreenManController>
{
    [SerializeField] Animator _animator = default;
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
            greenMan.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                Probability.BranchMethods(
                              (50, () =>
                              {
                                  greenMan.TransitReady((int)StateId.Shoot);
                              }
                ),
                              (50, () =>
                              {
                                  greenMan.TransitReady((int)StateId.Jump);
                              }
                ));
            });
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
                              (0, () =>
                              {
                              }
                ));
            });
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

        protected override void OnTopHitEnter(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(GreenManController greenMan, RaycastHit2D hit, IParentState parent)
        {
            greenMan.TransitReady((int)StateId.Idle);
        }
    }
}
