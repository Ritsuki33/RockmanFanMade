using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LiftYellow : PhysicalObject
{
    [SerializeField] Transform[] _liftPoints;
    [SerializeField] float idleTime;
    [SerializeField] float speed = 1;
    StateMachine<LiftYellow> _stateMachine = new StateMachine<LiftYellow>();

    int targetNumber = 0;
    bool reverse = false;

    protected override void Init()
    {
        _stateMachine.AddState(0, new Move());
        _stateMachine.TransitReady(0);

        base.Init();
    }

    public void Setup(Transform[] liftPoints)
    {
        _liftPoints = liftPoints;
    }

    protected override void OnFixedUpdate()
    {
        _stateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        _stateMachine.Update(this);
    }

    class Move : State<LiftYellow, Move>
    {
        protected override void FixedUpdate(LiftYellow lift)
        {
            if (lift._liftPoints.Length == 0) return;

            Vector2 TargetPos = lift._liftPoints[lift.targetNumber].position;
            Vector2 currentPos = lift.transform.position;

            Vector2 direction = TargetPos - currentPos;
            direction = direction.normalized;

            Vector2 movement = direction * lift.speed * Time.fixedDeltaTime;

            Vector2 nextPos = currentPos + movement;

            // 目標地点を超えたか判定
            if (TargetPos.IsBetween(currentPos,nextPos))
            {
                lift.rb.SetVelocty(lift._liftPoints[lift.targetNumber].position);
                lift.targetNumber = lift.Adjust(lift.targetNumber + (lift.reverse ? -1 : 1), lift._liftPoints.Length);
            }
            else
            {
                lift.rb.velocity = direction * lift.speed;
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
            int end= i + 1;
            if (end == _liftPoints.Length) end = 0;
            Gizmos.DrawLine(_liftPoints[start].position, _liftPoints[end].position);
        }
    }
}
