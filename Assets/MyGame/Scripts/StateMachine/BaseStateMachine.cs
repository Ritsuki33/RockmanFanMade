using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBaseState<T> where T : MonoBehaviour
{
    bool Immediate { get; }

    void Enter(T obj, int preId, int subId);
    IEnumerator EnterCoroutine(T obj, int preId);

    void FixedUpdate(T obj, IParentState parent);
    void Update(T obj, IParentState parent);

    void Exit(T obj, int nextId);
    IEnumerator ExitCoroutine(T obj, int nextId);
}

public interface IParentState
{
    void TransitSubReady(int id, int subId=-1, bool reset = false);
}

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseState<T, S, SM, G> : IBaseState<T>, IParentState
    where T : MonoBehaviour
    where S : class, IBaseState<T>
    where SM : class, IBaseStateMachine<T, S>
    where G : SM, new()
{
    public bool immediate { get; private set; }

    protected SM subStateMachine = null;
    public BaseState(bool immediate = true)
    {
        this.immediate = immediate;
    }

    /// <summary>
    /// サブステートの登録
    /// </summary>
    /// <param name="stateId"></param>
    /// <param name="state"></param>
    protected void AddSubState(int stateId,S state)
    {
        if (subStateMachine == null) subStateMachine = new G();
        subStateMachine.AddState(stateId, state);
    }

    /// <summary>
    /// サブステートの削除
    /// </summary>
    /// <param name="stateId"></param>
    protected void RemoveSubState(int stateId)
    {
        subStateMachine?.RemoveState(stateId);
    }

    protected void TransitSubReady(int id,bool reset = false)
    {
        subStateMachine.TransitReady(id, reset);
    }

    protected void TransitSubReady(int id, int subId, bool reset = false)
    {
        subStateMachine.TransitReady(id, reset, subId);
    }

    protected void CloseState(T obj)
    {
        subStateMachine.CloseState(obj);
    }

    /// <summary>
    /// サブステートの遷移準備
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subId"></param>
    /// <param name="reset"></param>
    void IParentState.TransitSubReady(int id, int subId, bool reset)
    {
        TransitSubReady(id, subId, reset);
    }

    virtual protected void Enter(T obj, int preId, int subId) { }
    virtual protected IEnumerator EnterCoroutine(T obj, int preId) { yield return null; }

    virtual protected void FixedUpdate(T obj, IParentState parent) { }
    virtual protected void Update(T obj, IParentState parent) { }

    virtual protected void Exit(T obj, int nextId) { }
    virtual protected IEnumerator ExitCoroutine(T obj, int nextId) { yield return null; }

    bool IBaseState<T>.Immediate => immediate;
    void IBaseState<T>.Enter(T obj, int preId, int subId) { Enter(obj, preId, subId); }
    IEnumerator IBaseState<T>.EnterCoroutine(T obj, int preId) { yield return EnterCoroutine(obj, preId); }

    void IBaseState<T>.FixedUpdate(T obj, IParentState parent)
    {
        FixedUpdate(obj, parent);
        subStateMachine?.FixedUpdate(obj, this);
    }

    void IBaseState<T>.Update(T obj, IParentState parent)
    {
        Update(obj, parent);
        subStateMachine?.Update(obj, this);
    }

    void IBaseState<T>.Exit(T obj, int nextId) {
        subStateMachine?.CloseState(obj);
        Exit(obj, nextId);
    }
    IEnumerator IBaseState<T>.ExitCoroutine(T obj, int nextId) { yield return ExitCoroutine(obj, nextId); }
}

public class State<T>
    : BaseState<T, IBaseState<T>, IBaseStateMachine<T, IBaseState<T>>, GenericBaseStateMachine<T, IBaseState<T>>>
    where T : MonoBehaviour
{ }

