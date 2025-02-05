using System;
using UnityEngine;

[Serializable]
public class GroundChecker
{
    [SerializeField] float length = 1.0f;
    [SerializeField] LayerMask groundLayer = default;


    public bool CheckGround(Vector2 position, Vector2 boxSize, bool isRight)
    {
        Vector2 basePosition = position + new Vector2((isRight) ? boxSize.x / 2 : -boxSize.x / 2, -boxSize.y / 2);
        RaycastHit2D hit = Physics2D.Raycast(basePosition, Vector2.down, length, groundLayer);

        Debug.DrawRay(basePosition, Vector2.down * length, Color.yellow);

        return hit;
    }
}
