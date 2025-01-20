using System;
using UnityEngine;

public enum PoolType
{
    RockBuster,
    ChargeShotSmall,
    ChargeShot,
    Bom,
    CrashBomb,
    Fire,
    MettoruFire,
    PlacedBomb,
    Explode,
    Explode2,
    Laser,
    PlayerDeathEffect,
}

public class ObjectManager : SingletonComponent<ObjectManager>
{
    [SerializeField] ObjectPoolList<PoolType> objectPoolList;

    UpdateList updateList = new UpdateList();

    public void OnFixedUpdate()
    {
        updateList.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        updateList.OnUpdate();
    }

    public void OnPause(bool isPause)
    {
        updateList.OnPause(isPause);
    }

    public void AllDelete()
    {
        updateList.AllDelete();
    }

    public void Init()
    {
        objectPoolList.Init(this.transform);
    }

    /// <summary>
    /// プールからオブジェクトを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public T OnGet<T>(PoolType type, Action<T> deleteCallback=null) where T : BaseObject, IObjectInterpreter
    {
        T obj = objectPoolList.OnGet<T>(type);

        if (obj == null) return null;
        obj.onDeleteCallback = () =>
        {
            deleteCallback?.Invoke(obj);

            // オブジェクトの退会
            updateList.Remove(obj);

            // プールへ返還
            objectPoolList.OnRelease(type, obj);
        };

        // オブジェクトの登録
        updateList.Add(obj);
        return obj;
    }

    /// <summary>
    /// オブジェクトをロードして取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T OnLoad<T>(string path, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        T res = Resources.Load<T>(path);

        if (res == null)
        {
            Debug.LogError("リソースをロードできませんでした。");
            return null;
        }

        T obj = Instantiate(res, this.transform);

        obj.onDeleteCallback = () =>
        {
            deleteCallback?.Invoke(obj);

            // オブジェクトの退会
            updateList.Remove(obj);

            // そのまま破棄
            Destroy(obj);
        };

        // オブジェクトの登録
        updateList.Add(obj);
        return obj;
    }
}
