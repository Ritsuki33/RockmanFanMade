using System;
using UnityEngine;

public class Projectile : ReusableObject
{

    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    Action<Rigidbody2D> moveAction;
    Action deleteCallback;

    Action<Projectile, Collision2D> onCollision;
    Action<Projectile, Collider2D> onTrigger;
    int attackPower = 1;

    public int AttackPower => attackPower;
    public Vector2 CurVelocity => rb.velocity;
    public float CurSpeed => rb.velocity.magnitude;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EventTriggerManager.Instance.Subscribe(EventType.ChangeCameraStart, Delete);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.Unsubscribe(EventType.ChangeCameraStart, Delete);
    }

    public void Init(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        start?.Invoke(rb); 
        this.attackPower = attackPower;
        moveAction = fixedUpdate;
    }

    public void Init(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<Projectile, Collision2D> onCollisionEnter = null, Action<Projectile,Collider2D> onTriggerEnter =null, Action deleteCallback = null)
    {
        Init(attackPower, start, fixedUpdate);
        this.deleteCallback = deleteCallback;
        this.onCollision = onCollisionEnter;
        this.onTrigger = onTriggerEnter;
    }

    private void FixedUpdate()
    {
        moveAction?.Invoke(this.rb);

        if (GameManager.Instance.MainCameraControll.CheckOutOfView(this.gameObject))
        {
            Delete();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.onTrigger?.Invoke(this, collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.onCollision?.Invoke(this, collision);

    }
}
