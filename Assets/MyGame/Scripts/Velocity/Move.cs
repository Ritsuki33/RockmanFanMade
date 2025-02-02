using System;
using UnityEngine;

[Serializable]
public class Move{
    [SerializeField] float _maxSpeed = 5.0f;
    [SerializeField] float _accelerate = 5.0f;

    Vector2 velocity;

    public enum InputType
    {
        None,
        Left,
        Right
    }

    bool leftHit;
    bool rightHit;

    public bool Lefthit => leftHit;
    public bool RightHit => rightHit;

    public bool Hit => leftHit || rightHit;
    public Vector2 CurrentVelocity => velocity;

    public void OnUpdate(Vector2 dir, InputType input, float friction = 1)
    {
        dir = (((dir.x > 0) ? 1 : -1) * dir);
        float realAccelerate = _accelerate * friction;

        switch (input)
        {
            case InputType.None:
                float magnitude = velocity.magnitude - realAccelerate;
                if (magnitude < 0) magnitude = 0;
                velocity = velocity.SetMagnitude(magnitude);
                break;
            case InputType.Left:
                velocity -= dir * realAccelerate;
                break;
            case InputType.Right:
                velocity += dir * realAccelerate;
                break;
            default:
                break;
        }

        velocity = velocity.Clamp(0, _maxSpeed);
    }
}
