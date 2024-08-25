using System;
using UnityEngine;


public class ExpandRigidBody : MonoBehaviour, IBaseExRbHit.IExRbCallbackSet
{
    enum Priority
    {
        None,
        Vertical,
        Horizen,
        Top,
        Bottom,
        Right,
        Left,
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

    [SerializeField] BoxCollider2D boxCollider = null;
    [SerializeField] Vector2 physicalOffset = new Vector2(0.05f, 0.05f);
    [SerializeField] float physicalGap = 0.005f;
    [SerializeField] Priority priority = Priority.Vertical;

    [Header("幅、高さの比率")]
    [SerializeField, Range(0, 1)] float ratio_x = 1.0f;
    [SerializeField, Range(0, 1)] float ratio_y = 1.0f;

    [SerializeField] LayerMask throughFloorLayer = default;
    [SerializeField]LayerMask triggerLayer = default;

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
    Vector2 TopColliderCenter
    {
        get { return BoxColliderCenter + new Vector2(0, boxColliderSize.y / 2); }
        set { BoxColliderCenter = value - new Vector2(0, boxColliderSize.y / 2); }
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
    Vector2 VerticalCheckHitTopSize
    {
        get
        {
            switch (priority)
            {
                case Priority.Vertical:
                case Priority.Top:
                    return new Vector2(boxColliderSize.x + physicalOffset.x * 2, 0.001f);
                case Priority.Horizen:
                case Priority.Bottom:
                    return new Vector2(boxColliderSize.x * ratio_x, 0.001f);
                case Priority.Right:
                    return new Vector2(boxColliderSize.x * ratio_x + VirtualLeftColliderCenter.x - Left, 0.001f);
                case Priority.Left:
                    return new Vector2(boxColliderSize.x * ratio_x + Right - VirtualRightColliderCenter.x, 0.001f);
                default:
                    return new Vector2(boxColliderSize.x, 0.001f);
            }
        }
    }

    Vector2 VerticalCheckHitBottomSize
    {
        get
        {
            switch (priority)
            {
                case Priority.Vertical:
                case Priority.Bottom:
                    return new Vector2(boxColliderSize.x + physicalOffset.x * 2, 0.001f);
                case Priority.Horizen:
                case Priority.Top:
                    return new Vector2(boxColliderSize.x * ratio_x, 0.001f);
                case Priority.Right:
                    return new Vector2(boxColliderSize.x * ratio_x + VirtualLeftColliderCenter.x - Left, 0.001f);
                case Priority.Left:
                    return new Vector2(boxColliderSize.x * ratio_x + Right - VirtualRightColliderCenter.x, 0.001f);
                default:
                    return new Vector2(boxColliderSize.x, 0.001f);
            }
        }
    }

    Vector2 HorizenCheckHitRightSize
    {
        get
        {
            switch (priority)
            {
                case Priority.Vertical:
                case Priority.Left:
                    return new Vector2(0.001f, boxColliderSize.y * ratio_y);
                case Priority.Bottom:
                    return new Vector2(0.001f, boxColliderSize.y * ratio_y + Top - VirtualTopColliderCenter.y);
                case Priority.Top:
                    return new Vector2(0.001f, boxColliderSize.y * ratio_y + VirtualBottomColliderCenter.y - Bottom);
                case Priority.Horizen:
                case Priority.Right:
                    return new Vector2(0.001f, boxColliderSize.y + physicalOffset.y * 2);
                default:
                    return new Vector2(0.001f, boxColliderSize.y); ;
            }
        }
    }

    Vector2 HorizenCheckHitLeftSize
    {
        get
        {
            switch (priority)
            {
                case Priority.Vertical:
                case Priority.Right:
                    return new Vector2(0.001f, boxColliderSize.y * ratio_y);
                case Priority.Bottom:
                    return new Vector2(0.001f, boxColliderSize.y * ratio_y + Top - VirtualTopColliderCenter.y);
                case Priority.Top:
                    return new Vector2(0.001f, boxColliderSize.y * ratio_y + VirtualBottomColliderCenter.y - Bottom);
                case Priority.Horizen:
                case Priority.Left:
                    return new Vector2(0.001f, boxColliderSize.y + physicalOffset.y * 2);
                default:
                    return new Vector2(0.001f, boxColliderSize.y); ;
            }
        }
    }

    /// <summary>
    /// 各種辺の基準値から成るサイズ
    /// </summary>
    public Vector2 VirtuaBaseSize => new Vector2(this.boxColliderSize.x * ratio_x, this.boxColliderSize.y * ratio_y);

    public Vector2 PhysicalBoxSize => this.boxCollider.size + physicalOffset * 2;
    /// <summary>
    /// コライダー各4辺の中心
    /// </summary>
    Vector2 VirtualTopColliderCenter
    {
        get
        {
            if (priority == Priority.Left)
            {
                float center_x = (Right + (BoxColliderCenter.x - VirtuaBaseSize.x / 2)) / 2;
                float center_y = BoxColliderCenter.y + VirtuaBaseSize.y / 2;

                return new Vector2(center_x, center_y);
            }
            else if (priority == Priority.Right)
            {
                float center_x = (Left + (BoxColliderCenter.x + VirtuaBaseSize.x / 2)) / 2; 
                float center_y = BoxColliderCenter.y + VirtuaBaseSize.y / 2;

                return new Vector2(center_x, center_y);
            }
            else
            {
                return BoxColliderCenter + new Vector2(0, VirtuaBaseSize.y / 2);
            }
        }
    }

    Vector2 VirtualBottomColliderCenter
    {
        get
        {
            if (priority == Priority.Left)
            {
                float center_x = (Right + (BoxColliderCenter.x - VirtuaBaseSize.x / 2)) / 2;
                float center_y = BoxColliderCenter.y - VirtuaBaseSize.y / 2;

                return new Vector2(center_x, center_y);
            }
            else if (priority == Priority.Right)
            {
                float center_x = (Left + (BoxColliderCenter.x + VirtuaBaseSize.x / 2)) / 2;
                float center_y = BoxColliderCenter.y - VirtuaBaseSize.y / 2;

                return new Vector2(center_x, center_y);
            }
            else
            {
                return BoxColliderCenter - new Vector2(0, VirtuaBaseSize.y / 2);
            }
        }
    }

    Vector2 VirtualRightColliderCenter
    {
        get
        {
            if (priority == Priority.Bottom)
            {
                float center_x = BoxColliderCenter.x + VirtuaBaseSize.x / 2;
                float center_y = (Top + (BoxColliderCenter.y - VirtuaBaseSize.y / 2)) / 2;

                return new Vector2(center_x, center_y);
            }
            else if (priority == Priority.Top)
            {
                float center_x = BoxColliderCenter.x + VirtuaBaseSize.x / 2;
                float center_y = (Bottom + (BoxColliderCenter.y + VirtuaBaseSize.y / 2)) / 2;

                return new Vector2(center_x, center_y);
            }
            else
            {
                return BoxColliderCenter + new Vector2(VirtuaBaseSize.x / 2, 0);
            }
        }
    }

    Vector2 VirtualLeftColliderCenter
    {
        get
        {
            if (priority == Priority.Bottom)
            {
                float center_x = BoxColliderCenter.x - VirtuaBaseSize.x / 2;
                float center_y = (Top + (BoxColliderCenter.y - VirtuaBaseSize.y / 2)) / 2;

                return new Vector2(center_x, center_y);
            }
            else if (priority == Priority.Top)
            {
                float center_x = BoxColliderCenter.x -  VirtuaBaseSize.x / 2;
                float center_y = (Bottom + (BoxColliderCenter.y + VirtuaBaseSize.y / 2)) / 2;

                return new Vector2(center_x, center_y);
            }
            else
            {
                return BoxColliderCenter - new Vector2(VirtuaBaseSize.x / 2, 0);
            }
        }
    }

    RaycastHit2D topHit;
    RaycastHit2D bottomHit;
    RaycastHit2D leftHit;
    RaycastHit2D rightHit;

    RaycastHit2D throughFloorBottomHit;

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

    public event Action<RaycastHit2D> onHitBottomEnter;
    public event Action<RaycastHit2D> onHitTopEnter;
    public event Action<RaycastHit2D> onHitLeftEnter;
    public event Action<RaycastHit2D> onHitRightEnter;

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
        physicalLayer |= triggerLayer;

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

        rb.velocity = currentVelocity;
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

    private void PhysicalVelocityCorrect(Vector2 currentVelocity)
    {
        topHit = Physics2D.BoxCast(
            VirtualTopColliderCenter
            , VerticalCheckHitTopSize
            , 0
            , Vector2.up
            , Top - VirtualTopColliderCenter.y + ((CurrentMovement.y > 0) ? Mathf.Abs(CurrentMovement.y) : 0)
            , physicalLayer);


        bottomHit = Physics2D.BoxCast(
            VirtualBottomColliderCenter
            , VerticalCheckHitBottomSize
            , 0
            , Vector2.down
            , VirtualBottomColliderCenter.y - Bottom + ((CurrentMovement.y < 0) ? Mathf.Abs(CurrentMovement.y) : 0)
            , physicalLayer);


        rightHit = Physics2D.BoxCast(
            VirtualRightColliderCenter
            , HorizenCheckHitRightSize
            , 0
            , Vector2.right
            , Right - VirtualRightColliderCenter.x + ((CurrentMovement.x > 0) ? Mathf.Abs(CurrentMovement.x) : 0)
            , physicalLayer);


        leftHit = Physics2D.BoxCast(
            VirtualLeftColliderCenter
            , HorizenCheckHitLeftSize
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

            if (!isCollideTop) onHitTopEnter?.Invoke(topHit);

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

            if (!isCollideBottom) onHitBottomEnter?.Invoke(bottomHit);

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

            if (!isCollideLeft) onHitLeftEnter?.Invoke(leftHit);
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

            if (!isCollideRight) onHitRightEnter?.Invoke(rightHit);
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
        throughFloorBottomHit = Physics2D.BoxCast(
           VirtualBottomColliderCenter
           , VerticalCheckHitBottomSize
           , 0
           , Vector2.down
           , VirtualBottomColliderCenter.y - Bottom + ((CurrentMovement.y < 0) ? Mathf.Abs(CurrentMovement.y) : 0)
           , throughFloorLayer);

        if (throughFloorBottomHit)
        {
            Rigidbody2D target_rb = throughFloorBottomHit.collider.attachedRigidbody;

            Vector2 target_vel = (target_rb != null) ? target_rb.velocity : Vector2.zero;


            if (currentVelocity.y - target_vel.y < 0)
            {
                if (!topHit)
                {
                    float correct = (throughFloorBottomHit.point.y - Bottom + physicalGap) / Time.fixedDeltaTime;
                    if (currentVelocity.y <= 0) currentVelocity.y = correct;
                    else currentVelocity.y += correct;

                    if (throughFloorBottomHit.rigidbody) currentVelocity += throughFloorBottomHit.rigidbody.velocity;
                }

                if (!isCollideBottom) onHitBottomEnter?.Invoke(bottomHit);

                onHitBottomStay?.Invoke(throughFloorBottomHit);
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
    /// <param name="hitEvent"></param>
     void IBaseExRbHit.IExRbCallbackSet.AddOnHitEventCallback(IHitEvent hitEvent)
    {
        onHitBottomEnter += hitEvent.BottomHitEnter;
        onHitTopEnter += hitEvent.TopHitEnter;
        onHitLeftEnter += hitEvent.LeftHitEnter;
        onHitRightEnter += hitEvent.RightHitEnter;
        onHitBottomStay += hitEvent.BottomHitStay;
        onHitTopStay += hitEvent.TopHitStay;
        onHitLeftStay += hitEvent.LeftHitStay;
        onHitRightStay += hitEvent.RightHitStay;
        onHitBottomExit += hitEvent.BottomHitExit;
        onHitTopExit += hitEvent.TopHitExit;
        onHitRightExit += hitEvent.RightHitExit;
        onHitLeftExit += hitEvent.LeftHitExit;
    }

    /// <summary>
    /// 上下左右ヒット時のコールバック削除
    /// </summary>
    /// <param name="hitEvent"></param>
    void IBaseExRbHit.IExRbCallbackSet.RemoveOnHitEventCallback(IHitEvent hitEvent)
    {
        onHitBottomEnter -= hitEvent.BottomHitEnter;
        onHitTopEnter -= hitEvent.TopHitEnter;
        onHitLeftEnter -= hitEvent.LeftHitEnter;
        onHitRightEnter -= hitEvent.RightHitEnter;
        onHitBottomStay -= hitEvent.BottomHitStay;
        onHitTopStay -= hitEvent.TopHitStay;
        onHitLeftStay -= hitEvent.LeftHitStay;
        onHitRightStay -= hitEvent.RightHitStay;
        onHitBottomExit -= hitEvent.BottomHitExit;
        onHitTopExit -= hitEvent.TopHitExit;
        onHitRightExit -= hitEvent.RightHitExit;
        onHitLeftExit -= hitEvent.LeftHitExit;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider && !boxCollider.enabled) return;
        Gizmos.color = Color.red;

        if (topHit) { Gizmos.DrawSphere(topHit.point, 0.05f); }
        if (bottomHit) { Gizmos.DrawSphere(bottomHit.point, 0.05f); }
        if (throughFloorBottomHit) { Gizmos.DrawSphere(bottomHit.point, 0.05f); }

        Gizmos.color = Color.blue;

        if (rightHit) { Gizmos.DrawSphere(rightHit.point, 0.05f); }
        if (leftHit) { Gizmos.DrawSphere(leftHit.point, 0.05f); }
        Gizmos.color = Color.red;

        Vector2 topCheckCenter = new Vector2(VirtualTopColliderCenter.x, BoxColliderCenter.y + VirtuaBaseSize.y / 2 + (Top - (BoxColliderCenter.y + VirtuaBaseSize.y / 2)) / 2 + ((CurrentMovement.y > 0) ? CurrentMovement.y / 2 : 0));
        Vector2 topCheckSize = new Vector2(VerticalCheckHitTopSize.x, Top - (BoxColliderCenter.y + VirtuaBaseSize.y / 2) + ((CurrentMovement.y > 0) ? CurrentMovement.y : 0));
        Gizmos.DrawWireCube(topCheckCenter, topCheckSize);

        Vector2 bottomCheckCenter = new Vector2(VirtualBottomColliderCenter.x, BoxColliderCenter.y - VirtuaBaseSize.y / 2 + (Bottom - (BoxColliderCenter.y - VirtuaBaseSize.y / 2)) / 2 + ((CurrentMovement.y < 0) ? CurrentMovement.y / 2 : 0));
        Vector2 bottomCheckSize = new Vector2(VerticalCheckHitBottomSize.x, Bottom - (BoxColliderCenter.y - VirtuaBaseSize.y / 2) + ((CurrentMovement.y < 0) ? CurrentMovement.y : 0));
        Gizmos.DrawWireCube(bottomCheckCenter, bottomCheckSize);


        Gizmos.color = Color.blue;

        Vector2 leftCheckCenter = new Vector2(BoxColliderCenter.x - VirtuaBaseSize.x / 2 + (Left - (BoxColliderCenter.x - VirtuaBaseSize.x / 2)) / 2 + ((CurrentMovement.x < 0) ? CurrentMovement.x / 2 : 0), VirtualLeftColliderCenter.y);
        Vector2 leftCheckSize = new Vector2(Left - (BoxColliderCenter.x - VirtuaBaseSize.x / 2) + ((CurrentMovement.x < 0) ? CurrentMovement.x : 0), HorizenCheckHitLeftSize.y);
        Gizmos.DrawWireCube(leftCheckCenter, leftCheckSize);

        Vector2 rightCheckCenter = new Vector2(BoxColliderCenter.x + VirtuaBaseSize.x / 2 + (Right - (BoxColliderCenter.x + VirtuaBaseSize.x / 2)) / 2 + ((CurrentMovement.x < 0) ? CurrentMovement.x / 2 : 0), VirtualRightColliderCenter.y);
        Vector2 rightCheckSize = new Vector2(Right - (BoxColliderCenter.x + VirtuaBaseSize.x / 2) + ((CurrentMovement.x > 0) ? CurrentMovement.x : 0), HorizenCheckHitRightSize.y);
        Gizmos.DrawWireCube(rightCheckCenter, rightCheckSize);
    }
}
