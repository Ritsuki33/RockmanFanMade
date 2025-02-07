using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateExRbVisitor<T, C> where C : IExRbVisitable
{
    void OnHitEnter(T obj, C hit);
    void OnBottomHitEnter(T obj, C hit);
    void OnTopHitEnter(T obj, C hit);
    void OnLeftHitEnter(T obj, C hit);
    void OnRightHitEnter(T obj, C hit);
    void OnHitStay(T obj, C hit);
    void OnBottomHitStay(T obj, C hit);
    void OnTopHitStay(T obj, C hit);
    void OnLeftHitStay(T obj, C hit);
    void OnRightHitStay(T obj, C hit);
    void OnHitExit(T obj, C hit);
    void OnBottomHitExit(T obj, C hit);
    void OnTopHitExit(T obj, C hit);
    void OnLeftHitExit(T obj, C hit);
    void OnRightHitExit(T obj, C hit);
}

public partial interface IStateExRbVisitor<T>
{ }

public interface ISubStateExRbVisitor<T, PS, C> where C : IExRbVisitable
{
    void OnHitEnter(T obj, PS parent, C hit);
    void OnBottomHitEnter(T obj, PS parent, C hit);
    void OnTopHitEnter(T obj, PS parent, C hit);
    void OnLeftHitEnter(T obj, PS parent, C hit);
    void OnRightHitEnter(T obj, PS parent, C hit);
    void OnHitStay(T obj, PS parent, C hit);
    void OnBottomHitStay(T obj, PS parent, C hit);
    void OnTopHitStay(T obj, PS parent, C hit);
    void OnLeftHitStay(T obj, PS parent, C hit);
    void OnRightHitStay(T obj, PS parent, C hit);
    void OnHitExit(T obj, PS parent, C hit);
    void OnBottomHitExit(T obj, PS parent, C hit);
    void OnTopHitExit(T obj, PS parent, C hit);
    void OnLeftHitExit(T obj, PS parent, C hit);
    void OnRightHitExit(T obj, PS parent, C hit);
}

public partial interface ISubStateExRbVisitor<T, PS>
{ }

public interface IExRbState<T> : IRbState<T>, IStateExRbVisitor<T>
{
    void OnHitEnter(T obj, RaycastHit2D hit);
    void OnBottomHitEnter(T obj, RaycastHit2D hit);
    void OnTopHitEnter(T obj, RaycastHit2D hit);
    void OnLeftHitEnter(T obj, RaycastHit2D hit);
    void OnRightHitEnter(T obj, RaycastHit2D hit);
    void OnHitStay(T obj, RaycastHit2D hit);
    void OnBottomHitStay(T obj, RaycastHit2D hit);
    void OnTopHitStay(T obj, RaycastHit2D hit);
    void OnLeftHitStay(T obj, RaycastHit2D hit);
    void OnRightHitStay(T obj, RaycastHit2D hit);
    void OnHitExit(T obj, RaycastHit2D hit);
    void OnBottomHitExit(T obj, RaycastHit2D hit);
    void OnTopHitExit(T obj, RaycastHit2D hit);
    void OnLeftHitExit(T obj, RaycastHit2D hit);
    void OnRightHitExit(T obj, RaycastHit2D hit);
}

public interface IExRbSubState<T, PS> : IRbSubState<T, PS>, ISubStateExRbVisitor<T, PS>
{
    void OnHitEnter(T obj, PS parent, RaycastHit2D hit);
    void OnBottomHitEnter(T obj, PS parent, RaycastHit2D hit);
    void OnTopHitEnter(T obj, PS parent, RaycastHit2D hit);
    void OnLeftHitEnter(T obj, PS parent, RaycastHit2D hit);
    void OnRightHitEnter(T obj, PS parent, RaycastHit2D hit);
    void OnHitStay(T obj, PS parent, RaycastHit2D hit);
    void OnBottomHitStay(T obj, PS parent, RaycastHit2D hit);
    void OnTopHitStay(T obj, PS parent, RaycastHit2D hit);
    void OnLeftHitStay(T obj, PS parent, RaycastHit2D hit);
    void OnRightHitStay(T obj, PS parent, RaycastHit2D hit);
    void OnHitExit(T obj, PS parent, RaycastHit2D hit);
    void OnBottomHitExit(T obj, PS parent, RaycastHit2D hit);
    void OnTopHitExit(T obj, PS parent, RaycastHit2D hit);
    void OnLeftHitExit(T obj, PS parent, RaycastHit2D hit);
    void OnRightHitExit(T obj, PS parent, RaycastHit2D hit);
}

