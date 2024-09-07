using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;

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

    public void UpdateVelocity(Vector2 dir, InputType input)
    {
        dir = (((dir.x > 0) ? 1 : -1) * dir);

        switch (input)
        {
            case InputType.None:
                velocity = Vector2.zero;
                break;
            case InputType.Left:
                    velocity = dir * -speed;
                break;
            case InputType.Right:
                    velocity = dir * speed;
                break;
            default:
                break;
        }
    }
}
