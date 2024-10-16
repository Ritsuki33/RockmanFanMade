using UnityEngine;

/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class DamageBase : MonoBehaviour, ITriggerVisitable
{
    [SerializeField] public int baseDamageValue = 3;

    public virtual void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    public virtual void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    public virtual void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    public virtual void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    public virtual void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    public virtual void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);

    // ここから定義



}


namespace RbStateMachine2
{
    public partial interface IStateTriggerVisitor<T>
         : IStateTriggerVisitor<T, DamageBase>
    { }

    public partial interface ISubStateTriggerVisitor<T, PS> : ISubStateTriggerVisitor<T, PS, DamageBase>
    {
    }

    public partial class BaseRbState<T, TS, SS, SM, G>
    {
        virtual protected void OnTriggerEnter(T obj, DamageBase collision) { }
        virtual protected void OnTriggerStay(T obj, DamageBase collision) { }
        virtual protected void OnTriggerExit(T obj, DamageBase collision) { }

        virtual protected void OnCollisionEnter(T obj, DamageBase collision) { }
        virtual protected void OnCollisionStay(T obj, DamageBase collision) { }
        virtual protected void OnCollisionExit(T obj, DamageBase collision) { }

        void IStateTriggerVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision)
        {
            OnTriggerEnter(obj, collision);
            subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
        }


        void IStateTriggerVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision)
        {
            OnTriggerStay(obj, collision);
            subStateMachine?.OnTriggerStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision)
        {
            OnTriggerExit(obj, collision);
            subStateMachine?.OnTriggerExit(obj, this as TS, collision);
        }


        void IStateTriggerVisitor<T, DamageBase>.OnCollisionEnter(T obj, DamageBase collision)
        {
            OnCollisionEnter(obj, collision);
            subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, DamageBase>.OnCollisionStay(T obj, DamageBase collision)
        {
            OnCollisionStay(obj, collision);
            subStateMachine?.OnCollisionStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, DamageBase>.OnCollisionExit(T obj, DamageBase collision)
        {
            OnCollisionExit(obj, collision);
            subStateMachine?.OnCollisionExit(obj, this as TS, collision);
        }

    }

    public partial class BaseRbSubState<T, TS, SS, SM, G, PS>
    {
        virtual protected void OnTriggerEnter(T obj,PS parent, DamageBase collision) { }
        virtual protected void OnTriggerStay(T obj, PS parent, DamageBase collision) { }
        virtual protected void OnTriggerExit(T obj, PS parent, DamageBase collision) { }

        virtual protected void OnCollisionEnter(T obj, PS parent, DamageBase collision) { }
        virtual protected void OnCollisionStay(T obj, PS parent, DamageBase collision) { }
        virtual protected void OnCollisionExit(T obj, PS parent, DamageBase collision) { }

        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerEnter(T obj, PS parent, DamageBase collision)
        {
            OnTriggerEnter(obj, parent, collision);
            subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
        }

        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerStay(T obj, PS parent, DamageBase collision)
        {
            OnTriggerStay(obj, parent, collision);
            subStateMachine?.OnTriggerStay(obj, this as TS, collision);
        }

        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerExit(T obj, PS parent, DamageBase collision)
        {
            OnTriggerExit(obj, parent, collision);
            subStateMachine?.OnTriggerExit(obj, this as TS, collision);
        }

        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionEnter(T obj, PS parent, DamageBase collision)
        {
            OnCollisionEnter(obj, parent, collision);
            subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
        }

        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionStay(T obj, PS parent, DamageBase collision)
        {
            OnCollisionStay(obj, parent, collision);
            subStateMachine?.OnCollisionStay(obj, this as TS, collision);
        }

        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionExit(T obj, PS parent, DamageBase collision)
        {
            OnCollisionExit(obj, parent, collision);
            subStateMachine?.OnCollisionExit(obj, this as TS, collision);
        }
    }

    public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
    {
        void IStateTriggerVisitor<T, DamageBase>.OnCollisionEnter(T obj, DamageBase collision) => curState.OnCollisionEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnCollisionExit(T obj, DamageBase collision) => curState.OnCollisionExit(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnCollisionStay(T obj, DamageBase collision) => curState.OnCollisionStay(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
    }

    public partial class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
    {
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
    }

    public partial class BaseRbStateMachine<T, S, SM, G>
    {
        void ITriggerVisitor<DamageBase>.OnTriggerEnter(DamageBase collision) => stateMachine.OnTriggerEnter((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnTriggerStay(DamageBase collision) => stateMachine.OnTriggerStay((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnTriggerExit(DamageBase collision) => stateMachine.OnTriggerExit((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnCollisionEnter(DamageBase collision) => stateMachine.OnCollisionEnter((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnCollisionStay(DamageBase collision) => stateMachine.OnCollisionStay((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnCollisionExit(DamageBase collision) => stateMachine.OnCollisionExit((T)this, collision);
    }
}

