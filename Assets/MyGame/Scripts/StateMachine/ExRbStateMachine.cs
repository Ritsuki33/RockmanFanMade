using UnityEngine;

public interface IExRbState<T> : IRbState<T> where T : MonoBehaviour
{
    void OnHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnBottomHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnTopHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnLeftHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnRightHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnBottomHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnTopHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnLeftHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnRightHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnBottomHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnTopHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnLeftHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnRightHitExit(T obj, RaycastHit2D hit,  IParentState parent);
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

    virtual protected void OnHitEnter(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnBottomHitEnter(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnTopHitEnter(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnLeftHitEnter(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnRightHitEnter(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnHitStay(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnBottomHitStay(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnTopHitStay(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnLeftHitStay(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnRightHitStay(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnHitExit(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnBottomHitExit(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnTopHitExit(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnLeftHitExit(T obj, RaycastHit2D hit,  IParentState parent) { }
    virtual protected void OnRightHitExit(T obj, RaycastHit2D hit,  IParentState parent) { }

    void IExRbState<T>.OnHitEnter(T obj, RaycastHit2D hit, IParentState parent)
    {
        OnHitEnter(obj, hit, parent);
        subStateMachine?.OnHitEnter(obj, hit, this);
    }
    void IExRbState<T>.OnHitStay(T obj, RaycastHit2D hit, IParentState parent)
    {
        OnHitStay(obj, hit, parent);
        subStateMachine?.OnHitStay(obj, hit, this);
    }
    void IExRbState<T>.OnHitExit(T obj, RaycastHit2D hit, IParentState parent)
    {
        OnHitExit(obj, hit, parent);
        subStateMachine?.OnHitExit(obj, hit, this);
    }
    void IExRbState<T>.OnBottomHitEnter(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnBottomHitEnter(obj, hit, parent);
        subStateMachine?.OnBottomHitEnter(obj, hit, this);
    }
    void IExRbState<T>.OnTopHitEnter(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnTopHitEnter(obj, hit, parent);
        subStateMachine?.OnTopHitEnter(obj, hit, this);
    }
    void IExRbState<T>.OnLeftHitEnter(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnLeftHitEnter(obj, hit, parent);
        subStateMachine?.OnLeftHitEnter(obj, hit, this);
    }
    void IExRbState<T>.OnRightHitEnter(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnRightHitEnter(obj, hit, parent);
        subStateMachine?.OnRightHitEnter(obj, hit, this);
    }
    void IExRbState<T>.OnBottomHitStay(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnBottomHitStay(obj, hit, parent);
        subStateMachine?.OnBottomHitStay(obj, hit, this);
    }
    void IExRbState<T>.OnTopHitStay(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnTopHitStay(obj, hit, parent);
        subStateMachine?.OnTopHitStay(obj, hit, this);
    }
    void IExRbState<T>.OnLeftHitStay(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnLeftHitStay(obj, hit, parent);
        subStateMachine?.OnLeftHitStay(obj, hit, this);
    }
    void IExRbState<T>.OnRightHitStay(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnRightHitStay(obj, hit, parent);
        subStateMachine?.OnRightHitStay(obj, hit, this);
    }
    void IExRbState<T>.OnBottomHitExit(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnBottomHitExit(obj, hit, parent);
        subStateMachine?.OnBottomHitExit(obj, hit, this);
    }
    void IExRbState<T>.OnTopHitExit(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnTopHitExit(obj, hit, parent);
        subStateMachine?.OnTopHitExit(obj, hit, this);
    }
    void IExRbState<T>.OnLeftHitExit(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnLeftHitExit(obj, hit, parent);
        subStateMachine?.OnLeftHitExit(obj, hit, this);
    }
    void IExRbState<T>.OnRightHitExit(T obj, RaycastHit2D hit,  IParentState parent)
    {
        OnRightHitExit(obj, hit, parent);
        subStateMachine?.OnRightHitExit(obj, hit, this);
    }
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
    void OnHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnBottomHitEnter(T obj, RaycastHit2D hit, IParentState parent);
    void OnTopHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnLeftHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnRightHitEnter(T obj, RaycastHit2D hit,  IParentState parent);
    void OnHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnBottomHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnTopHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnLeftHitStay(T obj, RaycastHit2D hit,  IParentState parent);
    void OnRightHitStay(T obj, RaycastHit2D hit,  IParentState parent); 
    void OnHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnBottomHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnTopHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnLeftHitExit(T obj, RaycastHit2D hit,  IParentState parent);
    void OnRightHitExit(T obj, RaycastHit2D hit,  IParentState parent); 
}

public class GenericExRbStateMachine<T, S> : GenericRbStateMachine<T, S>, IExRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
{
    public void OnHitEnter(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnHitEnter(obj, hit, parent);
    public void OnBottomHitEnter(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnBottomHitEnter(obj, hit, parent);
    public void OnTopHitEnter(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnTopHitEnter(obj, hit, parent);
    public void OnLeftHitEnter(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnLeftHitEnter(obj, hit, parent);
    public void OnRightHitEnter(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnRightHitEnter(obj, hit, parent);
    public void OnHitStay(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnHitStay(obj, hit, parent);
    public void OnBottomHitStay(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnBottomHitStay(obj, hit, parent);
    public void OnTopHitStay(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnTopHitStay(obj, hit, parent);
    public void OnLeftHitStay(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnLeftHitStay(obj, hit, parent);
    public void OnRightHitStay(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnRightHitStay(obj, hit, parent);
    public void OnHitExit(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnHitExit(obj, hit, parent);
    public void OnBottomHitExit(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnBottomHitExit(obj, hit, parent);
    public void OnTopHitExit(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnTopHitExit(obj, hit, parent);
    public void OnLeftHitExit(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnLeftHitExit(obj, hit, parent);
    public void OnRightHitExit(T obj, RaycastHit2D hit,  IParentState parent) => curState?.OnRightHitExit(obj, hit, parent);
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

    void IBaseExRbHit.OnHitEnter(RaycastHit2D hit) => stateMachine.OnHitEnter((T)this, hit, null);
    void IBaseExRbHit.OnBottomHitEnter(RaycastHit2D hit) => stateMachine.OnBottomHitEnter((T)this, hit, null);
    void IBaseExRbHit.OnTopHitEnter(RaycastHit2D hit) => stateMachine.OnTopHitEnter((T)this, hit, null);
    void IBaseExRbHit.OnLeftHitEnter(RaycastHit2D hit) => stateMachine.OnLeftHitEnter((T)this, hit, null);
    void IBaseExRbHit.OnRightHitEnter(RaycastHit2D hit) => stateMachine.OnRightHitEnter((T)this, hit, null);
    void IBaseExRbHit.OnHitStay(RaycastHit2D hit) => stateMachine.OnHitStay((T)this, hit, null);
    void IBaseExRbHit.OnBottomHitStay(RaycastHit2D hit) => stateMachine.OnBottomHitStay((T)this, hit, null);
    void IBaseExRbHit.OnTopHitStay(RaycastHit2D hit) => stateMachine.OnTopHitStay((T)this, hit, null);
    void IBaseExRbHit.OnLeftHitStay(RaycastHit2D hit) => stateMachine.OnLeftHitStay((T)this, hit, null);
    void IBaseExRbHit.OnRightHitStay(RaycastHit2D hit) => stateMachine.OnRightHitStay((T)this, hit, null);
    void IBaseExRbHit.OnHitExit(RaycastHit2D hit) => stateMachine.OnHitExit((T)this, hit, null);
    void IBaseExRbHit.OnBottomHitExit(RaycastHit2D hit) => stateMachine.OnBottomHitExit((T)this, hit, null);
    void IBaseExRbHit.OnTopHitExit(RaycastHit2D hit) => stateMachine.OnTopHitExit((T)this, hit, null);
    void IBaseExRbHit.OnLeftHitExit(RaycastHit2D hit) => stateMachine.OnLeftHitExit((T)this, hit, null);
    void IBaseExRbHit.OnRightHitExit(RaycastHit2D hit) => stateMachine.OnRightHitExit((T)this, hit, null);
}
