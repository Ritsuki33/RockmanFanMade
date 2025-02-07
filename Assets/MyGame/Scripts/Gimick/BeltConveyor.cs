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
public class BeltConveyor : PhysicalObject, IExRbVisitable
{
    protected virtual void AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitEnter(this, hit);
    protected virtual void AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitStay(this, hit);
    protected virtual void AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitExit(this, hit);
    protected virtual void AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitEnter(this, hit);
    protected virtual void AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitStay(this, hit);
    protected virtual void AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitExit(this, hit);
    protected virtual void AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitEnter(this, hit);
    protected virtual void AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitStay(this, hit);
    protected virtual void AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitExit(this, hit);
    protected virtual void AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitEnter(this, hit);
    protected virtual void AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitStay(this, hit);
    protected virtual void AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitExit(this, hit);
    protected virtual void AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitEnter(this, hit);
    protected virtual void AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitStay(this, hit);
    protected virtual void AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitExit(this, hit);

    void IExRbVisitable.AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitExit(visitor, hit);

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

    protected override void Awake()
    {
        base.Awake();
        actualWorldSpriteSize = spriteRenderer.sprite.rect.size / spriteRenderer.sprite.pixelsPerUnit;
    }
    private void OnValidate()
    {
        spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
        boxCollider.size = new Vector2(width, spriteRenderer.size.y);
    }

    protected override void OnFixedUpdate()
    {
        scrollSpriteController.Scroll(Vector2.right * offset);
        offset += speed * Time.fixedDeltaTime;
        offset %= 1.0f;
    }

    public void GetOn(IBeltConveyorVelocity obj)
    {
        obj.velocity += speed * actualWorldSpriteSize.x * Vector2.left;
    }
}

public partial interface IExRbVisitor : IExRbVisitor<BeltConveyor>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, BeltConveyor>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, BeltConveyor>
{ }

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

