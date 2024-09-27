using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class StageObject : MonoBehaviour
{
    [SerializeField,Header("マテリアル制御")]  MaterialController materialController = default;
    [SerializeField,Header("正方向")] bool forwardIsRight = false;

    public bool IsRight => (forwardIsRight) ? this.transform.localScale.x > 0 : this.transform.localScale.x < 0;

    // 正方向の単位値
    float forwardDirection=> ((forwardIsRight) ? 1 : -1);
    
    /// <summary>
    /// 方向を決定
    /// </summary>
    /// <param name="isRight"></param>
    public void TurnTo(bool isRight)
    {
        Vector3 localScale = this.transform.localScale;
        if (isRight)
        {
            localScale.x = Mathf.Abs(localScale.x) * forwardDirection;
        }
        else
        {
            localScale.x = Mathf.Abs(localScale.x) * -forwardDirection;
        }

        transform.localScale = localScale;

    }

    /// <summary>
    /// ターゲットの方を振り向き
    /// </summary>
    public void TurnToTarget(Vector2 targetPos)
    {
        Vector3 localScale = transform.localScale;
        if (transform.position.x > targetPos.x)
        {
            // 左を向かせる
            localScale.x = Mathf.Abs(localScale.x) * -forwardDirection;
            transform.localScale = localScale;
        }
        else
        {
            // 右を向かせる
            localScale.x = Mathf.Abs(localScale.x) * forwardDirection;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// 現在の方向から逆に振り返る
    /// </summary>
    public void TurnFace()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /// <summary>
    /// floatのセット
    /// </summary>
    /// <param name="propertyID"></param>
    /// <param name="val"></param>
    public void SetMaterialParam(int propertyID, float val) => materialController.SetFloat(propertyID, val);

    /// <summary>
    /// カラーのセット
    /// </summary>
    /// <param name="propertyID"></param>
    /// <param name="color"></param>
    public void SetMaterialParam(int propertyID, Color color) => materialController.SetColor(propertyID, color);
}
