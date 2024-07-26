using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonComponent<EffectManager>
{
    [SerializeField] ExplodePool explodePool = default;
    [SerializeField] RockBusterPool rockBusterPool = default;
    [SerializeField] ParticleSystem playerDeathEffect = default;

    public ExplodePool ExplodePool => explodePool;
    public RockBusterPool RockBusterPool => rockBusterPool;
    public ParticleSystem PlayerDeathEffect => playerDeathEffect;
}
