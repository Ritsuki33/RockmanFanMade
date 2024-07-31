using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonComponent<EffectManager>
{
    [SerializeField] BaseObjectPool explodePool = default;
    [SerializeField] BaseObjectPool explode2Pool = default;
    [SerializeField] BaseObjectPool rockBusterPool = default;
    [SerializeField] ParticleSystem playerDeathEffect = default;

    public BaseObjectPool ExplodePool => explodePool;
    public BaseObjectPool Explode2Pool => explode2Pool;
    public BaseObjectPool RockBusterPool => rockBusterPool;
    public ParticleSystem PlayerDeathEffect => playerDeathEffect;
}
