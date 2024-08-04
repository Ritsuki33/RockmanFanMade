using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MettoruController 
{

    class Idle : State<MettoruController>
    {

        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }

        public override void Enter(int preId, MettoruController mettoru)
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
          
        }
    }
}
