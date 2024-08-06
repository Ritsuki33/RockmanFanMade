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
        float time = 0f;
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

        public override void Exit(MettoruController obj, int nextId)
        {
            time = 0;
        }
    }

    class Hide : State<MettoruController>
    {
        int animationHash = 0;
        public Hide() { animationHash = Animator.StringToHash("Hide"); }

        float time = 2;
        public override void Enter(MettoruController mettoru, int preId)
        {
            time = 2;
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
                mettoru.TurnFace();
            }

            time -= Time.deltaTime;
            if (time <0)
            {
                mettoru.stateMachine.TransitState((int)StateID.Appear);
            }
        }
    }

    class Appear : State<MettoruController>
    {
        int animationHash = 0;
        float time = 2;
        public Appear() { animationHash = Animator.StringToHash("Appear"); }

        public override void Enter(MettoruController mettoru, int preId)
        {
            time = 2;
            mettoru._animator.Play(animationHash);
            mettoru.Fire();
            mettoru.invincible = false;
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

            time -= Time.deltaTime;
            if (time < 0)
            {
                mettoru.stateMachine.TransitState((int)StateID.Hide);
            }
           
        }
    }
}
