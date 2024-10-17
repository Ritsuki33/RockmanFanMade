using UnityEngine;
public interface IExRbState<T> : IRbState<T> where T : MonoBehaviour
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

public interface IExRbSubState<T, PS> : IRbSubState<T, PS> where T : MonoBehaviour
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

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseExRbState<T, TS, SS, SM, G> : BaseRbState<T, TS, SS, SM, G>, IExRbState<T>
    where T : MonoBehaviour
    where TS : class, IExRbState<T>
    where SS : class, IExRbSubState<T, TS>
    where SM : class, IExRbSubStateMachine<T, SS, TS>
    where G : SM, new()
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

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseExRbSubState<T, TS, SS, SM, G, PS> : BaseRbSubState<T, TS, SS, SM, G, PS>, IExRbSubState<T, PS>
    where T : MonoBehaviour
    where TS : class, IExRbSubState<T, PS>
    where SS : class, IExRbSubState<T, TS>
    where SM : class, IExRbSubStateMachine<T, SS, TS>
    where G : SM, new()
    where PS : class, IBaseCommonState<T>
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

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class ExRbState<T, TS> :
    BaseExRbState<T, TS, IExRbSubState<T, TS>, IExRbSubStateMachine<T, IExRbSubState<T, TS>, TS>, GenericExRbSubStateMachine<T, IExRbSubState<T, TS>, TS>>
    where T : MonoBehaviour
    where TS : class, IExRbState<T>
{ }

public class ExRbSubState<T, TS, PS> :
    BaseExRbSubState<T, TS, IExRbSubState<T, TS>, IExRbSubStateMachine<T, IExRbSubState<T, TS>, TS>, GenericExRbSubStateMachine<T, IExRbSubState<T, TS>, TS>, PS>
    where T : MonoBehaviour
    where TS : class, IExRbSubState<T, PS>
    where PS : class, IBaseCommonState<T>
{ }

public interface IExRbStateMachine<T, S> : IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
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

public interface IExRbSubStateMachine<T, S, PS> : IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IExRbSubState<T, PS>
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

public class GenericExRbStateMachine<T, S> : GenericRbStateMachine<T, S>, IExRbStateMachine<T, S> where T : MonoBehaviour where S : class, IExRbState<T>
{
    void IExRbStateMachine<T, S>.OnHitEnter(T obj, RaycastHit2D hit) => curState?.OnHitEnter(obj, hit);
    void IExRbStateMachine<T, S>.OnBottomHitEnter(T obj, RaycastHit2D hit) => curState?.OnBottomHitEnter(obj, hit);
    void IExRbStateMachine<T, S>.OnTopHitEnter(T obj, RaycastHit2D hit) => curState?.OnTopHitEnter(obj, hit);
    void IExRbStateMachine<T, S>.OnLeftHitEnter(T obj, RaycastHit2D hit) => curState?.OnLeftHitEnter(obj, hit);
    void IExRbStateMachine<T, S>.OnRightHitEnter(T obj, RaycastHit2D hit) => curState?.OnRightHitEnter(obj, hit);
    void IExRbStateMachine<T, S>.OnHitStay(T obj, RaycastHit2D hit) => curState?.OnHitStay(obj, hit);
    void IExRbStateMachine<T, S>.OnBottomHitStay(T obj, RaycastHit2D hit) => curState?.OnBottomHitStay(obj, hit);
    void IExRbStateMachine<T, S>.OnTopHitStay(T obj, RaycastHit2D hit) => curState?.OnTopHitStay(obj, hit);
    void IExRbStateMachine<T, S>.OnLeftHitStay(T obj, RaycastHit2D hit) => curState?.OnLeftHitStay(obj, hit);
    void IExRbStateMachine<T, S>.OnRightHitStay(T obj, RaycastHit2D hit) => curState?.OnRightHitStay(obj, hit);
    void IExRbStateMachine<T, S>.OnHitExit(T obj, RaycastHit2D hit) => curState?.OnHitExit(obj, hit);
    void IExRbStateMachine<T, S>.OnBottomHitExit(T obj, RaycastHit2D hit) => curState?.OnBottomHitExit(obj, hit);
    void IExRbStateMachine<T, S>.OnTopHitExit(T obj, RaycastHit2D hit) => curState?.OnTopHitExit(obj, hit);
    void IExRbStateMachine<T, S>.OnLeftHitExit(T obj, RaycastHit2D hit) => curState?.OnLeftHitExit(obj, hit);
    void IExRbStateMachine<T, S>.OnRightHitExit(T obj, RaycastHit2D hit) => curState?.OnRightHitExit(obj, hit);
}

