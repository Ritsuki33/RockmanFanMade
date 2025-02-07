using System;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class Tire : AnimObject, IRbVisitable, IExRbVisitable
{
    protected virtual void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

    protected virtual void AcceptOnHitEnter(IExRbVisitor visitor) => visitor.OnHitEnter(this);
    protected virtual void AcceptOnHitStay(IExRbVisitor visitor) => visitor.OnHitStay(this);
    protected virtual void AcceptOnHitExit(IExRbVisitor visitor) => visitor.OnHitExit(this);
    protected virtual void AcceptOnBottomHitEnter(IExRbVisitor visitor) => visitor.OnBottomHitEnter(this);
    protected virtual void AcceptOnBottomHitStay(IExRbVisitor visitor) => visitor.OnBottomHitStay(this);
    protected virtual void AcceptOnBottomHitExit(IExRbVisitor visitor) => visitor.OnBottomHitExit(this);
    protected virtual void AcceptOnTopHitEnter(IExRbVisitor visitor) => visitor.OnTopHitEnter(this);
    protected virtual void AcceptOnTopHitStay(IExRbVisitor visitor) => visitor.OnTopHitStay(this);
    protected virtual void AcceptOnTopHitExit(IExRbVisitor visitor) => visitor.OnTopHitExit(this);
    protected virtual void AcceptOnLeftHitEnter(IExRbVisitor visitor) => visitor.OnLeftHitEnter(this);
    protected virtual void AcceptOnLeftHitStay(IExRbVisitor visitor) => visitor.OnLeftHitStay(this);
    protected virtual void AcceptOnLeftHitExit(IExRbVisitor visitor) => visitor.OnLeftHitExit(this);
    protected virtual void AcceptOnRightHitEnter(IExRbVisitor visitor) => visitor.OnRightHitEnter(this);
    protected virtual void AcceptOnRightHitStay(IExRbVisitor visitor) => visitor.OnRightHitStay(this);
    protected virtual void AcceptOnRightHitExit(IExRbVisitor visitor) => visitor.OnRightHitExit(this);

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);

    void IExRbVisitable.AcceptOnHitEnter(IExRbVisitor visitor) => AcceptOnHitEnter(visitor);
    void IExRbVisitable.AcceptOnHitStay(IExRbVisitor visitor) => AcceptOnHitStay(visitor);
    void IExRbVisitable.AcceptOnHitExit(IExRbVisitor visitor) => AcceptOnHitExit(visitor);
    void IExRbVisitable.AcceptOnBottomHitEnter(IExRbVisitor visitor) => AcceptOnBottomHitEnter(visitor);
    void IExRbVisitable.AcceptOnBottomHitStay(IExRbVisitor visitor) => AcceptOnBottomHitStay(visitor);
    void IExRbVisitable.AcceptOnBottomHitExit(IExRbVisitor visitor) => AcceptOnBottomHitExit(visitor);
    void IExRbVisitable.AcceptOnTopHitEnter(IExRbVisitor visitor) => AcceptOnTopHitEnter(visitor);
    void IExRbVisitable.AcceptOnTopHitStay(IExRbVisitor visitor) => AcceptOnTopHitStay(visitor);
    void IExRbVisitable.AcceptOnTopHitExit(IExRbVisitor visitor) => AcceptOnTopHitExit(visitor);
    void IExRbVisitable.AcceptOnLeftHitEnter(IExRbVisitor visitor) => AcceptOnLeftHitEnter(visitor);
    void IExRbVisitable.AcceptOnLeftHitStay(IExRbVisitor visitor) => AcceptOnLeftHitStay(visitor);
    void IExRbVisitable.AcceptOnLeftHitExit(IExRbVisitor visitor) => AcceptOnLeftHitExit(visitor);
    void IExRbVisitable.AcceptOnRightHitEnter(IExRbVisitor visitor) => AcceptOnRightHitEnter(visitor);
    void IExRbVisitable.AcceptOnRightHitStay(IExRbVisitor visitor) => AcceptOnRightHitStay(visitor);
    void IExRbVisitable.AcceptOnRightHitExit(IExRbVisitor visitor) => AcceptOnRightHitExit(visitor);

    // ここから定義
    [SerializeField] float jumpPower = 60;
    public float JumpPower=> jumpPower;

    private StateMachine<Tire> stateMachine = new StateMachine<Tire>();

    protected override void Awake()
    {
        base.Awake();

        stateMachine.AddState(0, new Idle());
        stateMachine.AddState(1, new SteppedOn());
    }

    protected override void Init()
    {
        stateMachine.TransitReady(0);
    }

    protected override void OnUpdate()
    {
        stateMachine.Update(this);
    }

    class Idle : State<Tire, Idle>
    {
        protected override void Enter(Tire tire, int preId, int subId)
        {
            tire.MainAnimator.Play(AnimationNameHash.Idle);
        }
    }

    class SteppedOn : State<Tire, SteppedOn>
    {
        int animationHash;

        public SteppedOn()
        {
            animationHash = Animator.StringToHash("SteppedOn");
        }

        protected override void Enter(Tire tire, int preId, int subId)
        {
            tire.MainAnimator.Play(animationHash, 0, 0.0f);
        }

        protected override void Update(Tire tire)
        {
            if (!tire.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                tire.stateMachine.TransitReady(0);
            }
        }
    }

    public void OnSteppedOn()
    {
        stateMachine.TransitReady(1, true);
    }
}

#region 編集禁止
public partial interface IRbVisitor : IRbVisitor<Tire>
{ }

