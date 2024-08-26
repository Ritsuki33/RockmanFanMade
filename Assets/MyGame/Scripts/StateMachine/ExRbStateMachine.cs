using UnityEngine;

public interface IExRbState<T>: IRbState<T> where T : MonoBehaviour
{
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
public class BaseExRbState<T, S, SM, G> : BaseRbState<T, S, SM, G>, IExRbState<T>
    where T : MonoBehaviour
    where S : class, IExRbState<T>
    where SM : class, IExRbStateMachine<T, S>
    where G : SM, new()
{
    public BaseExRbState(bool immediate = true) : base(immediate) { }

    virtual protected void OnBottomHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitEnter(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitStay(T obj, RaycastHit2D hit) { }
    virtual protected void OnBottomHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnTopHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnLeftHitExit(T obj, RaycastHit2D hit) { }
    virtual protected void OnRightHitExit(T obj, RaycastHit2D hit) { }

    void IExRbState<T>.OnBottomHitEnter(T obj, RaycastHit2D hit) { OnBottomHitEnter(obj, hit); }
    void IExRbState<T>.OnTopHitEnter(T obj, RaycastHit2D hit) { OnTopHitEnter(obj, hit); }
    void IExRbState<T>.OnLeftHitEnter(T obj, RaycastHit2D hit) { OnLeftHitEnter(obj, hit); }
    void IExRbState<T>.OnRightHitEnter(T obj, RaycastHit2D hit) { OnRightHitEnter(obj, hit); }
    void IExRbState<T>.OnBottomHitStay(T obj, RaycastHit2D hit) { OnBottomHitStay(obj, hit); }
    void IExRbState<T>.OnTopHitStay(T obj, RaycastHit2D hit) { OnTopHitStay(obj, hit); }
    void IExRbState<T>.OnLeftHitStay(T obj, RaycastHit2D hit) { OnLeftHitStay(obj, hit); }
    void IExRbState<T>.OnRightHitStay(T obj, RaycastHit2D hit) { OnRightHitStay(obj, hit); }
    void IExRbState<T>.OnBottomHitExit(T obj, RaycastHit2D hit) { OnBottomHitExit(obj, hit); }
    void IExRbState<T>.OnTopHitExit(T obj, RaycastHit2D hit) { OnTopHitExit(obj, hit); }
    void IExRbState<T>.OnLeftHitExit(T obj, RaycastHit2D hit) { OnLeftHitExit(obj, hit); }
    void IExRbState<T>.OnRightHitExit(T obj, RaycastHit2D hit) { OnRightHitExit(obj, hit); }
}

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class ExRbState<T>
    : BaseExRbState<T, IExRbState<T>, IExRbStateMachine<T, IExRbState<T>>, GenericExRbStateMachine<T, IExRbState<T>>> where T : MonoBehaviour
{ }

public interface IExRbStateMachine<T, S> : IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{
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

public class GenericExRbStateMachine<T, S> : GenericRbStateMachine<T, S>, IExRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
{
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
public class ExRbStateMachine<T> 
    : BaseRbStateMachine<T, IExRbState<T>, IExRbStateMachine<T, IExRbState<T>>, GenericExRbStateMachine<T, IExRbState<T>>>, IBaseExRbHit 
    where T : ExRbStateMachine<T>
{
    private void OnEnable()
    {
        ((IBaseExRbHit)this).OnEnable(this.gameObject);
    }
    private void OnDisable()
    {
        ((IBaseExRbHit)this).OnDisable(this.gameObject);
    }

    void IBaseExRbHit.OnBottomHitEnter(RaycastHit2D hit) => stateMachine.OnBottomHitEnter((T)this, hit);
    void IBaseExRbHit.OnTopHitEnter(RaycastHit2D hit) => stateMachine.OnTopHitEnter((T)this, hit);
    void IBaseExRbHit.OnLeftHitEnter(RaycastHit2D hit) => stateMachine.OnLeftHitEnter((T)this, hit);
    void IBaseExRbHit.OnRightHitEnter(RaycastHit2D hit) => stateMachine.OnRightHitEnter((T)this, hit);
    void IBaseExRbHit.OnBottomHitStay(RaycastHit2D hit) => stateMachine.OnBottomHitStay((T)this, hit);
    void IBaseExRbHit.OnTopHitStay(RaycastHit2D hit) => stateMachine.OnTopHitStay((T)this, hit);
    void IBaseExRbHit.OnLeftHitStay(RaycastHit2D hit) => stateMachine.OnLeftHitStay((T)this, hit);
    void IBaseExRbHit.OnRightHitStay(RaycastHit2D hit) => stateMachine.OnRightHitStay((T)this, hit);
    void IBaseExRbHit.OnBottomHitExit(RaycastHit2D hit) => stateMachine.OnBottomHitExit((T)this, hit);
    void IBaseExRbHit.OnTopHitExit(RaycastHit2D hit) => stateMachine.OnTopHitExit((T)this, hit);
    void IBaseExRbHit.OnLeftHitExit(RaycastHit2D hit) => stateMachine.OnLeftHitExit((T)this, hit);
    void IBaseExRbHit.OnRightHitExit(RaycastHit2D hit) => stateMachine.OnRightHitExit((T)this, hit);
}
