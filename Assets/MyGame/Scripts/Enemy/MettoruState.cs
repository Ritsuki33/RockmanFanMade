using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MettoruController 
{
    class Idle : State<MettoruController>
    {
        AmbiguousTimer timer = new AmbiguousTimer();
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }
        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            timer.Start(0.5f, 2.0f);
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation())
            {
                mettoru.TurnToTarget(mettoru.Player.transform.position);

                mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        if (Probability.GetBoolean(0.35f))
                        {
                            mettoru.stateMachine.TransitState((int)StateID.Hide);
                        }
                    });
            }

            timer.MoveAheadTime(Time.deltaTime,
                   () =>
                   {
                       //if (mettoru.walk)
                       //{
                       //    mettoru.stateMachine.TransitState((int)StateID.Walk);
                       //}
                       //else
                       //{
                       //    mettoru.stateMachine.TransitState((int)StateID.Hide);
                       //}
                       mettoru.stateMachine.TransitState((int)StateID.Jump);

                   });
        }
    }

    class Walk : State<MettoruController>
    {
        AmbiguousTimer timer = new AmbiguousTimer();
        int animationHash = 0;
        public Walk() { animationHash = Animator.StringToHash("Walk"); }
        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.TurnToTarget(mettoru.Player.transform.position);
            timer.Start(0.5f, 2.0f);
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;

            if (mettoru.move.Hit|| !mettoru.groundChecker.CheckGround(mettoru.IsRight))
            {
                mettoru.TurnFace();
            }

            mettoru.move.UpdateVelocity(Vector2.right, (mettoru.IsRight) ? Move.InputType.Right : Move.InputType.Left);

            mettoru.exRb.velocity += mettoru.move.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
        {
            mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        if (Probability.GetBoolean(0.35f))
                        {
                            mettoru.stateMachine.TransitState((int)StateID.Hide);
                        }
                    });

            timer.MoveAheadTime(Time.deltaTime,
                  () =>
                  {
                      mettoru.stateMachine.TransitState((int)StateID.Hide);
                  });
        }
    }

    class Hide : State<MettoruController>
    {
        int animationHash = 0;
        public Hide() { animationHash = Animator.StringToHash("Hide"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.invincible = true;
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation())
            {
                mettoru.stateMachine.TransitState((int)StateID.Hiding);
            }
        }
    }

    class Hiding : State<MettoruController>
    {
        int animationHash = 0;
        AmbiguousTimer timer = new AmbiguousTimer();
        public Hiding() { animationHash = Animator.StringToHash("Hiding"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            timer.Start(1, 3);
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
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
                                mettoru.stateMachine.TransitState((int)StateID.Appear);
                            }
                        ),
                            (0, () =>
                            {
                                mettoru.stateMachine.TransitState((int)StateID.LookIn);
                            }
                        ));
                    });
            }
        }
    }

    class Appear : State<MettoruController>
    {
        int animationHash = 0;
        AmbiguousTimer timer=new AmbiguousTimer();
        public Appear() { animationHash = Animator.StringToHash("Appear"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.Fire();
            mettoru.invincible = false;
            timer.Start(1, 3);
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation())
            {
                if (mettoru.walk)
                {
                    mettoru.stateMachine.TransitState((int)StateID.Walk);
                }
                else
                {
                    mettoru.stateMachine.TransitState((int)StateID.Idle);
                }
            }
        }
    }

    class LookIn : State<MettoruController>
    {
        int animationHash = 0;
        public LookIn() { animationHash = Animator.StringToHash("LookIn"); }
        AmbiguousTimer timer = new AmbiguousTimer();

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.Fire();
            timer.Start(0.5f, 2.0f);
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation())
            {
                mettoru.TurnToTarget(mettoru.Player.transform.position);

                mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                            mettoru.stateMachine.TransitState((int)StateID.Hiding);
                    });
            }

            timer.MoveAheadTime(Time.deltaTime,
                   () =>
                   {
                       mettoru.stateMachine.TransitState((int)StateID.Hiding);
                   });
        }
    }

    float move_x = 0;

    class Jumping : State<MettoruController>
    {
        int animationHash = 0;
        public Jumping() { animationHash = Animator.StringToHash("Jump"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.gravity.Reset();
            mettoru.jump.Init(15);
            mettoru.move_x = ParabolaCalc.GetHorizonVelocity(mettoru.transform.position, mettoru.jumpTarget.position, 15, mettoru.gravity.GravityScale);
            //mettoru.jumpOverThere.Jump(mettoru.jumpTarget.position, 88, mettoru.gravity.GravityScale, () => { Debug.Log("error jump"); });
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.jump.UpdateVelocity(mettoru.gravity.GravityScale);
            mettoru.exRb.velocity += mettoru.jump.CurrentVelocity;
            mettoru.exRb.velocity += new Vector2(mettoru.move_x, 0);

            if (mettoru.jump.CurrentSpeed == 0)
            {
                mettoru.stateMachine.TransitState((int)StateID.JumpFloating);
            }
        }
    }

    class JumpFloating : State<MettoruController>
    {
        int animationHash = 0;
        public JumpFloating() { animationHash = Animator.StringToHash("Float"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            //mettoru.jumpOverThere.Jump(mettoru.jumpTarget.position, 88, mettoru.gravity.GravityScale, () => { Debug.Log("error jump"); });
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            if (mettoru.onTheGround.CheckBottomHit())
            {
                mettoru.stateMachine.TransitState((int)StateID.Idle);
            }
            else
            {
                mettoru.gravity.UpdateVelocity();
                mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
                mettoru.exRb.velocity += new Vector2(mettoru.move_x, 0);
            }
        }
    }
}
