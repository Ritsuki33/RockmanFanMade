using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EffectManager : MonoBehaviour
{
    [SerializeField] ExplodePool explodePool = default;
    [SerializeField] ExplodePool explode2Pool = default;
    [SerializeField] ProjectilePool rockBusterPool = default;
    [SerializeField] ProjectilePool rockBusterMiddlePool = default;
    [SerializeField] ProjectilePool rockBusterBigPool = default;
    [SerializeField] ProjectilePool mettoruFirePool = default;
    [SerializeField] ProjectilePool bomPool = default;
    [SerializeField] ProjectilePool firePool = default;
    [SerializeField] PsPool deathEffectPool = default;
    [SerializeField] PlacedBombPool placedBombPool = default;
    [SerializeField] ProjectilePool crashBombPool = default;
    [SerializeField] LaserPool laserPool = default;

    public ExplodePool ExplodePool => explodePool;
    public ExplodePool Explode2Pool => explode2Pool;
    public ProjectilePool RockBusterPool => rockBusterPool;
    public ProjectilePool RockBusterMiddlePool => rockBusterMiddlePool;
    public ProjectilePool RockBusterBigPool => rockBusterBigPool;
    public ProjectilePool MettoruFirePool => mettoruFirePool;
    public ProjectilePool BomPool => bomPool;
    public ProjectilePool FirePool => firePool;
    public PsPool DeathEffectPool => deathEffectPool;
    public PlacedBombPool PlacedBombPool => placedBombPool;
    public ProjectilePool CrashBombPool => crashBombPool;
    public LaserPool LaserPool => laserPool;

    public void Init()
    {
        explodePool.Init();
        explode2Pool.Init();
        rockBusterPool.Init();
        rockBusterMiddlePool.Init();
        rockBusterBigPool.Init();
        mettoruFirePool.Init();
        bomPool.Init();
        firePool.Init();
        deathEffectPool.Init();
        placedBombPool.Init();
        crashBombPool.Init();
        laserPool.Init();
    }
}
