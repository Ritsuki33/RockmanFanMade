using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class DamageBase : MonoBehaviour, ITriggerVisitable, IHitVisitable
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
    [SerializeField] public int baseDamageValue = 3;



}

public partial interface ITriggerVisitor : ITriggerVisitor<DamageBase>
{ }

public partial interface IHitVisitor : IHitVisitor<DamageBase>
{ }

public partial interface IStateTriggerVisitor<T>
     : IStateTriggerVisitor<T, DamageBase>
{ }
public partial interface IStateHitVisitor<T>
     : IStateHitVisitor<T, DamageBase>
{ }

public partial interface ISubStateTriggerVisitor<T, PS> : ISubStateTriggerVisitor<T, PS, DamageBase>
{
}

public partial interface ISubStateHitVisitor<T, PS> : ISubStateHitVisitor<T, PS, DamageBase>
{ }

public partial class BaseRbState<T, TS, SS, SM, G>
{
    virtual protected void OnTriggerEnter(T obj, DamageBase collision) { }
    virtual protected void OnTriggerStay(T obj, DamageBase collision) { }
    virtual protected void OnTriggerExit(T obj, DamageBase collision) { }

    virtual protected void OnCollisionEnter(T obj, DamageBase collision) { }
    virtual protected void OnCollisionStay(T obj, DamageBase collision) { }
    virtual protected void OnCollisionExit(T obj, DamageBase collision) { }

