using System.Collections.Generic;
using UnityEngine;
/* =================================================================
T ・・・　 オブジェクト
TS ・・・  当該先具象ステート
PS ・・・　親具象ステート
S ・・・　 ステート or サブステート
SS ・・・　サブステート
SM ・・・  ステートマシン
================================================================= */


/// <summary>
/// ステート、サブステートの共通部分のクラス化
/// </summary>
/// <typeparam name="T">ボディ</typeparam>
/// <typeparam name="TS">継承元具象ステート</typeparam>
/// <typeparam name="SM">ステートマシン</typeparam>
/// <typeparam name="S">ステート(サブステート)</typeparam>
public class GenericCommonState<T, TS, SM, S>
    where SM : InheritSubStateMachine<T, TS, S>, new()
    where S : class, ISubState<T, TS>
{
    protected SM subStateMachine = null;

    /// <summary>
    /// サブステートの登録
    /// </summary>
    /// <param name="stateId"></param>
    /// <param name="state"></param>
    public void AddSubState(int stateId, S state)
    {
        if (subStateMachine == null) subStateMachine = new SM();
        subStateMachine.AddState(stateId, state);
    }
    /// <summary>
    /// サブステートの削除
    /// </summary>
    /// <param name="stateId"></param>
    public void RemoveSubState(int stateId)
    {
        subStateMachine?.RemoveState(stateId);
    }

    /// <summary>
    /// 遷移
    /// </summary>
    /// <param name="id"></param>
    /// <param name="reset"></param>
    public void TransitSubReady(int id, bool reset = false)
    {
        subStateMachine.TransitReady(id, reset);
    }

    public void TransitSubReady(int id, int subId, bool reset = false)
    {
        subStateMachine.TransitReady(id, reset, subId);
    }
}

/// <summary>
/// ステート
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TS"></typeparam>
public interface IState<T>
{
    void Enter(T obj, int preId, int subId);
    void Exit(T obj, int nextId);

    void FixedUpdate(T obj);
    void Update(T obj);
}

/// <summary>
/// 上位クラスへの継承するステートクラス
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TS"></typeparam>
/// <typeparam name="SM"></typeparam>
public class InheritState<T, TS, SM, S> : GenericCommonState<T, TS, SM, S>, IState<T>
    where TS : InheritState<T, TS, SM, S>
    where SM : InheritSubStateMachine<T, TS, S>, new()
    where S : class, ISubState<T, TS>
{
    void IState<T>.Enter(T obj, int preId, int subId)
    {
        Enter(obj, preId, subId);
    }

    void IState<T>.Exit(T obj, int nextId)
    {
        subStateMachine?.CloseState(obj, this as TS);
        Exit(obj, nextId);
    }

    void IState<T>.FixedUpdate(T obj)
    {
        FixedUpdate(obj);
        subStateMachine?.FixedUpdate(obj, this as TS);
    }
    void IState<T>.Update(T obj)
    {
        Update(obj);
        subStateMachine?.Update(obj, this as TS);
    }

    protected virtual void Enter(T obj, int preId, int subId) { }
    protected virtual void Exit(T obj, int nextId) { }

    protected virtual void FixedUpdate(T obj) { }
    protected virtual void Update(T obj) { }
}


/// <summary>
/// サブステート
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="PS"></typeparam>
public interface ISubState<T, PS>
{
    void Enter(T obj, PS parent, int preId, int subId);
    void Exit(T obj, PS parent, int nextId);

    void FixedUpdate(T obj, PS parent);
    void Update(T obj, PS parent);
}

/// <summary>
/// 上位クラスへの継承するサブステートクラス
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TS"></typeparam>
/// <typeparam name="SM"></typeparam>
public class InheritSubState<T, TS, PS, SM, S> : GenericCommonState<T, TS, SM, S>, ISubState<T, PS>
    where TS : InheritSubState<T, TS, PS, SM, S>
    where SM : InheritSubStateMachine<T, TS, S>, new()
    where S : class, ISubState<T, TS>
{
    void ISubState<T, PS>.Enter(T obj, PS parent, int preId, int subId)
    {
        Enter(obj, parent, preId, subId);
    }

    void ISubState<T, PS>.Exit(T obj, PS parent, int nextId)
    {
        Exit(obj, parent, nextId);
        subStateMachine?.CloseState(obj, this as TS);
    }

    void ISubState<T, PS>.FixedUpdate(T obj, PS parent)
    {
        FixedUpdate(obj, parent);
        subStateMachine?.FixedUpdate(obj, this as TS);
    }
    void ISubState<T, PS>.Update(T obj, PS parent)
    {
        Update(obj, parent);
        subStateMachine?.Update(obj, this as TS);
    }

    protected virtual void FixedUpdate(T obj, PS parent) { }
    protected virtual void Update(T obj, PS parent) { }

    protected virtual void Enter(T obj, PS parent, int preId, int subId) { }
    protected virtual void Exit(T obj, PS parent, int nextId) { }
}


