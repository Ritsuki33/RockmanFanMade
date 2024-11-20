using UnityEngine;

/// <summary>
/// 円運動挙動系ヘルパー関数
/// </summary>
public static class CircleBehavior
{
    /// <summary>
    /// 中心からある座標のおける角度(ラジアン)を習得
    /// </summary>
    /// <param name="center"></param>
    /// <param name="currentPos"></param>
    /// <returns></returns>
    static public  float GetRadian(Vector2 center, Vector2 targetPos)
    {
        // 中心角をラジアンで求める
        float angle = Mathf.Atan2(targetPos.y - center.y, targetPos.x - center.x);
        return angle;
    }

    /// <summary>
    /// 円運動
    /// </summary>
    /// <param name="center"></param>
    /// <param name="currentPos"></param>
    /// <param name="deltaTime"></param>
    /// <param name="radius"></param>
    /// <param name="speed"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    static public Vector2 GetVelocityCircle(Transform center, Transform own, float deltaTime, float radius, float speed, ref float angle)
    {
        return EllipseBehavior.GetVelocityCircle(center, own, deltaTime, radius, radius, speed, ref angle);
    }


}
