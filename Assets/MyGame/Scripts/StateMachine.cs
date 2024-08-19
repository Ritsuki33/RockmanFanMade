using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : MonoBehaviour
{
    bool Immediate { get;}

    void Enter(T obj, int preId);
    IEnumerator EnterCoroutine(T obj, int preId);

    void FixedUpdate(T obj);
    void Update(T obj);

    void Exit(T obj, int nextId);
    IEnumerator ExitCoroutine(T obj, int nextId);

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
public class State<T> : IState<T> where T: MonoBehaviour
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

    bool IState<T>.Immediate => immediate;
    void IState<T>.Enter(T obj, int preId) { Enter(obj, preId); }
    IEnumerator IState<T>.EnterCoroutine(T obj, int preId) { yield return EnterCoroutine(obj, preId); }

    void IState<T>.FixedUpdate(T obj) { FixedUpdate(obj); }
    void IState<T>.Update(T obj) { Update(obj); }

    void IState<T>.Exit(T obj, int nextId) { Exit(obj, nextId); }
    IEnumerator IState<T>.ExitCoroutine(T obj, int nextId) { yield return ExitCoroutine(obj, nextId); }

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

/// <summary>
/// ステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class StateMachine<T> : BaseExRbHit where T : StateMachine<T>
{
    Dictionary<int, IState<T>> states = new Dictionary<int, IState<T>>();

    IState<T> curState = default;
    IState<T> nextState = default;

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

    public void AddState(int id, IState<T> state)
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

    void OnCollisionEnter2D(Collision2D collision) => curState?.OnCollisionEnter2D((T)this, collision);
    void OnCollisionStay2D(Collision2D collision) => curState?.OnCollisionEnter2D((T)this, collision);
    void OnCollisionExit2D(Collision2D collision) => curState?.OnCollisionEnter2D((T)this, collision);
    void OnTriggerEnter2D(Collider2D collision) => curState?.OnTriggerEnter2D((T)this, collision);
    void OnTriggerStay2D(Collider2D collision) => curState?.OnTriggerStay2D((T)this, collision);
    void OnTriggerExit2D(Collider2D collision) => curState?.OnTriggerExit2D((T)this, collision);

    protected override void OnBottomHitEnter(RaycastHit2D hit) => curState?.OnBottomHitEnter((T)this, hit);
    protected override void OnTopHitEnter(RaycastHit2D hit) => curState?.OnTopHitEnter((T)this, hit);
    protected override void OnLeftHitEnter(RaycastHit2D hit) => curState?.OnLeftHitEnter((T)this, hit);
    protected override void OnRightHitEnter(RaycastHit2D hit) => curState?.OnRightHitEnter((T)this, hit);
    protected override void OnBottomHitStay(RaycastHit2D hit) => curState?.OnBottomHitStay((T)this, hit);
    protected override void OnTopHitStay(RaycastHit2D hit) => curState?.OnTopHitStay((T)this, hit);
    protected override void OnLeftHitStay(RaycastHit2D hit) => curState?.OnLeftHitStay((T)this, hit);
    protected override void OnRightHitStay(RaycastHit2D hit) => curState?.OnRightHitStay((T)this, hit);
    protected override void OnBottomHitExit(RaycastHit2D hit) => curState?.OnBottomHitExit((T)this, hit);
    protected override void OnTopHitExit(RaycastHit2D hit) => curState?.OnTopHitExit((T)this, hit);
    protected override void OnLeftHitExit(RaycastHit2D hit) => curState?.OnLeftHitExit((T)this, hit);
    protected override void OnRightHitExit(RaycastHit2D hit) => curState?.OnRightHitExit((T)this, hit);
}
