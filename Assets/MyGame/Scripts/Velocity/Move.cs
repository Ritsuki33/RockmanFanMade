using System;
using UnityEngine;

[Serializable]
public class Move{
    [SerializeField] float _speed = 5.0f;

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

    public void OnUpdate(Vector2 dir, InputType input)
    {
        dir = (((dir.x > 0) ? 1 : -1) * dir);

        switch (input)
        {
            case InputType.None:
                velocity = Vector2.zero;
                break;
            case InputType.Left:
                    velocity = dir * -_speed;
                break;
            case InputType.Right:
                    velocity = dir * _speed;
                break;
            default:
                break;
        }
    }
}
