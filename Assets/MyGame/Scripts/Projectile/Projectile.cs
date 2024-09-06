using System;
using UnityEngine;

public class Projectile : ReusableObject
{
    float speed = 5f;
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

    public void Init(int attackPower, Action<Rigidbody2D> action)
    {
        this.attackPower = attackPower;
        moveAction = action;
    }

    public void Init(int attackPower, Action<Rigidbody2D> action,Action deleteCallback)
    {
        Init(attackPower, action);
        this.deleteCallback = deleteCallback;
    }

    private void FixedUpdate()
    {
        moveAction?.Invoke(this.rb);
        //this.rb.velocity += direction * speed;

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
