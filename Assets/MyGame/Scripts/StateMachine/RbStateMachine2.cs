using UnityEngine;

namespace RbStateMachine2 
{
    public interface IStateTriggerVisitor<T, C> where C : ITriggerVisitable
    {
        void OnTriggerEnter(T obj, C collision);
        void OnTriggerStay(T obj, C collision);
        void OnTriggerExit(T obj, C collision);

        void OnCollisionEnter(T obj, C collision);
        void OnCollisionStay(T obj, C collision);
        void OnCollisionExit(T obj, C collision);
    }


    public interface IStateTriggerVisitor<T>
         : IStateTriggerVisitor<T, PlayerTrigger>, IStateTriggerVisitor<T, DamageBase>, IStateTriggerVisitor<T, RockBusterDamage>
    { }

    public interface IRbState<T> : IBaseState<T>, IStateTriggerVisitor<T> where T : MonoBehaviour
    { }

    public interface ISubStateTriggerVisitor<T, PS, C> where C : ITriggerVisitable
    {
        void OnTriggerEnter(T obj, PS parent, C collision);
        void OnTriggerStay(T obj, PS parent, C collision);
        void OnTriggerExit(T obj, PS parent, C collision);

        void OnCollisionEnter(T obj, PS parent, C collision);
        void OnCollisionStay(T obj, PS parent, C collision);
        void OnCollisionExit(T obj, PS parent, C collision);
    }

    public interface ISubStateTriggerVisitor<T, PS>
       : ISubStateTriggerVisitor<T, PS, PlayerTrigger>, ISubStateTriggerVisitor<T, PS, DamageBase>, ISubStateTriggerVisitor<T, PS, RockBusterDamage>
    { }

    public interface IRbSubState<T, PS> : IBaseSubState<T, PS>, ISubStateTriggerVisitor<T, PS> where T : MonoBehaviour
    { }


