using System;
using UnityEngine;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class ThunderBolt : PlayerAttack, IDirect, IRbVisitable
{
    #region 編集禁止
    protected override void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected override void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected override void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected override void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected override void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected override void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);
    #endregion

    // ここから定義

    [SerializeField] private SimpleProjectile<ThunderBolt> simpleProjectile;
    [SerializeField] private Direct direct;
    protected override void Awake()
    {
        base.Awake();
        simpleProjectile = new SimpleProjectile<ThunderBolt>(this);
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

    public void Setup(Vector3 position, bool isRight, int attackPower)
    {
        Vector2 direction = isRight ? Vector2.right : Vector2.left;
        float speed = 24;

        simpleProjectile.Setup(
            position,
            attackPower,
            null,
            (rb) => rb.velocity = direction * speed,
            (pjt) =>
            {
                pjt.Delete();

                var pjt1 = ObjectManager.OnGet<ThunderBolt>(PoolType.ThunderBoltMini);
                var pjt2 = ObjectManager.OnGet<ThunderBolt>(PoolType.ThunderBoltMini);

                // 上下に飛散
                pjt1.SetupMini(pjt.transform.position, isRight, 1, Vector2.up);
                pjt2.SetupMini(pjt.transform.position, isRight, 1, Vector2.down);
            }
            );
        direct.TurnTo(isRight);
    }

    public void SetupMini(Vector3 position, bool isRight, int attackPower, Vector2 direction)
    {
        float speed = 24;
        simpleProjectile.Setup(
             position,
             attackPower,
             null,
            (rb) => rb.velocity = direction * speed,
            null
             );

        direct.TurnTo(isRight);
    }

    // public void Setup(Vector3 position, bool isRight, int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<ThunderBolt> onCollisionEnter = null)
    // {
    //     direct.TurnTo(isRight);
    //     simpleProjectile.Setup(position, attackPower, start, fixedUpdate, onCollisionEnter);
    // }

    public void ChangeBehavior(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        simpleProjectile.ChangeBehavior(attackPower, start, fixedUpdate);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) => simpleProjectile.OnCollisionEnter2D(collision);
    protected override void OnTriggerEnter2D(Collider2D collision) => simpleProjectile.OnTriggerEnter2D(collision);

    public bool IsRight => direct.IsRight;
    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();

    ObjectManager ObjectManager => ObjectManager.Instance;

}

public partial interface IRbVisitor
{
    void OnTriggerEnter(ThunderBolt collision) { OnTriggerEnter(collision as PlayerAttack); }
    void OnTriggerStay(ThunderBolt collision) { OnTriggerStay(collision as PlayerAttack); }
    void OnTriggerExit(ThunderBolt collision) { OnTriggerExit(collision as PlayerAttack); }

    void OnCollisionEnter(ThunderBolt collision) { OnCollisionEnter(collision as PlayerAttack); }
    void OnCollisionStay(ThunderBolt collision) { OnCollisionStay(collision as PlayerAttack); }
    void OnCollisionExit(ThunderBolt collision) { OnCollisionExit(collision as PlayerAttack); }
}

# region 編集禁止

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, ThunderBolt> { }
public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, ThunderBolt> { }

public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, ThunderBolt collision) { }
    virtual protected void OnTriggerStay(T obj, ThunderBolt collision) { }
    virtual protected void OnTriggerExit(T obj, ThunderBolt collision) { }

    virtual protected void OnCollisionEnter(T obj, ThunderBolt collision) { }
    virtual protected void OnCollisionStay(T obj, ThunderBolt collision) { }
    virtual protected void OnCollisionExit(T obj, ThunderBolt collision) { }

    void IStateRbVisitor<T, ThunderBolt>.OnTriggerEnter(T obj, ThunderBolt collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, ThunderBolt>.OnTriggerStay(T obj, ThunderBolt collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, ThunderBolt>.OnTriggerExit(T obj, ThunderBolt collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, ThunderBolt>.OnCollisionEnter(T obj, ThunderBolt collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, ThunderBolt>.OnCollisionStay(T obj, ThunderBolt collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, ThunderBolt>.OnCollisionExit(T obj, ThunderBolt collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}

public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, ThunderBolt collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, ThunderBolt collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, ThunderBolt collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, ThunderBolt collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, ThunderBolt collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, ThunderBolt collision) { }

    void ISubStateRbVisitor<T, PS, ThunderBolt>.OnTriggerEnter(T obj, PS parent, ThunderBolt collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, ThunderBolt>.OnTriggerStay(T obj, PS parent, ThunderBolt collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, ThunderBolt>.OnTriggerExit(T obj, PS parent, ThunderBolt collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, ThunderBolt>.OnCollisionEnter(T obj, PS parent, ThunderBolt collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, ThunderBolt>.OnCollisionStay(T obj, PS parent, ThunderBolt collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, ThunderBolt>.OnCollisionExit(T obj, PS parent, ThunderBolt collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, ThunderBolt collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, ThunderBolt collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, ThunderBolt collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, ThunderBolt collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, ThunderBolt collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, ThunderBolt collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, ThunderBolt collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, ThunderBolt collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, ThunderBolt collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, ThunderBolt collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, ThunderBolt collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, ThunderBolt collision) => curState?.OnTriggerStay(obj, parent, collision);
}
#endregion
