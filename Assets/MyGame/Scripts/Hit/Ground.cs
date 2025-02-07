using System;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class Ground : MonoBehaviour, IExRbVisitable
{
    #region 編集禁止
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
    #endregion

    // ここから定義
    [SerializeField, Range(0, 1), Header("摩擦力")] float friction;

    public float Friction => friction;
}

# region 編集禁止
public partial interface IExRbVisitor : IExRbVisitor<Ground>{ }
public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, Ground>{ }
public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, Ground>{ }

public partial class InheritExRbState<T, TS, SM, S>
{
    virtual protected void OnHitEnter(T obj, Ground collision) { }
    virtual protected void OnBottomHitEnter(T obj, Ground collision) { }
    virtual protected void OnTopHitEnter(T obj, Ground collision) { }

    virtual protected void OnLeftHitEnter(T obj, Ground collision) { }
    virtual protected void OnRightHitEnter(T obj, Ground collision) { }
    virtual protected void OnHitStay(T obj, Ground collision) { }

    virtual protected void OnBottomHitStay(T obj, Ground collision) { }
    virtual protected void OnTopHitStay(T obj, Ground collision) { }
    virtual protected void OnLeftHitStay(T obj, Ground collision) { }

    virtual protected void OnRightHitStay(T obj, Ground collision) { }
    virtual protected void OnHitExit(T obj, Ground collision) { }
    virtual protected void OnBottomHitExit(T obj, Ground collision) { }

    virtual protected void OnTopHitExit(T obj, Ground collision) { }
    virtual protected void OnLeftHitExit(T obj, Ground collision) { }
    virtual protected void OnRightHitExit(T obj, Ground collision) { }

    void IStateExRbVisitor<T, Ground>.OnHitEnter(T obj, Ground hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, Ground>.OnBottomHitEnter(T obj, Ground hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnTopHitEnter(T obj, Ground hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnLeftHitEnter(T obj, Ground hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, Ground>.OnRightHitEnter(T obj, Ground hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnHitStay(T obj, Ground hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnBottomHitStay(T obj, Ground hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnTopHitStay(T obj, Ground hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnLeftHitStay(T obj, Ground hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnRightHitStay(T obj, Ground hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnHitExit(T obj, Ground hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnBottomHitExit(T obj, Ground hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnTopHitExit(T obj, Ground hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnLeftHitExit(T obj, Ground hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Ground>.OnRightHitExit(T obj, Ground hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritExRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PS parent, Ground collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, Ground collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, Ground collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, Ground collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, Ground collision) { }
    virtual protected void OnHitStay(T obj, PS parent, Ground collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, Ground collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, Ground collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, Ground collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, Ground collision) { }
    virtual protected void OnHitExit(T obj, PS parent, Ground collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, Ground collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, Ground collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, Ground collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, Ground collision) { }

    void ISubStateExRbVisitor<T, PS, Ground>.OnHitEnter(T obj, PS parent, Ground hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnBottomHitEnter(T obj, PS parent, Ground hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnTopHitEnter(T obj, PS parent, Ground hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnLeftHitEnter(T obj, PS parent, Ground hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnRightHitEnter(T obj, PS parent, Ground hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnHitStay(T obj, PS parent, Ground hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnBottomHitStay(T obj, PS parent, Ground hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnTopHitStay(T obj, PS parent, Ground hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnLeftHitStay(T obj, PS parent, Ground hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnRightHitStay(T obj, PS parent, Ground hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnHitExit(T obj, PS parent, Ground hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnBottomHitExit(T obj, PS parent, Ground hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnTopHitExit(T obj, PS parent, Ground hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnLeftHitExit(T obj, PS parent, Ground hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Ground>.OnRightHitExit(T obj, PS parent, Ground hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, Ground hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, Ground hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, Ground hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, Ground hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, Ground hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, Ground hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, Ground hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, Ground hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, Ground hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, Ground hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, Ground hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, Ground hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, Ground hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, Ground hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, Ground hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, Ground hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, Ground hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, Ground hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, Ground hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, Ground hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, Ground hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, Ground hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, Ground hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, Ground hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, Ground hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, Ground hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, Ground hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, Ground hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, Ground hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, Ground hit) => curState.OnRightHitExit(obj, parent, hit);
}

#endregion