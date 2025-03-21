﻿using System;
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

    protected virtual void AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitEnter(this, hit);
    protected virtual void AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitStay(this, hit);
    protected virtual void AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitExit(this, hit);
    protected virtual void AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitEnter(this, hit);
    protected virtual void AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitStay(this, hit);
    protected virtual void AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitExit(this, hit);
    protected virtual void AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitEnter(this, hit);
    protected virtual void AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitStay(this, hit);
    protected virtual void AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitExit(this, hit);
    protected virtual void AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitEnter(this, hit);
    protected virtual void AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitStay(this, hit);
    protected virtual void AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitExit(this, hit);
    protected virtual void AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitEnter(this, hit);
    protected virtual void AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitStay(this, hit);
    protected virtual void AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitExit(this, hit);

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);

    void IExRbVisitable.AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitExit(visitor, hit);

    // ここから定義
    [SerializeField] public int baseDamageValue = 3;
}


public partial interface IRbVisitor
{
    void OnTriggerEnter(DamageBase collision) { }
    void OnTriggerStay(DamageBase collision) { }
    void OnTriggerExit(DamageBase collision) { }

    void OnCollisionEnter(DamageBase collision) { }
    void OnCollisionStay(DamageBase collision) { }
    void OnCollisionExit(DamageBase collision) { }
}
public partial interface IExRbVisitor
{
    void OnHitEnter(DamageBase obj, RaycastHit2D hit) { }
    void OnBottomHitEnter(DamageBase obj, RaycastHit2D hit) { }
    void OnTopHitEnter(DamageBase obj, RaycastHit2D hit) { }
    void OnLeftHitEnter(DamageBase obj, RaycastHit2D hit) { }
    void OnRightHitEnter(DamageBase obj, RaycastHit2D hit) { }
    void OnHitStay(DamageBase obj, RaycastHit2D hit) { }
    void OnBottomHitStay(DamageBase obj, RaycastHit2D hit) { }
    void OnTopHitStay(DamageBase obj, RaycastHit2D hit) { }
    void OnLeftHitStay(DamageBase obj, RaycastHit2D hit) { }
    void OnRightHitStay(DamageBase obj, RaycastHit2D hit) { }
    void OnHitExit(DamageBase obj, RaycastHit2D hit) { }
    void OnBottomHitExit(DamageBase obj, RaycastHit2D hit) { }
    void OnTopHitExit(DamageBase obj, RaycastHit2D hit) { }
    void OnLeftHitExit(DamageBase obj, RaycastHit2D hit) { }
    void OnRightHitExit(DamageBase obj, RaycastHit2D hit) { }
}

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

