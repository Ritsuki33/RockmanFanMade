using System;
using UnityEngine;

public enum ProjectileType
{
    RockBuster_Small,
    RockBuster_Middle,
    RockBuster_Big,
    MettoruFire,
    Fire,
    Bom,
    CrashBomb,
}

public class ObjectManager : SingletonComponent<ObjectManager>
{
    [SerializeField] EffectManager effectManager;

    public void Init()
    {
        effectManager.Init();
    }

    // Projectileの生成
    public void Create(ProjectileType type, Vector2 position, int attackPower, bool isRight, Action<Rigidbody2D> startCallback, Action<Rigidbody2D> fixedUpdateCallback, Action<Projectile> finishCallback = null)
    {
        // プール取得
        var pool = GetPool(type);
        if (pool == null)
        {
            Debug.Log($"{type}のPoolが取得できませんでした。");
            return;
        }

        // Projectileの初期化
        var projectile = pool.Pool.Get(); 
        Vector2 localScale = projectile.transform.localScale;
        localScale.x = (isRight) ? 1 : -1;
        projectile.transform.localScale = localScale;
        projectile.transform.position = new Vector3(position.x, position.y, -2);
        projectile.Setup(
            attackPower,
            startCallback,
            fixedUpdateCallback,
            (obj) =>
            {
                // 削除タイミング

                // プールへ返還
                pool?.Release(obj);
                // ワールドから削除
                WorldManager.Instance.RemoveObject(obj);

                finishCallback?.Invoke(obj);
            }
            );

        // ワールドへ追加
        WorldManager.Instance.AddObject(projectile);
    }

    /// <summary>
    /// 爆発オブジェクトの生成
    /// </summary>
    /// <param name="type"></param>
    /// <param name="layer"></param>
    /// <param name="damageVal"></param>
    /// <param name="position"></param>
    public void Create(ExplodeType type,Explode.Layer layer,int damageVal,Vector2 position)
    {
        // プール取得
        var pool = GetPool(type);

        var explode = pool.Pool.Get();

        explode.Setup(
            layer,
            damageVal,
            (obj) =>
            {
                // プールへ返還
                pool.Release(obj);

                // ワールドから削除
                WorldManager.Instance.RemoveObject(obj);
            }
        );
        explode.transform.position = position;

        // ワールドへ追加
        WorldManager.Instance.AddObject(explode);

    }

    /// <summary>
    /// PlacedBombの生成
    /// </summary>
    /// <param name="position"></param>
    /// <param name="targetPos"></param>
    /// <param name="finishCallback"></param>
    public void CreatePlacedBomb(Vector2 position, Vector2 targetPos, Action<ExpandRigidBody> orbitfixedUpdate, Action<PlacedBomb> finishCallback)
    {
        var pool = effectManager.PlacedBombPool;
        var bomb = pool.Pool.Get();
        bomb.transform.position = new Vector3(position.x, position.y, -2);
        Vector2 dir = targetPos - position;
        dir = dir.normalized;
        bomb.Setup(
           orbitfixedUpdate,
        (obj) =>
            {
                // プールへ返還
                pool.Release(obj);
                // ワールドから削除
                WorldManager.Instance.AddObject(obj);

                finishCallback.Invoke(obj);
            }
            );

        // ワールドへ追加
        WorldManager.Instance.AddObject(bomb);
    }

    /// <summary>
    /// デスエフェクトの生成
    /// </summary>
    /// <param name="position"></param>
    public void CreateDeathEffect(Vector2 position)
    {
        var pool = effectManager.DeathEffectPool;
        PooledPsObejct deathEffect = pool.Pool.Get();
        deathEffect.gameObject.transform.position = new Vector3(position.x, position.y, -3);

        deathEffect.Setup((obj) =>
        {
            pool.Release(deathEffect);
            WorldManager.Instance.RemoveObject(obj);
        });
        WorldManager.Instance.AddObject(deathEffect);
    }

    /// <summary>
    /// Projectileのプール取得
    /// </summary>
    /// <param name="rockBuster"></param>
    /// <returns></returns>
    private ObjectPoolWrapper<Projectile> GetPool(ProjectileType rockBuster)
    {
        switch (rockBuster)
        {
            case ProjectileType.RockBuster_Small:
                return effectManager.RockBusterPool;
            case ProjectileType.RockBuster_Middle:
                return effectManager.RockBusterMiddlePool;
            case ProjectileType.RockBuster_Big:
                return effectManager.RockBusterBigPool;
            case ProjectileType.MettoruFire:
                return effectManager.MettoruFirePool;
            case ProjectileType.Fire:
                return effectManager.FirePool;
            case ProjectileType.Bom:
                return effectManager.BomPool;
            case ProjectileType.CrashBomb:
                return effectManager.CrashBombPool;
            default: return null;
        }
    }

    private ObjectPoolWrapper<Explode> GetPool(ExplodeType type)
    {
        switch (type)
        {
            case ExplodeType.Explode1:
                return effectManager.ExplodePool;
            case ExplodeType.Explode2:
                return effectManager.Explode2Pool;
            default: return null;
        }
    }
}
