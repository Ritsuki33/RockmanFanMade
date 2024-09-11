using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 円運動挙動
/// </summary>
public static class CircleBehavior
{
    static public Vector2 GetVelocityCircle(float time, float frequency, float magnitude_x,float magnitude_y)
    {
        // Sin(2πft)をtで微分すると2πf・Cos(2πft)
        float vel_x = -2 * Mathf.PI * frequency * Mathf.Sin(time * 2 * Mathf.PI * frequency) * magnitude_x;
        float vel_y = 2 * Mathf.PI * frequency * Mathf.Cos(time * 2 * Mathf.PI * frequency) * magnitude_y;
        Vector2 delta = new Vector2(vel_x, vel_y);
        return delta;
    }
}
