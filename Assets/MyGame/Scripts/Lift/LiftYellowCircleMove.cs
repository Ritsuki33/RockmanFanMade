using UnityEngine;

public class LiftYellowCircleMove : PhysicalObject
{
    [SerializeField] Transform  _center;
    [SerializeField] float speed = 2.0f;   // 加速、減速(v/s)
    StateMachine<LiftYellowCircleMove> _stateMachine = new StateMachine<LiftYellowCircleMove>();

    bool reverse = false;

    protected override void Awake()
    {
        _stateMachine.AddState(0, new Move());
    }

    protected override void Init()
    {
        _stateMachine.TransitReady(0);
        base.Init();
    }
    
    protected override void OnFixedUpdate()
    {
        _stateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        _stateMachine.Update(this);
    }

    public void Setup(Transform center)
    {
        _center = center;
    }

    class Move : State<LiftYellowCircleMove, Move>
    {
        float radian = 0;
        float radius = 0;
        protected override void Enter(LiftYellowCircleMove lift, int preId, int subId)
        {
            radian = CircleBehaviorHelper.GetRadian(lift._center.position, lift.transform.position);
            radius = Vector2.Distance(lift._center.position, lift.transform.position);
        }

        protected override void FixedUpdate(LiftYellowCircleMove lift)
        {
            lift.rb.SetVelocty(CircleBehaviorHelper.GetStrobe(lift._center.position, radius, radian));

            radian +=lift.speed * Time.fixedDeltaTime;

            // 0から2πの範囲でループさせる
            if(radian> 2 * Mathf.PI) radian = Mathf.Repeat(radian, 2 * Mathf.PI);
        }
    }

    public void Reverse()
    {
        reverse = !reverse;
    }
}
