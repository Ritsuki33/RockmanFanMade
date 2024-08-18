using UnityEngine;

public class Gravity : BaseExRbHit
{
    [SerializeField] float speed = 1;
    [SerializeField] float maxSpeed = 10;

    [SerializeField] float canStopSlope = 45;
    float currentSpeed = default;

    public Vector2 CurrentVelocity => Vector2.down * currentSpeed;

    public float GravityScale => speed;

    public void UpdateVelocity()
    {
        currentSpeed += speed;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    /// <summary>
    /// 速度を変更する
    /// </summary>
    /// <param name="val"></param>
    public void Reset()
    {
        currentSpeed = 0;
    }

    protected override void OnBottomHitStay(RaycastHit2D hit)
    {
        float angle = Vector2.Angle(Vector2.up, hit.normal);
        if (angle < canStopSlope)
        {
            currentSpeed = 0;
        }
    }
}
