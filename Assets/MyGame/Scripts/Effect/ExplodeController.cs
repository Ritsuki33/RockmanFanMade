using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : ReusableObject
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!animator.IsPlayingCurrentAnimation())
        {
            Pool.Release(this);
        }
    }
}
