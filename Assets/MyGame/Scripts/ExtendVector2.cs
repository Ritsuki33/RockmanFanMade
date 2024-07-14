using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static public class ExtendVector2
{
    /// <summary>
    /// xy���n�̍��W����radian������]����x_dash-y_dash����ł̍��W�ɕϊ��ł���Vector2
    /// </summary>
    public static Vector2 AxisRotate(this Vector2 v,float angle)
    {
        float radian = Mathf.Deg2Rad * angle;

        Matrix4x4 matrix = new Matrix4x4();
        matrix.m00 = Mathf.Cos(radian);  // 1�s1��ڂ̗v�f
        matrix.m01 = Mathf.Sin(radian); // 1�s2��ڂ̗v�f
        matrix.m10 = -Mathf.Sin(radian);  // 2�s1��ڂ̗v�f
        matrix.m11 = Mathf.Cos(radian); // 2�s2��ڂ̗v�f

        // ===x_dash.y_dash��(xy����angle������]��������)==
        Vector2 v_dash = matrix * v;

        return v_dash;
    }

    /// <summary>
    /// ���W(0,0)�𒆐S�Ƃ��ĉ�]��̍��W
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 PositionRotate(this Vector2 v,float angle)
    {
        float radian = Mathf.Deg2Rad * angle;

        Matrix4x4 matrix = new Matrix4x4();
        matrix.m00 = Mathf.Cos(radian);  // 1�s1��ڂ̗v�f
        matrix.m01 = -Mathf.Sin(radian); // 1�s2��ڂ̗v�f
        matrix.m10 = Mathf.Sin(radian);  // 2�s1��ڂ̗v�f
        matrix.m11 = Mathf.Cos(radian); // 2�s2��ڂ̗v�f

        return matrix * v;
    }
    /// <summary>
    /// ����_�𒆐S�Ƃ��ĉ�]���������W
    /// </summary>
    /// <param name="v"></param>
    /// <param name="center"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 PositionRotate(this Vector2 v,Vector2 center,float angle)
    {
        // center��(0,0)�Ɏ����Ă����Ƃ���v_dash
        Vector2 v_dash = v - center;

        Vector2 v_dash_dash= v_dash.PositionRotate(angle);

        return v_dash_dash + center;
    }

    /// <summary>
    /// �����ȃx�N�g�������߂�i�P�ʃx�N�g���j
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
