using System;
using UnityEngine;


/// <summary>
/// プレイヤー攻撃用ベースコンポーネント
/// </summary>
public class PlayerAttack : PhysicalObject, IRbVisitable
{
    #region 編集禁止
    protected virtual void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);
    #endregion

    // ここから定義

    public virtual int AttackPower { get; } = 1;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Delete();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Delete();
    }
}

# region 編集禁止
public partial interface IRbVisitor
{
    void OnTriggerEnter(PlayerAttack collision) { }
    void OnTriggerStay(PlayerAttack collision) { }
    void OnTriggerExit(PlayerAttack collision) { }

    void OnCollisionEnter(PlayerAttack collision) { }
    void OnCollisionStay(PlayerAttack collision) { }
    void OnCollisionExit(PlayerAttack collision) { }
}

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, PlayerAttack> { }
public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, PlayerAttack> { }

public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PlayerAttack collision) { }
    virtual protected void OnTriggerStay(T obj, PlayerAttack collision) { }
    virtual protected void OnTriggerExit(T obj, PlayerAttack collision) { }

    virtual protected void OnCollisionEnter(T obj, PlayerAttack collision) { }
    virtual protected void OnCollisionStay(T obj, PlayerAttack collision) { }
    virtual protected void OnCollisionExit(T obj, PlayerAttack collision) { }

    void IStateRbVisitor<T, PlayerAttack>.OnTriggerEnter(T obj, PlayerAttack collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, PlayerAttack>.OnTriggerStay(T obj, PlayerAttack collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, PlayerAttack>.OnTriggerExit(T obj, PlayerAttack collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, PlayerAttack>.OnCollisionEnter(T obj, PlayerAttack collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, PlayerAttack>.OnCollisionStay(T obj, PlayerAttack collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, PlayerAttack>.OnCollisionExit(T obj, PlayerAttack collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}

public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, PlayerAttack collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, PlayerAttack collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, PlayerAttack collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, PlayerAttack collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, PlayerAttack collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, PlayerAttack collision) { }

    void ISubStateRbVisitor<T, PS, PlayerAttack>.OnTriggerEnter(T obj, PS parent, PlayerAttack collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerAttack>.OnTriggerStay(T obj, PS parent, PlayerAttack collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerAttack>.OnTriggerExit(T obj, PS parent, PlayerAttack collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerAttack>.OnCollisionEnter(T obj, PS parent, PlayerAttack collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerAttack>.OnCollisionStay(T obj, PS parent, PlayerAttack collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, PlayerAttack>.OnCollisionExit(T obj, PS parent, PlayerAttack collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, PlayerAttack collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, PlayerAttack collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, PlayerAttack collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, PlayerAttack collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, PlayerAttack collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, PlayerAttack collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, PlayerAttack collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, PlayerAttack collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, PlayerAttack collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, PlayerAttack collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, PlayerAttack collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, PlayerAttack collision) => curState?.OnTriggerStay(obj, parent, collision);
}
#endregion