public partial class InheritExRbState<T, TS, SM, S> : InheritRbState<T, TS, SM, S>, IExRbState<T>
    where TS : InheritExRbState<T, TS, SM, S>
    where SM : InheritExRbSubStateMachine<T, TS, S>, new()
    where S : class, IExRbSubState<T, TS>
{
    virtual protected void OnHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitExit(T obj, RaycastHit2D hit) { }

    void IExRbState<T>.OnHitEnter(T obj, RaycastHit2D hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }
    void IExRbState<T>.OnHitStay(T obj, RaycastHit2D hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }
    void IExRbState<T>.OnHitExit(T obj, RaycastHit2D hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }
    void IExRbState<T>.OnBottomHitEnter(T obj, RaycastHit2D hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }
    void IExRbState<T>.OnTopHitEnter(T obj, RaycastHit2D hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }
    void IExRbState<T>.OnLeftHitEnter(T obj, RaycastHit2D hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IExRbState<T>.OnRightHitEnter(T obj, RaycastHit2D hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }
    void IExRbState<T>.OnBottomHitStay(T obj, RaycastHit2D hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }
    void IExRbState<T>.OnTopHitStay(T obj, RaycastHit2D hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }
    void IExRbState<T>.OnLeftHitStay(T obj, RaycastHit2D hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }
    void IExRbState<T>.OnRightHitStay(T obj, RaycastHit2D hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }
    void IExRbState<T>.OnBottomHitExit(T obj, RaycastHit2D hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }
    void IExRbState<T>.OnTopHitExit(T obj, RaycastHit2D hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }
    void IExRbState<T>.OnLeftHitExit(T obj, RaycastHit2D hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }
    void IExRbState<T>.OnRightHitExit(T obj, RaycastHit2D hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritExRbSubState<T, TS, PS, SM, S> : InheritRbSubState<T, TS, PS, SM, S>, IExRbSubState<T, PS>
    where TS : InheritExRbSubState<T, TS, PS, SM, S>
    where SM : InheritExRbSubStateMachine<T, TS, S>, new()
    where S : class, IExRbSubState<T, TS>
{
    virtual protected void OnHitEnter(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnLeftHitEnter(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnHitStay(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnBottomHitStay(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnTopHitStay(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnRightHitStay(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnHitExit(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnTopHitExit(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, RaycastHit2D hit) { }
    virtual protected void OnRightHitExit(T obj, PS parent, RaycastHit2D hit) { }

    void IExRbSubState<T, PS>.OnHitEnter(T obj, PS parent, RaycastHit2D hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnHitStay(T obj, PS parent, RaycastHit2D hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnHitExit(T obj, PS parent, RaycastHit2D hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnBottomHitEnter(T obj, PS parent, RaycastHit2D hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnTopHitEnter(T obj, PS parent, RaycastHit2D hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnLeftHitEnter(T obj, PS parent, RaycastHit2D hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnRightHitEnter(T obj, PS parent, RaycastHit2D hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnBottomHitStay(T obj, PS parent, RaycastHit2D hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnTopHitStay(T obj, PS parent, RaycastHit2D hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnLeftHitStay(T obj, PS parent, RaycastHit2D hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnRightHitStay(T obj, PS parent, RaycastHit2D hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnBottomHitExit(T obj, PS parent, RaycastHit2D hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnTopHitExit(T obj, PS parent, RaycastHit2D hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnLeftHitExit(T obj, PS parent, RaycastHit2D hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }
    void IExRbSubState<T, PS>.OnRightHitExit(T obj, PS parent, RaycastHit2D hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritExRbStateMachine<T, S> : InheritRbStateMachine<T, S>
    where S : class, IExRbState<T>
{
    public void OnHitEnter(T obj, RaycastHit2D hit) => curState?.OnHitEnter(obj, hit);
    public void OnHitStay(T obj, RaycastHit2D hit) => curState?.OnHitStay(obj, hit);
    public void OnHitExit(T obj, RaycastHit2D hit) => curState?.OnHitExit(obj, hit);
    public void OnBottomHitEnter(T obj, RaycastHit2D hit) => curState?.OnBottomHitEnter(obj, hit);
    public void OnBottomHitStay(T obj, RaycastHit2D hit) => curState?.OnBottomHitStay(obj, hit);
    public void OnBottomHitExit(T obj, RaycastHit2D hit) => curState?.OnBottomHitExit(obj, hit);
    public void OnTopHitEnter(T obj, RaycastHit2D hit) => curState?.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, RaycastHit2D hit) => curState?.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, RaycastHit2D hit) => curState?.OnRightHitEnter(obj, hit);
    public void OnTopHitStay(T obj, RaycastHit2D hit) => curState?.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, RaycastHit2D hit) => curState?.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, RaycastHit2D hit) => curState?.OnRightHitStay(obj, hit);
    public void OnTopHitExit(T obj, RaycastHit2D hit) => curState?.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, RaycastHit2D hit) => curState?.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, RaycastHit2D hit) => curState?.OnRightHitExit(obj, hit);
}

public partial class InheritExRbSubStateMachine<T, PS, S> : InheritRbSubStateMachine<T, PS, S>
 where S : class, IExRbSubState<T, PS>
{
    public void OnHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnRightHitExit(obj, parent, hit);
}


public class ExRbState<T, TS> : InheritExRbState<T, TS, ExRbSubStateMachine<T, TS>, IExRbSubState<T, TS>> where TS : ExRbState<T, TS>
{ }

public class ExRbSubState<T, TS, PS> : InheritExRbSubState<T, TS, PS, ExRbSubStateMachine<T, TS>, IExRbSubState<T, TS>> where TS : ExRbSubState<T, TS, PS>
{ }

public class ExRbStateMachine<T> : InheritExRbStateMachine<T, IExRbState<T>>
{ }

public class ExRbSubStateMachine<T, PS> : InheritExRbSubStateMachine<T, PS, IExRbSubState<T, PS>>
{ }