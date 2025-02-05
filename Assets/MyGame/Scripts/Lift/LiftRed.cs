using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftRed : PhysicalObject
{
    [SerializeField] LayerMask m_Mask;
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] float idleSpeed = 5f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float accelerate = 15f;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] float checkRange = 0.5f;
    StateMachine<LiftRed> m_StateMachine = new StateMachine<LiftRed>();

    Vector2 BoxCenter => (Vector2)boxCollider.transform.position + boxCollider.offset + new Vector2(0, boxCollider.size.y / 2);
    Vector2 BoxSize => new Vector2(boxCollider.size.x, 0.001f);

    protected override void Awake()
    {
        m_StateMachine.AddState(0, new Idle());
        m_StateMachine.AddState(1, new Up());
        m_StateMachine.AddState(2, new UpIdle());

        base.Init();
        m_StateMachine.TransitReady(0);

        this.transform.position = start.position;
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

    public void Setup(Transform start,Transform end,float idleSpeed,float maxSpeed,float accelerate)
    {
        this.start = start;
        this.end = end;
        this.idleSpeed = idleSpeed;
        this.maxSpeed = maxSpeed;
        this.accelerate = accelerate;
    }

    class Idle : State<LiftRed, Idle>
    {
        protected override void Enter(LiftRed obj, int preId, int subId)
        {
            obj.MainAnimator.Play(AnimationNameHash.Idle);
        }

        protected override void FixedUpdate(LiftRed obj)
        {
            var currentPos = obj.transform.position;

            Vector2 start = obj.start.position;
            Vector2 end = obj.end.position;
            Vector2 direction = start - end;
            direction = direction.normalized;

            var nextPos = (Vector2)obj.transform.position + direction * obj.idleSpeed * Time.fixedDeltaTime;

            if (start.IsBetween(currentPos, nextPos))
            {
                obj.rb.SetVelocty(start);
            }
            else
            {
                obj.rb.velocity = direction * obj.idleSpeed;
            }

            RaycastHit2D hit = Physics2D.BoxCast(obj.BoxCenter, obj.BoxSize, 0, Vector2.up, obj.checkRange, obj.m_Mask);

            if (hit)
            {
                obj.m_StateMachine.TransitReady(1);
            }
        }
    }

    class Up : State<LiftRed, Idle>
    {
        int animationHash = 0;
        float curSpeed = 0;
        public Up()
        {
            animationHash = Animator.StringToHash("Up");
        }

        protected override void Enter(LiftRed obj, int preId, int subId)
        {
            curSpeed = 0;
            obj.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(LiftRed obj)
        {
            Vector2 currentPos = obj.transform.position;

            Vector2 start = obj.start.position;
            Vector2 end = obj.end.position;
            Vector2 direction = end - start;
            direction = direction.normalized;

            curSpeed += obj.accelerate * Time.fixedDeltaTime;
            curSpeed = Mathf.Clamp(curSpeed, 0, obj.maxSpeed);

            var nextPos = currentPos + direction * curSpeed * Time.fixedDeltaTime;

            if (end.IsBetween(currentPos, nextPos))
            {
                obj.rb.SetVelocty(end);
                obj.m_StateMachine.TransitReady(2);
            }
            else
            {
                obj.rb.velocity = direction * curSpeed;
            }
        }
    }

    class UpIdle:State<LiftRed,UpIdle>
    {
        float time = 0;
        protected override void Enter(LiftRed obj, int preId, int subId)
        {
            obj.rb.velocity = Vector2.zero;
            time = 0;
        }

        protected override void FixedUpdate(LiftRed obj)
        {

            if (time > 1)
            {
                obj.m_StateMachine.TransitReady(0);
            }
            time += Time.fixedDeltaTime;
        }
    }
}