public class GenericExRbSubStateMachine<T, S, PS> : GenericRbSubStateMachine<T, S, PS>, IExRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IExRbSubState<T, PS>
{
    void IExRbSubStateMachine<T, S, PS>.OnHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnHitEnter(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnBottomHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnBottomHitEnter(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnTopHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnTopHitEnter(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnLeftHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnLeftHitEnter(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnRightHitEnter(T obj, PS parent, RaycastHit2D hit) => curState?.OnRightHitEnter(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnHitStay(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnBottomHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnBottomHitStay(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnTopHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnTopHitStay(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnLeftHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnLeftHitStay(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnRightHitStay(T obj, PS parent, RaycastHit2D hit) => curState?.OnRightHitStay(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnHitExit(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnBottomHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnBottomHitExit(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnTopHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnTopHitExit(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnLeftHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnLeftHitExit(obj, parent, hit);
    void IExRbSubStateMachine<T, S, PS>.OnRightHitExit(T obj, PS parent, RaycastHit2D hit) => curState?.OnRightHitExit(obj, parent, hit);
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
    protected override void OnDisable()
    {
        base.OnDisable();
        ((IBaseExRbHit)this).OnDisable(this.gameObject);
    }

    void IBaseExRbHit.OnHitEnter(RaycastHit2D hit) => stateMachine.OnHitEnter((T)this, hit);
    void IBaseExRbHit.OnBottomHitEnter(RaycastHit2D hit) => stateMachine.OnBottomHitEnter((T)this, hit);
    void IBaseExRbHit.OnTopHitEnter(RaycastHit2D hit) => stateMachine.OnTopHitEnter((T)this, hit);
    void IBaseExRbHit.OnLeftHitEnter(RaycastHit2D hit) => stateMachine.OnLeftHitEnter((T)this, hit);
    void IBaseExRbHit.OnRightHitEnter(RaycastHit2D hit) => stateMachine.OnRightHitEnter((T)this, hit);
    void IBaseExRbHit.OnHitStay(RaycastHit2D hit) => stateMachine.OnHitStay((T)this, hit);
    void IBaseExRbHit.OnBottomHitStay(RaycastHit2D hit) => stateMachine.OnBottomHitStay((T)this, hit);
    void IBaseExRbHit.OnTopHitStay(RaycastHit2D hit) => stateMachine.OnTopHitStay((T)this, hit);
    void IBaseExRbHit.OnLeftHitStay(RaycastHit2D hit) => stateMachine.OnLeftHitStay((T)this, hit);
    void IBaseExRbHit.OnRightHitStay(RaycastHit2D hit) => stateMachine.OnRightHitStay((T)this, hit);
    void IBaseExRbHit.OnHitExit(RaycastHit2D hit) => stateMachine.OnHitExit((T)this, hit);
    void IBaseExRbHit.OnBottomHitExit(RaycastHit2D hit) => stateMachine.OnBottomHitExit((T)this, hit);
    void IBaseExRbHit.OnTopHitExit(RaycastHit2D hit) => stateMachine.OnTopHitExit((T)this, hit);
    void IBaseExRbHit.OnLeftHitExit(RaycastHit2D hit) => stateMachine.OnLeftHitExit((T)this, hit);
    void IBaseExRbHit.OnRightHitExit(RaycastHit2D hit) => stateMachine.OnRightHitExit((T)this, hit);
}