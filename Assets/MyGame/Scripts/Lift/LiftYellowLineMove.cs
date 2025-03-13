using System;
using Unity.VisualScripting;
using UnityEngine;

public class LiftYellowLineMove : PhysicalObject
{
    [SerializeField] Transform[] _liftPoints;
    [SerializeField] float maxSpeed = 5.0f;     // 最大速度 (v)
    [SerializeField] float accelerate = 2.0f;   // 加速、減速(v/s)
    StateMachine<LiftYellowLineMove> _stateMachine = new StateMachine<LiftYellowLineMove>();

    int targetNumber = 0;
    bool reverse = false;

    float currentSpeed = 0;
    protected override void Awake()
    {
        _stateMachine.AddState(0, new Move());
    }

    protected override void Init()
    {
        _stateMachine.TransitReady(0);
        base.Init();
    }

    public void Setup(Transform[] liftPoints, float maxSpeed, float accelerate)
    {
        _liftPoints = liftPoints;
    }

    protected override void OnFixedUpdate()
    {
        _stateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _stateMachine.Update(this);
    }

    class Move : State<LiftYellowLineMove, Move>
    {
        protected override void FixedUpdate(LiftYellowLineMove lift)
        {
            if (lift._liftPoints.Length == 0) return;

            Vector2 TargetPos = lift._liftPoints[lift.targetNumber].position;
            Vector2 currentPos = lift.transform.position;

            Vector2 direction = TargetPos - currentPos;
            direction = direction.normalized;

            float distance = Vector2.Distance(TargetPos, currentPos);

            if (lift.accelerate > 0)
            {
                // 制動距離 = 速度 * 速度 / ( 2 * 減速度 ) 
                float brakingDistance = (lift.accelerate == 0) ? 0 : lift.currentSpeed * lift.currentSpeed / (2 * lift.accelerate);

                if (distance < brakingDistance)
                {
                    // 速度増加分＝ 加速度 * 時間（１F）
                    lift.currentSpeed -= lift.accelerate * Time.fixedDeltaTime;
                }
                else
                {
                    // 速度増加分＝ 加速度 * 時間（１F）
                    lift.currentSpeed += lift.accelerate * Time.fixedDeltaTime;
                }
                lift.currentSpeed = Math.Clamp(lift.currentSpeed, 0, lift.maxSpeed);
            }
            else
            {
                lift.currentSpeed = lift.maxSpeed;
            }

            Vector2 velocity = direction * lift.currentSpeed;

            Vector2 movement = velocity * Time.fixedDeltaTime;
            Vector2 nextPos = currentPos + movement;

            // 目標地点を超えたか判定
            if (TargetPos.IsBetween(currentPos, nextPos))
            {
                lift.rb.SetVelocty(lift._liftPoints[lift.targetNumber].position);
                lift.targetNumber = lift.Adjust(lift.targetNumber + (lift.reverse ? -1 : 1), lift._liftPoints.Length);
                lift.currentSpeed = 0;
            }
            else
            {
                lift.rb.velocity = velocity;
            }
        }
    }

    public void Reverse()
    {
        reverse = !reverse;
        targetNumber = Adjust(targetNumber + (reverse ? -1 : 1), _liftPoints.Length);
    }

    private int Adjust(int i, int length)
    {
        int adjust = i;

        if (adjust < 0) adjust += length;

        adjust %= length;

        return adjust;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _liftPoints.Length; i++)
        {
            int start = i;
            int end = i + 1;
            if (end == _liftPoints.Length) end = 0;
            Gizmos.DrawLine(_liftPoints[start].position, _liftPoints[end].position);
        }
    }
}
