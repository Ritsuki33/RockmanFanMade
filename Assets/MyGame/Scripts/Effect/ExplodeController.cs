using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : ReusableObject
{
    Animator animator;

    Action<Collider2D> onTriggerEnter;
    BoxCollider2D boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Init(Action<Collider2D> onTriggerEnter)
    {
        if (onTriggerEnter != null) { 
        }
        this.onTriggerEnter = onTriggerEnter;
        boxCollider.enabled = onTriggerEnter != null;
    }

    private void Update()
    {
        if (!animator.IsPlayingCurrentAnimation())
        {
            Pool.Release(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter?.Invoke(collision);
    }
}
