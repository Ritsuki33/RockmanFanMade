using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 放物線運動の速度計算クラス
/// </summary>
public static class ParabolaCalc
{
    /// <summary>
    /// ジャンプ、重力、相手との高さに応じて水平方向を速度を計算
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetPos"></param>
    /// <param name="jumpSpeed"></param>
    /// <param name="gravityScale"></param>
    /// <returns></returns>
    public static float GetHorizonVelocity(Vector2 pos,Vector2 targetPos, float jumpSpeed, float gravityScale)
    {
        // 高さ = 初速 * 時間 - (重力加速度*時間の２乗) / 2 で算出可能
        // これから時間について解く
        float height = targetPos.y - pos.y;
        float v0 = jumpSpeed;
        float gravity = gravityScale / Time.fixedDeltaTime;
        float D = v0 * v0 - 2 * gravityScale * height;
        float time = 0;
        if (D >= 0)
        {
            // 高さがhになるまでの時間(解は２つ求まるが、そのうちの大きいほうをとる)
            time = (v0 + Mathf.Sqrt(v0 * v0 - 2 * gravity * height)) / gravity;
        }
        else
        {
            // 高さは0として考える
            time = 2 * v0 / gravityScale;
        }
        // 求めた時間から水平方向の速度を求める
        float width = targetPos.x - pos.x;
        float v_x = width / time;

        return v_x;
    }

    /// <summary>
    /// ジャンプ、重力、水平方向を速度を計算
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetPosX"></param>
    /// <param name="jumpSpeed"></param>
    /// <param name="gravity"></param>
    /// <returns></returns>
    public static float GetHorizonVelocity(Vector2 pos,float targetPosX, float jumpSpeed, float gravity)
    {
        // 高さ=初速*時間-(重力加速度*時間の２乗)/2 で算出可能
        // これから時間について解く
        float v0 = jumpSpeed;
        float time = 2 * v0 / gravity;
       
        // 求めた時間から水平方向の速度を求める
        float width = targetPosX - pos.x;
        float v_x = width / time;

        return v_x;
    }
}
