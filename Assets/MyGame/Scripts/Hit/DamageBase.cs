using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class DamageBase : MonoBehaviour, IRbVisitable, IExRbVisitable
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
    [SerializeField] public int baseDamageValue = 3;
}

public partial interface IRbVisitor : IRbVisitor<DamageBase>
{ }

public partial interface IExRbVisitor : IExRbVisitor<DamageBase>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, DamageBase>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, DamageBase>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, DamageBase>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, DamageBase>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, DamageBase collision) { }
    virtual protected void OnTriggerStay(T obj, DamageBase collision) { }
    virtual protected void OnTriggerExit(T obj, DamageBase collision) { }

    virtual protected void OnCollisionEnter(T obj, DamageBase collision) { }
    virtual protected void OnCollisionStay(T obj, DamageBase collision) { }
    virtual protected void OnCollisionExit(T obj, DamageBase collision) { }

    void IStateRbVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, DamageBase>.OnCollisionEnter(T obj, DamageBase collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, DamageBase>.OnCollisionStay(T obj, DamageBase collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, DamageBase>.OnCollisionExit(T obj, DamageBase collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
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

    void IStateExRbVisitor<T, DamageBase>.OnHitEnter(T obj, DamageBase hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, DamageBase>.OnBottomHitEnter(T obj, DamageBase hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnTopHitEnter(T obj, DamageBase hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnLeftHitEnter(T obj, DamageBase hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, DamageBase>.OnRightHitEnter(T obj, DamageBase hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnHitStay(T obj, DamageBase hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnBottomHitStay(T obj, DamageBase hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnTopHitStay(T obj, DamageBase hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnLeftHitStay(T obj, DamageBase hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnRightHitStay(T obj, DamageBase hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnHitExit(T obj, DamageBase hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnBottomHitExit(T obj, DamageBase hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnTopHitExit(T obj, DamageBase hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnLeftHitExit(T obj, DamageBase hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, DamageBase>.OnRightHitExit(T obj, DamageBase hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, DamageBase collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, DamageBase collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, DamageBase collision) { }

    void ISubStateRbVisitor<T, PS, DamageBase>.OnTriggerEnter(T obj, PS parent, DamageBase collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, DamageBase>.OnTriggerStay(T obj, PS parent, DamageBase collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, DamageBase>.OnTriggerExit(T obj, PS parent, DamageBase collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, DamageBase>.OnCollisionEnter(T obj, PS parent, DamageBase collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, DamageBase>.OnCollisionStay(T obj, PS parent, DamageBase collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, DamageBase>.OnCollisionExit(T obj, PS parent, DamageBase collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
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

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnBottomHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnTopHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnLeftHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnRightHitEnter(T obj, PS parent, DamageBase hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnHitStay(T obj, PS parent, DamageBase hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnBottomHitStay(T obj, PS parent, DamageBase hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnTopHitStay(T obj, PS parent, DamageBase hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnLeftHitStay(T obj, PS parent, DamageBase hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnRightHitStay(T obj, PS parent, DamageBase hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnHitExit(T obj, PS parent, DamageBase hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnBottomHitExit(T obj, PS parent, DamageBase hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnTopHitExit(T obj, PS parent, DamageBase hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnLeftHitExit(T obj, PS parent, DamageBase hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, DamageBase>.OnRightHitExit(T obj, PS parent, DamageBase hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, DamageBase collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, DamageBase collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, DamageBase collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, DamageBase hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, DamageBase hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, DamageBase hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, DamageBase hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, DamageBase hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, DamageBase hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, DamageBase hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, DamageBase hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, DamageBase hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, DamageBase hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, DamageBase hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, DamageBase hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, DamageBase hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, DamageBase hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, DamageBase hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, DamageBase collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, DamageBase collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, DamageBase collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, DamageBase hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, DamageBase hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, DamageBase hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, DamageBase hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, DamageBase hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, DamageBase hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, DamageBase hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, DamageBase hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, DamageBase hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, DamageBase hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, DamageBase hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, DamageBase hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, DamageBase hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, DamageBase hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, DamageBase hit) => curState.OnRightHitExit(obj, parent, hit);
}


public partial class RbCollide
{
    void IRbVisitor<DamageBase>.OnCollisionEnter(DamageBase collision) => onCollisionEnterDamageBase?.Invoke(collision);
    void IRbVisitor<DamageBase>.OnCollisionExit(DamageBase collision) => onCollisionExitDamageBase?.Invoke(collision);
    void IRbVisitor<DamageBase>.OnCollisionStay(DamageBase collision) => onCollisionStayDamageBase?.Invoke(collision);
    void IRbVisitor<DamageBase>.OnTriggerEnter(DamageBase collision) => onTriggerEnterDamageBase?.Invoke(collision);
    void IRbVisitor<DamageBase>.OnTriggerExit(DamageBase collision) => onTriggerExitDamageBase?.Invoke(collision);
    void IRbVisitor<DamageBase>.OnTriggerStay(DamageBase collision) => onTriggerStayDamageBase?.Invoke(collision);

    public event Action<DamageBase> onCollisionEnterDamageBase;
    public event Action<DamageBase> onCollisionExitDamageBase;
    public event Action<DamageBase> onCollisionStayDamageBase;
    public event Action<DamageBase> onTriggerEnterDamageBase;
    public event Action<DamageBase> onTriggerExitDamageBase;
    public event Action<DamageBase> onTriggerStayDamageBase;
}

public partial class ExRbHit
{
    void IExRbVisitor<DamageBase>.OnHitEnter(DamageBase hit) => onHitEnterDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnBottomHitEnter(DamageBase hit) => onBottomHitEnterDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnTopHitEnter(DamageBase hit) => onTopHitEnterDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnLeftHitEnter(DamageBase hit) => onLeftHitEnterDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnRightHitEnter(DamageBase hit) => onRightHitEnterDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnHitStay(DamageBase hit) => onHitStayDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnBottomHitStay(DamageBase hit) => onBottomHitStayDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnTopHitStay(DamageBase hit) => onTopHitStayDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnLeftHitStay(DamageBase hit) => onLeftHitStayDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnRightHitStay(DamageBase hit) => onRightHitStayDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnHitExit(DamageBase hit) => onHitExitDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnBottomHitExit(DamageBase hit) => onBottomHitExitDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnTopHitExit(DamageBase hit) => onTopHitExitDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnLeftHitExit(DamageBase hit) => onLeftHitExitDamageBase?.Invoke(hit);
    void IExRbVisitor<DamageBase>.OnRightHitExit(DamageBase hit) => onRightHitExitDamageBase?.Invoke(hit);

    public event Action<DamageBase> onHitEnterDamageBase;
    public event Action<DamageBase> onBottomHitEnterDamageBase;
    public event Action<DamageBase> onTopHitEnterDamageBase;
    public event Action<DamageBase> onLeftHitEnterDamageBase;
    public event Action<DamageBase> onRightHitEnterDamageBase;
    public event Action<DamageBase> onHitStayDamageBase;
    public event Action<DamageBase> onBottomHitStayDamageBase;
    public event Action<DamageBase> onTopHitStayDamageBase;
    public event Action<DamageBase> onLeftHitStayDamageBase;
    public event Action<DamageBase> onRightHitStayDamageBase;
    public event Action<DamageBase> onHitExitDamageBase;
    public event Action<DamageBase> onBottomHitExitDamageBase;
    public event Action<DamageBase> onTopHitExitDamageBase;
    public event Action<DamageBase> onLeftHitExitDamageBase;
    public event Action<DamageBase> onRightHitExitDamageBase;

    void SetInterpreterDamageBase(IHitInterpreter hitInterpreter)
    {
        onHitEnterDamageBase = hitInterpreter.OnHitEnter;
        onBottomHitEnterDamageBase = hitInterpreter.OnBottomHitEnter;
        onTopHitEnterDamageBase = hitInterpreter.OnTopHitEnter;
        onLeftHitEnterDamageBase = hitInterpreter.OnLeftHitEnter;
        onRightHitEnterDamageBase = hitInterpreter.OnRightHitEnter;
        onHitStayDamageBase = hitInterpreter.OnHitStay;
        onBottomHitStayDamageBase = hitInterpreter.OnBottomHitStay;
        onTopHitStayDamageBase = hitInterpreter.OnTopHitStay;
        onLeftHitStayDamageBase = hitInterpreter.OnLeftHitStay;
        onRightHitStayDamageBase = hitInterpreter.OnRightHitStay;
        onHitExitDamageBase = hitInterpreter.OnHitExit;
        onBottomHitExitDamageBase = hitInterpreter.OnBottomHitExit;
        onTopHitExitDamageBase = hitInterpreter.OnTopHitExit;
        onLeftHitExitDamageBase = hitInterpreter.OnLeftHitExit;
        onRightHitExitDamageBase = hitInterpreter.OnRightHitExit;
    }
}

public partial interface IHitInterpreter : IHitInterpreter<DamageBase> { }