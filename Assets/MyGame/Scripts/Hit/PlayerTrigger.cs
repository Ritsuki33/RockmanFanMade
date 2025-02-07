using System;
using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class PlayerTrigger : MonoBehaviour, IRbVisitable
{
    protected virtual void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);

    // ここから定義
    [SerializeField] StagePlayer player;

    public StagePlayer Player => player;

}

public partial interface IRbVisitor : IRbVisitor<PlayerTrigger>
{ }


public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, PlayerTrigger>
{ }


public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, PlayerTrigger>
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


public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, PlayerTrigger collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, PlayerTrigger collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, PlayerTrigger collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
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

