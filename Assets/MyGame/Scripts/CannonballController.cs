using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballController : ReusableObject
{
    private Rigidbody2D rb;

    private float gravity = 1.0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float gravity)
    {
        this.gravity = gravity;
    }

    void FixedUpdate()
    {
        rb.velocity += Vector2.down * gravity;
    }
}