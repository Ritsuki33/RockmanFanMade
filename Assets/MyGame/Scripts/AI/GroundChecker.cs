using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] ExpandRigidBody exRb = default;
    [SerializeField] float length = 1.0f;
    [SerializeField] LayerMask groundLayer = default;


    Vector2 boxSize => exRb.PhysicalBoxSize;
    public bool CheckGround(bool isRight)
    {
        Vector2 basePosition = (Vector2)this.transform.position + new Vector2((isRight) ? boxSize.x / 2 : -boxSize.x / 2, -boxSize.y / 2);
        RaycastHit2D hit = Physics2D.Raycast(basePosition, Vector2.down, length, groundLayer);

        Debug.DrawRay(basePosition, Vector2.down * length, Color.yellow);

        return hit;
    }
}
