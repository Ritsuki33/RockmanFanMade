//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using UnityEngine;

///// <summary>
///// 速度制御クラス
///// </summary>
//public class VelocityControll
//{
//    LayerMask physicalLayer = default;
//    float physicalOffset = 0.02f;

//    public LayerMask PhysicalLayer { get { return physicalLayer; } }
//    Rigidbody2D rb;
//    BoxCollider2D boxCollider = null;

//    Vector2 ColliderCenter => (Vector2)this.boxCollider.transform.position + this.boxCollider.offset;
        
//    Vector2 ColliderSize => this.boxCollider.size;

//    float Bottom
//    {
//        get { return ColliderCenter.y - ColliderSize.y / 2; }
//        set { this.boxCollider.transform.position = new Vector3(boxCollider.transform.position.x, value + ColliderSize.y / 2); }// y - s_y/2 = value -> y = value + s_y/2}
//    }

//    float Top
//    {
//        get { return ColliderCenter.y + ColliderSize.y / 2; }
//        set { this.boxCollider.transform.position = new Vector3(boxCollider.transform.position.x, value - ColliderSize.y / 2); }//  y + s_y/2 = value -> y = value - s_y/2
//    }

//    float Left
//    {
//        get { return ColliderCenter.x - ColliderSize.x / 2; }
//        set { this.boxCollider.transform.position = new Vector3(value + ColliderSize.x / 2, boxCollider.transform.position.y); }// y + s_y/2 = value -> y = value - s_y/2
//    }

//    float Right
//    {
//        get { return ColliderCenter.x + ColliderSize.x / 2; }
//        set { this.boxCollider.transform.position = new Vector3(value - ColliderSize.x / 2, boxCollider.transform.position.y); }//  y + s_y/2 = value -> y = value - s_y/2
//    }

//    public event Action<RaycastHit2D> onHitBottomStay;
//    public event Action<RaycastHit2D> onHitTopStay;
//    public event Action<RaycastHit2D> onHitLeftStay;
//    public event Action<RaycastHit2D> onHitRightStay;

//    public event Action<RaycastHit2D> onHitBottomExit;
//    public event Action<RaycastHit2D> onHitTopExit;
//    public event Action<RaycastHit2D> onHitLeftExit;
//    public event Action<RaycastHit2D> onHitRightExit;

//    RaycastHit2D verticalHit; // 垂直方向のヒット
//    RaycastHit2D horizonHit; // 水平方向のヒット


//    bool isCollideTop = false;
//    bool isCollideBottom = false;
//    bool isCollideRight = false;
//    bool isCollideLeft = false;

//    public void Init(Rigidbody2D rb, BoxCollider2D boxCollider)
//    {
//        this.rb =rb;
//        this.boxCollider = boxCollider;

//        physicalLayer = Physics2D.GetLayerCollisionMask(boxCollider.gameObject.layer);
//    }

//    /// <summary>
//    /// 速度による衝突検出
//    /// 速度がなければ検出しない
//    /// </summary>
//    public Vector2 CorrectVelocity(Vector2 curVelocity)
//    {
//        Vector2 correctVelocity = curVelocity;

//        Vector2 curMovement = rb.velocity * Time.fixedDeltaTime;
//        verticalHit = Physics2D.BoxCast(ColliderCenter, ColliderSize, 0, Vector3.up, ((curMovement.y > 0) ? 1 : -1) * physicalOffset + curMovement.y, physicalLayer);
//        horizonHit = Physics2D.BoxCast(ColliderCenter, ColliderSize, 0, Vector3.right, ((curMovement.x > 0) ? 1 : -1) * physicalOffset + curMovement.x, physicalLayer);

//        if (verticalHit && curMovement.y < 0)
//        {
//            float correctSpeed = correctVelocity.y;
//            correctSpeed = (verticalHit.point.y - Bottom + physicalOffset) / Time.fixedDeltaTime;
//            onHitBottomStay?.Invoke(verticalHit);
//            isCollideTop = true;

//            if (isCollideBottom) onHitBottomExit?.Invoke(verticalHit);
//            isCollideBottom = false;

//            correctSpeed = (float)Math.Round(correctSpeed, 4);
//            correctVelocity = new Vector2(correctVelocity.x, correctSpeed);
//        }
//        else if (verticalHit && curMovement.y > 0)
//        {
//            float correctSpeed = correctVelocity.y;
//            correctSpeed = (verticalHit.point.y - Top - physicalOffset) / Time.fixedDeltaTime;
//            onHitTopStay?.Invoke(verticalHit);
//            isCollideBottom = true;

//            if (isCollideTop) onHitTopExit?.Invoke(verticalHit);
//            isCollideTop = false;

//            correctSpeed = (float)Math.Round(correctSpeed, 4);
//            correctVelocity = new Vector2(correctVelocity.x, correctSpeed);
//        }
//        else
//        {
//            if (isCollideTop) onHitTopExit?.Invoke(verticalHit);
//            isCollideTop = false;

//            if (isCollideBottom) onHitBottomExit?.Invoke(verticalHit);
//            isCollideBottom = false;
//        }

//        if (horizonHit && curMovement.x < 0)
//        {
//            float correctSpeed = correctVelocity.x;
//            correctSpeed = (horizonHit.point.x - Left + physicalOffset) / Time.fixedDeltaTime;
//            onHitLeftStay?.Invoke(horizonHit);
//            isCollideLeft = true;
//            if (isCollideRight) onHitRightExit?.Invoke(horizonHit);
//            isCollideRight = false;


//            correctSpeed = (float)Math.Round(correctSpeed, 4);
//            correctVelocity = new Vector2(correctSpeed, correctVelocity.y);
//        }
//        else if (horizonHit && curMovement.x > 0)
//        {
//            onHitRightStay?.Invoke(horizonHit);
//            isCollideRight = true;
//            if (isCollideLeft) onHitLeftExit?.Invoke(horizonHit);
//            isCollideLeft = false;

//            float correctSpeed = correctVelocity.x;
//            correctSpeed = (horizonHit.point.x - Right - physicalOffset) / Time.fixedDeltaTime;
        
//            correctSpeed = (float)Math.Round(correctSpeed, 4);
//            correctVelocity = new Vector2(correctSpeed, correctVelocity.y);
//        }
//        else
//        {
//            if (isCollideLeft) onHitLeftExit?.Invoke(horizonHit);
//            isCollideLeft = false;

//            if (isCollideRight) onHitRightExit?.Invoke(horizonHit);
//            isCollideRight = false;
//        }

//        return correctVelocity;
//    }


//    public void OnDrawGizmos()
//    {
//        Gizmos.color = Color.blue;
//        if (boxCollider != null) Gizmos.DrawWireCube(ColliderCenter, ColliderSize + new Vector2(physicalOffset, physicalOffset) * 2);
//        if (verticalHit) Gizmos.DrawSphere(verticalHit.point, 0.1f);
//        if (horizonHit) Gizmos.DrawSphere(horizonHit.point, 0.01f);
//    }
//}
