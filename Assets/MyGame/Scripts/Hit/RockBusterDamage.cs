using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class RockBusterDamage : DamageBase
{

    protected override void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    protected override void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    protected override void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    protected override void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    protected override void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    protected override void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);

    protected override void AcceptOnHitEnter(IHitVisitor visitor) => visitor.OnHitEnter(this);
    protected override void AcceptOnHitStay(IHitVisitor visitor) => visitor.OnHitStay(this);
    protected override void AcceptOnHitExit(IHitVisitor visitor) => visitor.OnHitExit(this);
    protected override void AcceptOnBottomHitEnter(IHitVisitor visitor) => visitor.OnBottomHitEnter(this);
    protected override void AcceptOnBottomHitStay(IHitVisitor visitor) => visitor.OnBottomHitStay(this);
    protected override void AcceptOnBottomHitExit(IHitVisitor visitor) => visitor.OnBottomHitExit(this);
    protected override void AcceptOnTopHitEnter(IHitVisitor visitor) => visitor.OnTopHitEnter(this);
    protected override void AcceptOnTopHitStay(IHitVisitor visitor) => visitor.OnTopHitStay(this);
    protected override void AcceptOnTopHitExit(IHitVisitor visitor) => visitor.OnTopHitExit(this);
    protected override void AcceptOnLeftHitEnter(IHitVisitor visitor) => visitor.OnLeftHitEnter(this);
    protected override void AcceptOnLeftHitStay(IHitVisitor visitor) => visitor.OnLeftHitStay(this);
    protected override void AcceptOnLeftHitExit(IHitVisitor visitor) => visitor.OnLeftHitExit(this);
    protected override void AcceptOnRightHitEnter(IHitVisitor visitor) => visitor.OnRightHitEnter(this);
    protected override void AcceptOnRightHitStay(IHitVisitor visitor) => visitor.OnRightHitStay(this);
    protected override void AcceptOnRightHitExit(IHitVisitor visitor) => visitor.OnRightHitExit(this);

    // ここから定義
    [SerializeField] public ProjectileReusable projectile;

    public void DeleteBuster() => projectile.Delete();


}

public partial interface ITriggerVisitor : ITriggerVisitor<RockBusterDamage>
{ }

public partial interface IHitVisitor : IHitVisitor<RockBusterDamage>
{ }

public partial interface IStateTriggerVisitor<T>
     : IStateTriggerVisitor<T, RockBusterDamage>
{ }
public partial interface IStateHitVisitor<T>
     : IStateHitVisitor<T, RockBusterDamage>
{ }

public partial interface ISubStateTriggerVisitor<T, PS> : ISubStateTriggerVisitor<T, PS, RockBusterDamage>
{
}

public partial interface ISubStateHitVisitor<T, PS> : ISubStateHitVisitor<T, PS, RockBusterDamage>
{ }

public partial class BaseRbState<T, TS, SS, SM, G>
{
    virtual protected void OnTriggerEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnTriggerStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnTriggerExit(T obj, RockBusterDamage collision) { }

