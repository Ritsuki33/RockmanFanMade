using System;
using UnityEngine;

[Serializable]
public class ExpandRigidBody : IExRbCallbackSet
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

    [SerializeField] Transform transform = default;
    [SerializeField] Rigidbody2D rb = default;

    [SerializeField] BoxCollider2D boxCollider = null;
    [SerializeField] Vector2 physicalOffset = new Vector2(0.05f, 0.05f);
    [SerializeField] float physicalGap = 0.005f;
    [SerializeField] Priority priority = Priority.Vertical;

    [Header("幅、高さの比率")]
    [SerializeField, Range(0, 1)] float ratio_x = 1.0f;
    [SerializeField, Range(0, 1)] float ratio_y = 1.0f;

    [SerializeField] LayerMask throughFloorLayer = default;

    LayerMask physicalLayer = default;

    private Vector2 currentVelocity;
    /// <summary>
    /// コライダーの中心とサイズ
    /// </summary>
    public Vector3 BoxColliderCenter
    {
        get { return this.transform.position + ((boxCollider != null) ? boxCollider.offset : Vector3.zero); }
        set { this.transform.position = value - ((boxCollider != null) ? boxCollider.offset : Vector3.zero); }
    }
    Vector2 boxColliderSize => ((boxCollider != null) ? boxCollider.size : Vector2.zero);

    public Vector2 position => BoxColliderCenter;

    public Vector2 velocity { get { return currentVelocity; } set { currentVelocity = value; } }

    public BoxCollider2D BoxCollider => boxCollider;

    /// <summary>
    /// コライダー各4辺の中心
    /// </summary>
    Vector3 TopColliderCenter
    {
        get { return BoxColliderCenter + new Vector3(0, boxColliderSize.y / 2); }
        set { BoxColliderCenter = value - new Vector3(0, boxColliderSize.y / 2,0); }
    }
    Vector3 BottomColliderCenter
    {
        get { return BoxColliderCenter + new Vector3(0, -boxColliderSize.y / 2,0); }
        set { BoxColliderCenter = value - new Vector3(0, -boxColliderSize.y / 2,0); }
    }
    Vector3 RigthColliderCenter
    {
        get { return BoxColliderCenter + new Vector3(boxColliderSize.x / 2, 0); }
        set { BoxColliderCenter = value - new Vector3(boxColliderSize.x / 2, 0); }
    }
    Vector3 LeftColliderCenter
    {
        get { return BoxColliderCenter + new Vector3(-boxColliderSize.x / 2, 0); }
        set { BoxColliderCenter = value - new Vector3(-boxColliderSize.x / 2, 0); }
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
                return BoxColliderCenter + new Vector3(0, VirtuaBaseSize.y / 2,0);
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
                return BoxColliderCenter - new Vector3(0, VirtuaBaseSize.y / 2,0);
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
                return BoxColliderCenter + new Vector3(VirtuaBaseSize.x / 2, 0,0);
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
                return BoxColliderCenter - new Vector3(VirtuaBaseSize.x / 2, 0, 0);
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
    public float Bottom
    {
        get { return BottomColliderCenter.y - physicalOffset.y; }
        set { BottomColliderCenter = new Vector3(BottomColliderCenter.x, value + physicalOffset.y, BottomColliderCenter.z); }
    }

    public float Top
    {
        get { return TopColliderCenter.y + physicalOffset.y; }
        set { TopColliderCenter = new Vector3(BottomColliderCenter.x, value - physicalOffset.y, BottomColliderCenter.z); }
    }

    public float Left
    {
        get { return LeftColliderCenter.x - physicalOffset.x; }
        set { LeftColliderCenter = new Vector3(value + physicalOffset.x, LeftColliderCenter.y, BottomColliderCenter.z); }
    }

    public float Right
    {
        get { return RigthColliderCenter.x + physicalOffset.x; }
        set { RigthColliderCenter = new Vector3(value - physicalOffset.x, RigthColliderCenter.y, BottomColliderCenter.z); }
    }

    public event Action<RaycastHit2D> onHitEnter;
    public event Action<RaycastHit2D> onHitBottomEnter;
    public event Action<RaycastHit2D> onHitTopEnter;
    public event Action<RaycastHit2D> onHitLeftEnter;
    public event Action<RaycastHit2D> onHitRightEnter;

    public event Action<RaycastHit2D> onHitStay;
    public event Action<RaycastHit2D> onHitBottomStay;
    public event Action<RaycastHit2D> onHitTopStay;
    public event Action<RaycastHit2D> onHitLeftStay;
    public event Action<RaycastHit2D> onHitRightStay;

    public event Action<RaycastHit2D> onHitExit;
    public event Action<RaycastHit2D> onHitBottomExit;
    public event Action<RaycastHit2D> onHitTopExit;
    public event Action<RaycastHit2D> onHitLeftExit;
    public event Action<RaycastHit2D> onHitRightExit;

    public event Action<RaycastHit2D,RaycastHit2D> onHitBottomTopEnter;
    public event Action<RaycastHit2D,RaycastHit2D> onHitLeftRightEnter;

    public event Action<RaycastHit2D, RaycastHit2D> onHitBottomTopStay;
    public event Action<RaycastHit2D, RaycastHit2D> onHitLeftRightStay;

    public event Action<RaycastHit2D, RaycastHit2D> onHitBottomTopExit;
    public event Action<RaycastHit2D, RaycastHit2D> onHitLeftRightExit;

    RaycastHit2D casheCollideBottom = default;
    RaycastHit2D casheCollideTop = default;
    RaycastHit2D casheCollideLeft = default;
    RaycastHit2D casheCollideRight = default;

    bool isCollideThroughFloorBottom = false;

    Vector2 CurrentMovement => velocity * Time.fixedDeltaTime;

    public void Init()
    {
        if (boxCollider == null) boxCollider = transform.gameObject.GetComponent<BoxCollider2D>();
        physicalLayer = Physics2D.GetLayerCollisionMask(boxCollider.gameObject.layer);

        if (rb == null) rb = transform.gameObject.GetComponent<Rigidbody2D>();
        if (!rb) rb = transform.gameObject.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0;
        rb.drag = 0;
        rb.gravityScale = 0;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void FixedUpdate()
    {
       if((boxCollider == null) ? false : (!(boxCollider.enabled) ? false : (boxCollider.isTrigger) ? false : true)) CorrectVelocity();
        rb.velocity = currentVelocity;
        currentVelocity = Vector2.zero;
    }

    private void CorrectVelocity()
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
        if (physicalLayer == 0) Debug.LogError($"ExpandRigidBodyが初期化されていないため、レイの判定ができません。({this.transform.gameObject})");

        topHit = Physics2D.BoxCast(
            VirtualTopColliderCenter + new Vector2(0, -0.005f)
            , VerticalCheckHitTopSize
            , 0
            , Vector2.up
            , Top - (VirtualTopColliderCenter.y - 0.005f) + ((CurrentMovement.y > 0) ? Mathf.Abs(CurrentMovement.y) : 0)
            , physicalLayer);


        bottomHit = Physics2D.BoxCast(
            VirtualBottomColliderCenter + new Vector2(0, 0.005f)
            , VerticalCheckHitBottomSize
            , 0
            , Vector2.down
            , VirtualBottomColliderCenter.y + 0.005f - Bottom + ((CurrentMovement.y < 0) ? Mathf.Abs(CurrentMovement.y) : 0)
            , physicalLayer);


        rightHit = Physics2D.BoxCast(
            VirtualRightColliderCenter + new Vector2(-0.005f, 0)
            , HorizenCheckHitRightSize
            , 0
            , Vector2.right
            , Right - (VirtualRightColliderCenter.x-0.005f) + ((CurrentMovement.x > 0) ? Mathf.Abs(CurrentMovement.x) : 0)
            , physicalLayer);


        leftHit = Physics2D.BoxCast(
            VirtualLeftColliderCenter + new Vector2(0.005f, 0)
            , HorizenCheckHitLeftSize
            , 0
            , Vector2.left
            , VirtualLeftColliderCenter.x+ 0.005f - Left + ((CurrentMovement.x < 0) ? Mathf.Abs(CurrentMovement.x) : 0)
            , physicalLayer);

        // コライダー内部からのレイヒットをフィルタリング
        if (topHit.distance < 0.01f) topHit = default;
        if (bottomHit.distance < 0.01f) bottomHit = default;
        if (leftHit.distance < 0.01f) leftHit = default;
        if (rightHit.distance < 0.01f) rightHit = default;

        if (topHit)
        {
            if (!bottomHit)
            {
                float correct = (topHit.point.y - Top - physicalGap) / Time.fixedDeltaTime;
                if (currentVelocity.y >= 0) currentVelocity.y = correct;
                else currentVelocity.y += correct;
            }

            if (casheCollideTop != topHit)
            {
                if (casheCollideTop)
                {
                    onHitTopExit?.Invoke(casheCollideTop);
                    onHitExit?.Invoke(casheCollideTop);
                }

                onHitTopEnter?.Invoke(topHit);
                onHitEnter?.Invoke(topHit);
            }

            onHitTopStay?.Invoke(topHit);
            onHitStay?.Invoke(topHit);
        }
        else if (casheCollideTop)
        {
            onHitTopExit?.Invoke(casheCollideTop);
            onHitExit?.Invoke(casheCollideTop);
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

            if (casheCollideBottom != bottomHit)
            {
                if (casheCollideBottom)
                {
                    onHitBottomExit?.Invoke(casheCollideBottom);
                    onHitExit?.Invoke(casheCollideBottom);
                }

                onHitBottomEnter?.Invoke(bottomHit);
                onHitEnter?.Invoke(bottomHit);
            }

            onHitBottomStay?.Invoke(bottomHit);
            onHitStay?.Invoke(bottomHit);
        }
        else if (casheCollideBottom)
        {
            onHitBottomExit?.Invoke(casheCollideBottom);
            onHitExit?.Invoke(casheCollideBottom);
        }



        if (leftHit)
        {
            float correct = (leftHit.point.x - Left + physicalGap) / Time.fixedDeltaTime;
            if (currentVelocity.x <= 0) currentVelocity.x = correct;

            if (casheCollideLeft != leftHit)
            {
                if (casheCollideLeft)
                {
                    onHitLeftExit?.Invoke(casheCollideBottom);
                    onHitExit?.Invoke(casheCollideBottom);
                }

                onHitLeftEnter?.Invoke(leftHit);
                onHitEnter?.Invoke(leftHit);
            }
            onHitLeftStay?.Invoke(leftHit);
            onHitStay?.Invoke(leftHit);
        }
        else if (casheCollideLeft)
        {
            onHitLeftExit?.Invoke(casheCollideLeft);
            onHitExit?.Invoke(casheCollideLeft);
        }


        if (rightHit)
        {
            float correct = (rightHit.point.x - Right - physicalGap) / Time.fixedDeltaTime;
            if (currentVelocity.x >= 0) currentVelocity.x = correct;
            else currentVelocity.x += correct;

            if (casheCollideRight!=rightHit)
            {
                if (casheCollideRight)
                {
                    onHitRightExit?.Invoke(casheCollideRight);
                    onHitExit?.Invoke(casheCollideRight);
                }

                onHitRightEnter?.Invoke(rightHit);
                onHitEnter?.Invoke(rightHit);
            }

            onHitRightStay?.Invoke(rightHit);
            onHitStay?.Invoke(rightHit);
        }
        else if (casheCollideRight)
        {
                onHitRightExit?.Invoke(casheCollideRight);
                onHitExit?.Invoke(casheCollideRight);
        }


        if (topHit && bottomHit)
        {
            if (casheCollideTop != topHit || casheCollideBottom != bottomHit)
            {
                onHitBottomTopExit?.Invoke(casheCollideBottom, casheCollideTop);

                onHitBottomTopEnter?.Invoke(topHit, bottomHit);
            }

            onHitBottomTopStay?.Invoke(topHit, bottomHit);
        }
        else if (casheCollideTop && casheCollideBottom)
        {
            onHitBottomTopExit?.Invoke(casheCollideBottom, casheCollideTop);
        }

        if (rightHit && leftHit)
        {
            if (casheCollideRight != rightHit || casheCollideLeft != leftHit)
            {
                onHitLeftRightExit?.Invoke(casheCollideLeft, casheCollideRight);

                onHitLeftRightEnter?.Invoke(rightHit, leftHit);
            }

            onHitLeftRightStay?.Invoke(rightHit, leftHit);
        }
        else if (casheCollideRight && casheCollideLeft)
        {
            onHitLeftRightExit?.Invoke(casheCollideLeft, casheCollideRight);
        }

        this.currentVelocity = currentVelocity;

        casheCollideTop = topHit;
        casheCollideBottom = bottomHit;
        casheCollideLeft = leftHit;
        casheCollideRight = rightHit;
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

                if (!casheCollideBottom)
                {
                    onHitBottomEnter?.Invoke(throughFloorBottomHit);
                    onHitEnter?.Invoke(throughFloorBottomHit);
                }
                onHitBottomStay?.Invoke(throughFloorBottomHit);
                onHitStay?.Invoke(throughFloorBottomHit);
                isCollideThroughFloorBottom = true;
            }
        }
        else
        {
            if (isCollideThroughFloorBottom)
            {
                onHitBottomExit?.Invoke(throughFloorBottomHit);
                onHitExit?.Invoke(throughFloorBottomHit);
            }
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
    void IExRbCallbackSet.AddOnHitEventCallback(IHitEvent hitEvent)
    {
        onHitEnter += hitEvent.OnHitEnter;
        onHitBottomEnter += hitEvent.OnBottomHitEnter;
        onHitTopEnter += hitEvent.OnTopHitEnter;
        onHitLeftEnter += hitEvent.OnLeftHitEnter;
        onHitRightEnter += hitEvent.OnRightHitEnter;
        onHitStay += hitEvent.OnHitStay;
        onHitBottomStay += hitEvent.OnBottomHitStay;
        onHitTopStay += hitEvent.OnTopHitStay;
        onHitLeftStay += hitEvent.OnLeftHitStay;
        onHitRightStay += hitEvent.OnRightHitStay;
        onHitExit += hitEvent.OnHitExit;
        onHitBottomExit += hitEvent.OnBottomHitExit;
        onHitTopExit += hitEvent.OnTopHitExit;
        onHitRightExit += hitEvent.OnRightHitExit;
        onHitLeftExit += hitEvent.OnLeftHitExit;
        onHitBottomTopEnter += hitEvent.OnBottomTopHitEnter;
        onHitLeftRightEnter += hitEvent.OnLeftRightHitEnter;
        onHitBottomTopStay += hitEvent.OnBottomTopHitStay;
        onHitLeftRightStay += hitEvent.OnLeftRightHitStay;
        onHitBottomTopExit += hitEvent.OnBottomTopHitExit;
        onHitLeftRightExit += hitEvent.OnLeftRightHitExit;
    }

    /// <summary>
    /// 上下左右ヒット時のコールバック削除
    /// </summary>
    /// <param name="hitEvent"></param>
    void IExRbCallbackSet.RemoveOnHitEventCallback(IHitEvent hitEvent)
    {
        onHitEnter -= hitEvent.OnHitEnter;
        onHitBottomEnter -= hitEvent.OnBottomHitEnter;
        onHitTopEnter -= hitEvent.OnTopHitEnter;
        onHitLeftEnter -= hitEvent.OnLeftHitEnter;
        onHitRightEnter -= hitEvent.OnRightHitEnter;
        onHitStay -= hitEvent.OnHitStay;
        onHitBottomStay -= hitEvent.OnBottomHitStay;
        onHitTopStay -= hitEvent.OnTopHitStay;
        onHitLeftStay -= hitEvent.OnLeftHitStay;
        onHitRightStay -= hitEvent.OnRightHitStay;
        onHitExit -= hitEvent.OnHitExit;
        onHitBottomExit -= hitEvent.OnBottomHitExit;
        onHitTopExit -= hitEvent.OnTopHitExit;
        onHitRightExit -= hitEvent.OnRightHitExit;
        onHitLeftExit -= hitEvent.OnLeftHitExit;
        onHitBottomTopEnter -= hitEvent.OnBottomTopHitEnter;
        onHitLeftRightEnter -= hitEvent.OnLeftRightHitEnter;
        onHitBottomTopStay -= hitEvent.OnBottomTopHitStay;
        onHitLeftRightStay -= hitEvent.OnLeftRightHitStay;
        onHitBottomTopExit -= hitEvent.OnBottomTopHitExit;
        onHitLeftRightExit -= hitEvent.OnLeftRightHitExit;
    }

    public void OnDrawGizmos()
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
