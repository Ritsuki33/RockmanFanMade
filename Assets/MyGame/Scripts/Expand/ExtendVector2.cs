using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static public class ExtendVector2
{
    /// <summary>
    /// xy軸系の座標からradianだけ回転したx_dash-y_dash軸上での座標に変換できるVector2
    /// </summary>
    public static Vector2 AxisRotate(this Vector2 v,float angle)
    {
        float radian = Mathf.Deg2Rad * angle;

        Matrix4x4 matrix = new Matrix4x4();
        matrix.m00 = Mathf.Cos(radian);  // 1行1列目の要素
        matrix.m01 = Mathf.Sin(radian); // 1行2列目の要素
        matrix.m10 = -Mathf.Sin(radian);  // 2行1列目の要素
        matrix.m11 = Mathf.Cos(radian); // 2行2列目の要素

        // ===x_dash.y_dash軸(xy軸をangleだけ回転させた軸)==
        Vector2 v_dash = matrix * v;

        return v_dash;
    }

    /// <summary>
    /// 座標(0,0)を中心として回転後の座標
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 PositionRotate(this Vector2 v,float angle)
    {
        float radian = Mathf.Deg2Rad * angle;

        Matrix4x4 matrix = new Matrix4x4();
        matrix.m00 = Mathf.Cos(radian);  // 1行1列目の要素
        matrix.m01 = -Mathf.Sin(radian); // 1行2列目の要素
        matrix.m10 = Mathf.Sin(radian);  // 2行1列目の要素
        matrix.m11 = Mathf.Cos(radian); // 2行2列目の要素

        return matrix * v;
    }
    /// <summary>
    /// ある点を中心として回転させた座標
    /// </summary>
    /// <param name="v"></param>
    /// <param name="center"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 PositionRotate(this Vector2 v,Vector2 center,float angle)
    {
        // centerを(0,0)に持ってきたときのv_dash
        Vector2 v_dash = v - center;

        Vector2 v_dash_dash= v_dash.PositionRotate(angle);

        return v_dash_dash + center;
    }

    /// <summary>
    /// 垂直なベクトルを求める（単位ベクトル）
    /// </summary>
    /// <param name="v"></param>
    /// <param name="center"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 Verticalize(this Vector2 v)
    {
        Vector2 direction=new Vector2(v.y, -v.x);

        return direction.normalized;
    }

    public static void SetMagnitude(this Vector2 v,float scalar)
    {
        Vector2 e = v.normalized;

        v = e * scalar;
    }

    public static Vector2 DirectionVector(this Vector2 v,Vector2 dir)
    {
        Vector2 vector = Vector2.Dot(v, dir) * dir;

        return vector;
    }
}
