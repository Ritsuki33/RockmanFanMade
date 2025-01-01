using System;
using UnityEngine;

public class ProjectileReusable : Reusable
{
    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    Action<Rigidbody2D> fixedUpdate;
    Action deleteCallback;

    Action<ProjectileReusable> onCollision;

    int attackPower = 1;

    public int AttackPower => attackPower;
    public Vector2 CurVelocity => rb.velocity;
    public float CurSpeed => rb.velocity.magnitude;

    public Action<Rigidbody2D> FixedUpdateCallback=> fixedUpdate;
    public Action<ProjectileReusable> OnCollisionCallback => onCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Delete);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Delete);
    }

    public void Init(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        start?.Invoke(rb); 
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
    }

    public void Init(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<ProjectileReusable> onCollisionEnter = null, Action deleteCallback = null)
    {
        Init(attackPower, start, fixedUpdate);
        this.deleteCallback = deleteCallback;
        this.onCollision = onCollisionEnter;
    }

    protected override void OnGet()
    {
        if(boxCollider) boxCollider.enabled = true;
        if(boxTrigger) boxTrigger.enabled = true;
    }

    public void Delete()
    {
        Pool.Release(this);
        this.deleteCallback?.Invoke();
    }
}