public partial interface IExRbVisitor : IExRbVisitor<Tire>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, Tire>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, Tire>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, Tire>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, Tire>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, Tire collision) { }
    virtual protected void OnTriggerStay(T obj, Tire collision) { }
    virtual protected void OnTriggerExit(T obj, Tire collision) { }

    virtual protected void OnCollisionEnter(T obj, Tire collision) { }
    virtual protected void OnCollisionStay(T obj, Tire collision) { }
    virtual protected void OnCollisionExit(T obj, Tire collision) { }

    void IStateRbVisitor<T, Tire>.OnTriggerEnter(T obj, Tire collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, Tire>.OnTriggerStay(T obj, Tire collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Tire>.OnTriggerExit(T obj, Tire collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, Tire>.OnCollisionEnter(T obj, Tire collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Tire>.OnCollisionStay(T obj, Tire collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, Tire>.OnCollisionExit(T obj, Tire collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
{
    virtual protected void OnHitEnter(T obj, Tire collision) { }
    virtual protected void OnBottomHitEnter(T obj, Tire collision) { }
    virtual protected void OnTopHitEnter(T obj, Tire collision) { }

    virtual protected void OnLeftHitEnter(T obj, Tire collision) { }
    virtual protected void OnRightHitEnter(T obj, Tire collision) { }
    virtual protected void OnHitStay(T obj, Tire collision) { }

    virtual protected void OnBottomHitStay(T obj, Tire collision) { }
    virtual protected void OnTopHitStay(T obj, Tire collision) { }
    virtual protected void OnLeftHitStay(T obj, Tire collision) { }

    virtual protected void OnRightHitStay(T obj, Tire collision) { }
    virtual protected void OnHitExit(T obj, Tire collision) { }
    virtual protected void OnBottomHitExit(T obj, Tire collision) { }

    virtual protected void OnTopHitExit(T obj, Tire collision) { }
    virtual protected void OnLeftHitExit(T obj, Tire collision) { }
    virtual protected void OnRightHitExit(T obj, Tire collision) { }

    void IStateExRbVisitor<T, Tire>.OnHitEnter(T obj, Tire hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, Tire>.OnBottomHitEnter(T obj, Tire hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnTopHitEnter(T obj, Tire hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnLeftHitEnter(T obj, Tire hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, Tire>.OnRightHitEnter(T obj, Tire hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnHitStay(T obj, Tire hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnBottomHitStay(T obj, Tire hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnTopHitStay(T obj, Tire hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnLeftHitStay(T obj, Tire hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnRightHitStay(T obj, Tire hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnHitExit(T obj, Tire hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnBottomHitExit(T obj, Tire hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnTopHitExit(T obj, Tire hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnLeftHitExit(T obj, Tire hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, Tire>.OnRightHitExit(T obj, Tire hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, Tire collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, Tire collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, Tire collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, Tire collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, Tire collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, Tire collision) { }

    void ISubStateRbVisitor<T, PS, Tire>.OnTriggerEnter(T obj, PS parent, Tire collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Tire>.OnTriggerStay(T obj, PS parent, Tire collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Tire>.OnTriggerExit(T obj, PS parent, Tire collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Tire>.OnCollisionEnter(T obj, PS parent, Tire collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Tire>.OnCollisionStay(T obj, PS parent, Tire collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, Tire>.OnCollisionExit(T obj, PS parent, Tire collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PS parent, Tire collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, Tire collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, Tire collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, Tire collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, Tire collision) { }
    virtual protected void OnHitStay(T obj, PS parent, Tire collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, Tire collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, Tire collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, Tire collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, Tire collision) { }
    virtual protected void OnHitExit(T obj, PS parent, Tire collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, Tire collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, Tire collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, Tire collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, Tire collision) { }

    void ISubStateExRbVisitor<T, PS, Tire>.OnHitEnter(T obj, PS parent, Tire hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnBottomHitEnter(T obj, PS parent, Tire hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnTopHitEnter(T obj, PS parent, Tire hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnLeftHitEnter(T obj, PS parent, Tire hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnRightHitEnter(T obj, PS parent, Tire hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnHitStay(T obj, PS parent, Tire hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnBottomHitStay(T obj, PS parent, Tire hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnTopHitStay(T obj, PS parent, Tire hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnLeftHitStay(T obj, PS parent, Tire hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnRightHitStay(T obj, PS parent, Tire hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnHitExit(T obj, PS parent, Tire hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnBottomHitExit(T obj, PS parent, Tire hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnTopHitExit(T obj, PS parent, Tire hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnLeftHitExit(T obj, PS parent, Tire hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, Tire>.OnRightHitExit(T obj, PS parent, Tire hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, Tire collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, Tire collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, Tire collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, Tire collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, Tire collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, Tire collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, Tire hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, Tire hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, Tire hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, Tire hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, Tire hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, Tire hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, Tire hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, Tire hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, Tire hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, Tire hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, Tire hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, Tire hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, Tire hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, Tire hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, Tire hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, Tire collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, Tire collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, Tire collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, Tire collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, Tire collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, Tire collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, Tire hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, Tire hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, Tire hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, Tire hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, Tire hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, Tire hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, Tire hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, Tire hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, Tire hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, Tire hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, Tire hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, Tire hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, Tire hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, Tire hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, Tire hit) => curState.OnRightHitExit(obj, parent, hit);
}
public partial interface IHitInterpreter : IHitInterpreter<Tire> { }

#endregion