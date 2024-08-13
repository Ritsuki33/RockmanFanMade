using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseExRbHit
{
    [SerializeField] float maxSpeed = 15;

    float currentSpeed = 0;

    bool isBottomHit = false;
    
    public Vector2 CurrentVelocity => Vector2.up * currentSpeed;
    public float CurrentSpeed => currentSpeed;

    public void Init()
    {
        this.currentSpeed = maxSpeed;
    }

    public void Init(int maxSpeed)
    {
        this.currentSpeed = maxSpeed;
    }

    public void UpdateVelocity(float gravity)
    {
        currentSpeed -= gravity;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    public void SetSpeed(float val)
    {
        this.currentSpeed = val;
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
