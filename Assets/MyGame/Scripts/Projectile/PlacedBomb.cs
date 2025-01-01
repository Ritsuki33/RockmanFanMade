using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBomb : Reusable
{
    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    Action deleteCallback;


    protected override void OnGet()
    {
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
    }

    public void Delete()
    {
        Pool.Release(this);
        this.deleteCallback?.Invoke();
    }
}
