using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フレームレートを疑似的に下げる（カクカクさせる）ためのユーティリティ
/// </summary>
public static class FrameRateUtility
{
    /// <summary>
    /// ベクトルを指定されたステップに丸める
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Vector3 SnapVectorToStep(Vector3 vector, float step)
    {
        vector.x = Mathf.Round(vector.x / step) * step;
        vector.y = Mathf.Round(vector.y / step) * step;
        vector.z = Mathf.Round(vector.z / step) * step;
        return vector;
    }

    /// <summary>
    /// 浮動小数点数を指定されたステップに丸める
    /// </summary>
    /// <param name="value"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static float SnapFloatToStep(float value, float step)
    {
        return Mathf.Round(value / step) * step;
    }
}
