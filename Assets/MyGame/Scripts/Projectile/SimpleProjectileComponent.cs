using System;
using UnityEngine;


/// <summary>
/// シンプルプロジェクタイルコンポーネント
/// </summary>
public class SimpleProjectileComponent : PhysicalObject, IDirect, IRbVisitable
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
    private SimpleProjectile<SimpleProjectileComponent> simpleProjectile;
    [SerializeField] private Direct direct;
    // [SerializeField] private Direct direct;
    // Action<Rigidbody2D> fixedUpdate;

    // Action<Projectile> onCollision;

    // int attackPower = 1;

    // public int AttackPower => attackPower;
    // public Vector2 CurVelocity => rb.velocity;
    // public float CurSpeed => rb.velocity.magnitude;

    protected override void Awake()
    {
        base.Awake();
        simpleProjectile = new SimpleProjectile<SimpleProjectileComponent>(this);
    }

    protected override void Init()
    {
        base.Init();
        simpleProjectile.Init(Delete);
    }

    protected override void Destroy()
    {
        base.Destroy();
        simpleProjectile.Destory(Delete);
    }

    protected override void OnFixedUpdate()
    {
        simpleProjectile.OnFixedUpdate();
    }


    public void Setup(Vector3 position, bool isRight, int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<SimpleProjectileComponent> onCollisionEnter = null)
    {
        simpleProjectile.Setup(position, attackPower, start, fixedUpdate, onCollisionEnter);
        direct.TurnTo(isRight);
    }

    public void ChangeBehavior(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        simpleProjectile.ChangeBehavior(attackPower, start, fixedUpdate);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        simpleProjectile.OnCollisionEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        simpleProjectile.OnTriggerEnter2D(collision);
    }

    public bool IsRight => direct.IsRight;

    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();
}

# region 編集禁止
public partial interface IRbVisitor
{
    void OnTriggerEnter(SimpleProjectileComponent collision) { }
    void OnTriggerStay(SimpleProjectileComponent collision) { }
    void OnTriggerExit(SimpleProjectileComponent collision) { }

    void OnCollisionEnter(SimpleProjectileComponent collision) { }
    void OnCollisionStay(SimpleProjectileComponent collision) { }
    void OnCollisionExit(SimpleProjectileComponent collision) { }
}

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, SimpleProjectileComponent> { }
public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, SimpleProjectileComponent> { }

public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, SimpleProjectileComponent collision) { }
    virtual protected void OnTriggerStay(T obj, SimpleProjectileComponent collision) { }
    virtual protected void OnTriggerExit(T obj, SimpleProjectileComponent collision) { }

    virtual protected void OnCollisionEnter(T obj, SimpleProjectileComponent collision) { }
    virtual protected void OnCollisionStay(T obj, SimpleProjectileComponent collision) { }
    virtual protected void OnCollisionExit(T obj, SimpleProjectileComponent collision) { }

    void IStateRbVisitor<T, SimpleProjectileComponent>.OnTriggerEnter(T obj, SimpleProjectileComponent collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, SimpleProjectileComponent>.OnTriggerStay(T obj, SimpleProjectileComponent collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, SimpleProjectileComponent>.OnTriggerExit(T obj, SimpleProjectileComponent collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, SimpleProjectileComponent>.OnCollisionEnter(T obj, SimpleProjectileComponent collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, SimpleProjectileComponent>.OnCollisionStay(T obj, SimpleProjectileComponent collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, SimpleProjectileComponent>.OnCollisionExit(T obj, SimpleProjectileComponent collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}

public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, SimpleProjectileComponent collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, SimpleProjectileComponent collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, SimpleProjectileComponent collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, SimpleProjectileComponent collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, SimpleProjectileComponent collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, SimpleProjectileComponent collision) { }

    void ISubStateRbVisitor<T, PS, SimpleProjectileComponent>.OnTriggerEnter(T obj, PS parent, SimpleProjectileComponent collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, SimpleProjectileComponent>.OnTriggerStay(T obj, PS parent, SimpleProjectileComponent collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, SimpleProjectileComponent>.OnTriggerExit(T obj, PS parent, SimpleProjectileComponent collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, SimpleProjectileComponent>.OnCollisionEnter(T obj, PS parent, SimpleProjectileComponent collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, SimpleProjectileComponent>.OnCollisionStay(T obj, PS parent, SimpleProjectileComponent collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, SimpleProjectileComponent>.OnCollisionExit(T obj, PS parent, SimpleProjectileComponent collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, SimpleProjectileComponent collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, SimpleProjectileComponent collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, SimpleProjectileComponent collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, SimpleProjectileComponent collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, SimpleProjectileComponent collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, SimpleProjectileComponent collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, SimpleProjectileComponent collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, SimpleProjectileComponent collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, SimpleProjectileComponent collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, SimpleProjectileComponent collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, SimpleProjectileComponent collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, SimpleProjectileComponent collision) => curState?.OnTriggerStay(obj, parent, collision);
}
#endregion
