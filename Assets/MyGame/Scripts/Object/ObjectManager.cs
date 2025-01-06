using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RockBuster
{
    Small,
    Middle,
    Big
}

public class ObjectManager : SingletonComponent<ObjectManager>
{
    [SerializeField] EffectManager effectManager;

    public void Init()
    {
        effectManager.Init();
    }

    /// <summary>
    /// ロックバスター
    /// </summary>
    /// <param name="rockBuster"></param>
    /// <param name="position"></param>
    /// <param name="attackPower"></param>
    /// <param name="isRight"></param>
    /// <param name="startCallback"></param>
    /// <param name="fixedUpdateCallback"></param>
    /// <param name="finishCallback"></param>
    public void CreateRockBuster(RockBuster rockBuster, Vector2 position, int attackPower, bool isRight, Action<Rigidbody2D> startCallback, Action<Rigidbody2D> fixedUpdateCallback, Action finishCallback = null)
    {
        ProjectilePool pool = null;
        switch (rockBuster)
        {
            case RockBuster.Small:
                pool = effectManager.RockBusterPool;
                break;
            case RockBuster.Middle:
                pool = effectManager.RockBusterMiddlePool;
                break;
            case RockBuster.Big:
                pool = effectManager.RockBusterBigPool;
                break;
        }
        Projectile projectile = pool.Pool.Get();
        Vector2 localScale = projectile.transform.localScale;
        localScale.x = (isRight) ? 1 : -1;
        projectile.transform.localScale = localScale;
        projectile.transform.position = new Vector3(position.x, position.y, -2);
        projectile.Setup(
            attackPower,
            startCallback,
            fixedUpdateCallback,
            () =>
            {
                pool.Pool.Release(projectile); WorldManager.Instance.RemoveObject(projectile);
                finishCallback?.Invoke();
            });

        WorldManager.Instance.AddObject(projectile);
    }
}
