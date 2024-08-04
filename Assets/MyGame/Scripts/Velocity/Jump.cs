using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseExRbHit
{
    [SerializeField] float speed = 15;
    [SerializeField] float decrease = 1;

    float currentSpeed = 0;

    bool isBottomHit = false;

    public Vector2 CurrentVelocity => Vector2.up * currentSpeed;

    public void Init()
    {
        this.currentSpeed = speed;
    }
    public void UpdateVelocity(float gravity)
    {
        currentSpeed -= gravity;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);
    }

    public void Reset()
    {
        this.currentSpeed = 0;
    }

    protected override void OnBottomHitStay(RaycastHit2D hit)
    {
        if (!isBottomHit) currentSpeed = 0;
        isBottomHit = true;
    }

    protected override void OnBottomHitExit(RaycastHit2D hit)
    {
        isBottomHit = false;
    }
}