public interface IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{
    int CurId { get; }
    void FixedUpdate(T obj, IParentState parent);
    void Update(T obj, IParentState parent);
    void AddState(int id, S state);
    void RemoveState(int id);
    void TransitReady(int id, bool reset=false, int subId=-1);

    void CloseState(T obj);
}

public class GenericBaseStateMachine<T, S> : IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{
    Dictionary<int, S> states = new Dictionary<int, S>();

    protected S curState = default;

    bool reset = false;
    Coroutine coroutine;

    int preId = -1;
    int curId = -1;

    int requestId = -1;
    int requestSubId = -1;

    int IBaseStateMachine<T, S>.CurId => curId;

    void Init()
    {
        preId = -1;
        curId = -1;

        requestId = -1;
        reset = false;
        curState = null;
    }

    void IBaseStateMachine<T, S>.FixedUpdate(T obj, IParentState parent)
    {
        TransitState(obj);

        if (coroutine == null) curState?.FixedUpdate(obj, parent);
    }

    void IBaseStateMachine<T, S>.Update(T obj, IParentState parent)
    {
        TransitState(obj);

        if (coroutine == null) curState?.Update(obj, parent);
    }

    void IBaseStateMachine<T, S>.AddState(int id, S state)
    {
        states.Add(id, state);
    }

    void IBaseStateMachine<T, S>.RemoveState(int id)
    {
        states.Remove(id);
    }

    void IBaseStateMachine<T, S>.TransitReady(int id, bool reset,int subId)
    {
        if (states.ContainsKey(id))
        {
            requestId = id;
            requestSubId = subId;
        }
        this.reset = reset;
    }

    void IBaseStateMachine<T, S>.CloseState(T obj)
    {
        curState?.Exit(obj, curId);
        requestId = -1;
        curId= -1;
        curState = null;
    }

    void TransitState(T obj)
    {
        if (requestId != -1 && (reset || curId != requestId))
        {
            preId = curId;

            curId = requestId;
            requestId = -1;
            if (states[curId].Immediate)
            {
                // 出口処理
                curState?.Exit(obj, curId);
                curState = states[curId];
                // 入口処理
                curState?.Enter(obj, preId, requestSubId);
            }
            else
            {
                if (coroutine != null)
                {
                    obj.StopCoroutine(coroutine);
                }
                coroutine = obj.StartCoroutine(TransitStateCoroutine(obj, curId));
            }
        }
    }

    IEnumerator TransitStateCoroutine(T obj, int requestId)
    {
        // 出口処理
        if (curState != null) yield return curState.ExitCoroutine(obj, curId);

        curState = states[requestId];

        // 入口処理
        yield return curState.EnterCoroutine(obj, preId);

        coroutine = null;
    }
}

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseStateMachine<T, S, SM, G>
    : MonoBehaviour
    where T : BaseStateMachine<T, S, SM, G>
    where S : class, IBaseState<T>
    where SM: IBaseStateMachine<T, S>
    where G: SM, new()
{
    protected SM stateMachine = new G();

    void FixedUpdate()
    {
        StartFixedUpdate();
        stateMachine.FixedUpdate((T)this,null);
        EndtFixedUpdate();
    }

    void Update()
    {
        StartUpdate();
        stateMachine.Update((T)this, null);
        EndtUpdate();
    }

    public void AddState(int id, S state) => stateMachine.AddState(id, state);

    public void RemoveState(int id) => stateMachine.RemoveState(id);

    public void TransitReady(int id, bool reset = false, int subId = -1) => stateMachine.TransitReady(id, reset, subId);

    protected virtual void StartFixedUpdate() { }
    protected virtual void EndtFixedUpdate() { }

    protected virtual void StartUpdate() { }
    protected virtual void EndtUpdate() { }
}


public class StateMachine<T>
    : BaseStateMachine<T, IBaseState<T>, IBaseStateMachine<T, IBaseState<T>>, GenericBaseStateMachine<T, IBaseState<T>>>
    where T : StateMachine<T>
{}