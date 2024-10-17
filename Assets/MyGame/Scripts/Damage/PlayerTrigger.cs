using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class PlayerTrigger : MonoBehaviour, ITriggerVisitable
{
    public virtual void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    public virtual void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    public virtual void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    public virtual void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    public virtual void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    public virtual void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);

    // ここから定義

    [SerializeField] PlayerController controller;

    public PlayerController PlayerController => controller;

}


public partial interface IStateTriggerVisitor<T>
     : IStateTriggerVisitor<T, PlayerTrigger>
{ }

public partial interface ISubStateTriggerVisitor<T, PS> : ISubStateTriggerVisitor<T, PS, PlayerTrigger>
{
}

public partial class BaseRbState<T, TS, SS, SM, G>
{
    virtual protected void OnTriggerEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnTriggerStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnTriggerExit(T obj, PlayerTrigger collision) { }

    virtual protected void OnCollisionEnter(T obj, PlayerTrigger collision) { }
    virtual protected void OnCollisionStay(T obj, PlayerTrigger collision) { }
    virtual protected void OnCollisionExit(T obj, PlayerTrigger collision) { }

    void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerEnter(T obj, PlayerTrigger collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerStay(T obj, PlayerTrigger collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerExit(T obj, PlayerTrigger collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionEnter(T obj, PlayerTrigger collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionStay(T obj, PlayerTrigger collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionExit(T obj, PlayerTrigger collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}

public partial class BaseRbSubState<T, TS, SS, SM, G, PS>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, PlayerTrigger collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, PlayerTrigger collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, PlayerTrigger collision) { }

    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerEnter(T obj, PS parent, PlayerTrigger collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerStay(T obj, PS parent, PlayerTrigger collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerExit(T obj, PS parent, PlayerTrigger collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionEnter(T obj, PS parent, PlayerTrigger collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionStay(T obj, PS parent, PlayerTrigger collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionExit(T obj, PS parent, PlayerTrigger collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionEnter(T obj, PlayerTrigger collision) => curState.OnCollisionEnter(obj, collision);
    void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionExit(T obj, PlayerTrigger collision) => curState.OnCollisionExit(obj, collision);
    void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionStay(T obj, PlayerTrigger collision) => curState.OnCollisionStay(obj, collision);
    void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerEnter(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerExit(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerStay(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
{
    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionEnter(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionExit(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionStay(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerEnter(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerExit(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerStay(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
}

public partial class BaseRbStateMachine<T, S, SM, G>
{
    void ITriggerVisitor<PlayerTrigger>.OnTriggerEnter(PlayerTrigger collision) => stateMachine.OnTriggerEnter((T)this, collision);
    void ITriggerVisitor<PlayerTrigger>.OnTriggerStay(PlayerTrigger collision) => stateMachine.OnTriggerStay((T)this, collision);
    void ITriggerVisitor<PlayerTrigger>.OnTriggerExit(PlayerTrigger collision) => stateMachine.OnTriggerExit((T)this, collision);
    void ITriggerVisitor<PlayerTrigger>.OnCollisionEnter(PlayerTrigger collision) => stateMachine.OnCollisionEnter((T)this, collision);
    void ITriggerVisitor<PlayerTrigger>.OnCollisionStay(PlayerTrigger collision) => stateMachine.OnCollisionStay((T)this, collision);
    void ITriggerVisitor<PlayerTrigger>.OnCollisionExit(PlayerTrigger collision) => stateMachine.OnCollisionExit((T)this, collision);
}