public class CommonStateMachine<T, S> where S : class
{
    protected Dictionary<int, S> states = new Dictionary<int, S>();

    protected S curState = default;

    protected bool reset = false;

    protected int preId = -1;
    protected int curId = -1;

    protected int requestId = -1;
    protected int requestSubId = -1;

    public int CurId => curId;

    public virtual void Init()
    {
        preId = -1;
        curId = -1;

        requestId = -1;
        reset = false;
        curState = null;

        Clear();
    }

    public void AddState(int id, S state)
    {
        if (states.ContainsKey(id))
        {
            Debug.LogError($"id={id}: キーは既に存在しています。{state.ToString()}は登録できませんでした");
            return;
        }

        states.Add(id, state);
    }

    public void RemoveState(int id)
    {
        if (states.ContainsKey(id)) states.Remove(id);
    }

    public void TransitReady(int id, int subId)
    {
        TransitReady(id, false, subId);
    }

    public void TransitReady(int id, bool reset = false, int subId = -1)
    {
        if (states.ContainsKey(id) && requestId <= id)
        {
            requestId = id;
            requestSubId = subId;
        }
        this.reset = reset;
    }

    public void Clear()
    {
        states.Clear();
    }
}

public class InheritStateMachine<T, S> : CommonStateMachine<T, S>
     where S : class, IState<T>
{
    public void FixedUpdate(T obj)
    {
        TransitState(obj);

        curState?.FixedUpdate(obj);
    }

    public void Update(T obj)
    {
        TransitState(obj);

        curState?.Update(obj);
    }

    public void CloseState(T obj)
    {
        curState?.Exit(obj, curId);

        requestId = -1;
        curId = -1;
        curState = null;
    }

    void TransitState(T obj)
    {
        if (requestId != -1 && (reset || curId != requestId))
        {
            preId = curId;

            curId = requestId;
            requestId = -1;
            // 出口処理
            curState?.Exit(obj, curId);
            curState = states[curId];
            // 入口処理
            curState?.Enter(obj, preId, requestSubId);
        }
    }
}

public class InheritSubStateMachine<T, PS, S> : CommonStateMachine<T, S>
     where S : class, ISubState<T, PS>
{
    public void FixedUpdate(T obj, PS parent)
    {
        TransitState(obj, parent);

        curState?.FixedUpdate(obj, parent);
    }

    public void Update(T obj, PS parent)
    {
        TransitState(obj, parent);

        curState?.Update(obj, parent);
    }

    public void TransitState(T obj, PS parent)
    {
        if (requestId != -1 && (reset || curId != requestId))
        {
            preId = curId;

            curId = requestId;
            requestId = -1;
            // 出口処理
            curState?.Exit(obj, parent, curId);
            curState = states[curId];
            // 入口処理
            curState?.Enter(obj, parent, preId, requestSubId);
        }
    }

    public void CloseState(T obj, PS parent)
    {
        curState?.Exit(obj, parent, curId);
        requestId = -1;
        curId = -1;
        curState = null;
    }
}


public class State<T, TS> : InheritState<T, TS, SubStateMachine<T, TS>, ISubState<T, TS>> where TS : State<T, TS>
{ }

public class SubState<T, TS, PS> : InheritSubState<T, TS, PS, SubStateMachine<T, TS>, ISubState<T, TS>> where TS : SubState<T, TS, PS>
{ }

public class StateMachine<T> : InheritStateMachine<T, IState<T>>
{ }

public class SubStateMachine<T, PS> : InheritSubStateMachine<T, PS, ISubState<T, PS>>
{ }