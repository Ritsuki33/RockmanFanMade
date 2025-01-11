using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EffectManager : MonoBehaviour
{
    [SerializeField] ObjectPoolWrapper<Explode> explodePool = default;
    [SerializeField] ObjectPoolWrapper<Explode> explode2Pool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> rockBusterPool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> rockBusterMiddlePool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> rockBusterBigPool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> mettoruFirePool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> bomPool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> firePool = default;
    [SerializeField] ObjectPoolWrapper<PooledPsObejct> deathEffectPool = default;
    [SerializeField] ObjectPoolWrapper<PlacedBomb> placedBombPool = default;
    [SerializeField] ObjectPoolWrapper<Projectile> crashBombPool = default;
    [SerializeField] ObjectPoolWrapper<Laser> laserPool = default;

    public ObjectPoolWrapper<Explode> ExplodePool => explodePool;
    public ObjectPoolWrapper<Explode> Explode2Pool => explode2Pool;
    public ObjectPoolWrapper<Projectile> RockBusterPool => rockBusterPool;
    public ObjectPoolWrapper<Projectile> RockBusterMiddlePool => rockBusterMiddlePool;
    public ObjectPoolWrapper<Projectile> RockBusterBigPool => rockBusterBigPool;
    public ObjectPoolWrapper<Projectile> MettoruFirePool => mettoruFirePool;
    public ObjectPoolWrapper<Projectile> BomPool => bomPool;
    public ObjectPoolWrapper<Projectile> FirePool => firePool;
    public ObjectPoolWrapper<PooledPsObejct> DeathEffectPool => deathEffectPool;
    public ObjectPoolWrapper<PlacedBomb> PlacedBombPool => placedBombPool;
    public ObjectPoolWrapper<Projectile> CrashBombPool => crashBombPool;
    public ObjectPoolWrapper<Laser> LaserPool => laserPool;

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
