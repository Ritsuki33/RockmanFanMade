using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public interface IStateTriggerVisitor<T, C> where C : ITriggerVisitable
{
    void OnTriggerEnter(T obj, C collision);
    void OnTriggerStay(T obj, C collision);
    void OnTriggerExit(T obj, C collision);

    void OnCollisionEnter(T obj, C collision);
    void OnCollisionStay(T obj, C collision);
    void OnCollisionExit(T obj, C collision);
}


public partial interface IStateTriggerVisitor<T>
{}

public interface IRbState<T> : IBaseState<T>, IStateTriggerVisitor<T> where T : MonoBehaviour
{
    void OnTriggerEnter(T obj, Collider2D collision);
    void OnTriggerStay(T obj, Collider2D collision);
    void OnTriggerExit(T obj, Collider2D collision);

    void OnCollisionEnter(T obj, Collision2D collision);
    void OnCollisionStay(T obj, Collision2D collision);
    void OnCollisionExit(T obj, Collision2D collision);
}

public interface ISubStateTriggerVisitor<T, PS, C> where C : ITriggerVisitable
{
    void OnTriggerEnter(T obj, PS parent, C collision);
    void OnTriggerStay(T obj, PS parent, C collision);
    void OnTriggerExit(T obj, PS parent, C collision);

    void OnCollisionEnter(T obj, PS parent, C collision);
    void OnCollisionStay(T obj, PS parent, C collision);
    void OnCollisionExit(T obj, PS parent, C collision);
}

public partial interface ISubStateTriggerVisitor<T, PS>
{ }

public interface IRbSubState<T, PS> : IBaseSubState<T, PS>, ISubStateTriggerVisitor<T, PS> where T : MonoBehaviour
{
    void OnTriggerEnter(T obj, PS parent, Collider2D collision);
    void OnTriggerStay(T obj, PS parent, Collider2D collision);
    void OnTriggerExit(T obj, PS parent, Collider2D collision);

    void OnCollisionEnter(T obj, PS parent, Collision2D collision);
    void OnCollisionStay(T obj, PS parent, Collision2D collision);
    void OnCollisionExit(T obj, PS parent, Collision2D collision);
}


/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class BaseRbState<T, TS, SS, SM, G> : BaseState<T, TS, SS, SM, G>, IRbState<T>
    where T : MonoBehaviour
    where TS : class, IRbState<T>
    where SS : class, IRbSubState<T, TS>
    where SM : class, IRbSubStateMachine<T, SS, TS>
    where G : SM, new()
{
    protected virtual void OnCollisionEnter(T obj, Collision2D collision) { }
    protected virtual void OnCollisionExit(T obj, Collision2D collision) { }
    protected virtual void OnCollisionStay(T obj, Collision2D collision) { }
    protected virtual void OnTriggerEnter(T obj, Collider2D collision) { }
    protected virtual void OnTriggerExit(T obj, Collider2D collision) { }
    protected virtual void OnTriggerStay(T obj, Collider2D collision) { }

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


/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class BaseRbSubState<T, TS, SS, SM, G, PS> : BaseSubState<T, TS, SS, SM, G, PS>, IRbSubState<T, PS>
    where T : MonoBehaviour
    where TS : class, IRbSubState<T, PS>
    where SS : class, IRbSubState<T, TS>
    where SM : class, IRbSubStateMachine<T, SS, TS>
    where G : SM, new()
    where PS : class, IBaseCommonState<T>
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

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class RbState<T, TS> :
    BaseRbState<T, TS, IRbSubState<T, TS>, IRbSubStateMachine<T, IRbSubState<T, TS>, TS>, GenericRbSubStateMachine<T, IRbSubState<T, TS>, TS>>
    where T : MonoBehaviour
    where TS : class, IRbState<T>
{ }

public class RbSubState<T, TS, PS> :
BaseRbSubState<T, TS, IRbSubState<T, TS>, IRbSubStateMachine<T, IRbSubState<T, TS>, TS>, GenericRbSubStateMachine<T, IRbSubState<T, TS>, TS>, PS>
    where T : MonoBehaviour
    where TS : class, IRbSubState<T, PS>
    where PS : class, IBaseCommonState<T>
{ }

public interface IRbStateMachine<T, S>
    : IBaseStateMachine<T, S>, IStateTriggerVisitor<T>
    where T : MonoBehaviour where S : class, IRbState<T>
{
    void OnTriggerEnter(T obj, Collider2D collision);
    void OnTriggerStay(T obj, Collider2D collision);
    void OnTriggerExit(T obj, Collider2D collision);

    void OnCollisionEnter(T obj, Collision2D collision);
    void OnCollisionStay(T obj, Collision2D collision);
    void OnCollisionExit(T obj, Collision2D collision);
}

public interface IRbSubStateMachine<T, S, PS>
    : IBaseSubStateMachine<T, S, PS>, ISubStateTriggerVisitor<T, PS>
    where T : MonoBehaviour
{
    void OnTriggerEnter(T obj, PS parent, Collider2D collision);
    void OnTriggerStay(T obj, PS parent, Collider2D collision);
    void OnTriggerExit(T obj, PS parent, Collider2D collision);

    void OnCollisionEnter(T obj, PS parent, Collision2D collision);
    void OnCollisionStay(T obj, PS parent, Collision2D collision);
    void OnCollisionExit(T obj, PS parent, Collision2D collision);
}

public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    void IRbStateMachine<T, S>.OnCollisionEnter(T obj, Collision2D collision) => curState.OnCollisionEnter(obj, collision);
    void IRbStateMachine<T, S>.OnCollisionExit(T obj, Collision2D collision) => curState.OnCollisionExit(obj, collision);
    void IRbStateMachine<T, S>.OnCollisionStay(T obj, Collision2D collision) => curState.OnCollisionStay(obj, collision);
    void IRbStateMachine<T, S>.OnTriggerEnter(T obj, Collider2D collision) => curState.OnTriggerEnter(obj, collision);
    void IRbStateMachine<T, S>.OnTriggerExit(T obj, Collider2D collision) => curState.OnTriggerExit(obj, collision);
    void IRbStateMachine<T, S>.OnTriggerStay(T obj, Collider2D collision) => curState.OnTriggerStay(obj, collision);
}

