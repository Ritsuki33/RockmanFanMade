using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IGameObjectState<T> where T : MonoBehaviour
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
public class GameObjectState<T> : IGameObjectState<T> where T : MonoBehaviour
{
    public bool immediate { get; private set; }
    public GameObjectState(bool immediate = true)
    {
        this.immediate = immediate;
    }

    virtual protected void Enter(T obj, int preId) { }
    virtual protected IEnumerator EnterCoroutine(T obj, int preId) { yield return null; }

    virtual protected void FixedUpdate(T obj) { }
    virtual protected void Update(T obj) { }

    virtual protected void Exit(T obj, int nextId) { }
    virtual protected IEnumerator ExitCoroutine(T obj, int nextId) { yield return null; }

    bool IGameObjectState<T>.Immediate => immediate;
    void IGameObjectState<T>.Enter(T obj, int preId) { Enter(obj, preId); }
    IEnumerator IGameObjectState<T>.EnterCoroutine(T obj, int preId) { yield return EnterCoroutine(obj, preId); }

    void IGameObjectState<T>.FixedUpdate(T obj) { FixedUpdate(obj); }
    void IGameObjectState<T>.Update(T obj) { Update(obj); }

    void IGameObjectState<T>.Exit(T obj, int nextId) { Exit(obj, nextId); }
    IEnumerator IGameObjectState<T>.ExitCoroutine(T obj, int nextId) { yield return ExitCoroutine(obj, nextId); }

}

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class GameObjectStateMachine<T> : MonoBehaviour where T : GameObjectStateMachine<T>
{
    Dictionary<int, IGameObjectState<T>> states = new Dictionary<int, IGameObjectState<T>>();

    IGameObjectState<T> curState = default;
    IGameObjectState<T> nextState = default;

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

    public void AddState(int id, IGameObjectState<T> state)
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
