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
public class RbState<T> : State<T>, IRbState<T> where T : MonoBehaviour
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
    void IRbStateMachine<T, S>.OnCollisionEnter2D(T obj, Collision2D collision) => curState?.OnCollisionEnter2D(obj, collision);
    void IRbStateMachine<T, S>.OnCollisionStay2D(T obj, Collision2D collision) => curState?.OnCollisionStay2D(obj, collision);
    void IRbStateMachine<T, S>.OnCollisionExit2D(T obj, Collision2D collision) => curState?.OnCollisionExit2D(obj, collision);
                
    void IRbStateMachine<T, S>.OnTriggerEnter2D(T obj, Collider2D collision) => curState?.OnTriggerEnter2D(obj, collision);
    void IRbStateMachine<T, S>.OnTriggerStay2D(T obj, Collider2D collision) => curState?.OnTriggerStay2D(obj, collision);
    void IRbStateMachine<T, S>.OnTriggerExit2D(T obj, Collider2D collision) => curState?.OnTriggerExit2D(obj, collision);
}

public class BaseRbStateMachine<T, S, SM, G>
    : BaseStateMachine<T, S, SM, G>
    where T : BaseRbStateMachine<T, S, SM, G>
    where S : class, IRbState<T>
    where SM : IRbStateMachine<T, S>
    where G : SM, new()
{
    void OnCollisionEnter2D(Collision2D collision) => stateMachine.OnCollisionEnter2D((T)this, collision);
    void OnCollisionStay2D(Collision2D collision) => stateMachine.OnCollisionStay2D((T)this, collision);
    void OnCollisionExit2D(Collision2D collision) => stateMachine.OnCollisionExit2D((T)this, collision);
    void OnTriggerEnter2D(Collider2D collision) => stateMachine.OnTriggerEnter2D((T)this, collision);
    void OnTriggerStay2D(Collider2D collision) => stateMachine.OnTriggerStay2D((T)this, collision);
    void OnTriggerExit2D(Collider2D collision) => stateMachine.OnTriggerExit2D((T)this, collision);
}

/// <summary>
 /// ステートマシン
 /// </summary>
 /// <typeparam name="T"></typeparam>
public class RbStateMachine<T> :
    BaseRbStateMachine<T,IRbState<T>, IRbStateMachine<T, IRbState<T>>, GenericRbStateMachine<T, IRbState<T>>> 
    where T : RbStateMachine<T>
{}
