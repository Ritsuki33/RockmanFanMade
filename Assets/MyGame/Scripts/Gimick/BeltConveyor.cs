using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBeltConveyorVelocity
{
    Vector2 velocity { get; set; }
}

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class BeltConveyor : MonoBehaviour, IRbVisitable, IExRbVisitable
{
    protected virtual void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

    protected virtual void AcceptOnHitEnter(IExRbVisitor visitor) => visitor.OnHitEnter(this);
    protected virtual void AcceptOnHitStay(IExRbVisitor visitor) => visitor.OnHitStay(this);
    protected virtual void AcceptOnHitExit(IExRbVisitor visitor) => visitor.OnHitExit(this);
    protected virtual void AcceptOnBottomHitEnter(IExRbVisitor visitor) => visitor.OnBottomHitEnter(this);
    protected virtual void AcceptOnBottomHitStay(IExRbVisitor visitor) => visitor.OnBottomHitStay(this);
    protected virtual void AcceptOnBottomHitExit(IExRbVisitor visitor) => visitor.OnBottomHitExit(this);
    protected virtual void AcceptOnTopHitEnter(IExRbVisitor visitor) => visitor.OnTopHitEnter(this);
    protected virtual void AcceptOnTopHitStay(IExRbVisitor visitor) => visitor.OnTopHitStay(this);
    protected virtual void AcceptOnTopHitExit(IExRbVisitor visitor) => visitor.OnTopHitExit(this);
    protected virtual void AcceptOnLeftHitEnter(IExRbVisitor visitor) => visitor.OnLeftHitEnter(this);
    protected virtual void AcceptOnLeftHitStay(IExRbVisitor visitor) => visitor.OnLeftHitStay(this);
    protected virtual void AcceptOnLeftHitExit(IExRbVisitor visitor) => visitor.OnLeftHitExit(this);
    protected virtual void AcceptOnRightHitEnter(IExRbVisitor visitor) => visitor.OnRightHitEnter(this);
    protected virtual void AcceptOnRightHitStay(IExRbVisitor visitor) => visitor.OnRightHitStay(this);
    protected virtual void AcceptOnRightHitExit(IExRbVisitor visitor) => visitor.OnRightHitExit(this);

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);

    void IExRbVisitable.AcceptOnHitEnter(IExRbVisitor visitor) => AcceptOnHitEnter(visitor);
    void IExRbVisitable.AcceptOnHitStay(IExRbVisitor visitor) => AcceptOnHitStay(visitor);
    void IExRbVisitable.AcceptOnHitExit(IExRbVisitor visitor) => AcceptOnHitExit(visitor);
    void IExRbVisitable.AcceptOnBottomHitEnter(IExRbVisitor visitor) => AcceptOnBottomHitEnter(visitor);
    void IExRbVisitable.AcceptOnBottomHitStay(IExRbVisitor visitor) => AcceptOnBottomHitStay(visitor);
    void IExRbVisitable.AcceptOnBottomHitExit(IExRbVisitor visitor) => AcceptOnBottomHitExit(visitor);
    void IExRbVisitable.AcceptOnTopHitEnter(IExRbVisitor visitor) => AcceptOnTopHitEnter(visitor);
    void IExRbVisitable.AcceptOnTopHitStay(IExRbVisitor visitor) => AcceptOnTopHitStay(visitor);
    void IExRbVisitable.AcceptOnTopHitExit(IExRbVisitor visitor) => AcceptOnTopHitExit(visitor);
    void IExRbVisitable.AcceptOnLeftHitEnter(IExRbVisitor visitor) => AcceptOnLeftHitEnter(visitor);
    void IExRbVisitable.AcceptOnLeftHitStay(IExRbVisitor visitor) => AcceptOnLeftHitStay(visitor);
    void IExRbVisitable.AcceptOnLeftHitExit(IExRbVisitor visitor) => AcceptOnLeftHitExit(visitor);
    void IExRbVisitable.AcceptOnRightHitEnter(IExRbVisitor visitor) => AcceptOnRightHitEnter(visitor);
    void IExRbVisitable.AcceptOnRightHitStay(IExRbVisitor visitor) => AcceptOnRightHitStay(visitor);
    void IExRbVisitable.AcceptOnRightHitExit(IExRbVisitor visitor) => AcceptOnRightHitExit(visitor);

    // ここから定義
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] ScrollSpriteController scrollSpriteController;
    [SerializeField] LayerMask mask;
    [SerializeField] float width = 4;
    [SerializeField] float speed = 1;

    float offset = 0;

    Vector2 BoxCastCenter => (Vector2)boxCollider.transform.position + boxCollider.offset + new Vector2(0, boxCollider.size.y / 2);
    Vector2 BoxCastSize => new Vector2(boxCollider.size.x, 0.001f);

    Vector2 actualWorldSpriteSize;

    List<IBeltConveyorVelocity> objs = new List<IBeltConveyorVelocity>();
    private void Awake()
    {
        actualWorldSpriteSize = spriteRenderer.sprite.rect.size / spriteRenderer.sprite.pixelsPerUnit;
    }
    private void OnValidate()
    {
        spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
        boxCollider.size = new Vector2(width, spriteRenderer.size.y);
    }

    private void FixedUpdate()
    {
        scrollSpriteController.Scroll(Vector2.right * offset);
        offset += speed * Time.fixedDeltaTime;
        offset %= 1.0f;

        foreach (var obj in objs)
        {
            obj.velocity += speed * actualWorldSpriteSize.x * Vector2.left;
        }
    }

    public void AddObject(IBeltConveyorVelocity obj)
    {
        //objs.Add(obj);
        obj.velocity += speed * actualWorldSpriteSize.x * Vector2.left;
    }

    public void RemoveObject(IBeltConveyorVelocity obj)
    {
        //objs.Remove(obj);
    }
}

