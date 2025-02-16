using System;
using UnityEngine;

[Serializable]
public class CameraBoundLimiter
{
    [System.Flags]
    public enum BoundType
    {
        Right = 1 << 0,  // 1
        Left = 1 << 1,  // 2
        Top = 1 << 2,  // 4
        Bottom = 1 << 3,  // 8
    }

    [SerializeField]private BoundType boundType;

    bool CheckBoudType(BoundType type) => (boundType & type) == type;

    public void ForceAdjustPosition(BoxCollider2D boxCollider)
    {
        // カメラの境界をワールド座標で取得
        Vector3 minScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float objectWidth = (boxCollider) ? boxCollider.bounds.extents.x : 0;
        float objectHeight = (boxCollider) ? boxCollider.bounds.extents.y : 0;
        Vector2 offset = (boxCollider) ? boxCollider.offset : Vector2.zero;

        // キャラクターの位置を取得
        Vector3 characterPosition = boxCollider.transform.position;

        // コライダーのオフセットを考慮して、カメラの外に出ないように位置を制限
        characterPosition.x = Mathf.Clamp(characterPosition.x, (CheckBoudType(BoundType.Left)) ? minScreenBounds.x + objectWidth - offset.x : float.MinValue, (CheckBoudType(BoundType.Right)) ? maxScreenBounds.x - objectWidth - offset.x : float.MaxValue);
        characterPosition.y = Mathf.Clamp(characterPosition.y, (CheckBoudType(BoundType.Bottom)) ? minScreenBounds.y + objectHeight - offset.y : float.MinValue, (CheckBoudType(BoundType.Top)) ? maxScreenBounds.y - objectHeight - offset.y : float.MaxValue);

        // 制限された位置を設定
        boxCollider.transform.position = characterPosition;
    }

    public void ChangeBoundType(BoundType boundType)
    {
        this.boundType = boundType;
    }
}
