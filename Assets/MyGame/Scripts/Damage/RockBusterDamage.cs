using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class RockBusterDamage : DamageBase
{
    public override void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    public override void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    public override void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    public override void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    public override void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    public override void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);

    // ここから定義
    [SerializeField] public Projectile projectile;

    public void DeleteBuster() => projectile.Delete();
}


public partial interface IStateTriggerVisitor<T>
     : IStateTriggerVisitor<T, RockBusterDamage>
{ }

public partial interface ISubStateTriggerVisitor<T, PS> : ISubStateTriggerVisitor<T, PS, RockBusterDamage>
{
}

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

public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionEnter(T obj, RockBusterDamage collision) => curState.OnCollisionEnter(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionExit(T obj, RockBusterDamage collision) => curState.OnCollisionExit(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionStay(T obj, RockBusterDamage collision) => curState.OnCollisionStay(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerEnter(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerExit(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
    void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerStay(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
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

public partial class BaseRbStateMachine<T, S, SM, G>
{
    void ITriggerVisitor<RockBusterDamage>.OnTriggerEnter(RockBusterDamage collision) => stateMachine.OnTriggerEnter((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnTriggerStay(RockBusterDamage collision) => stateMachine.OnTriggerStay((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnTriggerExit(RockBusterDamage collision) => stateMachine.OnTriggerExit((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnCollisionEnter(RockBusterDamage collision) => stateMachine.OnCollisionEnter((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnCollisionStay(RockBusterDamage collision) => stateMachine.OnCollisionStay((T)this, collision);
    void ITriggerVisitor<RockBusterDamage>.OnCollisionExit(RockBusterDamage collision) => stateMachine.OnCollisionExit((T)this, collision);
}

