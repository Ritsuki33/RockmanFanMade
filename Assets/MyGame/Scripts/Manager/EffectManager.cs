using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EffectManager : SingletonComponent<EffectManager>
{
    [SerializeField] BaseObjectPool explodePool = default;
    [SerializeField] BaseObjectPool explode2Pool = default;
    [SerializeField] BaseObjectPool rockBusterPool = default;
    [SerializeField] BaseObjectPool rockBusterMiddlePool = default;
    [SerializeField] BaseObjectPool rockBusterBigPool = default;
    [SerializeField] BaseObjectPool mettoruFirePool = default;
    [SerializeField] BaseObjectPool bomPool = default;
    [SerializeField] BaseObjectPool firePool = default;
    [SerializeField] ParticleSystem playerDeathEffect = default;
    [SerializeField] BaseObjectPool placedBombPool = default;
    [SerializeField] BaseObjectPool crashBombPool = default;

    public BaseObjectPool ExplodePool => explodePool;
    public BaseObjectPool Explode2Pool => explode2Pool;
    public BaseObjectPool RockBusterPool => rockBusterPool;
    public BaseObjectPool RockBusterMiddlePool => rockBusterMiddlePool;
    public BaseObjectPool RockBusterBigPool => rockBusterBigPool;
    public BaseObjectPool MettoruFirePool => mettoruFirePool;
    public BaseObjectPool BomPool => bomPool;
    public BaseObjectPool FirePool => firePool;
    public ParticleSystem PlayerDeathEffect => playerDeathEffect;
    public BaseObjectPool PlacedBombPool => placedBombPool;
    public BaseObjectPool CrashBombPool => crashBombPool;
}
