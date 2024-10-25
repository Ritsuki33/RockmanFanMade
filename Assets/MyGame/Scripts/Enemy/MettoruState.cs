using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MettoruController 
{
    class Idle : ExRbState<MettoruController, Idle>
    {
        AmbiguousTimer timer = new AmbiguousTimer();
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }
        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            timer.Start(0.5f, 2.0f);
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation(animationHash))
            {
                mettoru.TurnToTarget(mettoru.Player.transform.position);

                mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        if (Probability.GetBoolean(0.35f))
                        {
                            mettoru.TransitReady((int)StateID.Hide);
                        }
                    });
            }

            timer.MoveAheadTime(Time.deltaTime,
                   () =>
                   {
                       if (mettoru.walk)
                       {
                           mettoru.TransitReady((int)StateID.Walk);
                       }
                       else
                       {
                           mettoru.TransitReady((int)StateID.Hide);
                       }
                       //mettoru.TransitReady((int)StateID.Jump);

                   });
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Damaged(collision);
        }
    }

    class Walk : ExRbState<MettoruController, Walk>
    {
        AmbiguousTimer timer = new AmbiguousTimer();
        int animationHash = 0;
        public Walk() { animationHash = Animator.StringToHash("Walk"); }
        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.TurnToTarget(mettoru.Player.transform.position);
            timer.Start(0.5f, 2.0f);
        }

        protected override void FixedUpdate(MettoruController mettoru)
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

        protected override void Update(MettoruController mettoru)
        {
            mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                        if (Probability.GetBoolean(0.35f))
                        {
                            mettoru.TransitReady((int)StateID.Hide);
                        }
                    });

            timer.MoveAheadTime(Time.deltaTime,
                  () =>
                  {
                      mettoru.TransitReady((int)StateID.Hide);
                  });
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Damaged(collision);
        }
    }

    class Hide : ExRbState<MettoruController, Hide>
    {
        int animationHash = 0;
        public Hide() { animationHash = Animator.StringToHash("Hide"); }

        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation(animationHash))
            {
                mettoru.TransitReady((int)StateID.Hiding);
            }
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Defense(collision);
        }
       
    }

    class Hiding : ExRbState<MettoruController, Hiding>
    {
        int animationHash = 0;
        AmbiguousTimer timer = new AmbiguousTimer();
        public Hiding() { animationHash = Animator.StringToHash("Hiding"); }

        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            timer.Start(1, 3);
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(MettoruController mettoru)
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
                                mettoru.TransitReady((int)StateID.Appear);
                            }
                        ),
                            (0, () =>
                            {
                                mettoru.TransitReady((int)StateID.LookIn);
                            }
                        ));
                    });
            }
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Defense(collision);
        }
    }

    class Appear : ExRbState<MettoruController, Appear>
    {
        int animationHash = 0;
        AmbiguousTimer timer=new AmbiguousTimer();
        public Appear() { animationHash = Animator.StringToHash("Appear"); }

        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.Fire();
            timer.Start(1, 3);
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation(animationHash))
            {
                if (mettoru.walk)
                {
                    mettoru.TransitReady((int)StateID.Walk);
                }
                else
                {
                    mettoru.TransitReady((int)StateID.Idle);
                }
            }
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Damaged(collision);
        }
    }

    class LookIn : ExRbState<MettoruController, LookIn>
    {
        int animationHash = 0;
        public LookIn() { animationHash = Animator.StringToHash("LookIn"); }
        AmbiguousTimer timer = new AmbiguousTimer();

        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.Fire();
            timer.Start(0.5f, 2.0f);
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        protected override void Update(MettoruController mettoru)
        {
            if (!mettoru._animator.IsPlayingCurrentAnimation(animationHash))
            {
                mettoru.TurnToTarget(mettoru.Player.transform.position);

                mettoru.raycastSensor.SearchForTargetEnter((mettoru.IsRight) ? Vector2.right : Vector2.left,
                    (hit) =>
                    {
                            mettoru.TransitReady((int)StateID.Hiding);
                    });
            }

            timer.MoveAheadTime(Time.deltaTime,
                   () =>
                   {
                       mettoru.TransitReady((int)StateID.Hiding);
                   });
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Damaged(collision);
        }
    }

    float move_x = 0;

    class Jumping : ExRbState<MettoruController, Jumping>
    {
        int animationHash = 0;
        public Jumping() { animationHash = Animator.StringToHash("Jump"); }

        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.gravity.Reset();
            mettoru.jump.Init(15);
            mettoru.move_x = ParabolaCalc.GetHorizonVelocity(mettoru.transform.position, mettoru.jumpTarget.position, 15, mettoru.gravity.GravityScale);
            //mettoru.jumpOverThere.Jump(mettoru.jumpTarget.position, 88, mettoru.gravity.GravityScale, () => { Debug.Log("error jump"); });
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.jump.UpdateVelocity(mettoru.gravity.GravityScale);
            mettoru.exRb.velocity += mettoru.jump.CurrentVelocity;
            mettoru.exRb.velocity += new Vector2(mettoru.move_x, 0);

            if (mettoru.jump.CurrentSpeed == 0)
            {
                mettoru.TransitReady((int)StateID.JumpFloating);
            }
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Damaged(collision);
        }
    }

    class JumpFloating : ExRbState<MettoruController, JumpFloating>
    {
        int animationHash = 0;
        public JumpFloating() { animationHash = Animator.StringToHash("Float"); }

        protected override void Enter(MettoruController mettoru, int preId, int subId)
        {
            mettoru._animator.Play(animationHash);
            //mettoru.jumpOverThere.Jump(mettoru.jumpTarget.position, 88, mettoru.gravity.GravityScale, () => { Debug.Log("error jump"); });
        }

        protected override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
            mettoru.exRb.velocity += new Vector2(mettoru.move_x, 0);
        }

        protected override void OnBottomHitStay(MettoruController mettoru, RaycastHit2D hit)
        {
                mettoru.TransitReady((int)StateID.Idle);
        }

        protected override void OnTriggerEnter(MettoruController mettoru, RockBusterDamage collision)
        {
            mettoru.Damaged(collision);
        }
    }
}
