using System;
using System.Collections.Generic;
using System.Linq;
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

public class ObjectManager : SingletonComponent<ObjectManager>, IRegister
{
    [SerializeField] EffectManager effectManager;
    UpdateList updateList = new UpdateList();
    List<Spawn> spawns = new List<Spawn>();
    [SerializeField] Transform enemyRoot;

    public void Init()
    {
        effectManager.Init(this);

        spawns = enemyRoot.GetComponentsInChildren<Spawn>().ToList();

        foreach (var e in spawns)
        {
            e.Init(this);
        }

        updateList.Clear();
    }

    public void OnFixedUpdate()
    {
        updateList.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        updateList.OnUpdate();

        foreach (var element in spawns)
        {
            element.OnUpdate();
        }
    }

    public void OnPause(bool isPause)
    {
        updateList.OnPause(isPause);
    }

    public void OnReset()
    {
        updateList.OnReset();
        updateList.Clear();

        foreach (var e in spawns)
        {
            e.OnReset();
        }
    }

    public void OnRegist(IObjectInterpreter obj)
    {
        updateList.Add(obj);
    }

    public void OnUnregist(IObjectInterpreter obj)
    {
        updateList.Remove(obj);
    }

    // Projectileの生成
    public void Create(ProjectileType type, Vector2 position, int attackPower, bool isRight, Action<Rigidbody2D> startCallback, Action<Rigidbody2D> fixedUpdateCallback, Action<Projectile> collisionCallback = null, Action<Projectile> finishCallback = null)
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
        projectile.TurnTo(isRight);
        projectile.transform.position = new Vector3(position.x, position.y, -2);
        projectile.Setup(
            attackPower,
            startCallback,
            fixedUpdateCallback,
            () =>
            {
                // プールへ返還
                pool?.Release(projectile);
                finishCallback?.Invoke(projectile);
            },
            collisionCallback
            );

        // ワールドへ追加
        OnRegist(projectile);
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
            }
        );
        explode.transform.position = position;
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
                finishCallback.Invoke(obj);
            }
            );
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
        });
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
