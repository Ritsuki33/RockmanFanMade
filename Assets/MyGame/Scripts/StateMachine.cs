using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    virtual public void Enter(int preId, T obj) { }
    virtual public IEnumerator EnterCoroutine(T obj) { yield return null; }

    virtual public void FixedUpdate(T obj) { }
    virtual public void Update(T obj) { }

    virtual public void Exit(T obj) { }
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

        if (nextState != null && (reset || curState != nextState))
        {
            if (nextState.immediate)
            {
                // 出口処理
                curState?.Exit(obj);
                curState = nextState;
                nextState = null;
                // 入口処理
                curState?.Enter(preId, obj);
            }
            else
            {
                var tmpNextState = nextState;
                nextState = null;
                if (coroutine != null)
                {
                    obj.StopCoroutine(coroutine);
                }
                coroutine = obj.StartCoroutine(TransitStateCoroutine(obj, tmpNextState));
            }
        }
    }

    public void TransitState(T obj,int id, bool reset = false)
    {
        if (states.ContainsKey(id))
        {
            nextState = states[id];
            preId = curId;
            curId = id;
        }
        this.reset = reset;
    }

    IEnumerator TransitStateCoroutine(T obj,State<T> nextState)
    {
        // 出口処理
        if (curState != null) yield return curState.ExitCoroutine(obj);

        curState = nextState;

        // 入口処理
        yield return curState.EnterCoroutine(obj);

        coroutine = null;
    }
}
