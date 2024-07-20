using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : ReusableObject<Explode>
{
    private Animator _animator;

    private ExplodePool ExplodePool => EffectManager.Instance.ExplodePool;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnFinishedAnimation()
    {
        ExplodePool.Pool.Release(this);
    }
}
