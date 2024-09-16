using System;
using UnityEngine;

public class Projectile : ReusableObject
{
    protected Vector2 direction = default;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    public Vector2 Direction => direction;
    Action<Rigidbody2D> moveAction;
    Action deleteCallback;
    int attackPower = 1;

    public int AttackPower => attackPower;
    public Vector2 CurVelocity => rb.velocity;
    public float CurSpeed => rb.velocity.magnitude;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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

    public void Init(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action deleteCallback = null)
    {
        Init(attackPower, start, fixedUpdate);
        this.deleteCallback = deleteCallback;
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
        boxCollider.enabled = true;
    }

    public void Delete()
    {
        Pool.Release(this);
        this.deleteCallback?.Invoke();
    }
}
