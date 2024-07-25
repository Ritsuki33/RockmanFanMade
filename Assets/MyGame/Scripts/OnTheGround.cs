using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTheGround : MonoBehaviour,IHitEvent
{
    [SerializeField]float offset_y = 0.01f;
    [SerializeField]float check_y = 0.01f;
    [SerializeField] LayerMask physicalLayer = default;

    [SerializeField] BoxCollider2D boxCollider;

    Vector2 CheckSize => new Vector2(this.boxCollider.size.x, check_y);

    Vector2 center => new Vector2(this.transform.position.x, this.transform.position.y + offset_y - CheckSize.y / 2);

    Vector2 topCenter => new Vector2(center.x, center.y + CheckSize.y / 2);

    private RaycastHit2D hit;

    public RaycastHit2D GroundHit => hit;

    bool bottomhit = false;

    //public Vector2 MoveOnTheGround { get; set; }

    private void Awake()
    {
        boxCollider=GetComponent<BoxCollider2D>();
    }

    public void Reset()
    {
        hit = default;
    }
    public bool CheckBottomHit()
    {
        return bottomhit;
    }
    public RaycastHit2D Check()
    {
        hit = Physics2D.BoxCast(topCenter, new Vector2(CheckSize.x, 0.001f), 0, Vector2.down, CheckSize.y, physicalLayer);

        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, CheckSize);

        if (hit) Gizmos.DrawSphere(hit.point, 0.05f);
    }

    public void OnBottomHitStay(RaycastHit2D hit)
    {
        bottomhit = true;
    }
    public void OnBottomHitExit(RaycastHit2D hit)
    {
        bottomhit = false;
    }
}
