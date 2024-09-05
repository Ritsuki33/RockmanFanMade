using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : ReusableObject
{
    float speed = 5f;
    protected Vector2 direction = default;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    public Vector2 Direction => direction;
    Action<Rigidbody2D> moveAction;

    int attackPower = 1;

    public int AttackPower => attackPower;
    public Vector2 CurVelocity => rb.velocity;
    public float CurSpeed => rb.velocity.magnitude;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    //public void Init(Vector2 direction, Vector2 position, float speed = 5, int attackPower = 1)
    //{
    //    this.direction = direction;
    //    var localScale = this.transform.localScale;
    //    localScale.x = (direction.x > 0) ? 1 : -1;
    //    this.transform.localScale = localScale;
    //    this.transform.position = position;

    //    this.speed = speed;

    //    this.attackPower = attackPower;
    //}

    public void Init(int attackPower, Action<Rigidbody2D> action)
    {
        this.attackPower = attackPower;
        moveAction = action;
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
    }
}
