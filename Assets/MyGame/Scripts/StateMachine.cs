using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

/// <summary>
/// 状態ノード
/// </summary>
/// <typeparam name="T"></typeparam>
public class State<T> where T: MonoBehaviour
{
    public bool immediate { get; private set; }
    public State(bool immediate = true)
    {
        this.immediate = immediate;
    }

    virtual public void Enter(T obj, int preId) { }
    virtual public IEnumerator EnterCoroutine(int preId, T obj) { yield return null; }

    virtual public void FixedUpdate(T obj) { }
    virtual public void Update(T obj) { }

    virtual public void Exit(T obj, int nextId) { }
    virtual public IEnumerator ExitCoroutine(T obj) { yield return null; }
}

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class StateMachine<T> where T : MonoBehaviour
{
    Dictionary<int,State<T>> states=new Dictionary<int, State<T>>();

    State<T> curState = default;
    State<T> nextState = default;

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
    public void AddState(int id, State<T> state)
    {
        states.Add(id, state);
    }

    public void RemoveState(int id)
    {
        states.Remove(id);
    }

    public void FixedUpdate(T obj)
    {
        if (coroutine == null) curState?.FixedUpdate(obj);
    }

    public void Update(T obj)
    {
        if(coroutine == null) curState?.Update(obj);

        if (requestId != -1 && (reset || curId != requestId))
        {
            preId = curId;

            curId = requestId;
            requestId = -1;
            if (true||nextState.immediate)
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

    public void TransitState(int id, bool reset = false)
    {
        if (states.ContainsKey(id))
        {
            requestId = id;
        }
        this.reset = reset;
    }

    IEnumerator TransitStateCoroutine(T obj,int requestId)
    {
        // 出口処理
        if (curState != null) yield return curState.ExitCoroutine(obj);

        curState = states[requestId];

        // 入口処理
        yield return curState.EnterCoroutine(preId, obj);

        coroutine = null;
    }
}
