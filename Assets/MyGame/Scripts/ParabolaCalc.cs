using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// �������^���̑��x�v�Z�N���X
/// </summary>
public static class ParabolaCalc
{
    /// <summary>
    /// �W�����v�A�d�́A����Ƃ̍����ɉ����Đ��������𑬓x���v�Z
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetPos"></param>
    /// <param name="jumpSpeed"></param>
    /// <param name="gravityScale"></param>
    /// <returns></returns>
    public static float GetHorizonVelocity(Vector2 pos,Vector2 targetPos, float jumpSpeed, float gravityScale)
    {
        // ���� = ���� * ���� - (�d�͉����x*���Ԃ̂Q��) / 2 �ŎZ�o�\
        // ���ꂩ�玞�Ԃɂ��ĉ���
        float height = targetPos.y - pos.y;
        float v0 = jumpSpeed;
        float gravity = gravityScale / Time.fixedDeltaTime;
        float D = v0 * v0 - 2 * gravityScale * height;
        float time = 0;
        if (D >= 0)
        {
            // ������h�ɂȂ�܂ł̎���(���͂Q���܂邪�A���̂����̑傫���ق����Ƃ�)
            time = (v0 + Mathf.Sqrt(v0 * v0 - 2 * gravity * height)) / gravity;
        }
        else
        {
            // ������0�Ƃ��čl����
            time = 2 * v0 / gravityScale;
        }
        // ���߂����Ԃ��琅�������̑��x�����߂�
        float width = targetPos.x - pos.x;
        float v_x = width / time;

        return v_x;
    }

    /// <summary>
    /// �W�����v�A�d�́A���������𑬓x���v�Z
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetPosX"></param>
    /// <param name="jumpSpeed"></param>
    /// <param name="gravity"></param>
    /// <returns></returns>
    public static float GetHorizonVelocity(Vector2 pos,float targetPosX, float jumpSpeed, float gravity)
    {
        // ����=����*����-(�d�͉����x*���Ԃ̂Q��)/2 �ŎZ�o�\
        // ���ꂩ�玞�Ԃɂ��ĉ���
        float v0 = jumpSpeed;
        float time = 2 * v0 / gravity;
       
        // ���߂����Ԃ��琅�������̑��x�����߂�
        float width = targetPosX - pos.x;
        float v_x = width / time;

        return v_x;
    }
}
