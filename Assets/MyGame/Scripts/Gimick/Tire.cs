using System;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class Tire : AnimObject, IExRbVisitable
{
    protected virtual void AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitEnter(this, hit);
    protected virtual void AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitStay(this, hit);
    protected virtual void AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnHitExit(this, hit);
    protected virtual void AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitEnter(this, hit);
    protected virtual void AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitStay(this, hit);
    protected virtual void AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnBottomHitExit(this, hit);
    protected virtual void AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitEnter(this, hit);
    protected virtual void AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitStay(this, hit);
    protected virtual void AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnTopHitExit(this, hit);
    protected virtual void AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitEnter(this, hit);
    protected virtual void AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitStay(this, hit);
    protected virtual void AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnLeftHitExit(this, hit);
    protected virtual void AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitEnter(this, hit);
    protected virtual void AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitStay(this, hit);
    protected virtual void AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit) => visitor.OnRightHitExit(this, hit);

    void IExRbVisitable.AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnBottomHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnTopHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnLeftHitExit(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitEnter(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitStay(visitor, hit);
    void IExRbVisitable.AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit) => AcceptOnRightHitExit(visitor, hit);

    // ここから定義
    [SerializeField] float jumpPower = 60;
    public float JumpPower => jumpPower;

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
public partial interface IExRbVisitor : IExRbVisitor<Tire>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, Tire>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, Tire>
{ }

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

#endregion