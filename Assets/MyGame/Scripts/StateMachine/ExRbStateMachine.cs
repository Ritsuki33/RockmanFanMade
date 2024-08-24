using UnityEngine;

public interface IState<T>:IBaseState<T> where T : MonoBehaviour
{
    void OnCollisionEnter2D(T obj, Collision2D collision) ;
    void OnCollisionStay2D(T obj, Collision2D collision) ;
    void OnCollisionExit2D(T obj, Collision2D collision) ;
    void OnTriggerEnter2D(T obj, Collider2D collision) ;
    void OnTriggerStay2D(T obj, Collider2D collision) ;
    void OnTriggerExit2D(T obj, Collider2D collision) ;

    void OnBottomHitEnter(T obj, RaycastHit2D hit);
    void OnTopHitEnter(T obj, RaycastHit2D hit);
    void OnLeftHitEnter(T obj, RaycastHit2D hit);
    void OnRightHitEnter(T obj, RaycastHit2D hit);
    void OnBottomHitStay(T obj, RaycastHit2D hit) ;
    void OnTopHitStay(T obj, RaycastHit2D hit) ;
    void OnLeftHitStay(T obj, RaycastHit2D hit) ;
    void OnRightHitStay(T obj, RaycastHit2D hit) ;
    void OnBottomHitExit(T obj, RaycastHit2D hit) ;
    void OnTopHitExit(T obj, RaycastHit2D hit) ;
    void OnLeftHitExit(T obj, RaycastHit2D hit) ;
    void OnRightHitExit(T obj, RaycastHit2D hit) ;
}

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class State<T> : BaseState<T>, IState<T> where T: MonoBehaviour
{
    public State(bool immediate = true) : base(immediate) { }

    virtual protected void OnCollisionEnter2D(T obj, Collision2D collision) { }
    virtual protected void OnCollisionStay2D(T obj, Collision2D collision) { }
    virtual protected void OnCollisionExit2D(T obj, Collision2D collision) { }
                        
    virtual protected void OnTriggerEnter2D(T obj, Collider2D collision) { }
    virtual protected void OnTriggerStay2D(T obj, Collider2D collision) { }
    virtual protected void OnTriggerExit2D(T obj, Collider2D collision) { }

    virtual protected void OnBottomHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitStay(T obj,RaycastHit2D hit) { }
    virtual protected void OnTopHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitExit(T obj, RaycastHit2D hit) { }

    void IState<T>.OnCollisionEnter2D(T obj, Collision2D collision) { OnCollisionEnter2D(obj, collision); }
    void IState<T>.OnCollisionStay2D(T obj, Collision2D collision) { OnCollisionStay2D(obj, collision); }
    void IState<T>.OnCollisionExit2D(T obj, Collision2D collision) { OnCollisionExit2D(obj, collision); }

    void IState<T>.OnTriggerEnter2D(T obj, Collider2D collision) { OnTriggerEnter2D(obj, collision); }
    void IState<T>.OnTriggerStay2D(T obj, Collider2D collision) { OnTriggerStay2D(obj, collision); }
    void IState<T>.OnTriggerExit2D(T obj, Collider2D collision) { OnTriggerExit2D(obj, collision); }

    void IState<T>.OnBottomHitEnter(T obj, RaycastHit2D hit) { OnBottomHitEnter(obj, hit); }
    void IState<T>.OnTopHitEnter(T obj, RaycastHit2D hit) { OnTopHitEnter(obj, hit); }
    void IState<T>.OnLeftHitEnter(T obj, RaycastHit2D hit) { OnLeftHitEnter(obj, hit); }
    void IState<T>.OnRightHitEnter(T obj, RaycastHit2D hit) { OnRightHitEnter(obj, hit); }
    void IState<T>.OnBottomHitStay(T obj, RaycastHit2D hit) { OnBottomHitStay(obj, hit); }
    void IState<T>.OnTopHitStay(T obj, RaycastHit2D hit) { OnTopHitStay(obj, hit); }
    void IState<T>.OnLeftHitStay(T obj, RaycastHit2D hit) { OnLeftHitStay(obj, hit); }
    void IState<T>.OnRightHitStay(T obj, RaycastHit2D hit) { OnRightHitStay(obj, hit); }
    void IState<T>.OnBottomHitExit(T obj, RaycastHit2D hit) { OnBottomHitExit(obj, hit); }
    void IState<T>.OnTopHitExit(T obj, RaycastHit2D hit) { OnTopHitExit(obj, hit); }
    void IState<T>.OnLeftHitExit(T obj, RaycastHit2D hit) { OnLeftHitExit(obj, hit); }
    void IState<T>.OnRightHitExit(T obj, RaycastHit2D hit) { OnRightHitExit(obj, hit); }
}

public interface IStateMachine<T, S> : IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{

    void OnCollisionEnter2D(T obj, Collision2D collision);
    void OnCollisionStay2D(T obj, Collision2D collision);
    void OnCollisionExit2D(T obj, Collision2D collision);

    void OnTriggerEnter2D(T obj, Collider2D collision);
    void OnTriggerStay2D(T obj, Collider2D collision);
    void OnTriggerExit2D(T obj, Collider2D collision);

