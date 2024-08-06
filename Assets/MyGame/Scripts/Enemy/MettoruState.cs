using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MettoruController 
{
    private bool invincible = false;
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
            time += Time.deltaTime;

            if (time > 1f)
            {
                mettoru.stateMachine.TransitState((int)StateID.Hide);
            }
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
            if (!mettoru._animator.IsPlaying())
            {
                mettoru.stateMachine.TransitState((int)StateID.Appear);
            }
        }
    }

    class Appear : State<MettoruController>
    {
        int animationHash = 0;
        public Appear() { animationHash = Animator.StringToHash("Appear"); }

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
            if (!mettoru._animator.IsPlaying())
            {
                mettoru.stateMachine.TransitState((int)StateID.Hide);
            }
        }
    }
}
