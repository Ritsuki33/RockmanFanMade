using System;
using UnityEngine;
using UnityEngine.Pool;



public class Projectile : PhysicalObject,IDirect
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


    private IObjectPool<Projectile> pool = null;

    protected override void Init()
    {
        base.Init();
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Delete);
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
    }

    protected override void Destroy()
    {
        base.Destroy();
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Delete);
    }

    protected override void OnFixedUpdate()
    {
        fixedUpdate.Invoke(rb);
    }


    public void Setup(Vector3 position, bool isRight, int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<Projectile> onCollisionEnter = null)
    {
        this.transform.position = position;
        TurnTo(isRight);
        start?.Invoke(rb);
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
        this.onCollision = onCollisionEnter;
    }

    public void ChangeBehavior(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        start?.Invoke(rb);
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollision?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollision?.Invoke(this);
    }

    public bool IsRight => direct.IsRight;

    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();
}
