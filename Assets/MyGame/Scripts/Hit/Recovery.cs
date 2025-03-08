using System;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class Recovery : PhysicalObject, IHitEvent, IRbVisitable, IExRbVisitable
{
    #region 編集禁止
    protected virtual void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

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

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);

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
    #endregion

    // ここから定義
    [SerializeField, Header("回復量")] int amount = 3;
    [SerializeField] Gravity gravity;

    [SerializeField] ExpandRigidBody exRb;

    public int Amount => amount;

    protected override void Awake()
    {
        exRb.Init(this);
    }

    protected override void OnFixedUpdate()
    {
        gravity.OnUpdate();
        this.exRb.velocity = gravity.CurrentVelocity;

    }

    protected override void OnLateFixedUpdate()
    {
        this.exRb.FixedUpdate();
    }

    void IHitEvent.OnBottomHitStay(RaycastHit2D hit)
    {
        gravity.Reset();
    }

    private void OnDrawGizmos()
    {
        exRb.OnDrawGizmos();
    }
}

# region 編集禁止
public partial interface IRbVisitor
{
    void OnTriggerEnter(Recovery collision) { }
    void OnTriggerStay(Recovery collision) { }
    void OnTriggerExit(Recovery collision) { }

    void OnCollisionEnter(Recovery collision) { }
    void OnCollisionStay(Recovery collision) { }
    void OnCollisionExit(Recovery collision) { }
}

