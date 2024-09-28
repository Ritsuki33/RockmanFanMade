using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public interface IBaseCommonState<T> where T : MonoBehaviour
    {
        void Enter(T obj, int preId, int subId);
        void Exit(T obj, int nextId);
    }

    public interface IBaseState<T>: IBaseCommonState<T> where T : MonoBehaviour
    {
        void FixedUpdate(T obj);
        void Update(T obj);
    }

    public interface IBaseSubState<T, PS> : IBaseCommonState<T>
        where T : MonoBehaviour
    {
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

        protected void CloseState(T obj)
        {
            subStateMachine.CloseState(obj);
        }

        void IBaseCommonState<T>.Enter(T obj, int preId, int subId) { Enter(obj, preId, subId); }

        void IBaseCommonState<T>.Exit(T obj, int nextId)
        {
            subStateMachine?.CloseState(obj);
            Exit(obj, nextId);
        }
        virtual protected void Enter(T obj, int preId, int subId) { }
        virtual protected IEnumerator EnterCoroutine(T obj, int preId) { yield return null; }


        virtual protected void Exit(T obj, int nextId) { }
        virtual protected IEnumerator ExitCoroutine(T obj, int nextId) { yield return null; }
    }

    /// <summary>
    /// 状態ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseState<T, S, SM, G, PS> : BaseCommonState<T, S, SM, G>, IBaseState<T>
        where T : MonoBehaviour
        where S :class, IBaseSubState<T, IBaseState<T>>
        where SM : class, IBaseSubStateMachine<T, S, PS>
        where G : SM, new()
        where PS : class, IBaseState<T>
    {
        virtual protected void FixedUpdate(T obj) { }
        virtual protected void Update(T obj) { }


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

    public class BaseSubState<T, S, SM, G, PS> : BaseCommonState<T, S, SM, G>, IBaseSubState<T,PS>
       where T : MonoBehaviour
       where S : class, IBaseSubState<T, PS>
       where SM : class, IBaseSubStateMachine<T, S, PS>
       where G : SM, new()
       where PS: class, IBaseCommonState<T>
    {
        virtual protected void FixedUpdate(T obj, PS parent) { }
        virtual protected void Update(T obj, PS parent) { }


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

    public class State<T> : 
        BaseState<T, IBaseSubState<T, IBaseState<T>>, IBaseSubStateMachine<T, IBaseSubState<T, IBaseState<T>>, IBaseState<T>>, GenericBaseSubStateMachine<T, IBaseSubState<T, IBaseState<T>>, IBaseState<T>>, IBaseState<T>> 
        where T : MonoBehaviour
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

        void CloseState(T obj);
    }

    public interface IBaseStateMachine<T, S>: IBaseCommonStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseState<T>
    {
        void FixedUpdate(T obj);
        void Update(T obj);
    }

    public interface IBaseSubStateMachine<T, S, PS> : IBaseCommonStateMachine<T, S>
        where T : MonoBehaviour 
    {
        void FixedUpdate(T obj, PS parent);
        void Update(T obj, PS parent);
    }

    public class GenericBaseCommonStateMachine<T, S> : IBaseCommonStateMachine<T, S> where T : MonoBehaviour where S : class, IBaseCommonState<T>
    {
        Dictionary<int, S> states = new Dictionary<int, S>();

        protected S curState = default;

        bool reset = false;

        int preId = -1;
        int curId = -1;

        int requestId = -1;
        int requestSubId = -1;

        int IBaseCommonStateMachine<T, S>.CurId => curId;

        protected void Init()
        {
            preId = -1;
            curId = -1;

            requestId = -1;
            reset = false;
            curState = null;
        }

        protected void TransitState(T obj)
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

        void IBaseCommonStateMachine<T, S>.CloseState(T obj)
        {
            curState?.Exit(obj, curId);
            requestId = -1;
            curId = -1;
            curState = null;
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
    }

    public class GenericBaseSubStateMachine<T, S, PS> : GenericBaseCommonStateMachine<T, S>, IBaseSubStateMachine<T, S, PS> 
        where T : MonoBehaviour 
        where S : class, IBaseSubState<T,PS>
    {
        void IBaseSubStateMachine<T, S, PS>.FixedUpdate(T obj,PS parent)
        {
            TransitState(obj);

            curState?.FixedUpdate(obj,parent);
        }

        void IBaseSubStateMachine<T, S, PS>.Update(T obj, PS parent)
        {
            TransitState(obj);

            curState?.Update(obj, parent);
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
}