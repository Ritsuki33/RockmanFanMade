using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* =================================================================
T ・・・　 オブジェクト
TS ・・・  当該具象ステート
PS ・・・　親具象ステート
S ・・・　 ステート or サブステート
SS ・・・　サブステート
SM ・・・  ステートマシン
================================================================= */

public interface IStateRbVisitor<T, C> where C : IRbVisitable
{
    void OnTriggerEnter(T obj, C collision);
    void OnTriggerStay(T obj, C collision);
    void OnTriggerExit(T obj, C collision);

    void OnCollisionEnter(T obj, C collision);
    void OnCollisionStay(T obj, C collision);
    void OnCollisionExit(T obj, C collision);
}


public partial interface IStateRbVisitor<T>
{ }

public interface ISubStateRbVisitor<T, PS, C> where C : IRbVisitable
{
    void OnTriggerEnter(T obj, PS parent, C collision);
    void OnTriggerStay(T obj, PS parent, C collision);
    void OnTriggerExit(T obj, PS parent, C collision);

    void OnCollisionEnter(T obj, PS parent, C collision);
    void OnCollisionStay(T obj, PS parent, C collision);
    void OnCollisionExit(T obj, PS parent, C collision);
}

public partial interface ISubStateRbVisitor<T, PS>
{ }

public interface IRbState<T> : IState<T>, IStateRbVisitor<T>
{
    void OnTriggerEnter(T obj, Collider2D collision);
    void OnTriggerStay(T obj, Collider2D collision);
    void OnTriggerExit(T obj, Collider2D collision);

    void OnCollisionEnter(T obj, Collision2D collision);
    void OnCollisionStay(T obj, Collision2D collision);
    void OnCollisionExit(T obj, Collision2D collision);
}

public partial class InheritRbState<T, TS, SM, S> : InheritState<T, TS, SM, S>, IRbState<T>
    where TS : InheritRbState<T, TS, SM, S>
    where SM : InheritRbSubStateMachine<T, TS, S>, new()
    where S : class, IRbSubState<T, TS>
{
    protected virtual void OnTriggerEnter(T obj, Collider2D collision) { }
    protected virtual void OnTriggerStay(T obj, Collider2D collision) { }
    protected virtual void OnTriggerExit(T obj, Collider2D collision) { }
    protected virtual void OnCollisionEnter(T obj, Collision2D collision) { }
    protected virtual void OnCollisionStay(T obj, Collision2D collision) { }
    protected virtual void OnCollisionExit(T obj, Collision2D collision) { }

    void IRbState<T>.OnCollisionEnter(T obj, Collision2D collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IRbState<T>.OnCollisionExit(T obj, Collision2D collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

    void IRbState<T>.OnCollisionStay(T obj, Collision2D collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }
    void IRbState<T>.OnTriggerEnter(T obj, Collider2D collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }
    void IRbState<T>.OnTriggerExit(T obj, Collider2D collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }
    void IRbState<T>.OnTriggerStay(T obj, Collider2D collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }
}

public interface IRbSubState<T, PS> : ISubState<T, PS>, ISubStateRbVisitor<T, PS>
{
    void OnTriggerEnter(T obj, PS parent, Collider2D collision);
    void OnTriggerStay(T obj, PS parent, Collider2D collision);
    void OnTriggerExit(T obj, PS parent, Collider2D collision);

    void OnCollisionEnter(T obj, PS parent, Collision2D collision);
    void OnCollisionStay(T obj, PS parent, Collision2D collision);
    void OnCollisionExit(T obj, PS parent, Collision2D collision);
}

public partial class InheritRbSubState<T, TS, PS, SM, S> : InheritSubState<T, TS, PS, SM, S>, IRbSubState<T, PS>
    where TS : InheritRbSubState<T, TS, PS, SM, S>
    where SM : InheritRbSubStateMachine<T, TS, S>, new()
    where S : class, IRbSubState<T, TS>
{
    protected virtual void OnCollisionEnter(T obj, PS parent, Collision2D collision) { }
    protected virtual void OnCollisionExit(T obj, PS parent, Collision2D collision) { }
    protected virtual void OnCollisionStay(T obj, PS parent, Collision2D collision) { }
    protected virtual void OnTriggerEnter(T obj, PS parent, Collider2D collision) { }
    protected virtual void OnTriggerExit(T obj, PS parent, Collider2D collision) { }
    protected virtual void OnTriggerStay(T obj, PS parent, Collider2D collision) { }

    void IRbSubState<T, PS>.OnCollisionEnter(T obj, PS parent, Collision2D collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }
    void IRbSubState<T, PS>.OnCollisionExit(T obj, PS parent, Collision2D collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
    void IRbSubState<T, PS>.OnCollisionStay(T obj, PS parent, Collision2D collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }
    void IRbSubState<T, PS>.OnTriggerEnter(T obj, PS parent, Collider2D collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }
    void IRbSubState<T, PS>.OnTriggerExit(T obj, PS parent, Collider2D collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }
    void IRbSubState<T, PS>.OnTriggerStay(T obj, PS parent, Collider2D collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }
}

public partial class InheritRbStateMachine<T, S> : InheritStateMachine<T, S>
    where S : class, IRbState<T>
{
    Dictionary<GameObject, IRbVisitable> cacheCollider = new Dictionary<GameObject, IRbVisitable>();

    public void CacheClear()
    {
        cacheCollider?.Clear();
    }

    public void OnCollisionEnter(T obj, Collision2D collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionStay(T obj, Collision2D collision) => curState.OnCollisionStay(obj, collision);
    public void OnCollisionExit(T obj, Collision2D collision) => curState.OnCollisionExit(obj, collision);
    public void OnTriggerEnter(T obj, Collider2D collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, Collider2D collision) => curState.OnTriggerStay(obj, collision);
    public void OnTriggerExit(T obj, Collider2D collision) => curState.OnTriggerExit(obj, collision);
}

public partial class InheritRbSubStateMachine<T, PS, S> : InheritSubStateMachine<T, PS, S>
     where S : class, IRbSubState<T, PS>
{
    public void OnCollisionEnter(T obj, PS parent, Collision2D collision) => curState.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, Collision2D collision) => curState.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, Collision2D collision) => curState.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, Collider2D collision) => curState.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, Collider2D collision) => curState.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, Collider2D collision) => curState.OnTriggerStay(obj, parent, collision);
}


public class RbState<T, TS> : InheritRbState<T, TS, RbSubStateMachine<T, TS>, IRbSubState<T, TS>> where TS : RbState<T, TS>
{ }

public class RbSubState<T, TS, PS> : InheritRbSubState<T, TS, PS, RbSubStateMachine<T, TS>, IRbSubState<T, TS>> where TS : RbSubState<T, TS, PS>
{ }

public partial class RbStateMachine<T> : InheritRbStateMachine<T, IRbState<T>>
{ }

public class RbSubStateMachine<T, PS> : InheritRbSubStateMachine<T, PS, IRbSubState<T, PS>>
{ }