using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IHitEvent
{
    [SerializeField] float speed = 5.0f;

    [Header("サイドチェック")]
    [SerializeField] float interval_x = 1.0f;
    [SerializeField] Vector2 checkSize = default;

    LayerMask physicalLayer = default;

    // left check
    Vector2 leftCheckCenter => (Vector2)this.transform.position - new Vector2(interval_x / 2, 0);
    Vector2 RightCheckCenter => (Vector2)this.transform.position + new Vector2(interval_x / 2, 0);

    Rigidbody2D rb;

    Vector2 velocity;
    enum Direction
    {
        Left,
        Right
    }

    enum InputType
    {
        None,
        Left,
        Right
    }

    Direction direction;
    InputType input;


    RaycastHit2D leftHit;
    RaycastHit2D rightHit;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicalLayer = Physics2D.GetLayerCollisionMask(this.gameObject.layer);
    }

    public Vector2 GetVelocity(Vector2 dir)
    {
        dir = (((dir.x > 0) ? 1 : -1) * dir);
      
        switch (input)
        {
            case InputType.None:
                velocity = Vector2.zero;
                break;
            case InputType.Left:
                if (!leftHit)
                {
                    velocity = dir * -speed;
                    direction = Direction.Left;
                }
                break;
            case InputType.Right:
                if (!rightHit)
                {
                    velocity = dir * speed;
                    direction = Direction.Right;
                }
                break;
            default:
                break;
        }
      
        CheckSide();
        return velocity;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input = InputType.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            input = InputType.Right;
        }
        else
        {
            input = InputType.None;
        }
    }


    public void OnRightHitStay(RaycastHit2D hit) {
        velocity = Vector2.zero;
    }

    public void OnLeftHitStay(RaycastHit2D hit)
    {
        velocity = Vector2.zero;
    }

    private void CheckSide()
    {
         leftHit = Physics2D.BoxCast(leftCheckCenter, new Vector2(0.001f, checkSize.y), 0, Vector2.left, checkSize.x, physicalLayer);
         rightHit = Physics2D.BoxCast(RightCheckCenter, new Vector2(0.001f, checkSize.y), 0, Vector2.right, checkSize.x, physicalLayer);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;

    //    Vector2 leftCenter = new Vector2(this.transform.position.x - interval_x / 2 - checkSize.x / 2, this.transform.position.y);
    //    Gizmos.DrawWireCube(leftCenter, checkSize);

    //    Vector2 rightCenter = new Vector2(this.transform.position.x + interval_x / 2 + checkSize.x / 2, this.transform.position.y);
    //    Gizmos.DrawWireCube(rightCenter, checkSize);

    //    if (leftHit) Gizmos.DrawSphere(leftHit.point, 0.05f);
    //    if (rightHit) Gizmos.DrawSphere(rightHit.point, 0.05f);
    //}
}