    /// <summary>
    /// 状態ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRbState<T, TS, SS, SM, G> : BaseState<T, TS, SS, SM, G>, IRbState<T>
        where T : MonoBehaviour
        where TS : class, IRbState<T>
        where SS : class, IRbSubState<T, TS>
        where SM : class, IRbSubStateMachine<T, SS, TS>
        where G : SM, new()
    {
        virtual protected void OnTriggerEnter(T obj, PlayerTrigger collision) { }
        virtual protected void OnTriggerStay(T obj, PlayerTrigger collision) { }
        virtual protected void OnTriggerExit(T obj, PlayerTrigger collision) { }
        virtual protected void OnTriggerEnter(T obj, DamageBase collision) { }
        virtual protected void OnTriggerStay(T obj, DamageBase collision) { }
        virtual protected void OnTriggerExit(T obj, DamageBase collision) { }
        virtual protected void OnTriggerEnter(T obj, RockBusterDamage collision) { }
        virtual protected void OnTriggerStay(T obj, RockBusterDamage collision) { }
        virtual protected void OnTriggerExit(T obj, RockBusterDamage collision) { }

        virtual protected void OnCollisionEnter(T obj, PlayerTrigger collision) { }
        virtual protected void OnCollisionStay(T obj, PlayerTrigger collision) { }
        virtual protected void OnCollisionExit(T obj, PlayerTrigger collision) { }
        virtual protected void OnCollisionEnter(T obj, DamageBase collision) { }
        virtual protected void OnCollisionStay(T obj, DamageBase collision) { }
        virtual protected void OnCollisionExit(T obj, DamageBase collision) { }
        virtual protected void OnCollisionEnter(T obj, RockBusterDamage collision) { }
        virtual protected void OnCollisionStay(T obj, RockBusterDamage collision) { }
        virtual protected void OnCollisionExit(T obj, RockBusterDamage collision) { }

        void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerEnter(T obj, PlayerTrigger collision)
        {
            OnTriggerEnter(obj, collision);
            subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision)
        {
            OnTriggerEnter(obj, collision);
            subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerEnter(T obj, RockBusterDamage collision)
        {
            OnTriggerEnter(obj, collision);
            subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerStay(T obj, PlayerTrigger collision)
        {
            OnTriggerStay(obj, collision);
            subStateMachine?.OnTriggerStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision)
        {
            OnTriggerStay(obj, collision);
            subStateMachine?.OnTriggerStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerStay(T obj, RockBusterDamage collision)
        {
            OnTriggerStay(obj, collision);
            subStateMachine?.OnTriggerStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerExit(T obj, PlayerTrigger collision)
        {
            OnTriggerExit(obj, collision);
            subStateMachine?.OnTriggerExit(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision)
        {
            OnTriggerExit(obj, collision);
            subStateMachine?.OnTriggerExit(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerExit(T obj, RockBusterDamage collision)
        {
            OnTriggerExit(obj, collision);
            subStateMachine?.OnTriggerExit(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionEnter(T obj, PlayerTrigger collision)
        {
            OnCollisionEnter(obj, collision);
            subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionStay(T obj, PlayerTrigger collision)
        {
            OnCollisionStay(obj, collision);
            subStateMachine?.OnCollisionStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionExit(T obj, PlayerTrigger collision)
        {
            OnCollisionExit(obj, collision);
            subStateMachine?.OnCollisionExit(obj, this as TS, collision);
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

        void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionEnter(T obj, RockBusterDamage collision)
        {
            OnCollisionEnter(obj, collision);
            subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionStay(T obj, RockBusterDamage collision)
        {
            OnCollisionStay(obj, collision);
            subStateMachine?.OnCollisionStay(obj, this as TS, collision);
        }

        void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionExit(T obj, RockBusterDamage collision)
        {
            OnCollisionExit(obj, collision);
            subStateMachine?.OnCollisionExit(obj, this as TS, collision);
        }
    }

    public interface IRbStateMachine<T, S>
        : IBaseStateMachine<T, S>, IStateTriggerVisitor<T>
        where T : MonoBehaviour where S : class, IRbState<T>
    {
    }

    public interface IRbSubStateMachine<T, S, PS>
        : IBaseSubStateMachine<T, S, PS>, ISubStateTriggerVisitor<T, PS>
        where T : MonoBehaviour
    {
    }

    public class GenericRbStateMachine<T, S,C> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
    {
        void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionEnter(T obj, PlayerTrigger collision) => curState.OnCollisionEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnCollisionEnter(T obj, DamageBase collision) => curState.OnCollisionEnter(obj, collision);
        void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionEnter(T obj, RockBusterDamage collision) => curState.OnCollisionEnter(obj, collision);
        void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionExit(T obj, PlayerTrigger collision) => curState.OnCollisionExit(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnCollisionExit(T obj, DamageBase collision) => curState.OnCollisionExit(obj, collision);
        void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionExit(T obj, RockBusterDamage collision) => curState.OnCollisionExit(obj, collision);
        void IStateTriggerVisitor<T, PlayerTrigger>.OnCollisionStay(T obj, PlayerTrigger collision) => curState.OnCollisionStay(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnCollisionStay(T obj, DamageBase collision) => curState.OnCollisionStay(obj, collision);
        void IStateTriggerVisitor<T, RockBusterDamage>.OnCollisionStay(T obj, RockBusterDamage collision) => curState.OnCollisionStay(obj, collision);
        void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerEnter(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnTriggerEnter(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerEnter(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerExit(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnTriggerExit(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerExit(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, PlayerTrigger>.OnTriggerStay(T obj, PlayerTrigger collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, DamageBase>.OnTriggerStay(T obj, DamageBase collision) => curState.OnTriggerEnter(obj, collision);
        void IStateTriggerVisitor<T, RockBusterDamage>.OnTriggerStay(T obj, RockBusterDamage collision) => curState.OnTriggerEnter(obj, collision);

    }

    public class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
    {
        void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionEnter(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionEnter(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionExit(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionExit(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnCollisionStay(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnCollisionStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnCollisionStay(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerEnter(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerEnter(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerEnter(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerExit(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerExit(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerExit(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, PlayerTrigger>.OnTriggerStay(T obj, PS parent, PlayerTrigger collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, DamageBase>.OnTriggerStay(T obj, PS parent, DamageBase collision) => curState?.OnTriggerEnter(obj, parent, collision);
        void ISubStateTriggerVisitor<T, PS, RockBusterDamage>.OnTriggerStay(T obj, PS parent, RockBusterDamage collision) => curState?.OnTriggerEnter(obj, parent, collision);
    }

    public class BaseRbStateMachine<T, S, SM, G>
    : BaseStateMachine<T, S, SM, G>,ITriggerVisitor
    where T : BaseRbStateMachine<T, S, SM, G>
    where S : class, IRbState<T>
    where SM : IRbStateMachine<T, S>
    where G : SM, new()
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var collide = collision.gameObject.GetComponent<ITriggerVisitable>();
            collide?.AcceptOnTriggerEnter(this);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            var collide = collision.gameObject.GetComponent<ITriggerVisitable>();
            collide?.AcceptOnTriggerStay(this);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var collide = collision.gameObject.GetComponent<ITriggerVisitable>();
            collide?.AcceptOnTriggerExit(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collide = collision.gameObject.GetComponent<ITriggerVisitable>();
            collide?.AcceptOnTriggerEnter(this);
        }

        void ITriggerVisitor<PlayerTrigger>.OnTriggerEnter(PlayerTrigger collision) => stateMachine.OnTriggerEnter((T)this, collision);
        void ITriggerVisitor<PlayerTrigger>.OnTriggerStay(PlayerTrigger collision) => stateMachine.OnTriggerStay((T)this, collision);
        void ITriggerVisitor<PlayerTrigger>.OnTriggerExit(PlayerTrigger collision) => stateMachine.OnTriggerExit((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnTriggerEnter(DamageBase collision) => stateMachine.OnTriggerEnter((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnTriggerStay(DamageBase collision) => stateMachine.OnTriggerStay((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnTriggerExit(DamageBase collision) => stateMachine.OnTriggerExit((T)this, collision);
        void ITriggerVisitor<RockBusterDamage>.OnTriggerEnter(RockBusterDamage collision) => stateMachine.OnTriggerEnter((T)this, collision);
        void ITriggerVisitor<RockBusterDamage>.OnTriggerStay(RockBusterDamage collision) => stateMachine.OnTriggerStay((T)this, collision);
        void ITriggerVisitor<RockBusterDamage>.OnTriggerExit(RockBusterDamage collision) => stateMachine.OnTriggerExit((T)this, collision);
        void ITriggerVisitor<PlayerTrigger>.OnCollisionEnter(PlayerTrigger collision) => stateMachine.OnCollisionEnter((T)this, collision);
        void ITriggerVisitor<PlayerTrigger>.OnCollisionStay(PlayerTrigger collision) => stateMachine.OnCollisionStay((T)this, collision);
        void ITriggerVisitor<PlayerTrigger>.OnCollisionExit(PlayerTrigger collision) => stateMachine.OnCollisionExit((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnCollisionEnter(DamageBase collision) => stateMachine.OnCollisionEnter((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnCollisionStay(DamageBase collision) => stateMachine.OnCollisionStay((T)this, collision);
        void ITriggerVisitor<DamageBase>.OnCollisionExit(DamageBase collision) => stateMachine.OnCollisionExit((T)this, collision);
        void ITriggerVisitor<RockBusterDamage>.OnCollisionEnter(RockBusterDamage collision) => stateMachine.OnCollisionEnter((T)this, collision);
        void ITriggerVisitor<RockBusterDamage>.OnCollisionStay(RockBusterDamage collision) => stateMachine.OnCollisionStay((T)this, collision);
        void ITriggerVisitor<RockBusterDamage>.OnCollisionExit(RockBusterDamage collision) => stateMachine.OnCollisionExit((T)this, collision);
    }
}
