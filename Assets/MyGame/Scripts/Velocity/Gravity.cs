using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] float gravityScale = 1;
    [SerializeField] float maxSpeed = 10;

    [SerializeField] float canStopSlope = 45;
    float currentSpeed = default;

    public Vector2 CurrentVelocity => Vector2.down * currentSpeed;

    public float GravityScale => gravityScale;

    public void UpdateVelocity()
    {
        currentSpeed += gravityScale;
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

    public void OnGround(Vector2 normal)
    {
        float angle = Vector2.Angle(Vector2.up, normal);
        if (angle < canStopSlope)
        {
            currentSpeed = 0;
        }
    }
}
