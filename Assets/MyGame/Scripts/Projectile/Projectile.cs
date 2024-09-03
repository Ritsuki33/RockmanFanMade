using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : ReusableObject
{
    float speed = 5f;
    protected Vector2 direction = default;

    private BoxCollider2D boxCollider;

    public Vector2 Direction => direction;

    int attackPower = 1;

    public int AttackPower => attackPower;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Init(Vector2 direction, Vector2 position, float speed = 5, int attackPower = 1)
    {
        this.direction = direction;
        var localScale = this.transform.localScale;
        localScale.x = (direction.x > 0) ? 1 : -1;
        this.transform.localScale = localScale;
        this.transform.position = position;

        this.speed = speed;

        this.attackPower = attackPower;
    }

    private void FixedUpdate()
    {
        this.transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;

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

    public void DisableDamageDetection()
    {
        boxCollider.enabled = false;
    }

    public void ChangeDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}
