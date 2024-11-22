using UnityEngine;

/// <summary>
/// スプライトが動いている方向を向く
/// </summary>
public static class SpriteRotationBehavior
{
    static public void RotateForward(Transform own, Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.01f) // スプライトが動いている場合のみ回転を更新
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            own.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}