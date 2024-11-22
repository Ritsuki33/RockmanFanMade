using System;
using UnityEngine;

// 放物線運動（振舞い）を制御するクラス
public static class ParabolicBehaviorHelper
{
    /// <summary>
    /// 砲弾発射
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="angle"></param>
    /// <param name="gravityScale"></param>
    /// <param name="failed"></param>
    public static Vector2 Start(Vector3 targetPosition, Vector2 myPosition, float angle, float gravityScale, Action failed = null)
    {
        bool isRight = targetPosition.x > myPosition.x;

        // 各種距離の算出
        float length_x = Mathf.Abs(targetPosition.x - myPosition.x);
        float length_y = targetPosition.y - myPosition.y;

        // ラジアン変換
        float radian = angle * Mathf.Deg2Rad;

        // 重力加速度
        float gravity = gravityScale / Time.fixedDeltaTime;

        // 角度から速度を計算
        float speed = Mathf.Sqrt(gravity * length_x * length_x / (2 * Mathf.Cos(radian) * Mathf.Cos(radian) * (length_x * Mathf.Tan(radian) - length_y)));

        Vector2 startVec = default;
        if (float.IsNaN(speed))
        {
            failed?.Invoke();
            return startVec;
        }

        if (isRight)
        {
            Vector2 axis = Vector2.right.PositionRotate(angle);

            Vector2 vec = speed * axis;
            startVec = vec;
        }
        else
        {
            Vector2 axis = Vector2.left.PositionRotate(-angle);

            Vector2 vec = speed * axis;
            startVec = vec;
        }

        return startVec;
    }

    public static void FixedUpdate(Rigidbody2D rb, float gravityScale)
    {
        rb.velocity += Vector2.down * gravityScale;
    }
}
