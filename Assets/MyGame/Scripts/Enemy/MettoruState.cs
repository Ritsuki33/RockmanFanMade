using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MettoruController 
{
    class Idle : State<MettoruController>
    {
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }
        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
        }

        public override void FixedUpdate(MettoruController mettoru)
        {
            mettoru.gravity.UpdateVelocity();
            mettoru.exRb.velocity = mettoru.gravity.CurrentVelocity;
        }

        public override void Update(MettoruController mettoru)
        {
            mettoru.TurnFace();
        }

    
    }

    class Hide : State<MettoruController>
    {
        int animationHash = 0;
        AmbiguousTimer timer=new AmbiguousTimer();
        public Hide() { animationHash = Animator.StringToHash("Hide"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            mettoru._animator.Play(animationHash);
            mettoru.invincible = true;
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
                mettoru.TurnFace();
            }

            if (mettoru.defense == null)
            {
                timer.MoveAheadTime(Time.deltaTime,
                    () =>
                    {
                        mettoru.stateMachine.TransitState((int)StateID.Appear);
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
                mettoru.TurnFace();

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
                       mettoru.stateMachine.TransitState((int)StateID.Hide);
                   });
          
           
        }
    }
}
