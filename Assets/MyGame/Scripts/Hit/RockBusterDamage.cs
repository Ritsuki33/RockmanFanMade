using System;
using UnityEngine;

/// <summary>RockBusterDamage
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class RockBusterDamage : DamageBase
{

    protected override void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected override void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected override void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected override void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected override void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected override void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

    protected override void AcceptOnHitEnter(IExRbVisitor visitor) => visitor.OnHitEnter(this);
    protected override void AcceptOnHitStay(IExRbVisitor visitor) => visitor.OnHitStay(this);
    protected override void AcceptOnHitExit(IExRbVisitor visitor) => visitor.OnHitExit(this);
    protected override void AcceptOnBottomHitEnter(IExRbVisitor visitor) => visitor.OnBottomHitEnter(this);
    protected override void AcceptOnBottomHitStay(IExRbVisitor visitor) => visitor.OnBottomHitStay(this);
    protected override void AcceptOnBottomHitExit(IExRbVisitor visitor) => visitor.OnBottomHitExit(this);
    protected override void AcceptOnTopHitEnter(IExRbVisitor visitor) => visitor.OnTopHitEnter(this);
    protected override void AcceptOnTopHitStay(IExRbVisitor visitor) => visitor.OnTopHitStay(this);
    protected override void AcceptOnTopHitExit(IExRbVisitor visitor) => visitor.OnTopHitExit(this);
    protected override void AcceptOnLeftHitEnter(IExRbVisitor visitor) => visitor.OnLeftHitEnter(this);
    protected override void AcceptOnLeftHitStay(IExRbVisitor visitor) => visitor.OnLeftHitStay(this);
    protected override void AcceptOnLeftHitExit(IExRbVisitor visitor) => visitor.OnLeftHitExit(this);
    protected override void AcceptOnRightHitEnter(IExRbVisitor visitor) => visitor.OnRightHitEnter(this);
    protected override void AcceptOnRightHitStay(IExRbVisitor visitor) => visitor.OnRightHitStay(this);
    protected override void AcceptOnRightHitExit(IExRbVisitor visitor) => visitor.OnRightHitExit(this);

    // ここから定義
    [SerializeField] public Projectile projectile;

    public void DeleteBuster() => projectile.Delete();

}

public partial interface IRbVisitor : IRbVisitor<RockBusterDamage>
{ }

public partial interface IExRbVisitor : IExRbVisitor<RockBusterDamage>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, RockBusterDamage>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, RockBusterDamage>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, RockBusterDamage>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, RockBusterDamage>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnTriggerStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnTriggerExit(T obj, RockBusterDamage collision) { }

    virtual protected void OnCollisionEnter(T obj, RockBusterDamage collision) { }
    virtual protected void OnCollisionStay(T obj, RockBusterDamage collision) { }
    virtual protected void OnCollisionExit(T obj, RockBusterDamage collision) { }

    void IStateRbVisitor<T, RockBusterDamage>.OnTriggerEnter(T obj, RockBusterDamage collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, RockBusterDamage>.OnTriggerStay(T obj, RockBusterDamage collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, RockBusterDamage>.OnTriggerExit(T obj, RockBusterDamage collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, RockBusterDamage>.OnCollisionEnter(T obj, RockBusterDamage collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, RockBusterDamage>.OnCollisionStay(T obj, RockBusterDamage collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, RockBusterDamage>.OnCollisionExit(T obj, RockBusterDamage collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
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

    void IStateExRbVisitor<T, RockBusterDamage>.OnHitEnter(T obj, RockBusterDamage hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, RockBusterDamage>.OnBottomHitEnter(T obj, RockBusterDamage hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnTopHitEnter(T obj, RockBusterDamage hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnLeftHitEnter(T obj, RockBusterDamage hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, RockBusterDamage>.OnRightHitEnter(T obj, RockBusterDamage hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnHitStay(T obj, RockBusterDamage hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnBottomHitStay(T obj, RockBusterDamage hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnTopHitStay(T obj, RockBusterDamage hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnLeftHitStay(T obj, RockBusterDamage hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnRightHitStay(T obj, RockBusterDamage hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnHitExit(T obj, RockBusterDamage hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnBottomHitExit(T obj, RockBusterDamage hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnTopHitExit(T obj, RockBusterDamage hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnLeftHitExit(T obj, RockBusterDamage hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, RockBusterDamage>.OnRightHitExit(T obj, RockBusterDamage hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, RockBusterDamage collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, RockBusterDamage collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, RockBusterDamage collision) { }

    void ISubStateRbVisitor<T, PS, RockBusterDamage>.OnTriggerEnter(T obj, PS parent, RockBusterDamage collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBusterDamage>.OnTriggerStay(T obj, PS parent, RockBusterDamage collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBusterDamage>.OnTriggerExit(T obj, PS parent, RockBusterDamage collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBusterDamage>.OnCollisionEnter(T obj, PS parent, RockBusterDamage collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBusterDamage>.OnCollisionStay(T obj, PS parent, RockBusterDamage collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBusterDamage>.OnCollisionExit(T obj, PS parent, RockBusterDamage collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
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

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnBottomHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnTopHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnLeftHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnRightHitEnter(T obj, PS parent, RockBusterDamage hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnBottomHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnTopHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnLeftHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnRightHitStay(T obj, PS parent, RockBusterDamage hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnBottomHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnTopHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnLeftHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, RockBusterDamage>.OnRightHitExit(T obj, PS parent, RockBusterDamage hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, RockBusterDamage collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, RockBusterDamage collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, RockBusterDamage collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, RockBusterDamage hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, RockBusterDamage hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, RockBusterDamage hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, RockBusterDamage hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, RockBusterDamage hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, RockBusterDamage hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, RockBusterDamage hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, RockBusterDamage hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, RockBusterDamage hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, RockBusterDamage hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, RockBusterDamage hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, RockBusterDamage hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, RockBusterDamage hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, RockBusterDamage hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, RockBusterDamage hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, RockBusterDamage collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, RockBusterDamage collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, RockBusterDamage collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, RockBusterDamage hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, RockBusterDamage hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, RockBusterDamage hit) => curState.OnRightHitExit(obj, parent, hit);
}

public partial interface IHitInterpreter : IHitInterpreter<RockBusterDamage> { }