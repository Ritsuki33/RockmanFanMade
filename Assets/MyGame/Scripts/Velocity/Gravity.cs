using UnityEngine;

public class Gravity : BaseExRbHit
{
    [SerializeField] float speed = 1;
    [SerializeField] float masSpeed = 10;

    [SerializeField] float canStopSlope = 45;
    float currentSpeed = default;

    public Vector2 CurrentVelocity => Vector2.down * currentSpeed;

    protected override void Awake()
    {
       base.Awake();
    }

    public Vector2 GetVelocity()
    {
        currentSpeed += speed;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, masSpeed);

        return Vector2.down * currentSpeed;
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
