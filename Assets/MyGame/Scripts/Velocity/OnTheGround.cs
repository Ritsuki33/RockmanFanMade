using System;
using UnityEngine;

[Serializable]
public class OnTheGround
{
    [SerializeField] float offset_y = 0.01f;
    [SerializeField] float check_y = 0.01f;
    [SerializeField] LayerMask physicalLayer = default;

    private RaycastHit2D hit;

    public RaycastHit2D GroundHit => hit;

    public void Reset()
    {
        hit = default;
    }

    public RaycastHit2D Check(Vector2 position, float boxSize_x)
    {
        Vector2 checkSize = new Vector2(boxSize_x, check_y);
        Vector2 center = new Vector2(position.x, position.y + offset_y - checkSize.y / 2);

        Vector2 topCenter = new Vector2(center.x, center.y + checkSize.y / 2);
        hit = Physics2D.BoxCast(topCenter, new Vector2(checkSize.x, 0.001f), 0, Vector2.down, checkSize.y, physicalLayer);

        return hit;
    }

    public void OnDrawGizmos(Vector2 position, float boxSize_x)
    {
        Vector2 checkSize = new Vector2(boxSize_x, check_y);
        Vector2 center = new Vector2(position.x, position.y + offset_y - checkSize.y / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, checkSize);

        if (hit) Gizmos.DrawSphere(hit.point, 0.05f);
    }
}
