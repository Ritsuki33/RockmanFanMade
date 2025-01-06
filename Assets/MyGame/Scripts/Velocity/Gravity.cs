using System;
using UnityEngine;

[Serializable]
public class Gravity
{
    [SerializeField]private float _gravityScale = 1;
    [SerializeField]private float _maxSpeed = 10;

    private float canStopSlope = 45;
    float currentSpeed = default;

    public Vector2 CurrentVelocity => Vector2.down * currentSpeed;

    public float GravityScale => _gravityScale;

    public void OnUpdate()
    {
        currentSpeed += _gravityScale;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, _maxSpeed);
    }

    /// <summary>
    /// 速度を変更する
    /// </summary>
    /// <param name="val"></param>
    public void Reset()
    {
        currentSpeed = 0;
    }

    public void OnGround(Vector2 normal)
    {
        float angle = Vector2.Angle(Vector2.up, normal);
        if (angle < canStopSlope)
        {
            currentSpeed = 0;
        }
    }
}
