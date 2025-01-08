using System;
using UnityEngine;

[Serializable]
public class Jump
{
    [SerializeField] float maxSpeed = 15;

    float currentSpeed = 0;

    
    public Vector2 CurrentVelocity => Vector2.up * currentSpeed;
    public float CurrentSpeed => currentSpeed;

    public void Init()
    {
        this.currentSpeed = maxSpeed;
    }

    public void Init(float maxSpeed)
    {
        this.currentSpeed = maxSpeed;
        this.maxSpeed = maxSpeed;
    }

    public void OnUpdate(float gravity)
    {
        currentSpeed -= gravity;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    public void SetSpeed(float val)
    {
        this.currentSpeed = val;
    }
}
