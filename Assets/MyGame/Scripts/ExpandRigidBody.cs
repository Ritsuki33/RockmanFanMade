using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandRigidBody : MonoBehaviour
{
    enum Priority
    {
        None,
        X,
        Y,
    }

    public enum PostionSetType
    {
        Top,
        Bottom,
        Left,
        Right,
        Center
    }
    Rigidbody2D rb;

    [SerializeField] Vector2 physicalOffset = new Vector2(0.05f, 0.05f);
    [SerializeField] float physicalGap = 0.005f;
    [SerializeField] Priority priority = Priority.X;

    [Header("幅、高さの比率")]
    [SerializeField, Range(0, 1)] float ratio_x = 1.0f;
    [SerializeField, Range(0, 1)] float ratio_y = 1.0f;

    [SerializeField] LayerMask throughFloorLayer = default;
    BoxCollider2D boxCollider = null;
    LayerMask physicalLayer = default;

    private Vector2 currentVelocity;
    /// <summary>
    /// コライダーの中心とサイズ
    /// </summary>
    public Vector2 BoxColliderCenter
    {
        get { return (Vector2)this.transform.position + ((boxCollider != null) ? boxCollider.offset : Vector2.zero); }
        set { this.transform.position = value - ((boxCollider != null) ? boxCollider.offset : Vector2.zero); }
    }
    Vector2 boxColliderSize => ((boxCollider != null) ? boxCollider.size : Vector2.zero);

    public Vector2 position => BoxColliderCenter;

    public Vector2 velocity { get { return currentVelocity; } set { currentVelocity = value; } }

    /// <summary>
    /// コライダー各4辺の中心
    /// </summary>
    Vector2 TopColliderCenter {
        get { return BoxColliderCenter + new Vector2(0, boxColliderSize.y / 2); }
        set { BoxColliderCenter=value - new Vector2(0, boxColliderSize.y / 2); }
        }
    Vector2 BottomColliderCenter
    {
        get { return BoxColliderCenter + new Vector2(0, -boxColliderSize.y / 2); }
        set { BoxColliderCenter = value - new Vector2(0, -boxColliderSize.y / 2); }
    }
    Vector2 RigthColliderCenter
    {
        get { return BoxColliderCenter + new Vector2(boxColliderSize.x / 2, 0); }
        set { BoxColliderCenter = value - new Vector2(boxColliderSize.x / 2, 0); }
    }
    Vector2 LeftColliderCenter
    {
        get { return BoxColliderCenter + new Vector2(-boxColliderSize.x / 2, 0); }
        set { BoxColliderCenter = value - new Vector2(-boxColliderSize.x / 2, 0); }
    }

    /// <summary>
    /// ボックスキャストの線化
    /// </summary>
    Vector2 VerticalCheckHitSize => new Vector2(CheckSize.x , 0.001f);
    Vector2 HorizenCheckHitSize => new Vector2(0.001f, CheckSize.y);

    RaycastHit2D topHit;
    RaycastHit2D bottomHit;
    RaycastHit2D leftHit;
    RaycastHit2D rightHit;

    /// <summary>
    /// 実質サイズ
    /// </summary>
    public Vector2 CheckSize
    {
        get
        {
            switch (priority)
            {
                case Priority.None:
                    return boxColliderSize;
                case Priority.X:
                    return new Vector2(boxColliderSize.x + physicalOffset.x * 2, boxColliderSize.y * ratio_y);
                case Priority.Y:
                    return new Vector2(boxColliderSize.x * ratio_x, boxColliderSize.y + physicalOffset.y * 2);
                default:
                    return boxColliderSize;
            }
        }
    }

    /// <summary>
    /// 実質４辺
    /// </summary>
    float Bottom
    {
        get { return BottomColliderCenter.y - physicalOffset.y; }
        set { BottomColliderCenter = new Vector2(BottomColliderCenter.x, value + physicalOffset.y); }
    }

    float Top
    {
        get { return TopColliderCenter.y + physicalOffset.y; }
        set { TopColliderCenter = new Vector2(BottomColliderCenter.x, value - physicalOffset.y); }
    }

    float Left
    {
        get { return LeftColliderCenter.x - physicalOffset.x; }
        set { LeftColliderCenter = new Vector2(value + physicalOffset.x, LeftColliderCenter.y); }
    }

    float Right
    {
        get { return RigthColliderCenter.x + physicalOffset.x; }
        set { RigthColliderCenter = new Vector2(value - physicalOffset.x, RigthColliderCenter.y); }
    }


    /// <summary>
    /// 仮想サイズ
    /// </summary>
    public Vector2 VirtualSize => new Vector2(this.boxColliderSize.x * ratio_x, this.boxColliderSize.y * ratio_y);

    /// <summary>
    /// コライダー各4辺の中心
    /// </summary>
    Vector2 VirtualTopColliderCenter => BoxColliderCenter + new Vector2(0, VirtualSize.y / 2);
    Vector2 VirtualBottomColliderCenter => BoxColliderCenter + new Vector2(0, -VirtualSize.y / 2);
    Vector2 VirtualRigthColliderCenter => BoxColliderCenter + new Vector2(VirtualSize.x / 2, 0);
    Vector2 VirtualLeftColliderCenter => BoxColliderCenter + new Vector2(-VirtualSize.x / 2, 0);

    public event Action<RaycastHit2D> onHitBottomStay;
    public event Action<RaycastHit2D> onHitTopStay;
    public event Action<RaycastHit2D> onHitLeftStay;
    public event Action<RaycastHit2D> onHitRightStay;

    public event Action<RaycastHit2D> onHitBottomExit;
    public event Action<RaycastHit2D> onHitTopExit;
    public event Action<RaycastHit2D> onHitLeftExit;
    public event Action<RaycastHit2D> onHitRightExit;

    bool isCollideBottom = false;
    bool isCollideTop = false;
    bool isCollideLeft = false;
    bool isCollideRight = false;

    bool isCollideThroughFloorBottom = false;

    Vector2 CurrentMovement => velocity * Time.fixedDeltaTime;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        physicalLayer = Physics2D.GetLayerCollisionMask(boxCollider.gameObject.layer);


        rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0;
        rb.drag = 0;
        rb.gravityScale = 0;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// 辺の位置を指定
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    public void SetPosition(PostionSetType type, float position)
    {
        switch (type)
        {
            case PostionSetType.Top:
                Top = position;
                break;
            case PostionSetType.Bottom:
                Bottom = position;
                break;
            case PostionSetType.Left:
                Left = position;
                break;
            case PostionSetType.Right:
                Left = position;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (boxCollider.enabled) CorrectVelocity();

        rb.velocity= currentVelocity;
        currentVelocity = Vector2.zero;
    }


    public void CorrectVelocity()
    {
        PhysicalVelocityCorrect(currentVelocity);
        ThroughFloorVelocityCorrect(currentVelocity);
    }

    public void SetPosition(Vector2 pos)
    {
        BoxColliderCenter = pos;
    }

    /// <summary>
    /// リジッドボディへの速度付加
    /// </summary>
    /// <param name="velocity"></param>
    //protected void AddVelocity(Vector2 velocity)
    //{
    //    rb.velocity += velocity;
    //}

    private void PhysicalVelocityCorrect(Vector2 currentVelocity)
    {
        topHit = Physics2D.BoxCast(
            VirtualTopColliderCenter
            , VerticalCheckHitSize
            , 0
            , Vector2.up
            , Top - VirtualTopColliderCenter.y + ((CurrentMovement.y > 0) ? Mathf.Abs(CurrentMovement.y) : 0)
            , physicalLayer);


        bottomHit = Physics2D.BoxCast(
            VirtualBottomColliderCenter
            , VerticalCheckHitSize
            , 0
            , Vector2.down
            , VirtualBottomColliderCenter.y - Bottom + ((CurrentMovement.y < 0) ? Mathf.Abs(CurrentMovement.y) : 0)
            , physicalLayer);


        rightHit = Physics2D.BoxCast(
            VirtualRigthColliderCenter
            , HorizenCheckHitSize
            , 0
            , Vector2.right
            , Right - VirtualRigthColliderCenter.x + ((CurrentMovement.x > 0) ? Mathf.Abs(CurrentMovement.x) : 0)
            , physicalLayer);


        leftHit = Physics2D.BoxCast(
            VirtualLeftColliderCenter
            , HorizenCheckHitSize
            , 0
            , Vector2.left
            , VirtualLeftColliderCenter.x - Left + ((CurrentMovement.x < 0) ? Mathf.Abs(CurrentMovement.x) : 0)
            , physicalLayer);


        if (topHit)
        {
            if (!bottomHit)
            {
                float correct = (topHit.point.y - Top - physicalGap) / Time.fixedDeltaTime;
                if (currentVelocity.y >= 0) currentVelocity.y = correct;
                else currentVelocity.y += correct;
            }

            onHitTopStay?.Invoke(topHit);
            isCollideTop = true;
        }
        else
        {
            if (isCollideTop) onHitTopExit?.Invoke(topHit);
            isCollideTop = false;
        }


        if (bottomHit)
        {
            if (!topHit)
            {
                float correct = (bottomHit.point.y - Bottom + physicalGap) / Time.fixedDeltaTime;
                if (currentVelocity.y <= 0) currentVelocity.y = correct;
                else currentVelocity.y += correct;

                if (bottomHit.rigidbody) currentVelocity += bottomHit.rigidbody.velocity;
            }

            onHitBottomStay?.Invoke(bottomHit);
            isCollideBottom = true;
        }
        else
        {
            if (isCollideBottom) onHitBottomExit?.Invoke(bottomHit);
            isCollideBottom = false;
        }

        if (leftHit)
        {
            float correct = (leftHit.point.x - Left + physicalGap) / Time.fixedDeltaTime;
            if (currentVelocity.x <= 0) currentVelocity.x = correct;
            else currentVelocity.x += correct;
            onHitLeftStay?.Invoke(leftHit);
            isCollideLeft = true;
        }
        else
        {
            if (isCollideLeft) onHitLeftExit?.Invoke(leftHit);
            isCollideLeft = false;
        }


        if (rightHit)
        {
            float correct = (rightHit.point.x - Right - physicalGap) / Time.fixedDeltaTime;
            if (currentVelocity.x >= 0) currentVelocity.x = correct;
            else currentVelocity.x += correct;

            onHitRightStay?.Invoke(rightHit);
            isCollideRight = true;
        }
        else
        {
            if (isCollideRight) onHitRightExit?.Invoke(rightHit);
            isCollideRight = false;
        }

        this.currentVelocity = currentVelocity;

    }
    
    
    private void ThroughFloorVelocityCorrect(Vector2 currentVelocity)
    {
        bottomHit = Physics2D.BoxCast(
           VirtualBottomColliderCenter
           , VerticalCheckHitSize
           , 0
           , Vector2.down
           , VirtualBottomColliderCenter.y - Bottom + ((CurrentMovement.y < 0) ? Mathf.Abs(CurrentMovement.y) : 0)
           , throughFloorLayer);

        if (bottomHit)
        {
            Rigidbody2D target_rb = bottomHit.collider.attachedRigidbody;

            Vector2 target_vel = (target_rb != null) ? target_rb.velocity : Vector2.zero;


            if (currentVelocity.y - target_vel.y < 0)
            {
                if (!topHit)
                {
                    float correct = (bottomHit.point.y - Bottom + physicalGap) / Time.fixedDeltaTime;
                    if (currentVelocity.y >= 0) currentVelocity.y = correct;
                    else currentVelocity.y += correct;

                    if (bottomHit.rigidbody) currentVelocity += bottomHit.rigidbody.velocity;
                }

                onHitBottomStay?.Invoke(bottomHit);
                isCollideThroughFloorBottom = true;
            }
        }
        else
        {
            if (isCollideThroughFloorBottom) onHitBottomExit?.Invoke(bottomHit);
            isCollideThroughFloorBottom = false;
        }

        this.currentVelocity = currentVelocity;
    }

    public void RemoveThroughFloorLayer(int excludeLayer)
    {
        int layerMaskToExclude = 1 << excludeLayer;
        throughFloorLayer = throughFloorLayer & ~layerMaskToExclude;
    }

    public void AddThroughFloorLayer(int addLayer)
    {
        int layerMaskToExclude = 1 << addLayer;
        throughFloorLayer = throughFloorLayer | layerMaskToExclude;
    }

    /// <summary>
    /// 上下左右ヒット時のコールバック登録
    /// </summary>
    /// <param name="createVelocity"></param>
    public void AddOnHitEventCallback(IHitEvent createVelocity)
    {
        onHitBottomStay += createVelocity.OnBottomHitStay;
        onHitTopStay += createVelocity.OnTopHitStay;
        onHitLeftStay += createVelocity.OnLeftHitStay;
        onHitRightStay += createVelocity.OnRightHitStay;
        onHitBottomExit += createVelocity.OnBottomHitExit;
        onHitTopExit += createVelocity.OnTopHitExit;
        onHitRightExit += createVelocity.OnRightHitExit;
        onHitLeftExit += createVelocity.OnLeftHitExit;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider && !boxCollider.enabled) return;
        Gizmos.color = Color.red;

        //Vector2 topCheckCenter = new Vector2(boxColliderCenter.x, boxColliderCenter.y + boxColliderSize.y / 2 + physicalOffset.y /2 + ((currentMovement.y > 0) ? currentMovement.y / 2 : 0));
        //Vector2 topCheckSize = new Vector2(BodySize.x, physicalOffset.y + ((currentMovement.y > 0) ? currentMovement.y : 0));

        //Vector2 bottomCheckCenter = new Vector2(boxColliderCenter.x, boxColliderCenter.y - boxColliderSize.y / 2 - physicalOffset.y / 2 + ((currentMovement.y < 0) ? currentMovement.y / 2 : 0));
        //Vector2 bottomCheckSize = new Vector2(BodySize.x , physicalOffset.y + ((currentMovement.y < 0) ? currentMovement.y : 0));
        //Gizmos.DrawWireCube(bottomCheckCenter, bottomCheckSize);
        if (topHit) { Gizmos.DrawSphere(topHit.point, 0.05f); }
        if (bottomHit) { Gizmos.DrawSphere(bottomHit.point, 0.05f); }
        //Gizmos.color = Color.blue;

        //Vector2 rightCheckCenter = new Vector2(boxColliderCenter.x + boxColliderSize.x / 2 + physicalOffset.x / 2 + ((currentMovement.x > 0) ? currentMovement.x / 2 : 0), boxColliderCenter.y);
        //Vector2 rightCheckSize = new Vector2(physicalOffset.x + ((currentMovement.x > 0) ? currentMovement.x : 0), BodySize.y);
        //Gizmos.DrawWireCube(rightCheckCenter, rightCheckSize);

        //Vector2 leftCheckCenter = new Vector2(boxColliderCenter.x - boxColliderSize.x / 2 - physicalOffset.x / 2 + ((currentMovement.x < 0) ? currentMovement.x / 2 : 0), boxColliderCenter.y);
        //Vector2 leftCheckSize = new Vector2(physicalOffset.x + ((currentMovement.x < 0) ? currentMovement.x: 0), BodySize.y);
        //Gizmos.DrawWireCube(leftCheckCenter, leftCheckSize);
        Gizmos.color = Color.blue;

        if (rightHit) { Gizmos.DrawSphere(rightHit.point, 0.05f); }
        if (leftHit) { Gizmos.DrawSphere(leftHit.point, 0.05f); }
        Gizmos.color = Color.red;

        Vector2 topCheckCenter = new Vector2(BoxColliderCenter.x, BoxColliderCenter.y + VirtualSize.y / 2 + (Top - (BoxColliderCenter.y + VirtualSize.y / 2)) / 2 + ((CurrentMovement.y > 0) ? CurrentMovement.y / 2 : 0));
        Vector2 topCheckSize = new Vector2(CheckSize.x, Top - (BoxColliderCenter.y + VirtualSize.y / 2) + ((CurrentMovement.y > 0) ? CurrentMovement.y : 0));
        Gizmos.DrawWireCube(topCheckCenter, topCheckSize);

        Vector2 bottomCheckCenter = new Vector2(BoxColliderCenter.x, BoxColliderCenter.y - VirtualSize.y / 2 + (Bottom - (BoxColliderCenter.y - VirtualSize.y / 2)) / 2 + ((CurrentMovement.y < 0) ? CurrentMovement.y / 2 : 0));
        Vector2 bottomCheckSize = new Vector2(CheckSize.x, Bottom - (BoxColliderCenter.y - VirtualSize.y / 2) + ((CurrentMovement.y < 0) ? CurrentMovement.y : 0));
        Gizmos.DrawWireCube(bottomCheckCenter, bottomCheckSize);


        Gizmos.color = Color.blue;

        Vector2 leftCheckCenter = new Vector2(BoxColliderCenter.x - VirtualSize.x / 2 + (Left - (BoxColliderCenter.x - VirtualSize.x / 2)) / 2 + ((CurrentMovement.x < 0) ? CurrentMovement.x / 2 : 0), BoxColliderCenter.y);
        Vector2 leftCheckSize = new Vector2(Left - (BoxColliderCenter.x - VirtualSize.x / 2) + ((CurrentMovement.x < 0) ? CurrentMovement.x : 0), CheckSize.y);
        Gizmos.DrawWireCube(leftCheckCenter, leftCheckSize);

        Vector2 rightCheckCenter = new Vector2(BoxColliderCenter.x + VirtualSize.x / 2 + (Right - (BoxColliderCenter.x + VirtualSize.x / 2)) / 2 + ((CurrentMovement.x < 0) ? CurrentMovement.x / 2 : 0), BoxColliderCenter.y);
        Vector2 rightCheckSize = new Vector2(Right - (BoxColliderCenter.x + VirtualSize.x / 2) + ((CurrentMovement.x > 0) ? CurrentMovement.x : 0), CheckSize.y);
        Gizmos.DrawWireCube(rightCheckCenter, rightCheckSize);
    }
}
