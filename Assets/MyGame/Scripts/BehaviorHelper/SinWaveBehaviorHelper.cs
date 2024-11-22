using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サイン波挙動
/// </summary>
public static class SinWaveBehaviorHelper
{
    /// <summary>
    /// サイン波の挙動(Y軸)
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <param name="frequency">1秒間にSin波が何回往復するか</param>
    /// <param name="magnitude">サイン波の振幅</param>
    /// <returns></returns>
    static public Vector2 GetVelocityYAxis(float time, float frequency, float magnitude)
    {
        var radian = time * 2 * Mathf.PI * frequency;

        // Sin(2πft)をtで微分すると2πf・Cos(2πft)
        float sinValue = 2 * Mathf.PI * frequency * Mathf.Cos(radian) * magnitude;
        Vector2 delta = new Vector2(0, sinValue);

        return delta;
    }

    /// <summary>
    /// サイン波の挙動(X軸)
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <param name="frequency">1秒間にSin波が何回往復するか</param>
    /// <param name="magnitude">サイン波の振幅</param>
    /// <returns></returns>
    static public Vector2 GetVelocityXAxis(float time, float frequency, float magnitude)
    {
        var radian = time * 2 * Mathf.PI * frequency;

        // Sin(2πft)をtで微分すると2πf・Cos(2πft)
        float sinValue = 2 * Mathf.PI * frequency * Mathf.Cos(radian) * magnitude;
        Vector2 delta = new Vector2(sinValue, 0);
        return delta;
    }
}
