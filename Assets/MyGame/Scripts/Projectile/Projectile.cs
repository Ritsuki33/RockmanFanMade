using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : ReusableObject
{
    [SerializeField] protected float speedRatio = 5f;
    protected Vector2 direction = default;

    private BoxCollider2D boxCollider;

    public Vector2 Direction => direction;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Init(Vector2 direction,Vector2 position,float speed=-1)
    {
        this.direction = direction;
        this.transform.position = position;

        if (speed > 0) speedRatio = speed;
    }

    private void FixedUpdate()
    {
        this.transform.position += (Vector3)direction * speedRatio * Time.fixedDeltaTime;

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
