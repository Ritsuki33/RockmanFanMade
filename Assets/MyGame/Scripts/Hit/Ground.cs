using System;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class Ground : MonoBehaviour, IRbVisitable, IExRbVisitable
{
    #region 編集禁止
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
    #endregion

    // ここから定義
    [SerializeField, Range(0, 1),Header("摩擦力")] float friction;

    public float Friction => friction;
}

# region 編集禁止
public partial interface IRbVisitor : IRbVisitor<Ground>
{ }

public partial interface IExRbVisitor : IExRbVisitor<Ground>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, Ground>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, Ground>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, Ground>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, Ground>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, Ground collision) { }
    virtual protected void OnTriggerStay(T obj, Ground collision) { }
    virtual protected void OnTriggerExit(T obj, Ground collision) { }

    virtual protected void OnCollisionEnter(T obj, Ground collision) { }
    virtual protected void OnCollisionStay(T obj, Ground collision) { }
    virtual protected void OnCollisionExit(T obj, Ground collision) { }

    void IStateRbVisitor<T, Ground>.OnTriggerEnter(T obj, Ground collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, Ground>.OnTriggerStay(T obj, Ground collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Ground>.OnTriggerExit(T obj, Ground collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, Ground>.OnCollisionEnter(T obj, Ground collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Ground>.OnCollisionStay(T obj, Ground collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Ground>.OnCollisionExit(T obj, Ground collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


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


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, Ground collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, Ground collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, Ground collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, Ground collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, Ground collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, Ground collision) { }

    void ISubStateRbVisitor<T, PS, Ground>.OnTriggerEnter(T obj, PS parent, Ground collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Ground>.OnTriggerStay(T obj, PS parent, Ground collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Ground>.OnTriggerExit(T obj, PS parent, Ground collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Ground>.OnCollisionEnter(T obj, PS parent, Ground collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Ground>.OnCollisionStay(T obj, PS parent, Ground collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Ground>.OnCollisionExit(T obj, PS parent, Ground collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
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

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, Ground collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, Ground collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, Ground collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, Ground collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, Ground collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, Ground collision) => curState.OnTriggerEnter(obj, collision);
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

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, Ground collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, Ground collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, Ground collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, Ground collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, Ground collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, Ground collision) => curState?.OnTriggerStay(obj, parent, collision);
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


public partial class RbCollide
{
    void IRbVisitor<Ground>.OnCollisionEnter(Ground collision) => onCollisionEnterGround?.Invoke(collision);
    void IRbVisitor<Ground>.OnCollisionExit(Ground collision) => onCollisionExitGround?.Invoke(collision);
    void IRbVisitor<Ground>.OnCollisionStay(Ground collision) => onCollisionStayGround?.Invoke(collision);
    void IRbVisitor<Ground>.OnTriggerEnter(Ground collision) => onTriggerEnterGround?.Invoke(collision);
    void IRbVisitor<Ground>.OnTriggerExit(Ground collision) => onTriggerExitGround?.Invoke(collision);
    void IRbVisitor<Ground>.OnTriggerStay(Ground collision) => onTriggerStayGround?.Invoke(collision);

    public event Action<Ground> onCollisionEnterGround;
    public event Action<Ground> onCollisionExitGround;
    public event Action<Ground> onCollisionStayGround;
    public event Action<Ground> onTriggerEnterGround;
    public event Action<Ground> onTriggerExitGround;
    public event Action<Ground> onTriggerStayGround;
}

public partial class ExRbHit
{
    void IExRbVisitor<Ground>.OnHitEnter(Ground hit) => onHitEnterGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnBottomHitEnter(Ground hit) => onBottomHitEnterGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnTopHitEnter(Ground hit) => onTopHitEnterGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnLeftHitEnter(Ground hit) => onLeftHitEnterGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnRightHitEnter(Ground hit) => onRightHitEnterGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnHitStay(Ground hit) => onHitStayGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnBottomHitStay(Ground hit) => onBottomHitStayGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnTopHitStay(Ground hit) => onTopHitStayGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnLeftHitStay(Ground hit) => onLeftHitStayGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnRightHitStay(Ground hit) => onRightHitStayGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnHitExit(Ground hit) => onHitExitGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnBottomHitExit(Ground hit) => onBottomHitExitGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnTopHitExit(Ground hit) => onTopHitExitGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnLeftHitExit(Ground hit) => onLeftHitExitGround?.Invoke(hit);
    void IExRbVisitor<Ground>.OnRightHitExit(Ground hit) => onRightHitExitGround?.Invoke(hit);

    public event Action<Ground> onHitEnterGround;
    public event Action<Ground> onBottomHitEnterGround;
    public event Action<Ground> onTopHitEnterGround;
    public event Action<Ground> onLeftHitEnterGround;
    public event Action<Ground> onRightHitEnterGround;
    public event Action<Ground> onHitStayGround;
    public event Action<Ground> onBottomHitStayGround;
    public event Action<Ground> onTopHitStayGround;
    public event Action<Ground> onLeftHitStayGround;
    public event Action<Ground> onRightHitStayGround;
    public event Action<Ground> onHitExitGround;
    public event Action<Ground> onBottomHitExitGround;
    public event Action<Ground> onTopHitExitGround;
    public event Action<Ground> onLeftHitExitGround;
    public event Action<Ground> onRightHitExitGround;

    void SetInterpreterGround(IHitInterpreter hitInterpreter)
    {
        onHitEnterGround = hitInterpreter.OnHitEnter;
        onBottomHitEnterGround = hitInterpreter.OnBottomHitEnter;
        onTopHitEnterGround = hitInterpreter.OnTopHitEnter;
        onLeftHitEnterGround = hitInterpreter.OnLeftHitEnter;
        onRightHitEnterGround = hitInterpreter.OnRightHitEnter;
        onHitStayGround = hitInterpreter.OnHitStay;
        onBottomHitStayGround = hitInterpreter.OnBottomHitStay;
        onTopHitStayGround = hitInterpreter.OnTopHitStay;
        onLeftHitStayGround = hitInterpreter.OnLeftHitStay;
        onRightHitStayGround = hitInterpreter.OnRightHitStay;
        onHitExitGround = hitInterpreter.OnHitExit;
        onBottomHitExitGround = hitInterpreter.OnBottomHitExit;
        onTopHitExitGround = hitInterpreter.OnTopHitExit;
        onLeftHitExitGround = hitInterpreter.OnLeftHitExit;
        onRightHitExitGround = hitInterpreter.OnRightHitExit;
    }
}

public partial interface IHitInterpreter : IHitInterpreter<Ground> { }

#endregion