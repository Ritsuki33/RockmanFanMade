using System;
using UnityEngine;

/// <summary>RockBusterDamage
/// ロックバスターコンポーネント
/// </summary>
public class RockBuster : PlayerAttack, IDirect
{
    # region 編集禁止
    protected override void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected override void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected override void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected override void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected override void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected override void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);
    #endregion

    // ここから定義
    public enum BusterType
    {
        Mame,
        Middle,
        Big,
    }

    [SerializeField] private BusterType type = BusterType.Mame;

    [SerializeField] private SimpleProjectile<RockBuster> simpleProjectile = null;
    [SerializeField] private Direct direct = null;

    public BusterType Type => type;
    override public int AttackPower => simpleProjectile.AttackPower;

    protected override void Awake()
    {
        base.Awake();
        simpleProjectile = new SimpleProjectile<RockBuster>(this);
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
    protected override void OnFixedUpdate() => simpleProjectile.OnFixedUpdate();

    public void Setup(Vector3 position, bool isRight, int attackPower, float speed)
    {
        Vector2 direction = isRight ? Vector2.right : Vector2.left;
        simpleProjectile.Setup(position, attackPower, null, (rb) => rb.velocity = direction * speed);
        direct.TurnTo(isRight);
    }

    // public void Setup(Vector3 position, bool isRight, int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<RockBuster> onCollisionEnter = null)
    // {
    //     simpleProjectile.Setup(position, attackPower, start, fixedUpdate, onCollisionEnter);
    //     direct.TurnTo(isRight);
    // }

    public void ChangeBehavior(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate) => simpleProjectile.ChangeBehavior(attackPower, start, fixedUpdate);
    protected override void OnCollisionEnter2D(Collision2D collision) => simpleProjectile.OnCollisionEnter2D(collision);
    protected override void OnTriggerEnter2D(Collider2D collision) => simpleProjectile.OnTriggerEnter2D(collision);
    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();
    public bool IsRight => direct.IsRight;

    public Vector2 CurVelocity => simpleProjectile.CurVelocity;

    public float CurSpeed => simpleProjectile.CurSpeed;
}

# region 編集禁止
public partial interface IRbVisitor
{
    void OnTriggerEnter(RockBuster collision) { OnTriggerEnter(collision as PlayerAttack); }
    void OnTriggerStay(RockBuster collision) { OnTriggerStay(collision as PlayerAttack); }
    void OnTriggerExit(RockBuster collision) { OnTriggerExit(collision as PlayerAttack); }

    void OnCollisionEnter(RockBuster collision) { OnCollisionEnter(collision as PlayerAttack); }
    void OnCollisionStay(RockBuster collision) { OnCollisionStay(collision as PlayerAttack); }
    void OnCollisionExit(RockBuster collision) { OnCollisionExit(collision as PlayerAttack); }
}
public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, RockBuster>
{ }


public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, RockBuster>
{ }


public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, RockBuster collision) { }
    virtual protected void OnTriggerStay(T obj, RockBuster collision) { }
    virtual protected void OnTriggerExit(T obj, RockBuster collision) { }

    virtual protected void OnCollisionEnter(T obj, RockBuster collision) { }
    virtual protected void OnCollisionStay(T obj, RockBuster collision) { }
    virtual protected void OnCollisionExit(T obj, RockBuster collision) { }

    void IStateRbVisitor<T, RockBuster>.OnTriggerEnter(T obj, RockBuster collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, RockBuster>.OnTriggerStay(T obj, RockBuster collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, RockBuster>.OnTriggerExit(T obj, RockBuster collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, RockBuster>.OnCollisionEnter(T obj, RockBuster collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, RockBuster>.OnCollisionStay(T obj, RockBuster collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, RockBuster>.OnCollisionExit(T obj, RockBuster collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}



public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, RockBuster collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, RockBuster collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, RockBuster collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, RockBuster collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, RockBuster collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, RockBuster collision) { }

    void ISubStateRbVisitor<T, PS, RockBuster>.OnTriggerEnter(T obj, PS parent, RockBuster collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBuster>.OnTriggerStay(T obj, PS parent, RockBuster collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBuster>.OnTriggerExit(T obj, PS parent, RockBuster collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBuster>.OnCollisionEnter(T obj, PS parent, RockBuster collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBuster>.OnCollisionStay(T obj, PS parent, RockBuster collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, RockBuster>.OnCollisionExit(T obj, PS parent, RockBuster collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, RockBuster collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, RockBuster collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, RockBuster collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, RockBuster collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, RockBuster collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, RockBuster collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, RockBuster collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, RockBuster collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, RockBuster collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, RockBuster collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, RockBuster collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, RockBuster collision) => curState?.OnTriggerStay(obj, parent, collision);
}
#endregion
