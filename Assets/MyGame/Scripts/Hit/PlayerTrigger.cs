using System;
using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class PlayerTrigger : MonoBehaviour, IRbVisitable, IExRbVisitable
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
    [SerializeField] StagePlayer player;

    public StagePlayer Player => player;


}

public partial interface IRbVisitor : IRbVisitor<PlayerTrigger>
{ }

public partial interface IExRbVisitor : IExRbVisitor<PlayerTrigger>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, PlayerTrigger>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, PlayerTrigger>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, PlayerTrigger>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, PlayerTrigger>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnTriggerStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnTriggerExit(T obj, PlayerTrigger collision) { }

    virtual protected void OnCollisionEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnCollisionStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnCollisionExit(T obj, PlayerTrigger collision) { }

    void IStateRbVisitor<T, PlayerTrigger>.OnTriggerEnter(T obj, PlayerTrigger collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, PlayerTrigger>.OnTriggerStay(T obj, PlayerTrigger collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, PlayerTrigger>.OnTriggerExit(T obj, PlayerTrigger collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, PlayerTrigger>.OnCollisionEnter(T obj, PlayerTrigger collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, PlayerTrigger>.OnCollisionStay(T obj, PlayerTrigger collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, PlayerTrigger>.OnCollisionExit(T obj, PlayerTrigger collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnBottomHitEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnTopHitEnter(T obj, PlayerTrigger collision) { }

    virtual protected void OnLeftHitEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnRightHitEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnHitStay(T obj, PlayerTrigger collision) { }

    virtual protected void OnBottomHitStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnTopHitStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnLeftHitStay(T obj, PlayerTrigger collision) { }

    virtual protected void OnRightHitStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnHitExit(T obj, PlayerTrigger collision) { }
    virtual protected void OnBottomHitExit(T obj, PlayerTrigger collision) { }

    virtual protected void OnTopHitExit(T obj, PlayerTrigger collision) { }
    virtual protected void OnLeftHitExit(T obj, PlayerTrigger collision) { }
    virtual protected void OnRightHitExit(T obj, PlayerTrigger collision) { }

    void IStateExRbVisitor<T, PlayerTrigger>.OnHitEnter(T obj, PlayerTrigger hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, PlayerTrigger>.OnBottomHitEnter(T obj, PlayerTrigger hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnTopHitEnter(T obj, PlayerTrigger hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnLeftHitEnter(T obj, PlayerTrigger hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, PlayerTrigger>.OnRightHitEnter(T obj, PlayerTrigger hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnHitStay(T obj, PlayerTrigger hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnBottomHitStay(T obj, PlayerTrigger hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnTopHitStay(T obj, PlayerTrigger hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnLeftHitStay(T obj, PlayerTrigger hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnRightHitStay(T obj, PlayerTrigger hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnHitExit(T obj, PlayerTrigger hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnBottomHitExit(T obj, PlayerTrigger hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnTopHitExit(T obj, PlayerTrigger hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnLeftHitExit(T obj, PlayerTrigger hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, PlayerTrigger>.OnRightHitExit(T obj, PlayerTrigger hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, PlayerTrigger collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, PlayerTrigger collision) { }

    void ISubStateRbVisitor<T, PS, PlayerTrigger>.OnTriggerEnter(T obj, PS parent, PlayerTrigger collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerTrigger>.OnTriggerStay(T obj, PS parent, PlayerTrigger collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerTrigger>.OnTriggerExit(T obj, PS parent, PlayerTrigger collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerTrigger>.OnCollisionEnter(T obj, PS parent, PlayerTrigger collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerTrigger>.OnCollisionStay(T obj, PS parent, PlayerTrigger collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerTrigger>.OnCollisionExit(T obj, PS parent, PlayerTrigger collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, PlayerTrigger collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnHitStay(T obj, PS parent, PlayerTrigger collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, PlayerTrigger collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnHitExit(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, PlayerTrigger collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, PlayerTrigger collision) { }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnBottomHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnTopHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnLeftHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnRightHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnBottomHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnTopHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnLeftHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnRightHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnBottomHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnTopHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnLeftHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, PlayerTrigger>.OnRightHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, PlayerTrigger collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, PlayerTrigger collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, PlayerTrigger collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, PlayerTrigger hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, PlayerTrigger hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, PlayerTrigger hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, PlayerTrigger hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, PlayerTrigger hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, PlayerTrigger hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, PlayerTrigger hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, PlayerTrigger hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, PlayerTrigger hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, PlayerTrigger hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, PlayerTrigger hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, PlayerTrigger hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, PlayerTrigger hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, PlayerTrigger hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, PlayerTrigger hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, PlayerTrigger collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, PlayerTrigger collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, PlayerTrigger collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnRightHitExit(obj, parent, hit);
}

public partial interface IHitInterpreter : IHitInterpreter<PlayerTrigger> { }