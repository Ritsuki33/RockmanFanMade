using System;
using UnityEngine;

public static class BezierCurveBeheviorHelper
{

    //public static Vector2 GetVelocity(Vector2 curPos, float timeElapsed, float duration, Transform a, Transform b, Transform c, Action callback = null)
    //{

    //    // 移動時間を計算
    //    float t = timeElapsed / duration;

    //    // tが1を超えたらコールバック呼び出し(コールバックがない場合は無視)
    //    if (callback != null && t > 1f)
    //    {
    //        t = 1f;
    //        callback?.Invoke();
    //    }

    //    // 2次ベジェ曲線の計算
    //    Vector2 position = Mathf.Pow(1 - t, 2) * a.position
    //                     + 2 * (1 - t) * t * b.position
    //                     + Mathf.Pow(t, 2) * c.position;

    //    Vector2 movement = position - curPos;

    //    return movement / Time.fixedDeltaTime;
    //}

    public static Vector2 GetStrobe(Vector2 curPos, Vector3  a, Vector3 b, Vector3 c,float progress)
    {

        // 2次ベジェ曲線の計算
        Vector2 position = Mathf.Pow(1 - progress, 2) * a
                         + 2 * (1 - progress) * progress * b
                         + Mathf.Pow(progress, 2) * c;

        return position;
    }
}