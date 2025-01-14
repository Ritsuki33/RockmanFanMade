using System;
using UnityEngine;
using UnityEngine.Pool;



public class Projectile : PhysicalObject,IPooledObject<Projectile>,IDirect
{
    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Direct direct;
    Action<Rigidbody2D> fixedUpdate;

    Action<Projectile> onCollision;

    int attackPower = 1;

    public int AttackPower => attackPower;
    public Vector2 CurVelocity => rb.velocity;
    public float CurSpeed => rb.velocity.magnitude;

    RbCollide rbCollide = new RbCollide();

    private IObjectPool<Projectile> pool = null;

    protected override void Init()
    {
        rbCollide.Init();

        rbCollide.onCollisionEnter += OnCollisionEnterBase;
        rbCollide.onTriggerEnter += OnTriggerEnterBase;
    }

    protected override void OnFixedUpdate()
    {
        fixedUpdate.Invoke(rb);
    }

    public void Setup(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        start?.Invoke(rb);
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
    }

    public void Setup(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action deleteCallback)
    {
        Setup(attackPower, start, fixedUpdate);
        Setup(deleteCallback);
    }

    public void Setup(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action deleteCallback, Action<Projectile> onCollisionEnter = null)
    {
        Setup(attackPower, start, fixedUpdate, deleteCallback);
        this.onCollision = onCollisionEnter;
    }

    void OnCollisionEnterBase(Collision2D collision)
    {
        onCollision?.Invoke(this);
    }

    void OnTriggerEnterBase(Collider2D collision)
    {
        onCollision?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rbCollide.OnCollisionEnter(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(collision);
    }

    IObjectPool<Projectile> IPooledObject<Projectile>.Pool { get => pool; set => pool = value; }


    void IPooledObject<Projectile>.OnGet()
    {
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
    }


    public bool IsRight => direct.IsRight;

    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();
}
