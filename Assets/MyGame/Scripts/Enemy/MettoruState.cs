using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mettoru
{
    class Idle : ExRbState<Mettoru, Idle>
    {
        AmbiguousTimer timer = new AmbiguousTimer();
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }
        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
            timer.Start(0.5f, 2.0f);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(Mettoru mettoru)
        {
            if (!mettoru.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                mettoru.TurnToTarget(mettoru.Player.transform.position);

                mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        if (Probability.GetBoolean(0.35f))
                        {
                            mettoru.m_stateMachine.TransitReady((int)StateID.Hide);
                        }
                    });
            }

            timer.MoveAheadTime(Time.deltaTime,
                   () =>
                   {
                       if (mettoru.walk)
                       {
                           mettoru.m_stateMachine.TransitReady((int)StateID.Walk);
                       }
                       else
                       {
                           mettoru.m_stateMachine.TransitReady((int)StateID.Hide);
                       }
                   });
        }
        protected override void OnTriggerEnter(Mettoru mettoru, PlayerAttack collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Damaged(collision);
        }


    }

    class Walk : ExRbState<Mettoru, Walk>
    {
        AmbiguousTimer timer = new AmbiguousTimer();
        int animationHash = 0;
        public Walk() { animationHash = Animator.StringToHash("Walk"); }
        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
            mettoru.TurnToTarget(mettoru.Player.transform.position);
            timer.Start(0.5f, 2.0f);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;

            if (mettoru.move.Hit || !mettoru.groundChecker.CheckGround(mettoru.transform.position, mettoru.exRb.PhysicalBoxSize, mettoru.IsRight))
            {
                mettoru.TurnFace();
            }

            mettoru.move.OnUpdate(Vector2.right, (mettoru.IsRight) ? Move.InputType.Right : Move.InputType.Left);

            mettoru.exRb.velocity += mettoru.move.CurrentVelocity;
        }

        protected override void Update(Mettoru mettoru)
        {
            mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        if (Probability.GetBoolean(0.35f))
                        {
                            mettoru.m_stateMachine.TransitReady((int)StateID.Hide);
                        }
                    });

            timer.MoveAheadTime(Time.deltaTime,
                  () =>
                  {
                      mettoru.m_stateMachine.TransitReady((int)StateID.Hide);
                  });
        }

        protected override void OnTriggerEnter(Mettoru mettoru, PlayerAttack collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnLeftHitStay(Mettoru mettoru, RaycastHit2D hit)
        {
            mettoru.direct.TurnFace();
        }

        protected override void OnRightHitStay(Mettoru mettoru, RaycastHit2D hit)
        {
            mettoru.direct.TurnFace();
        }
    }

    class Hide : ExRbState<Mettoru, Hide>
    {
        int animationHash = 0;
        public Hide() { animationHash = Animator.StringToHash("Hide"); }

        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(Mettoru mettoru)
        {
            if (!mettoru.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                mettoru.m_stateMachine.TransitReady((int)StateID.Hiding);
            }
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Defense(collision);
        }

    }

    class Hiding : ExRbState<Mettoru, Hiding>
    {
        int animationHash = 0;
        AmbiguousTimer timer = new AmbiguousTimer();
        public Hiding() { animationHash = Animator.StringToHash("Hiding"); }

        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
            timer.Start(1, 3);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(Mettoru mettoru)
        {
            mettoru.TurnToTarget(mettoru.Player.transform.position);

            if (mettoru.defense == null)
            {
                timer.MoveAheadTime(Time.deltaTime,
                    () =>
                    {
                        Probability.BranchMethods(
                            (50, () =>
                            {
                                mettoru.m_stateMachine.TransitReady((int)StateID.Appear);
                            }
                        ),
                            (0, () =>
                            {
                                mettoru.m_stateMachine.TransitReady((int)StateID.LookIn);
                            }
                        ));
                    });
            }
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Defense(collision);
        }
    }

    class Appear : ExRbState<Mettoru, Appear>
    {
        int animationHash = 0;
        AmbiguousTimer timer = new AmbiguousTimer();
        public Appear() { animationHash = Animator.StringToHash("Appear"); }

        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
            mettoru.Fire();
            timer.Start(1, 3);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(Mettoru mettoru)
        {
            if (!mettoru.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                if (mettoru.walk)
                {
                    mettoru.m_stateMachine.TransitReady((int)StateID.Walk);
                }
                else
                {
                    mettoru.m_stateMachine.TransitReady((int)StateID.Idle);
                }
            }
        }

        protected override void OnTriggerEnter(Mettoru mettoru, PlayerAttack collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Damaged(collision);
        }
    }

    class LookIn : ExRbState<Mettoru, LookIn>
    {
        int animationHash = 0;
        public LookIn() { animationHash = Animator.StringToHash("LookIn"); }
        AmbiguousTimer timer = new AmbiguousTimer();

        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
            mettoru.Fire();
            timer.Start(0.5f, 2.0f);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(Mettoru mettoru)
        {
            if (!mettoru.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                mettoru.TurnToTarget(mettoru.Player.transform.position);

                mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        mettoru.m_stateMachine.TransitReady((int)StateID.Hiding);
                    });
            }

            timer.MoveAheadTime(Time.deltaTime,
                   () =>
                   {
                       mettoru.m_stateMachine.TransitReady((int)StateID.Hiding);
                   });
        }

        protected override void OnTriggerEnter(Mettoru mettoru, PlayerAttack collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Damaged(collision);
        }
    }

    float move_x = 0;

    class Jumping : ExRbState<Mettoru, Jumping>
    {
        int animationHash = 0;
        public Jumping() { animationHash = Animator.StringToHash("Jump"); }

        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
            mettoru.gravity.Reset();
            mettoru.jump.Init(15);

            Vector2 direction = (mettoru.IsRight) ? Vector2.right : Vector2.left;
            mettoru.move_x = ParabolaCalc.GetHorizonVelocity(mettoru.transform.position, (Vector2)mettoru.transform.position + direction * 5, 15, mettoru.gravity.GravityScale);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.jump.OnUpdate(mettoru.gravity.GravityScale);
            mettoru.exRb.velocity += mettoru.jump.CurrentVelocity;
            mettoru.exRb.velocity += new Vector2(mettoru.move_x, 0);

            if (mettoru.jump.CurrentSpeed == 0)
            {
                mettoru.m_stateMachine.TransitReady((int)StateID.JumpFloating);
            }
        }

        protected override void OnTriggerEnter(Mettoru mettoru, PlayerAttack collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Damaged(collision);
        }
    }

    class JumpFloating : ExRbState<Mettoru, JumpFloating>
    {
        int animationHash = 0;
        public JumpFloating() { animationHash = Animator.StringToHash("Float"); }

        protected override void Enter(Mettoru mettoru, int preId, int subId)
        {
            mettoru.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(Mettoru mettoru)
        {
            mettoru.gravity.OnUpdate();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
            mettoru.exRb.velocity += new Vector2(mettoru.move_x, 0);
        }

        protected override void OnBottomHitStay(Mettoru mettoru, RaycastHit2D hit)
        {
            mettoru.m_stateMachine.TransitReady((int)StateID.Idle);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, PlayerAttack collision)
        {
            mettoru.Damaged(collision);
        }

        protected override void OnTriggerEnter(Mettoru mettoru, RockBuster collision)
        {
            mettoru.Damaged(collision);
        }
    }
}