    void IStateTriggerVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateTriggerVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateTriggerVisitor<T, DamageBase>.OnCollisionEnter(T obj, DamageBase collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, DamageBase>.OnCollisionStay(T obj, DamageBase collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, DamageBase>.OnCollisionExit(T obj, DamageBase collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}

public partial class BaseExRbState<T, TS, SS, SM, G>
{
    virtual protected void OnHitEnter(T obj, DamageBase collision) { }
    virtual protected void OnBottomHitEnter(T obj, DamageBase collision) { }
    virtual protected void OnTopHitEnter(T obj, DamageBase collision) { }

    virtual protected void OnLeftHitEnter(T obj, DamageBase collision) { }
    virtual protected void OnRightHitEnter(T obj, DamageBase collision) { }
    virtual protected void OnHitStay(T obj, DamageBase collision) { }

    virtual protected void OnBottomHitStay(T obj, DamageBase collision) { }
    virtual protected void OnTopHitStay(T obj, DamageBase collision) { }
    virtual protected void OnLeftHitStay(T obj, DamageBase collision) { }

    virtual protected void OnRightHitStay(T obj, DamageBase collision) { }
    virtual protected void OnHitExit(T obj, DamageBase collision) { }
    virtual protected void OnBottomHitExit(T obj, DamageBase collision) { }

    virtual protected void OnTopHitExit(T obj, DamageBase collision) { }
    virtual protected void OnLeftHitExit(T obj, DamageBase collision) { }
    virtual protected void OnRightHitExit(T obj, DamageBase collision) { }

    void IStateHitVisitor<T, DamageBase>.OnHitEnter(T obj, DamageBase hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateHitVisitor<T, DamageBase>.OnBottomHitEnter(T obj, DamageBase hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnTopHitEnter(T obj, DamageBase hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnLeftHitEnter(T obj, DamageBase hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateHitVisitor<T, DamageBase>.OnRightHitEnter(T obj, DamageBase hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnHitStay(T obj, DamageBase hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnBottomHitStay(T obj, DamageBase hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnTopHitStay(T obj, DamageBase hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnLeftHitStay(T obj, DamageBase hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnRightHitStay(T obj, DamageBase hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnHitExit(T obj, DamageBase hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnBottomHitExit(T obj, DamageBase hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnTopHitExit(T obj, DamageBase hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnLeftHitExit(T obj, DamageBase hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, DamageBase>.OnRightHitExit(T obj, DamageBase hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class BaseRbSubState<T, TS, SS, SM, G, PS>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, DamageBase collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, DamageBase collision) { }

    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerEnter(T obj, PS parent, DamageBase collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerStay(T obj, PS parent, DamageBase collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerExit(T obj, PS parent, DamageBase collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionEnter(T obj, PS parent, DamageBase collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionStay(T obj, PS parent, DamageBase collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionExit(T obj, PS parent, DamageBase collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class BaseExRbSubState<T, TS, SS, SM, G, PS>
{
    virtual protected void OnHitEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, DamageBase collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnHitStay(T obj, PS parent, DamageBase collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, DamageBase collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnHitExit(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, DamageBase collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, DamageBase collision) { }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnBottomHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnTopHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnLeftHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnRightHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnHitStay(T obj, PS parent, DamageBase hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnBottomHitStay(T obj, PS parent, DamageBase hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnTopHitStay(T obj, PS parent, DamageBase hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnLeftHitStay(T obj, PS parent, DamageBase hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnRightHitStay(T obj, PS parent, DamageBase hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnHitExit(T obj, PS parent, DamageBase hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnBottomHitExit(T obj, PS parent, DamageBase hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnTopHitExit(T obj, PS parent, DamageBase hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnLeftHitExit(T obj, PS parent, DamageBase hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, DamageBase>.OnRightHitExit(T obj, PS parent, DamageBase hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    void IStateTriggerVisitor<T, DamageBase>.OnCollisionEnter(T obj, DamageBase collision) => curState.OnCollisionEnter(obj, collision);
    void IStateTriggerVisitor<T, DamageBase>.OnCollisionExit(T obj, DamageBase collision) => curState.OnCollisionExit(obj, collision);
    void IStateTriggerVisitor<T, DamageBase>.OnCollisionStay(T obj, DamageBase collision) => curState.OnCollisionStay(obj, collision);
    void IStateTriggerVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
}
public partial class GenericExRbStateMachine<T, S> : GenericRbStateMachine<T, S>, IExRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
{

    void IStateHitVisitor<T, DamageBase>.OnHitEnter(T obj, DamageBase hit) => curState.OnHitEnter(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnBottomHitEnter(T obj, DamageBase hit) => curState.OnBottomHitEnter(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnTopHitEnter(T obj, DamageBase hit) => curState.OnTopHitEnter(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnLeftHitEnter(T obj, DamageBase hit) => curState.OnLeftHitEnter(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnRightHitEnter(T obj, DamageBase hit) => curState.OnRightHitEnter(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnHitStay(T obj, DamageBase hit) => curState.OnHitStay(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnBottomHitStay(T obj, DamageBase hit) => curState.OnBottomHitStay(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnTopHitStay(T obj, DamageBase hit) => curState.OnTopHitStay(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnLeftHitStay(T obj, DamageBase hit) => curState.OnLeftHitStay(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnRightHitStay(T obj, DamageBase hit) => curState.OnRightHitStay(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnHitExit(T obj, DamageBase hit) => curState.OnHitExit(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnBottomHitExit(T obj, DamageBase hit) => curState.OnBottomHitExit(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnTopHitExit(T obj, DamageBase hit) => curState.OnTopHitExit(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnLeftHitExit(T obj, DamageBase hit) => curState.OnLeftHitExit(obj, hit);
    void IStateHitVisitor<T, DamageBase>.OnRightHitExit(T obj, DamageBase hit) => curState.OnRightHitExit(obj, hit);
}

public partial class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
{
    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
}
public partial class GenericExRbSubStateMachine<T, S, PS>
{

    void ISubStateHitVisitor<T, PS, DamageBase>.OnHitEnter(T obj, PS parent, DamageBase hit) => curState.OnHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnBottomHitEnter(T obj, PS parent, DamageBase hit) => curState.OnBottomHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnTopHitEnter(T obj, PS parent, DamageBase hit) => curState.OnTopHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnLeftHitEnter(T obj, PS parent, DamageBase hit) => curState.OnLeftHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnRightHitEnter(T obj, PS parent, DamageBase hit) => curState.OnRightHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnHitStay(T obj, PS parent, DamageBase hit) => curState.OnHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnBottomHitStay(T obj, PS parent, DamageBase hit) => curState.OnBottomHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnTopHitStay(T obj, PS parent, DamageBase hit) => curState.OnTopHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnLeftHitStay(T obj, PS parent, DamageBase hit) => curState.OnLeftHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnRightHitStay(T obj, PS parent, DamageBase hit) => curState.OnRightHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnHitExit(T obj, PS parent, DamageBase hit) => curState.OnHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnBottomHitExit(T obj, PS parent, DamageBase hit) => curState.OnBottomHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnTopHitExit(T obj, PS parent, DamageBase hit) => curState.OnTopHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnLeftHitExit(T obj, PS parent, DamageBase hit) => curState.OnLeftHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, DamageBase>.OnRightHitExit(T obj, PS parent, DamageBase hit) => curState.OnRightHitExit(obj, parent, hit);
}
public partial class BaseRbStateMachine<T, S, SM, G>
{
    void ITriggerVisitor<DamageBase>.OnTriggerEnter(DamageBase collision) => stateMachine.OnTriggerEnter((T)this, collision);
    void ITriggerVisitor<DamageBase>.OnTriggerStay(DamageBase collision) => stateMachine.OnTriggerStay((T)this, collision);
    void ITriggerVisitor<DamageBase>.OnTriggerExit(DamageBase collision) => stateMachine.OnTriggerExit((T)this, collision);
    void ITriggerVisitor<DamageBase>.OnCollisionEnter(DamageBase collision) => stateMachine.OnCollisionEnter((T)this, collision);
    void ITriggerVisitor<DamageBase>.OnCollisionStay(DamageBase collision) => stateMachine.OnCollisionStay((T)this, collision);
    void ITriggerVisitor<DamageBase>.OnCollisionExit(DamageBase collision) => stateMachine.OnCollisionExit((T)this, collision);
}

public partial class ExRbStateMachine<T>
{

    void IHitVisitor<DamageBase>.OnHitEnter(DamageBase hit) => stateMachine.OnHitEnter((T)this, hit);
    void IHitVisitor<DamageBase>.OnBottomHitEnter(DamageBase hit) => stateMachine.OnBottomHitEnter((T)this, hit);
    void IHitVisitor<DamageBase>.OnTopHitEnter(DamageBase hit) => stateMachine.OnTopHitEnter((T)this, hit);
    void IHitVisitor<DamageBase>.OnLeftHitEnter(DamageBase hit) => stateMachine.OnLeftHitEnter((T)this, hit);
    void IHitVisitor<DamageBase>.OnRightHitEnter(DamageBase hit) => stateMachine.OnRightHitEnter((T)this, hit);
    void IHitVisitor<DamageBase>.OnHitStay(DamageBase hit) => stateMachine.OnHitStay((T)this, hit);
    void IHitVisitor<DamageBase>.OnBottomHitStay(DamageBase hit) => stateMachine.OnBottomHitStay((T)this, hit);
    void IHitVisitor<DamageBase>.OnTopHitStay(DamageBase hit) => stateMachine.OnTopHitStay((T)this, hit);
    void IHitVisitor<DamageBase>.OnLeftHitStay(DamageBase hit) => stateMachine.OnLeftHitStay((T)this, hit);
    void IHitVisitor<DamageBase>.OnRightHitStay(DamageBase hit) => stateMachine.OnRightHitStay((T)this, hit);
    void IHitVisitor<DamageBase>.OnHitExit(DamageBase hit) => stateMachine.OnHitExit((T)this, hit);
    void IHitVisitor<DamageBase>.OnBottomHitExit(DamageBase hit) => stateMachine.OnBottomHitExit((T)this, hit);
    void IHitVisitor<DamageBase>.OnTopHitExit(DamageBase hit) => stateMachine.OnTopHitExit((T)this, hit);
    void IHitVisitor<DamageBase>.OnLeftHitExit(DamageBase hit) => stateMachine.OnLeftHitExit((T)this, hit);
    void IHitVisitor<DamageBase>.OnRightHitExit(DamageBase hit) => stateMachine.OnRightHitExit((T)this, hit);
}