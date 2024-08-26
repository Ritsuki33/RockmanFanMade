using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IBaseState<T> where T : MonoBehaviour
{
    bool Immediate { get; }

    void Enter(T obj, int preId);
    IEnumerator EnterCoroutine(T obj, int preId);

    void FixedUpdate(T obj);
    void Update(T obj);

    void Exit(T obj, int nextId);
    IEnumerator ExitCoroutine(T obj, int nextId);
}


/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class State<T> : IBaseState<T> where T : MonoBehaviour
{
    public bool immediate { get; private set; }
    public State(bool immediate = true)
    {
        this.immediate = immediate;
    }

    virtual protected void Enter(T obj, int preId) { }
    virtual protected IEnumerator EnterCoroutine(T obj, int preId) { yield return null; }

    virtual protected void FixedUpdate(T obj) { }
    virtual protected void Update(T obj) { }

    virtual protected void Exit(T obj, int nextId) { }
    virtual protected IEnumerator ExitCoroutine(T obj, int nextId) { yield return null; }

    bool IBaseState<T>.Immediate => immediate;
    void IBaseState<T>.Enter(T obj, int preId) { Enter(obj, preId); }
    IEnumerator IBaseState<T>.EnterCoroutine(T obj, int preId) { yield return EnterCoroutine(obj, preId); }

    void IBaseState<T>.FixedUpdate(T obj) { FixedUpdate(obj); }
    void IBaseState<T>.Update(T obj) { Update(obj); }

    void IBaseState<T>.Exit(T obj, int nextId) { Exit(obj, nextId); }
    IEnumerator IBaseState<T>.ExitCoroutine(T obj, int nextId) { yield return ExitCoroutine(obj, nextId); }

}


public interface IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{
    int CurId { get; }
    void FixedUpdate(T obj);
    void Update(T obj);
    void AddState(int id, S state);
    void RemoveState(int id);
    void TransitReady(int id, bool reset=false);
}

public class GenericBaseStateMachine<T, S> : IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
{
    Dictionary<int, S> states = new Dictionary<int, S>();

    protected S curState = default;
    S nextState = default;

    bool reset = false;
    Coroutine coroutine;

    int preId = -1;
    int curId = -1;

    int requestId = -1;

    int IBaseStateMachine<T, S>.CurId => curId;

    void IBaseStateMachine<T, S>.FixedUpdate(T obj)
    {
        TransitState(obj);

        if (coroutine == null) curState?.FixedUpdate(obj);
    }

    void IBaseStateMachine<T, S>.Update(T obj)
    {
        TransitState(obj);

        if (coroutine == null) curState?.Update(obj);
    }

    void IBaseStateMachine<T, S>.AddState(int id, S state)
    {
        states.Add(id, state);
    }

    void IBaseStateMachine<T, S>.RemoveState(int id)
    {
        states.Remove(id);
    }

    void IBaseStateMachine<T, S>.TransitReady(int id, bool reset)
    {
        if (states.ContainsKey(id))
        {
            requestId = id;
        }
        this.reset = reset;
    }

    void TransitState(T obj)
    {
        if (requestId != -1 && (reset || curId != requestId))
        {
            preId = curId;

            curId = requestId;
            requestId = -1;
            if (true || nextState.Immediate)
            {
                // 出口処理
                curState?.Exit(obj, curId);
                curState = states[curId];
                nextState = null;
                // 入口処理
                curState?.Enter(obj, preId);
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
        stateMachine.FixedUpdate((T)this);
        EndtFixedUpdate();
    }

    void Update()
    {
        StartUpdate();
        stateMachine.Update((T)this);
        EndtUpdate();
    }

    public void AddState(int id, S state) => stateMachine.AddState(id, state);

    public void RemoveState(int id) => stateMachine.RemoveState(id);

    public void TransitReady(int id, bool reset = false) => stateMachine.TransitReady(id, reset);

    protected virtual void StartFixedUpdate() { }
    protected virtual void EndtFixedUpdate() { }

    protected virtual void StartUpdate() { }
    protected virtual void EndtUpdate() { }
}


public class StateMachine<T>
    : BaseStateMachine<T, IBaseState<T>, IBaseStateMachine<T, IBaseState<T>>, GenericBaseStateMachine<T, IBaseState<T>>>
    where T : StateMachine<T>
{}