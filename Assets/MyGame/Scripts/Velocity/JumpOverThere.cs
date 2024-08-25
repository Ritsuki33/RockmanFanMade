using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JumpOverThere : MonoBehaviour
{

    private Vector2 velocity;
    //bool isBottomHit = false;

    bool onTheGround = false;
    public Vector2 CurrentVelocity => velocity;
    public bool OnTheGround => onTheGround;

    /// <summary>
    /// ターゲット位置にジャンプする
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="gravityScale"></param>
    /// <param name="failed"></param>
    public void Jump(Vector2 targetPosition, float angle,float gravityScale, Action failed)
    {
        bool isRight = targetPosition.x > transform.position.x;
        // 各種距離の算出
        float length_x = Mathf.Abs(targetPosition.x - transform.position.x);
        float length_y = targetPosition.y - transform.position.y;

        // ラジアン変換
        float radian = angle * Mathf.Deg2Rad;

        // 重力加速度
        float gravity = gravityScale / Time.fixedDeltaTime;

        // 角度から速度を計算
        float speed = Mathf.Sqrt(gravity * length_x * length_x / (2 * Mathf.Cos(radian) * Mathf.Cos(radian) * (length_x * Mathf.Tan(radian) - length_y)));

        if (float.IsNaN(speed) || speed <= gravityScale)
        {
            failed?.Invoke();
            return;
        }

        if (isRight)
        {
            Vector2 axis = Vector2.right.PositionRotate(angle);

            Vector2 vec = speed * axis;
            velocity = vec;
        }
        else
        {
            Vector2 axis = Vector2.left.PositionRotate(-angle);

            Vector2 vec = speed * axis;
            velocity = vec;
        }


        onTheGround = false;

    }

    public void UpdateVelocity(float gravityScale)
    {
       velocity += Vector2.down * gravityScale;
    }

    //protected override void OnBottomHitStay(RaycastHit2D hit)
    //{
    //    if (!isBottomHit)
    //    {
    //        onTheGround = true;
    //        velocity = Vector2.zero;
    //    }
    //    isBottomHit = true;
    //}

    //protected override void OnBottomHitExit(RaycastHit2D hit)
    //{
    //    isBottomHit = false;
    //}
}
