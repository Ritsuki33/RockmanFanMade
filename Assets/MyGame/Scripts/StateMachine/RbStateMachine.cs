using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRbState<T> : IBaseState<T> where T : MonoBehaviour
{
    void OnCollisionEnter2D(T obj, Collision2D collision);
    void OnCollisionStay2D(T obj, Collision2D collision);
    void OnCollisionExit2D(T obj, Collision2D collision);
    void OnTriggerEnter2D(T obj, Collider2D collision);
    void OnTriggerStay2D(T obj, Collider2D collision);
    void OnTriggerExit2D(T obj, Collider2D collision);
}

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class RbState<T> : BaseState<T>, IRbState<T> where T : MonoBehaviour
{
    public RbState(bool immediate = true) : base(immediate) { }

    virtual protected void OnCollisionEnter2D(T obj, Collision2D collision) { }
    virtual protected void OnCollisionStay2D(T obj, Collision2D collision) { }
    virtual protected void OnCollisionExit2D(T obj, Collision2D collision) { }

    virtual protected void OnTriggerEnter2D(T obj, Collider2D collision) { }
    virtual protected void OnTriggerStay2D(T obj, Collider2D collision) { }
    virtual protected void OnTriggerExit2D(T obj, Collider2D collision) { }

    void IRbState<T>.OnCollisionEnter2D(T obj, Collision2D collision) { OnCollisionEnter2D(obj, collision); }
    void IRbState<T>.OnCollisionStay2D(T obj, Collision2D collision) { OnCollisionStay2D(obj, collision); }
    void IRbState<T>.OnCollisionExit2D(T obj, Collision2D collision) { OnCollisionExit2D(obj, collision); }

    void IRbState<T>.OnTriggerEnter2D(T obj, Collider2D collision) { OnTriggerEnter2D(obj, collision); }
    void IRbState<T>.OnTriggerStay2D(T obj, Collider2D collision) { OnTriggerStay2D(obj, collision); }
    void IRbState<T>.OnTriggerExit2D(T obj, Collider2D collision) { OnTriggerExit2D(obj, collision); }
}

public interface IRbStateMachine<T, S> : IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{

    void OnCollisionEnter2D(T obj, Collision2D collision);
    void OnCollisionStay2D(T obj, Collision2D collision);
    void OnCollisionExit2D(T obj, Collision2D collision);

    void OnTriggerEnter2D(T obj, Collider2D collision);
    void OnTriggerStay2D(T obj, Collider2D collision);
    void OnTriggerExit2D(T obj, Collider2D collision);
}
public class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    public void OnCollisionEnter2D(T obj, Collision2D collision) => curState?.OnCollisionEnter2D(obj, collision);
    public void OnCollisionStay2D(T obj, Collision2D collision) => curState?.OnCollisionStay2D(obj, collision);
    public void OnCollisionExit2D(T obj, Collision2D collision) => curState?.OnCollisionExit2D(obj, collision);

    public void OnTriggerEnter2D(T obj, Collider2D collision) => curState?.OnTriggerEnter2D(obj, collision);
    public void OnTriggerStay2D(T obj, Collider2D collision) => curState?.OnTriggerStay2D(obj, collision);
    public void OnTriggerExit2D(T obj, Collider2D collision) => curState?.OnTriggerExit2D(obj, collision);
}

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class RbStateMachine<T> : MonoBehaviour where T : RbStateMachine<T>
{
    IRbStateMachine<T, IRbState<T>> rbStateMachine = new GenericRbStateMachine<T, IRbState<T>>();

    void FixedUpdate() => rbStateMachine.FixedUpdate((T)this);

    void Update() => rbStateMachine.Update((T)this);

    public void AddState(int id, IRbState<T> state) => rbStateMachine.AddState(id, state);

    public void RemoveState(int id) => rbStateMachine.RemoveState(id);

    public void TransitReady(int id, bool reset = false) => rbStateMachine.TransitReady(id, reset);

    void OnCollisionEnter2D(Collision2D collision) => rbStateMachine.OnCollisionEnter2D((T)this, collision);
    void OnCollisionStay2D(Collision2D collision) => rbStateMachine.OnCollisionStay2D((T)this, collision);
    void OnCollisionExit2D(Collision2D collision) => rbStateMachine.OnCollisionExit2D((T)this, collision);
    void OnTriggerEnter2D(Collider2D collision) => rbStateMachine.OnTriggerEnter2D((T)this, collision);
    void OnTriggerStay2D(Collider2D collision) => rbStateMachine.OnTriggerStay2D((T)this, collision);
    void OnTriggerExit2D(Collider2D collision) => rbStateMachine.OnTriggerExit2D((T)this, collision);
}
