using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBlue : PhysicalObject
{
    [SerializeField] LayerMask m_Mask;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] float checkRange = 0.5f;
    [SerializeField] Gravity gravity;
    StateMachine<LiftBlue> m_StateMachine=new StateMachine<LiftBlue>();

    Vector2 BoxCenter => (Vector2)boxCollider.transform.position + boxCollider.offset;
    protected override void Awake()
    {
        m_StateMachine.AddState(0, new Idle());
        m_StateMachine.AddState(1, new Down());
    }

    protected override void Init()
    {
        base.Init();
        m_StateMachine.TransitReady(0);
    }

    protected override void OnFixedUpdate()
    {
        m_StateMachine.FixedUpdate(this);
    }


    class Idle :ExRbState<LiftBlue,Idle>
    {
        protected override void Enter(LiftBlue obj, int preId, int subId)
        {
            obj.gravity.Reset();
            obj.MainAnimator.Play(AnimationNameHash.Idle);
        }
        protected override void FixedUpdate(LiftBlue obj)
        {
            RaycastHit2D hit = Physics2D.BoxCast(obj.BoxCenter, obj.boxCollider.size, 0, Vector2.up, obj.checkRange, obj.m_Mask);

            if (hit)
            {
                obj.m_StateMachine.TransitReady(1);
            }
        }
    }

    class Down : ExRbState<LiftBlue, Idle>
    {
        int animationHash = 0;

        public Down()
        {
            animationHash = Animator.StringToHash("Down");
        }

        protected override void Enter(LiftBlue obj, int preId, int subId)
        {
            obj.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(LiftBlue obj)
        {
            obj.gravity.OnUpdate();
            obj.rb.velocity = obj.gravity.CurrentVelocity;
        }
    }

}