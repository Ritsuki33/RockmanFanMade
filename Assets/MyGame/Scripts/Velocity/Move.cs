using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : BaseExRbHit
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


    protected override void OnRightHitStay(RaycastHit2D hit)
    {
        if (velocity.x > 0)
        {
            velocity.x = 0;
            rightHit = true;
        }
        else
        {
            rightHit = false;
        }
    }

    protected override void OnLeftHitStay(RaycastHit2D hit)
    {
        if (velocity.x < 0)
        {
            velocity.x = 0;
            leftHit = true;
        }
        else
        {
            leftHit = false;
        }
    }

    protected override void OnRightHitExit(RaycastHit2D hit)
    {
        rightHit = false;
    }

    protected override void OnLeftHitExit(RaycastHit2D hit)
    {
        leftHit = false;
    }
}
