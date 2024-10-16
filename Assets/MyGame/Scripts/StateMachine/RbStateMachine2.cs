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


    public partial interface IStateTriggerVisitor<T>
         : IStateTriggerVisitor<T, PlayerTrigger>, IStateTriggerVisitor<T, RockBusterDamage>
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

    public partial interface ISubStateTriggerVisitor<T, PS>
       : ISubStateTriggerVisitor<T, PS, PlayerTrigger>, ISubStateTriggerVisitor<T, PS, RockBusterDamage>
    { }

    public interface IRbSubState<T, PS> : IBaseSubState<T, PS>, ISubStateTriggerVisitor<T, PS> where T : MonoBehaviour
    { }


    /// <summary>
    /// 状態ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseRbState<T, TS, SS, SM, G> : BaseState<T, TS, SS, SM, G>, IRbState<T>
        where T : MonoBehaviour
        where TS : class, IRbState<T>
        where SS : class, IRbSubState<T, TS>
        where SM : class, IRbSubStateMachine<T, SS, TS>
        where G : SM, new()
    {}


    /// <summary>
    /// 状態ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseRbSubState<T, TS, SS, SM, G, PS> : BaseSubState<T, TS, SS, SM, G, PS>, IRbSubState<T, PS>
        where T : MonoBehaviour
        where TS : class, IRbSubState<T, PS>
        where SS : class, IRbSubState<T, TS>
        where SM : class, IRbSubStateMachine<T, SS, TS>
        where G : SM, new()
        where PS : class, IBaseCommonState<T>
    { }

    /// <summary>
    /// 状態ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RbState<T, TS> :
        BaseRbState<T, TS, IRbSubState<T, TS>, IRbSubStateMachine<T, IRbSubState<T, TS>, TS>, GenericRbSubStateMachine<T, IRbSubState<T, TS>, TS>>
        where T : MonoBehaviour
        where TS : class, IRbState<T>
    { }

    public class RbSubState<T, TS, PS> :
    BaseRbSubState<T, TS, IRbSubState<T, TS>, IRbSubStateMachine<T, IRbSubState<T, TS>, TS>, GenericRbSubStateMachine<T, IRbSubState<T, TS>, TS>, PS>
        where T : MonoBehaviour
        where TS : class, IRbSubState<T, PS>
        where PS : class, IBaseCommonState<T>
    { }

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

    public partial class GenericRbStateMachine<T, S> : GenericBaseStateMachine<T, S>, IRbStateMachine<T, S> where T : MonoBehaviour where S : class, IRbState<T>
    { }

    public partial class GenericRbSubStateMachine<T, S, PS> : GenericBaseSubStateMachine<T, S, PS>, IRbSubStateMachine<T, S, PS> where T : MonoBehaviour where S : class, IRbSubState<T, PS>
    {}

    public partial class BaseRbStateMachine<T, S, SM, G>
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
    }

    /// <summary>
    /// ステートマシン
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RbStateMachine<T> :
        BaseRbStateMachine<T, IRbState<T>, IRbStateMachine<T, IRbState<T>>, GenericRbStateMachine<T, IRbState<T>>>
        where T : RbStateMachine<T>
    { }
}