public partial class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
{
    void IRbSubStateMachine<T, S, PS>.OnCollisionEnter(T obj, PS parent, Collision2D collision) => curState.OnCollisionEnter(obj, parent, collision);

    void IRbSubStateMachine<T, S, PS>.OnCollisionExit(T obj, PS parent, Collision2D collision) => curState.OnCollisionExit(obj, parent, collision);

    void IRbSubStateMachine<T, S, PS>.OnCollisionStay(T obj, PS parent, Collision2D collision) => curState.OnCollisionStay(obj, parent, collision);
    void IRbSubStateMachine<T, S, PS>.OnTriggerEnter(T obj, PS parent, Collider2D collision) => curState.OnTriggerEnter(obj, parent, collision);

    void IRbSubStateMachine<T, S, PS>.OnTriggerExit(T obj, PS parent, Collider2D collision) => curState.OnTriggerExit(obj, parent, collision);
    void IRbSubStateMachine<T, S, PS>.OnTriggerStay(T obj, PS parent, Collider2D collision) => curState.OnTriggerStay(obj, parent, collision);
}

public partial class BaseRbStateMachine<T, S, SM, G>
: BaseStateMachine<T, S, SM, G>, ITriggerVisitor
where T : BaseRbStateMachine<T, S, SM, G>
where S : class, IRbState<T>
where SM : IRbStateMachine<T, S>
where G : SM, new()
{
    Dictionary<Collider2D, ITriggerVisitable> cacheCollider2D = new Dictionary<Collider2D, ITriggerVisitable>();
    Dictionary<Collision2D, ITriggerVisitable> cachCollision2D = new Dictionary<Collision2D, ITriggerVisitable>();

    protected virtual void OnDisable()
    {
        cacheCollider2D.Clear();
        cachCollision2D.Clear();
    }

    protected virtual void OnDestroy()
    {
        cacheCollider2D.Clear();
        cachCollision2D.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateMachine.OnTriggerEnter((T)this, collision);

        var collide = collision.gameObject.GetComponent<ITriggerVisitable>();

        // キャッシュ
        if (collide != null && !cacheCollider2D.ContainsKey(collision)) cacheCollider2D.Add(collision, collide);

        collide?.AcceptOnTriggerEnter(this);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        stateMachine.OnTriggerStay((T)this, collision);

        ITriggerVisitable collide = null;

        if (cacheCollider2D.ContainsKey(collision))
        {
            collide = cacheCollider2D[collision];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            collide = collision.gameObject.GetComponent<ITriggerVisitable>();
            if (collide != null) cacheCollider2D.Add(collision, collide);
        }

        collide?.AcceptOnTriggerStay(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stateMachine.OnTriggerExit((T)this, collision);

        ITriggerVisitable collide = null;

        if (cacheCollider2D.ContainsKey(collision))
        {
            collide = cacheCollider2D[collision];
        }
        else
        {
            // キャッシュがない場合は改めて取得
            collide = collision.gameObject.GetComponent<ITriggerVisitable>();
        }
        collide?.AcceptOnTriggerExit(this);

        // キャッシュの削除
        if (cacheCollider2D.ContainsKey(collision)) cacheCollider2D.Remove(collision);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        stateMachine.OnCollisionEnter((T)this, collision);
        var collide = collision.gameObject.GetComponent<ITriggerVisitable>();
        // キャッシュ
        if (!cachCollision2D.ContainsKey(collision)) cachCollision2D.Add(collision, collide);
        collide?.AcceptOnCollisionEnter(this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        stateMachine.OnCollisionStay((T)this, collision);
        ITriggerVisitable collide = null;

        if (cachCollision2D.ContainsKey(collision))
        {
            collide = cachCollision2D[collision];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            collide = collision.gameObject.GetComponent<ITriggerVisitable>();
            cachCollision2D.Add(collision, collide);
        }
        collide?.AcceptOnCollisionStay(this);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stateMachine.OnCollisionExit((T)this, collision);
        ITriggerVisitable collide = null;

        if (cachCollision2D.ContainsKey(collision))
        {
            collide = cachCollision2D[collision];
        }
        else
        {
            // キャッシュがない場合は改めて取得
            collide = collision.gameObject.GetComponent<ITriggerVisitable>();
        }
        collide?.AcceptOnCollisionExit(this);

        if (cachCollision2D.ContainsKey(collision)) cachCollision2D.Remove(collision);
    }
}

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class RbStateMachine<T> :
    BaseRbStateMachine<T, IRbState<T>, IRbStateMachine<T, IRbState<T>>, GenericRbStateMachine<T, IRbState<T>>>
    where T : RbStateMachine<T>
{ }