    void OnBottomHitEnter(T obj, RaycastHit2D hit);
    void OnTopHitEnter(T obj, RaycastHit2D hit);
    void OnLeftHitEnter(T obj, RaycastHit2D hit);
    void OnRightHitEnter(T obj, RaycastHit2D hit);
    void OnBottomHitStay(T obj, RaycastHit2D hit);
    void OnTopHitStay(T obj, RaycastHit2D hit);
    void OnLeftHitStay(T obj, RaycastHit2D hit);
    void OnRightHitStay(T obj, RaycastHit2D hit); 
    void OnBottomHitExit(T obj, RaycastHit2D hit);
    void OnTopHitExit(T obj, RaycastHit2D hit);
    void OnLeftHitExit(T obj, RaycastHit2D hit);
    void OnRightHitExit(T obj, RaycastHit2D hit); 
}
public class GenericStateMachine<T, S> : GenericBaseStateMachine<T, S>, IStateMachine<T, S> where T : MonoBehaviour where S : class, IState<T>
{
    public void OnCollisionEnter2D(T obj, Collision2D collision) => curState?.OnCollisionEnter2D(obj, collision);
    public void OnCollisionStay2D(T obj, Collision2D collision) => curState?.OnCollisionStay2D(obj, collision);
    public void OnCollisionExit2D(T obj, Collision2D collision) => curState?.OnCollisionExit2D(obj, collision);

    public void OnTriggerEnter2D(T obj, Collider2D collision) => curState?.OnTriggerEnter2D(obj, collision);
    public void OnTriggerStay2D(T obj, Collider2D collision) => curState?.OnTriggerStay2D(obj, collision);
    public void OnTriggerExit2D(T obj, Collider2D collision) => curState?.OnTriggerExit2D(obj, collision);

    public void OnBottomHitEnter(T obj, RaycastHit2D hit) => curState?.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, RaycastHit2D hit) => curState?.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, RaycastHit2D hit) => curState?.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, RaycastHit2D hit) => curState?.OnRightHitEnter(obj, hit);
    public void OnBottomHitStay(T obj, RaycastHit2D hit) => curState?.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, RaycastHit2D hit) => curState?.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, RaycastHit2D hit) => curState?.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, RaycastHit2D hit) => curState?.OnRightHitStay(obj, hit);
    public void OnBottomHitExit(T obj, RaycastHit2D hit) => curState?.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, RaycastHit2D hit) => curState?.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, RaycastHit2D hit) => curState?.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, RaycastHit2D hit) => curState?.OnRightHitExit(obj, hit);
}
/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class ExRbStateMachine<T> : BaseExRbHit where T : ExRbStateMachine<T>
{
    IStateMachine<T, IState<T>> stateMachine = new GenericStateMachine<T, IState<T>>();

    void FixedUpdate() => stateMachine.FixedUpdate((T)this);

    void Update() => stateMachine.Update((T)this);

    public void AddState(int id, IState<T> state) => stateMachine.AddState(id, state);

    public void RemoveState(int id) => stateMachine.RemoveState(id);

    public void TransitReady(int id, bool reset = false) => stateMachine.TransitReady(id, reset);

    void OnCollisionEnter2D(Collision2D collision) => stateMachine.OnCollisionEnter2D((T)this, collision);
    void OnCollisionStay2D(Collision2D collision) => stateMachine.OnCollisionStay2D((T)this, collision);
    void OnCollisionExit2D(Collision2D collision) => stateMachine.OnCollisionExit2D((T)this, collision);
    void OnTriggerEnter2D(Collider2D collision) => stateMachine.OnTriggerEnter2D((T)this, collision);
    void OnTriggerStay2D(Collider2D collision) => stateMachine.OnTriggerStay2D((T)this, collision);
    void OnTriggerExit2D(Collider2D collision) => stateMachine.OnTriggerExit2D((T)this, collision);

    protected override void OnBottomHitEnter(RaycastHit2D hit) => stateMachine.OnBottomHitEnter((T)this, hit);
    protected override void OnTopHitEnter(RaycastHit2D hit) => stateMachine.OnTopHitEnter((T)this, hit);
    protected override void OnLeftHitEnter(RaycastHit2D hit) => stateMachine.OnLeftHitEnter((T)this, hit);
    protected override void OnRightHitEnter(RaycastHit2D hit) => stateMachine.OnRightHitEnter((T)this, hit);
    protected override void OnBottomHitStay(RaycastHit2D hit) => stateMachine.OnBottomHitStay((T)this, hit);
    protected override void OnTopHitStay(RaycastHit2D hit) => stateMachine.OnTopHitStay((T)this, hit);
    protected override void OnLeftHitStay(RaycastHit2D hit) => stateMachine.OnLeftHitStay((T)this, hit);
    protected override void OnRightHitStay(RaycastHit2D hit) => stateMachine.OnRightHitStay((T)this, hit);
    protected override void OnBottomHitExit(RaycastHit2D hit) => stateMachine.OnBottomHitExit((T)this, hit);
    protected override void OnTopHitExit(RaycastHit2D hit) => stateMachine.OnTopHitExit((T)this, hit);
    protected override void OnLeftHitExit(RaycastHit2D hit) => stateMachine.OnLeftHitExit((T)this, hit);
    protected override void OnRightHitExit(RaycastHit2D hit) => stateMachine.OnRightHitExit((T)this, hit);
}
