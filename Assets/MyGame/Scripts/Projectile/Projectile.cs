using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Pool;



public class Projectile : StageDirectionalObject,IPooledObject<Projectile>
{
    [SerializeField] Rigidbody2D rb = default;

    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    Action<Rigidbody2D> fixedUpdate;
    Action<Projectile> deleteCallback;

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

        if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(gameObject))
        {
            Delete();
        }
    }

    public void Setup(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<Projectile> deleteCallback)
    {
        start?.Invoke(rb);
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
        this.deleteCallback = deleteCallback;
    }

    public void Setup(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<Projectile> deleteCallback, Action<Projectile> onCollisionEnter = null)
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

    public void Delete()
    {
        this.deleteCallback?.Invoke(this);
    }

    IObjectPool<Projectile> IPooledObject<Projectile>.Pool { get => pool; set => pool = value; }

    void IPooledObject<Projectile>.OnGet()
    {
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
    }
}
