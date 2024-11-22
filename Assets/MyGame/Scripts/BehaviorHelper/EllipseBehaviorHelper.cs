using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 楕円運動挙動系ヘルパー関数
/// </summary>
public static class EllipseBehaviorHelper
{
    /// <summary>
    /// 楕円運動
    /// </summary>
    /// <param name="center"></param>
    /// <param name="currentPos"></param>
    /// <param name="deltaTime"></param>
    /// <param name="radius_x"></param>
    /// <param name="radius_y"></param>
    /// <param name="speed"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    //static public Vector2 GetVelocityCircle(Transform center, Transform own, float deltaTime, float radius_x, float radius_y, float speed, ref float angle)
    //{
    //    angle += speed * deltaTime;
    //    angle %= (Mathf.PI * 2);
    //    // 角度に応じた新しい位置を計算
    //    float x = center.position.x + Mathf.Cos(angle) * radius_x;
    //    float y = center.position.y + Mathf.Sin(angle) * radius_y;
    //    Vector2 targetPosition = new Vector2(x, y);

    //    // 現在位置から目標位置までのベクトルを計算
    //    Vector2 direction = (targetPosition - (Vector2)own.position);

    //    // 速度ベクトルを設定
    //    return direction / deltaTime;
    //}


    /// <summary>
    /// 楕円運動
    /// </summary>
    /// <param name="center"></param>
    /// <param name="currentPos"></param>
    /// <param name="deltaTime"></param>
    /// <param name="radius_x"></param>
    /// <param name="radius_y"></param>
    /// <param name="speed"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    static public Vector2 GetStrobe(Transform center,float radius_x, float radius_y, float angle )
    {
        // 角度に応じた新しい位置を計算
        float x = center.position.x + Mathf.Cos(angle) * radius_x;
        float y = center.position.y + Mathf.Sin(angle) * radius_y;

        // 速度ベクトルを設定
        return new Vector2(x, y); ;
    }
}
