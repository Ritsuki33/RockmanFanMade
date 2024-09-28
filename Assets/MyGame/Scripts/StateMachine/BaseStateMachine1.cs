using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Test
{
    public interface IBaseCommonState<T> where T : MonoBehaviour { }

    public interface IBaseState<T> : IBaseCommonState<T> where T : MonoBehaviour
    {
        void Enter(T obj, int preId, int subId);
        void Exit(T obj, int nextId);

        void FixedUpdate(T obj);
        void Update(T obj);
    }

    public interface IBaseSubState<T, PS> : IBaseCommonState<T>
        where T : MonoBehaviour
    {
        void Enter(T obj, PS parent, int preId, int subId);
        void Exit(T obj, PS parent, int nextId);

        void FixedUpdate(T obj, PS parent);
        void Update(T obj, PS parent);
    }

    public class BaseCommonState<T, S, SM, G> : IBaseCommonState<T>
      where T : MonoBehaviour
      where S : class, IBaseCommonState<T>                      // サブステートの制約
      where SM : class, IBaseCommonStateMachine<T, S>　　       // サブステートマシンの制約
      where G : SM, new()
    {
        protected SM subStateMachine = null;

        /// <summary>
        /// サブステートの登録
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="state"></param>
        protected void AddSubState(int stateId, S state)
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
    /// 状態ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseState<T, S, SM, G, PS> : BaseCommonState<T, S, SM, G>, IBaseState<T>
        where T : MonoBehaviour
        where S : class, IBaseSubState<T, PS>
        where SM : class, IBaseSubStateMachine<T, S, PS>
        where G : SM, new()
        where PS : class, IBaseState<T>
    {
        virtual protected void FixedUpdate(T obj) { }
        virtual protected void Update(T obj) { }
        virtual protected void Enter(T obj, int preId, int subId) { }
        virtual protected void Exit(T obj, int nextId) { }

        void IBaseState<T>.Enter(T obj, int preId, int subId) { Enter(obj, preId, subId); }

        void IBaseState<T>.Exit(T obj, int nextId)
        {
            subStateMachine?.CloseState(obj, this as PS);
            Exit(obj, nextId);
        }

        void IBaseState<T>.FixedUpdate(T obj)
        {
            FixedUpdate(obj);
            subStateMachine?.FixedUpdate(obj, this as PS);
        }

        void IBaseState<T>.Update(T obj)
        {
            Update(obj);
            subStateMachine?.Update(obj, this as PS);
        }
    }

    public class BaseSubState<T, S, SM, G, PS> : BaseCommonState<T, S, SM, G>, IBaseSubState<T, PS>
       where T : MonoBehaviour
       where S : class, IBaseSubState<T, PS>
       where SM : class, IBaseSubStateMachine<T, S, PS>
       where G : SM, new()
       where PS : class, IBaseCommonState<T>
    {
        virtual protected void FixedUpdate(T obj, PS parent) { }
        virtual protected void Update(T obj, PS parent) { }
        virtual protected void Enter(T obj, PS parent, int preId, int subId) { }
        virtual protected void Exit(T obj, PS parent, int nextId) { }

        void IBaseSubState<T, PS>.Enter(T obj, PS parent, int preId, int subId)
        {
            Enter(obj, parent, preId, subId);
        }
        void IBaseSubState<T, PS>.Exit(T obj, PS parent, int nextId)
        {
            subStateMachine?.CloseState(obj, parent);
            Exit(obj, parent, nextId);
        }

        void IBaseSubState<T, PS>.FixedUpdate(T obj, PS parent)
        {
            FixedUpdate(obj, parent);
            subStateMachine?.FixedUpdate(obj, this as PS);
        }

        void IBaseSubState<T, PS>.Update(T obj, PS parent)
        {
            Update(obj, parent);
            subStateMachine?.Update(obj, this as PS);
        }
    }

    public class State<T, PS> :
        BaseState<T, IBaseSubState<T, PS>, IBaseSubStateMachine<T, IBaseSubState<T, PS>, PS>, GenericBaseSubStateMachine<T, IBaseSubState<T, PS>, PS>, PS>
        where T : MonoBehaviour
        where PS : class, IBaseState<T>
    { }

    public class SubState<T, PS> :
        BaseSubState<T, IBaseSubState<T, PS>, IBaseSubStateMachine<T, IBaseSubState<T, PS>, PS>, GenericBaseSubStateMachine<T, IBaseSubState<T, PS>, PS>, PS>
        where T : MonoBehaviour
        where PS : class, IBaseCommonState<T>
    { }

    public interface IBaseCommonStateMachine<T, S> where T : MonoBehaviour
    {
        int CurId { get; }
        void AddState(int id, S state);
        void RemoveState(int id);
        void TransitReady(int id, bool reset = false, int subId = -1);

    }

    public interface IBaseStateMachine<T, S> : IBaseCommonStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
    {
        void FixedUpdate(T obj);
        void Update(T obj);
        void CloseState(T obj);
    }

    public interface IBaseSubStateMachine<T, S, PS> : IBaseCommonStateMachine<T, S>
        where T : MonoBehaviour
    {
        void FixedUpdate(T obj, PS parent);
        void Update(T obj, PS parent);
        void CloseState(T obj, PS parent);
    }

    public class GenericBaseCommonStateMachine<T, S> : IBaseCommonStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseCommonState<T>
    {
        protected Dictionary<int, S> states = new Dictionary<int, S>();

        protected S curState = default;

        protected bool reset = false;

        protected int preId = -1;
        protected int curId = -1;

        protected int requestId = -1;
        protected int requestSubId = -1;

        int IBaseCommonStateMachine<T, S>.CurId => curId;

        protected void Init()
        {
            preId = -1;
            curId = -1;

            requestId = -1;
            reset = false;
            curState = null;
        }

        void IBaseCommonStateMachine<T, S>.AddState(int id, S state)
        {
            states.Add(id, state);
        }

        void IBaseCommonStateMachine<T, S>.RemoveState(int id)
        {
            states.Remove(id);
        }

        void IBaseCommonStateMachine<T, S>.TransitReady(int id, bool reset, int subId)
        {
            if (states.ContainsKey(id))
            {
                requestId = id;
                requestSubId = subId;
            }
            this.reset = reset;
        }


    }
    public class GenericBaseStateMachine<T, S> : GenericBaseCommonStateMachine<T, S>, IBaseStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
    {
        void IBaseStateMachine<T, S>.FixedUpdate(T obj)
        {
            TransitState(obj);

            curState?.FixedUpdate(obj);
        }

        void IBaseStateMachine<T, S>.Update(T obj)
        {
            TransitState(obj);

            curState?.Update(obj);
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
        void IBaseStateMachine<T, S>.CloseState(T obj)
        {
            curState?.Exit(obj, curId);
            requestId = -1;
            curId = -1;
            curState = null;
        }
    }

    public class GenericBaseSubStateMachine<T, S, PS> : GenericBaseCommonStateMachine<T, S>, IBaseSubStateMachine<T, S, PS>
        where T : MonoBehaviour
        where S : class, IBaseSubState<T, PS>
    {
        void IBaseSubStateMachine<T, S, PS>.FixedUpdate(T obj, PS parent)
        {
            TransitState(obj, parent);

            curState?.FixedUpdate(obj, parent);
        }

        void IBaseSubStateMachine<T, S, PS>.Update(T obj, PS parent)
        {
            TransitState(obj, parent);

            curState?.Update(obj, parent);
        }

        void TransitState(T obj, PS parent)
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

        void IBaseSubStateMachine<T, S, PS>.CloseState(T obj, PS parent)
        {
            curState?.Exit(obj, parent, curId);
            requestId = -1;
            curId = -1;
            curState = null;
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
        where SM : IBaseStateMachine<T, S>
        where G : SM, new()
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

        public void TransitReady(int id, bool reset = false, int subId = -1) => stateMachine.TransitReady(id, reset, subId);

        protected virtual void StartFixedUpdate() { }
        protected virtual void EndtFixedUpdate() { }

        protected virtual void StartUpdate() { }
        protected virtual void EndtUpdate() { }
    }


    public class StateMachine<T>
        : BaseStateMachine<T, IBaseState<T>, IBaseStateMachine<T, IBaseState<T>>, GenericBaseStateMachine<T, IBaseState<T>>>
        where T : StateMachine<T>
    { }


    public class Idle : State<PlayerController, Idle>
    {
        protected override void Enter(PlayerController obj, int preId, int subId)
        {
            base.Enter(obj, preId, subId);

            AddSubState(0, new SubIdle());
        }
    }

    class SubIdle : SubState<PlayerController, Idle>
    {
        protected override void Enter(PlayerController obj, Idle parent, int preId, int subId)
        {
            base.Enter(obj, parent, preId, subId);
        }

        protected override void Update(PlayerController obj, Idle parent)
        {
            base.Update(obj, parent);
        }

        protected override void FixedUpdate(PlayerController obj, Idle parent)
        {
            base.FixedUpdate(obj, parent);
        }
    }
}