public partial interface IRbVisitor : IRbVisitor<BeltConveyor>
{ }

public partial interface IExRbVisitor : IExRbVisitor<BeltConveyor>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, BeltConveyor>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, BeltConveyor>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, BeltConveyor>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, BeltConveyor>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, BeltConveyor collision) { }
    virtual protected void OnTriggerStay(T obj, BeltConveyor collision) { }
    virtual protected void OnTriggerExit(T obj, BeltConveyor collision) { }

    virtual protected void OnCollisionEnter(T obj, BeltConveyor collision) { }
    virtual protected void OnCollisionStay(T obj, BeltConveyor collision) { }
    virtual protected void OnCollisionExit(T obj, BeltConveyor collision) { }

    void IStateRbVisitor<T, BeltConveyor>.OnTriggerEnter(T obj, BeltConveyor collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, BeltConveyor>.OnTriggerStay(T obj, BeltConveyor collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, BeltConveyor>.OnTriggerExit(T obj, BeltConveyor collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, BeltConveyor>.OnCollisionEnter(T obj, BeltConveyor collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, BeltConveyor>.OnCollisionStay(T obj, BeltConveyor collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, BeltConveyor>.OnCollisionExit(T obj, BeltConveyor collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
{
    virtual protected void OnHitEnter(T obj, BeltConveyor collision) { }
    virtual protected void OnBottomHitEnter(T obj, BeltConveyor collision) { }
    virtual protected void OnTopHitEnter(T obj, BeltConveyor collision) { }

    virtual protected void OnLeftHitEnter(T obj, BeltConveyor collision) { }
    virtual protected void OnRightHitEnter(T obj, BeltConveyor collision) { }
    virtual protected void OnHitStay(T obj, BeltConveyor collision) { }

    virtual protected void OnBottomHitStay(T obj, BeltConveyor collision) { }
    virtual protected void OnTopHitStay(T obj, BeltConveyor collision) { }
    virtual protected void OnLeftHitStay(T obj, BeltConveyor collision) { }

    virtual protected void OnRightHitStay(T obj, BeltConveyor collision) { }
    virtual protected void OnHitExit(T obj, BeltConveyor collision) { }
    virtual protected void OnBottomHitExit(T obj, BeltConveyor collision) { }

    virtual protected void OnTopHitExit(T obj, BeltConveyor collision) { }
    virtual protected void OnLeftHitExit(T obj, BeltConveyor collision) { }
    virtual protected void OnRightHitExit(T obj, BeltConveyor collision) { }

    void IStateExRbVisitor<T, BeltConveyor>.OnHitEnter(T obj, BeltConveyor hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, BeltConveyor>.OnBottomHitEnter(T obj, BeltConveyor hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnTopHitEnter(T obj, BeltConveyor hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnLeftHitEnter(T obj, BeltConveyor hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, BeltConveyor>.OnRightHitEnter(T obj, BeltConveyor hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnHitStay(T obj, BeltConveyor hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnBottomHitStay(T obj, BeltConveyor hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnTopHitStay(T obj, BeltConveyor hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnLeftHitStay(T obj, BeltConveyor hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnRightHitStay(T obj, BeltConveyor hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnHitExit(T obj, BeltConveyor hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnBottomHitExit(T obj, BeltConveyor hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnTopHitExit(T obj, BeltConveyor hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnLeftHitExit(T obj, BeltConveyor hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BeltConveyor>.OnRightHitExit(T obj, BeltConveyor hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, BeltConveyor collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, BeltConveyor collision) { }

    void ISubStateRbVisitor<T, PS, BeltConveyor>.OnTriggerEnter(T obj, PS parent, BeltConveyor collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BeltConveyor>.OnTriggerStay(T obj, PS parent, BeltConveyor collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BeltConveyor>.OnTriggerExit(T obj, PS parent, BeltConveyor collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BeltConveyor>.OnCollisionEnter(T obj, PS parent, BeltConveyor collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BeltConveyor>.OnCollisionStay(T obj, PS parent, BeltConveyor collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BeltConveyor>.OnCollisionExit(T obj, PS parent, BeltConveyor collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, BeltConveyor collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnHitStay(T obj, PS parent, BeltConveyor collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, BeltConveyor collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnHitExit(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, BeltConveyor collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, BeltConveyor collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, BeltConveyor collision) { }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnHitEnter(T obj, PS parent, BeltConveyor hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnBottomHitEnter(T obj, PS parent, BeltConveyor hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnTopHitEnter(T obj, PS parent, BeltConveyor hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnLeftHitEnter(T obj, PS parent, BeltConveyor hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnRightHitEnter(T obj, PS parent, BeltConveyor hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnHitStay(T obj, PS parent, BeltConveyor hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnBottomHitStay(T obj, PS parent, BeltConveyor hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnTopHitStay(T obj, PS parent, BeltConveyor hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnLeftHitStay(T obj, PS parent, BeltConveyor hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnRightHitStay(T obj, PS parent, BeltConveyor hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnHitExit(T obj, PS parent, BeltConveyor hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnBottomHitExit(T obj, PS parent, BeltConveyor hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnTopHitExit(T obj, PS parent, BeltConveyor hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnLeftHitExit(T obj, PS parent, BeltConveyor hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BeltConveyor>.OnRightHitExit(T obj, PS parent, BeltConveyor hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, BeltConveyor collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, BeltConveyor collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, BeltConveyor collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, BeltConveyor collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, BeltConveyor collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, BeltConveyor collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, BeltConveyor hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, BeltConveyor hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, BeltConveyor hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, BeltConveyor hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, BeltConveyor hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, BeltConveyor hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, BeltConveyor hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, BeltConveyor hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, BeltConveyor hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, BeltConveyor hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, BeltConveyor hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, BeltConveyor hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, BeltConveyor hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, BeltConveyor hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, BeltConveyor hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, BeltConveyor collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, BeltConveyor collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, BeltConveyor collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, BeltConveyor collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, BeltConveyor collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, BeltConveyor collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, BeltConveyor hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, BeltConveyor hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, BeltConveyor hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, BeltConveyor hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, BeltConveyor hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, BeltConveyor hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, BeltConveyor hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, BeltConveyor hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, BeltConveyor hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, BeltConveyor hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, BeltConveyor hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, BeltConveyor hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, BeltConveyor hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, BeltConveyor hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, BeltConveyor hit) => curState.OnRightHitExit(obj, parent, hit);
}


public partial class RbCollide
{
    void IRbVisitor<BeltConveyor>.OnCollisionEnter(BeltConveyor collision) => onCollisionEnterBeltConveyor?.Invoke(collision);
    void IRbVisitor<BeltConveyor>.OnCollisionExit(BeltConveyor collision) => onCollisionExitBeltConveyor?.Invoke(collision);
    void IRbVisitor<BeltConveyor>.OnCollisionStay(BeltConveyor collision) => onCollisionStayBeltConveyor?.Invoke(collision);
    void IRbVisitor<BeltConveyor>.OnTriggerEnter(BeltConveyor collision) => onTriggerEnterBeltConveyor?.Invoke(collision);
    void IRbVisitor<BeltConveyor>.OnTriggerExit(BeltConveyor collision) => onTriggerExitBeltConveyor?.Invoke(collision);
    void IRbVisitor<BeltConveyor>.OnTriggerStay(BeltConveyor collision) => onTriggerStayBeltConveyor?.Invoke(collision);

    public event Action<BeltConveyor> onCollisionEnterBeltConveyor;
    public event Action<BeltConveyor> onCollisionExitBeltConveyor;
    public event Action<BeltConveyor> onCollisionStayBeltConveyor;
    public event Action<BeltConveyor> onTriggerEnterBeltConveyor;
    public event Action<BeltConveyor> onTriggerExitBeltConveyor;
    public event Action<BeltConveyor> onTriggerStayBeltConveyor;
}

public partial class ExRbHit
{
    void IExRbVisitor<BeltConveyor>.OnHitEnter(BeltConveyor hit) => onHitEnterBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnBottomHitEnter(BeltConveyor hit) => onBottomHitEnterBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnTopHitEnter(BeltConveyor hit) => onTopHitEnterBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnLeftHitEnter(BeltConveyor hit) => onLeftHitEnterBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnRightHitEnter(BeltConveyor hit) => onRightHitEnterBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnHitStay(BeltConveyor hit) => onHitStayBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnBottomHitStay(BeltConveyor hit) => onBottomHitStayBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnTopHitStay(BeltConveyor hit) => onTopHitStayBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnLeftHitStay(BeltConveyor hit) => onLeftHitStayBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnRightHitStay(BeltConveyor hit) => onRightHitStayBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnHitExit(BeltConveyor hit) => onHitExitBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnBottomHitExit(BeltConveyor hit) => onBottomHitExitBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnTopHitExit(BeltConveyor hit) => onTopHitExitBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnLeftHitExit(BeltConveyor hit) => onLeftHitExitBeltConveyor?.Invoke(hit);
    void IExRbVisitor<BeltConveyor>.OnRightHitExit(BeltConveyor hit) => onRightHitExitBeltConveyor?.Invoke(hit);

    public event Action<BeltConveyor> onHitEnterBeltConveyor;
    public event Action<BeltConveyor> onBottomHitEnterBeltConveyor;
    public event Action<BeltConveyor> onTopHitEnterBeltConveyor;
    public event Action<BeltConveyor> onLeftHitEnterBeltConveyor;
    public event Action<BeltConveyor> onRightHitEnterBeltConveyor;
    public event Action<BeltConveyor> onHitStayBeltConveyor;
    public event Action<BeltConveyor> onBottomHitStayBeltConveyor;
    public event Action<BeltConveyor> onTopHitStayBeltConveyor;
    public event Action<BeltConveyor> onLeftHitStayBeltConveyor;
    public event Action<BeltConveyor> onRightHitStayBeltConveyor;
    public event Action<BeltConveyor> onHitExitBeltConveyor;
    public event Action<BeltConveyor> onBottomHitExitBeltConveyor;
    public event Action<BeltConveyor> onTopHitExitBeltConveyor;
    public event Action<BeltConveyor> onLeftHitExitBeltConveyor;
    public event Action<BeltConveyor> onRightHitExitBeltConveyor;

    void SetInterpreterBeltConveyor(IHitInterpreter hitInterpreter)
    {
        onHitEnterBeltConveyor = hitInterpreter.OnHitEnter;
        onBottomHitEnterBeltConveyor = hitInterpreter.OnBottomHitEnter;
        onTopHitEnterBeltConveyor = hitInterpreter.OnTopHitEnter;
        onLeftHitEnterBeltConveyor = hitInterpreter.OnLeftHitEnter;
        onRightHitEnterBeltConveyor = hitInterpreter.OnRightHitEnter;
        onHitStayBeltConveyor = hitInterpreter.OnHitStay;
        onBottomHitStayBeltConveyor = hitInterpreter.OnBottomHitStay;
        onTopHitStayBeltConveyor = hitInterpreter.OnTopHitStay;
        onLeftHitStayBeltConveyor = hitInterpreter.OnLeftHitStay;
        onRightHitStayBeltConveyor = hitInterpreter.OnRightHitStay;
        onHitExitBeltConveyor = hitInterpreter.OnHitExit;
        onBottomHitExitBeltConveyor = hitInterpreter.OnBottomHitExit;
        onTopHitExitBeltConveyor = hitInterpreter.OnTopHitExit;
        onLeftHitExitBeltConveyor = hitInterpreter.OnLeftHitExit;
        onRightHitExitBeltConveyor = hitInterpreter.OnRightHitExit;
    }
}

public partial interface IHitInterpreter : IHitInterpreter<BeltConveyor> { }