public partial interface IExRbVisitor
{
    void OnHitEnter(Recovery obj, RaycastHit2D hit) { }
    void OnBottomHitEnter(Recovery obj, RaycastHit2D hit) { }
    void OnTopHitEnter(Recovery obj, RaycastHit2D hit) { }
    void OnLeftHitEnter(Recovery obj, RaycastHit2D hit) { }
    void OnRightHitEnter(Recovery obj, RaycastHit2D hit) { }
    void OnHitStay(Recovery obj, RaycastHit2D hit) { }
    void OnBottomHitStay(Recovery obj, RaycastHit2D hit) { }
    void OnTopHitStay(Recovery obj, RaycastHit2D hit) { }
    void OnLeftHitStay(Recovery obj, RaycastHit2D hit) { }
    void OnRightHitStay(Recovery obj, RaycastHit2D hit) { }
    void OnHitExit(Recovery obj, RaycastHit2D hit) { }
    void OnBottomHitExit(Recovery obj, RaycastHit2D hit) { }
    void OnTopHitExit(Recovery obj, RaycastHit2D hit) { }
    void OnLeftHitExit(Recovery obj, RaycastHit2D hit) { }
    void OnRightHitExit(Recovery obj, RaycastHit2D hit) { }
}

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, Recovery>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, Recovery>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, Recovery>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, Recovery>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, Recovery collision) { }
    virtual protected void OnTriggerStay(T obj, Recovery collision) { }
    virtual protected void OnTriggerExit(T obj, Recovery collision) { }

    virtual protected void OnCollisionEnter(T obj, Recovery collision) { }
    virtual protected void OnCollisionStay(T obj, Recovery collision) { }
    virtual protected void OnCollisionExit(T obj, Recovery collision) { }

    void IStateRbVisitor<T, Recovery>.OnTriggerEnter(T obj, Recovery collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, Recovery>.OnTriggerStay(T obj, Recovery collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Recovery>.OnTriggerExit(T obj, Recovery collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, Recovery>.OnCollisionEnter(T obj, Recovery collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Recovery>.OnCollisionStay(T obj, Recovery collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Recovery>.OnCollisionExit(T obj, Recovery collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
{
    virtual protected void OnHitEnter(T obj, Recovery collision) { }
    virtual protected void OnBottomHitEnter(T obj, Recovery collision) { }
    virtual protected void OnTopHitEnter(T obj, Recovery collision) { }

    virtual protected void OnLeftHitEnter(T obj, Recovery collision) { }
    virtual protected void OnRightHitEnter(T obj, Recovery collision) { }
    virtual protected void OnHitStay(T obj, Recovery collision) { }

    virtual protected void OnBottomHitStay(T obj, Recovery collision) { }
    virtual protected void OnTopHitStay(T obj, Recovery collision) { }
    virtual protected void OnLeftHitStay(T obj, Recovery collision) { }

    virtual protected void OnRightHitStay(T obj, Recovery collision) { }
    virtual protected void OnHitExit(T obj, Recovery collision) { }
    virtual protected void OnBottomHitExit(T obj, Recovery collision) { }

    virtual protected void OnTopHitExit(T obj, Recovery collision) { }
    virtual protected void OnLeftHitExit(T obj, Recovery collision) { }
    virtual protected void OnRightHitExit(T obj, Recovery collision) { }

    void IStateExRbVisitor<T, Recovery>.OnHitEnter(T obj, Recovery hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, Recovery>.OnBottomHitEnter(T obj, Recovery hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnTopHitEnter(T obj, Recovery hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnLeftHitEnter(T obj, Recovery hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, Recovery>.OnRightHitEnter(T obj, Recovery hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnHitStay(T obj, Recovery hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnBottomHitStay(T obj, Recovery hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnTopHitStay(T obj, Recovery hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnLeftHitStay(T obj, Recovery hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnRightHitStay(T obj, Recovery hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnHitExit(T obj, Recovery hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnBottomHitExit(T obj, Recovery hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnTopHitExit(T obj, Recovery hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnLeftHitExit(T obj, Recovery hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Recovery>.OnRightHitExit(T obj, Recovery hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, Recovery collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, Recovery collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, Recovery collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, Recovery collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, Recovery collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, Recovery collision) { }

    void ISubStateRbVisitor<T, PS, Recovery>.OnTriggerEnter(T obj, PS parent, Recovery collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Recovery>.OnTriggerStay(T obj, PS parent, Recovery collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Recovery>.OnTriggerExit(T obj, PS parent, Recovery collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Recovery>.OnCollisionEnter(T obj, PS parent, Recovery collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Recovery>.OnCollisionStay(T obj, PS parent, Recovery collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Recovery>.OnCollisionExit(T obj, PS parent, Recovery collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PS parent, Recovery collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, Recovery collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, Recovery collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, Recovery collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, Recovery collision) { }
    virtual protected void OnHitStay(T obj, PS parent, Recovery collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, Recovery collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, Recovery collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, Recovery collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, Recovery collision) { }
    virtual protected void OnHitExit(T obj, PS parent, Recovery collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, Recovery collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, Recovery collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, Recovery collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, Recovery collision) { }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnHitEnter(T obj, PS parent, Recovery hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnBottomHitEnter(T obj, PS parent, Recovery hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnTopHitEnter(T obj, PS parent, Recovery hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnLeftHitEnter(T obj, PS parent, Recovery hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnRightHitEnter(T obj, PS parent, Recovery hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnHitStay(T obj, PS parent, Recovery hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnBottomHitStay(T obj, PS parent, Recovery hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnTopHitStay(T obj, PS parent, Recovery hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnLeftHitStay(T obj, PS parent, Recovery hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnRightHitStay(T obj, PS parent, Recovery hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnHitExit(T obj, PS parent, Recovery hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnBottomHitExit(T obj, PS parent, Recovery hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnTopHitExit(T obj, PS parent, Recovery hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnLeftHitExit(T obj, PS parent, Recovery hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Recovery>.OnRightHitExit(T obj, PS parent, Recovery hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, Recovery collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, Recovery collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, Recovery collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, Recovery collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, Recovery collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, Recovery collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, Recovery hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, Recovery hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, Recovery hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, Recovery hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, Recovery hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, Recovery hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, Recovery hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, Recovery hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, Recovery hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, Recovery hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, Recovery hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, Recovery hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, Recovery hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, Recovery hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, Recovery hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, Recovery collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, Recovery collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, Recovery collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, Recovery collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, Recovery collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, Recovery collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, Recovery hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, Recovery hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, Recovery hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, Recovery hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, Recovery hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, Recovery hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, Recovery hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, Recovery hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, Recovery hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, Recovery hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, Recovery hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, Recovery hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, Recovery hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, Recovery hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, Recovery hit) => curState.OnRightHitExit(obj, parent, hit);
}

#endregion