    virtual protected void OnCollisionEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnCollisionStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnCollisionExit(T obj, RockBusterDamage collision) { }

    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerEnter(T obj, RockBusterDamage collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerStay(T obj, RockBusterDamage collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerExit(T obj, RockBusterDamage collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionEnter(T obj, RockBusterDamage collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionStay(T obj, RockBusterDamage collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionExit(T obj, RockBusterDamage collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}

public partial class BaseExRbState<T, TS, SS, SM, G>
{
    virtual protected void OnHitEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnBottomHitEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnTopHitEnter(T obj, RockBusterDamage collision) { }

    virtual protected void OnLeftHitEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnRightHitEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnHitStay(T obj, RockBusterDamage collision) { }

    virtual protected void OnBottomHitStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnTopHitStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnLeftHitStay(T obj, RockBusterDamage collision) { }

    virtual protected void OnRightHitStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnHitExit(T obj, RockBusterDamage collision) { }
    virtual protected void OnBottomHitExit(T obj, RockBusterDamage collision) { }

    virtual protected void OnTopHitExit(T obj, RockBusterDamage collision) { }
    virtual protected void OnLeftHitExit(T obj, RockBusterDamage collision) { }
    virtual protected void OnRightHitExit(T obj, RockBusterDamage collision) { }

    void IStateHitVisitor<T, RockBusterDamage>.OnHitEnter(T obj, RockBusterDamage hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateHitVisitor<T, RockBusterDamage>.OnBottomHitEnter(T obj, RockBusterDamage hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnTopHitEnter(T obj, RockBusterDamage hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnLeftHitEnter(T obj, RockBusterDamage hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateHitVisitor<T, RockBusterDamage>.OnRightHitEnter(T obj, RockBusterDamage hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnHitStay(T obj, RockBusterDamage hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnBottomHitStay(T obj, RockBusterDamage hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnTopHitStay(T obj, RockBusterDamage hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnLeftHitStay(T obj, RockBusterDamage hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnRightHitStay(T obj, RockBusterDamage hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnHitExit(T obj, RockBusterDamage hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnBottomHitExit(T obj, RockBusterDamage hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnTopHitExit(T obj, RockBusterDamage hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnLeftHitExit(T obj, RockBusterDamage hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateHitVisitor<T, RockBusterDamage>.OnRightHitExit(T obj, RockBusterDamage hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class BaseRbSubState<T, TS, SS, SM, G, PS>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, RockBusterDamage collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, RockBusterDamage collision) { }

    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerEnter(T obj, PS parent, RockBusterDamage collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerStay(T obj, PS parent, RockBusterDamage collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerExit(T obj, PS parent, RockBusterDamage collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionEnter(T obj, PS parent, RockBusterDamage collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionStay(T obj, PS parent, RockBusterDamage collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionExit(T obj, PS parent, RockBusterDamage collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class BaseExRbSubState<T, TS, SS, SM, G, PS>
{
    virtual protected void OnHitEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, RockBusterDamage collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnHitStay(T obj, PS parent, RockBusterDamage collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, RockBusterDamage collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnHitExit(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, RockBusterDamage collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, RockBusterDamage collision) { }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnBottomHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnTopHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnLeftHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnRightHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnBottomHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnTopHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnLeftHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnRightHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnBottomHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnTopHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnLeftHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnRightHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionEnter(T obj, RockBusterDamage collision) => curState.OnCollisionEnter(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionExit(T obj, RockBusterDamage collision) => curState.OnCollisionExit(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionStay(T obj, RockBusterDamage collision) => curState.OnCollisionStay(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerEnter(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerExit(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerStay(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
}
public partial class GenericExRbStateMachine<T, S> : GenericRbStateMachine<T, S>, IExRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
{

    void IStateHitVisitor<T, RockBusterDamage>.OnHitEnter(T obj, RockBusterDamage hit) => curState.OnHitEnter(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnBottomHitEnter(T obj, RockBusterDamage hit) => curState.OnBottomHitEnter(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnTopHitEnter(T obj, RockBusterDamage hit) => curState.OnTopHitEnter(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnLeftHitEnter(T obj, RockBusterDamage hit) => curState.OnLeftHitEnter(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnRightHitEnter(T obj, RockBusterDamage hit) => curState.OnRightHitEnter(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnHitStay(T obj, RockBusterDamage hit) => curState.OnHitStay(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnBottomHitStay(T obj, RockBusterDamage hit) => curState.OnBottomHitStay(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnTopHitStay(T obj, RockBusterDamage hit) => curState.OnTopHitStay(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnLeftHitStay(T obj, RockBusterDamage hit) => curState.OnLeftHitStay(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnRightHitStay(T obj, RockBusterDamage hit) => curState.OnRightHitStay(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnHitExit(T obj, RockBusterDamage hit) => curState.OnHitExit(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnBottomHitExit(T obj, RockBusterDamage hit) => curState.OnBottomHitExit(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnTopHitExit(T obj, RockBusterDamage hit) => curState.OnTopHitExit(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnLeftHitExit(T obj, RockBusterDamage hit) => curState.OnLeftHitExit(obj, hit);
    void IStateHitVisitor<T, RockBusterDamage>.OnRightHitExit(T obj, RockBusterDamage hit) => curState.OnRightHitExit(obj, hit);
}

public partial class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
{
    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionEnter(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionExit(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionStay(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerEnter(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerExit(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerStay(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
}
public partial class GenericExRbSubStateMachine<T, S, PS>
{

    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnBottomHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnBottomHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnTopHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnTopHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnLeftHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnLeftHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnRightHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnRightHitEnter(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnBottomHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnBottomHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnTopHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnTopHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnLeftHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnLeftHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnRightHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnRightHitStay(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnBottomHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnBottomHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnTopHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnTopHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnLeftHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnLeftHitExit(obj, parent, hit);
    void ISubStateHitVisitor<T, PS, RockBusterDamage>.OnRightHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnRightHitExit(obj, parent, hit);
}
public partial class BaseRbStateMachine<T, S, SM, G>
{
    void ITriggerVisitor<RockBusterDamage>.OnTriggerEnter(RockBusterDamage collision) => stateMachine.OnTriggerEnter((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnTriggerStay(RockBusterDamage collision) => stateMachine.OnTriggerStay((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnTriggerExit(RockBusterDamage collision) => stateMachine.OnTriggerExit((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnCollisionEnter(RockBusterDamage collision) => stateMachine.OnCollisionEnter((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnCollisionStay(RockBusterDamage collision) => stateMachine.OnCollisionStay((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnCollisionExit(RockBusterDamage collision) => stateMachine.OnCollisionExit((T)this, collision);
}

public partial class ExRbStateMachine<T>
{

    void IHitVisitor<RockBusterDamage>.OnHitEnter(RockBusterDamage hit) => stateMachine.OnHitEnter((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnBottomHitEnter(RockBusterDamage hit) => stateMachine.OnBottomHitEnter((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnTopHitEnter(RockBusterDamage hit) => stateMachine.OnTopHitEnter((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnLeftHitEnter(RockBusterDamage hit) => stateMachine.OnLeftHitEnter((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnRightHitEnter(RockBusterDamage hit) => stateMachine.OnRightHitEnter((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnHitStay(RockBusterDamage hit) => stateMachine.OnHitStay((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnBottomHitStay(RockBusterDamage hit) => stateMachine.OnBottomHitStay((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnTopHitStay(RockBusterDamage hit) => stateMachine.OnTopHitStay((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnLeftHitStay(RockBusterDamage hit) => stateMachine.OnLeftHitStay((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnRightHitStay(RockBusterDamage hit) => stateMachine.OnRightHitStay((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnHitExit(RockBusterDamage hit) => stateMachine.OnHitExit((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnBottomHitExit(RockBusterDamage hit) => stateMachine.OnBottomHitExit((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnTopHitExit(RockBusterDamage hit) => stateMachine.OnTopHitExit((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnLeftHitExit(RockBusterDamage hit) => stateMachine.OnLeftHitExit((T)this, hit);
    void IHitVisitor<RockBusterDamage>.OnRightHitExit(RockBusterDamage hit) => stateMachine.OnRightHitExit((T)this, hit);
}