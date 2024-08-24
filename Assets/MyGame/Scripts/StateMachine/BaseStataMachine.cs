using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseState<T>
{
    void Enter(T obj, int preId);

    void FixedUpdate(T obj);
    void Update(T obj);

    void Exit(T obj, int nextId);
}


/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseState<T> : IBaseState<T>
{
    virtual protected void Enter(T obj, int preId) { }
    virtual protected IEnumerator EnterCoroutine(T obj, int preId) { yield return null; }

    virtual protected void FixedUpdate(T obj) { }
    virtual protected void Update(T obj) { }

    virtual protected void Exit(T obj, int nextId) { }
    virtual protected IEnumerator ExitCoroutine(T obj, int nextId) { yield return null; }

    void IBaseState<T>.Enter(T obj, int preId) { Enter(obj, preId); }

    void IBaseState<T>.FixedUpdate(T obj) { FixedUpdate(obj); }
    void IBaseState<T>.Update(T obj) { Update(obj); }

    void IBaseState<T>.Exit(T obj, int nextId) { Exit(obj, nextId); }
}

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseStateMachine<T>  where T : BaseStateMachine<T>
{
    Dictionary<int, IBaseState<T>> states = new Dictionary<int, IBaseState<T>>();

    IBaseState<T> curState = default;
    IBaseState<T> nextState = default;

    bool reset = false;
    Coroutine coroutine;

    int preId = -1;
    int curId = -1;

    /// <summary>
    /// 前ステートのID
    /// </summary>
    public int PreStateID => preId;

    /// <summary>
    /// 現在ステートのID
    /// </summary>
    public int CurrentStateID => curId;

    public int requestId = -1;

    void FixedUpdate()
    {
        TransitState((T)this);

        if (coroutine == null) curState?.FixedUpdate((T)this);
    }

    void Update()
    {
        TransitState((T)this);

        if (coroutine == null) curState?.Update((T)this);
    }

    public void AddState(int id, IBaseState<T> state)
    {
        states.Add(id, state);
    }

    public void RemoveState(int id)
    {
        states.Remove(id);
    }

    public void TransitReady(int id, bool reset = false)
    {
        if (states.ContainsKey(id))
        {
            requestId = id;
        }
        this.reset = reset;
    }

    private void TransitState(T obj)
    {
        if (requestId != -1 && (reset || curId != requestId))
        {
            preId = curId;

            curId = requestId;
            requestId = -1;
            // 出口処理
            curState?.Exit(obj, curId);
            curState = states[curId];
            nextState = null;
            // 入口処理
            curState?.Enter(obj, preId);
        }
    }
}
