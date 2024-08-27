using UnityEngine;

public interface IRbState<T> : IBaseState<T> where T : MonoBehaviour
{
    void OnCollisionEnter2D(T obj, Collision2D collision,  IParentState parent);
    void OnCollisionStay2D(T obj, Collision2D collision,  IParentState parent);
    void OnCollisionExit2D(T obj, Collision2D collision,  IParentState parent);
    void OnTriggerEnter2D(T obj, Collider2D collision,  IParentState parent);
    void OnTriggerStay2D(T obj, Collider2D collision,  IParentState parent);
    void OnTriggerExit2D(T obj, Collider2D collision,  IParentState parent);
}

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRbState<T, S, SM, G> : BaseState<T, S, SM, G>, IRbState<T>
    where T : MonoBehaviour
    where S : class, IRbState<T>
    where SM : class, IRbStateMachine<T, S>
    where G : SM, new()
{
    public BaseRbState(bool immediate = true) : base(immediate) { }

    virtual protected void OnCollisionEnter2D(T obj, Collision2D collision,  IParentState parent) { }
    virtual protected void OnCollisionStay2D(T obj, Collision2D collision,  IParentState parent) { }
    virtual protected void OnCollisionExit2D(T obj, Collision2D collision,  IParentState parent) { }

    virtual protected void OnTriggerEnter2D(T obj, Collider2D collision,  IParentState parent) { }
    virtual protected void OnTriggerStay2D(T obj, Collider2D collision,  IParentState parent) { }
    virtual protected void OnTriggerExit2D(T obj, Collider2D collision,  IParentState parent) { }

    void IRbState<T>.OnCollisionEnter2D(T obj, Collision2D collision,  IParentState parent)
    {
        OnCollisionEnter2D(obj, collision, parent);
        subStateMachine?.OnCollisionEnter2D(obj, collision,this);
    }

    void IRbState<T>.OnCollisionStay2D(T obj, Collision2D collision,  IParentState parent)
    {
        OnCollisionStay2D(obj, collision, parent);
        subStateMachine?.OnCollisionStay2D(obj, collision,this);
    }

    void IRbState<T>.OnCollisionExit2D(T obj, Collision2D collision,  IParentState parent)
    {
        OnCollisionExit2D(obj, collision, parent);
        subStateMachine?.OnCollisionExit2D(obj, collision, this);
    }

    void IRbState<T>.OnTriggerEnter2D(T obj, Collider2D collision,  IParentState parent)
    {
        OnTriggerEnter2D(obj, collision, parent);
        subStateMachine?.OnTriggerEnter2D(obj, collision, this);
    }

    void IRbState<T>.OnTriggerStay2D(T obj, Collider2D collision,  IParentState parent)
    {
        OnTriggerStay2D(obj, collision, parent);
        subStateMachine?.OnTriggerStay2D(obj, collision, this);
    }

    void IRbState<T>.OnTriggerExit2D(T obj, Collider2D collision,  IParentState parent)
    {
        OnTriggerExit2D(obj, collision, parent);
        subStateMachine?.OnTriggerExit2D(obj, collision, this);
    }
}

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class RbState<T>
    : BaseRbState<T, IRbState<T>, IRbStateMachine<T, IRbState<T>>, GenericRbStateMachine<T, IRbState<T>>>
    where T : MonoBehaviour
{ }

public interface IRbStateMachine<T, S> : IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{
    void OnCollisionEnter2D(T obj, Collision2D collision,  IParentState parent);
    void OnCollisionStay2D(T obj, Collision2D collision,  IParentState parent);
    void OnCollisionExit2D(T obj, Collision2D collision,  IParentState parent);

    void OnTriggerEnter2D(T obj, Collider2D collision,  IParentState parent);
    void OnTriggerStay2D(T obj, Collider2D collision,  IParentState parent);
    void OnTriggerExit2D(T obj, Collider2D collision,  IParentState parent);
}

public class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
{
    void IRbStateMachine<T, S>.OnCollisionEnter2D(T obj, Collision2D collision,  IParentState parent) => curState?.OnCollisionEnter2D(obj, collision, parent);
    void IRbStateMachine<T, S>.OnCollisionStay2D(T obj, Collision2D collision,  IParentState parent) => curState?.OnCollisionStay2D(obj, collision, parent);
    void IRbStateMachine<T, S>.OnCollisionExit2D(T obj, Collision2D collision,  IParentState parent) => curState?.OnCollisionExit2D(obj, collision, parent);
                
    void IRbStateMachine<T, S>.OnTriggerEnter2D(T obj, Collider2D collision,  IParentState parent) => curState?.OnTriggerEnter2D(obj, collision, parent);
    void IRbStateMachine<T, S>.OnTriggerStay2D(T obj, Collider2D collision,  IParentState parent) => curState?.OnTriggerStay2D(obj, collision, parent);
    void IRbStateMachine<T, S>.OnTriggerExit2D(T obj, Collider2D collision,  IParentState parent) => curState?.OnTriggerExit2D(obj, collision, parent);
}

public class BaseRbStateMachine<T, S, SM, G>
    : BaseStateMachine<T, S, SM, G>
    where T : BaseRbStateMachine<T, S, SM, G>
    where S : class, IRbState<T>
    where SM : IRbStateMachine<T, S>
    where G : SM, new()
{
    void OnCollisionEnter2D(Collision2D collision) => stateMachine.OnCollisionEnter2D((T)this, collision,null);
    void OnCollisionStay2D(Collision2D collision) => stateMachine.OnCollisionStay2D((T)this, collision, null);
    void OnCollisionExit2D(Collision2D collision) => stateMachine.OnCollisionExit2D((T)this, collision, null);
    void OnTriggerEnter2D(Collider2D collision) => stateMachine.OnTriggerEnter2D((T)this, collision, null);
    void OnTriggerStay2D(Collider2D collision) => stateMachine.OnTriggerStay2D((T)this, collision, null);
    void OnTriggerExit2D(Collider2D collision) => stateMachine.OnTriggerExit2D((T)this, collision, null);
}

/// <summary>
 /// ステートマシン
 /// </summary>
 /// <typeparam name="T"></typeparam>
public class RbStateMachine<T> :
    BaseRbStateMachine<T,IRbState<T>, IRbStateMachine<T, IRbState<T>>, GenericRbStateMachine<T, IRbState<T>>> 
    where T : RbStateMachine<T>
{}
