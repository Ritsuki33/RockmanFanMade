using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class PlayerTrigger : MonoBehaviour, ITriggerVisitable, IHitVisitable
{

    protected virtual void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);

    protected virtual void AcceptOnHitEnter(IHitVisitor visitor) => visitor.OnHitEnter(this);
    protected virtual void AcceptOnHitStay(IHitVisitor visitor) => visitor.OnHitStay(this);
    protected virtual void AcceptOnHitExit(IHitVisitor visitor) => visitor.OnHitExit(this);
    protected virtual void AcceptOnBottomHitEnter(IHitVisitor visitor) => visitor.OnBottomHitEnter(this);
    protected virtual void AcceptOnBottomHitStay(IHitVisitor visitor) => visitor.OnBottomHitStay(this);
    protected virtual void AcceptOnBottomHitExit(IHitVisitor visitor) => visitor.OnBottomHitExit(this);
    protected virtual void AcceptOnTopHitEnter(IHitVisitor visitor) => visitor.OnTopHitEnter(this);
    protected virtual void AcceptOnTopHitStay(IHitVisitor visitor) => visitor.OnTopHitStay(this);
    protected virtual void AcceptOnTopHitExit(IHitVisitor visitor) => visitor.OnTopHitExit(this);
    protected virtual void AcceptOnLeftHitEnter(IHitVisitor visitor) => visitor.OnLeftHitEnter(this);
    protected virtual void AcceptOnLeftHitStay(IHitVisitor visitor) => visitor.OnLeftHitStay(this);
    protected virtual void AcceptOnLeftHitExit(IHitVisitor visitor) => visitor.OnLeftHitExit(this);
    protected virtual void AcceptOnRightHitEnter(IHitVisitor visitor) => visitor.OnRightHitEnter(this);
    protected virtual void AcceptOnRightHitStay(IHitVisitor visitor) => visitor.OnRightHitStay(this);
    protected virtual void AcceptOnRightHitExit(IHitVisitor visitor) => visitor.OnRightHitExit(this);

    void ITriggerVisitable.AcceptOnTriggerEnter(ITriggerVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void ITriggerVisitable.AcceptOnCollisionEnter(ITriggerVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void ITriggerVisitable.AcceptOnCollisionExit(ITriggerVisitor visitor) => AcceptOnCollisionExit(visitor);
    void ITriggerVisitable.AcceptOnCollisionStay(ITriggerVisitor visitor) => AcceptOnCollisionStay(visitor);
    void ITriggerVisitable.AcceptOnTriggerExit(ITriggerVisitor visitor) => AcceptOnTriggerExit(visitor);
    void ITriggerVisitable.AcceptOnTriggerStay(ITriggerVisitor visitor) => AcceptOnTriggerStay(visitor);

    void IHitVisitable.AcceptOnHitEnter(IHitVisitor visitor) => AcceptOnHitEnter(visitor);
    void IHitVisitable.AcceptOnHitStay(IHitVisitor visitor) => AcceptOnHitStay(visitor);
    void IHitVisitable.AcceptOnHitExit(IHitVisitor visitor) => AcceptOnHitExit(visitor);
    void IHitVisitable.AcceptOnBottomHitEnter(IHitVisitor visitor) => AcceptOnBottomHitEnter(visitor);
    void IHitVisitable.AcceptOnBottomHitStay(IHitVisitor visitor) => AcceptOnBottomHitStay(visitor);
    void IHitVisitable.AcceptOnBottomHitExit(IHitVisitor visitor) => AcceptOnBottomHitExit(visitor);
    void IHitVisitable.AcceptOnTopHitEnter(IHitVisitor visitor) => AcceptOnTopHitEnter(visitor);
    void IHitVisitable.AcceptOnTopHitStay(IHitVisitor visitor) => AcceptOnTopHitStay(visitor);
    void IHitVisitable.AcceptOnTopHitExit(IHitVisitor visitor) => AcceptOnTopHitExit(visitor);
    void IHitVisitable.AcceptOnLeftHitEnter(IHitVisitor visitor) => AcceptOnLeftHitEnter(visitor);
    void IHitVisitable.AcceptOnLeftHitStay(IHitVisitor visitor) => AcceptOnLeftHitStay(visitor);
    void IHitVisitable.AcceptOnLeftHitExit(IHitVisitor visitor) => AcceptOnLeftHitExit(visitor);
    void IHitVisitable.AcceptOnRightHitEnter(IHitVisitor visitor) => AcceptOnRightHitEnter(visitor);
    void IHitVisitable.AcceptOnRightHitStay(IHitVisitor visitor) => AcceptOnRightHitStay(visitor);
    void IHitVisitable.AcceptOnRightHitExit(IHitVisitor visitor) => AcceptOnRightHitExit(visitor);

    // ここから定義
    [SerializeField] PlayerController controller;

    public PlayerController PlayerController => controller;


}

public partial interface ITriggerVisitor : ITriggerVisitor<PlayerTrigger>
{ }

public partial interface IHitVisitor : IHitVisitor<PlayerTrigger>
{ }

public partial interface IStateTriggerVisitor<T>
     : IStateTriggerVisitor<T, PlayerTrigger>
{ }
public partial interface IStateHitVisitor<T>
     : IStateHitVisitor<T, PlayerTrigger>
{ }

public partial interface ISubStateTriggerVisitor<T, PS> : ISubStateTriggerVisitor<T, PS, PlayerTrigger>
{
}

public partial interface ISubStateHitVisitor<T, PS> : ISubStateHitVisitor<T, PS, PlayerTrigger>
{ }

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

public partial class BaseExRbState<T, TS, SS, SM, G>
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

    void IStateHitVisitor<T, PlayerTrigger>.OnHitEnter(T obj, PlayerTrigger hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateHitVisitor<T, PlayerTrigger>.OnBottomHitEnter(T obj, PlayerTrigger hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnTopHitEnter(T obj, PlayerTrigger hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnLeftHitEnter(T obj, PlayerTrigger hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateHitVisitor<T, PlayerTrigger>.OnRightHitEnter(T obj, PlayerTrigger hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnHitStay(T obj, PlayerTrigger hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnBottomHitStay(T obj, PlayerTrigger hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnTopHitStay(T obj, PlayerTrigger hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnLeftHitStay(T obj, PlayerTrigger hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnRightHitStay(T obj, PlayerTrigger hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnHitExit(T obj, PlayerTrigger hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnBottomHitExit(T obj, PlayerTrigger hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnTopHitExit(T obj, PlayerTrigger hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnLeftHitExit(T obj, PlayerTrigger hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, PlayerTrigger>.OnRightHitExit(T obj, PlayerTrigger hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
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

public partial class BaseExRbSubState<T, TS, SS, SM, G, PS>
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

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnBottomHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnTopHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnLeftHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnRightHitEnter(T obj, PS parent, PlayerTrigger hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnBottomHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnTopHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnLeftHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnRightHitStay(T obj, PS parent, PlayerTrigger hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnBottomHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnTopHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnLeftHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnRightHitExit(T obj, PS parent, PlayerTrigger hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine.OnRightHitExit(obj, this as TS, hit);
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
public partial class GenericExRbStateMachine<T, S> : GenericRbStateMachine<T, S>, IExRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
{

    void IStateHitVisitor<T, PlayerTrigger>.OnHitEnter(T obj, PlayerTrigger hit) => curState.OnHitEnter(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnBottomHitEnter(T obj, PlayerTrigger hit) => curState.OnBottomHitEnter(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnTopHitEnter(T obj, PlayerTrigger hit) => curState.OnTopHitEnter(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnLeftHitEnter(T obj, PlayerTrigger hit) => curState.OnLeftHitEnter(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnRightHitEnter(T obj, PlayerTrigger hit) => curState.OnRightHitEnter(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnHitStay(T obj, PlayerTrigger hit) => curState.OnHitStay(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnBottomHitStay(T obj, PlayerTrigger hit) => curState.OnBottomHitStay(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnTopHitStay(T obj, PlayerTrigger hit) => curState.OnTopHitStay(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnLeftHitStay(T obj, PlayerTrigger hit) => curState.OnLeftHitStay(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnRightHitStay(T obj, PlayerTrigger hit) => curState.OnRightHitStay(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnHitExit(T obj, PlayerTrigger hit) => curState.OnHitExit(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnBottomHitExit(T obj, PlayerTrigger hit) => curState.OnBottomHitExit(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnTopHitExit(T obj, PlayerTrigger hit) => curState.OnTopHitExit(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnLeftHitExit(T obj, PlayerTrigger hit) => curState.OnLeftHitExit(obj, hit);
    void IStateHitVisitor<T, PlayerTrigger>.OnRightHitExit(T obj, PlayerTrigger hit) => curState.OnRightHitExit(obj, hit);
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
public partial class GenericExRbSubStateMachine<T, S, PS>
{

    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnBottomHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnBottomHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnTopHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnTopHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnLeftHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnLeftHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnRightHitEnter(T obj, PS parent, PlayerTrigger hit) => curState.OnRightHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnBottomHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnBottomHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnTopHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnTopHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnLeftHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnLeftHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnRightHitStay(T obj, PS parent, PlayerTrigger hit) => curState.OnRightHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnBottomHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnBottomHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnTopHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnTopHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnLeftHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnLeftHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, PlayerTrigger>.OnRightHitExit(T obj, PS parent, PlayerTrigger hit) => curState.OnRightHitExit(obj, parent, hit);
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

public partial class ExRbStateMachine<T>
{

    void IHitVisitor<PlayerTrigger>.OnHitEnter(PlayerTrigger hit) => stateMachine.OnHitEnter((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnBottomHitEnter(PlayerTrigger hit) => stateMachine.OnBottomHitEnter((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnTopHitEnter(PlayerTrigger hit) => stateMachine.OnTopHitEnter((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnLeftHitEnter(PlayerTrigger hit) => stateMachine.OnLeftHitEnter((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnRightHitEnter(PlayerTrigger hit) => stateMachine.OnRightHitEnter((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnHitStay(PlayerTrigger hit) => stateMachine.OnHitStay((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnBottomHitStay(PlayerTrigger hit) => stateMachine.OnBottomHitStay((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnTopHitStay(PlayerTrigger hit) => stateMachine.OnTopHitStay((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnLeftHitStay(PlayerTrigger hit) => stateMachine.OnLeftHitStay((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnRightHitStay(PlayerTrigger hit) => stateMachine.OnRightHitStay((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnHitExit(PlayerTrigger hit) => stateMachine.OnHitExit((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnBottomHitExit(PlayerTrigger hit) => stateMachine.OnBottomHitExit((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnTopHitExit(PlayerTrigger hit) => stateMachine.OnTopHitExit((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnLeftHitExit(PlayerTrigger hit) => stateMachine.OnLeftHitExit((T)this, hit);
    void IHitVisitor<PlayerTrigger>.OnRightHitExit(PlayerTrigger hit) => stateMachine.OnRightHitExit((T)this, hit);
}