using System;
using System.Security.Cryptography;
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
    Batman,
    LiftYellow_LineMove,
    LiftYellow_CircleMove,
    LiftBlue,
    GreenMan,
    RoadRoller,
    RocketMask,
    Mettoru,
    LiftRed,
    Recovery,
    Recovery_Big,
    BlockBreakEffect,
    ExplodeParticleSystem,
}

public class ObjectManager : SingletonComponent<ObjectManager>
{
    [SerializeField] ObjectPoolList<PoolType> objectPoolList;

    UpdateList updateList = new UpdateList();

    public void OnFixedUpdate()
    {
        updateList.OnFixedUpdate();

        updateList.OnLateFixedUpdate();
    }

    public void OnUpdate()
    {
        updateList.OnUpdate();

        updateList.OnLateUpdate();
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

    public void Destroy()
    {
        updateList.AllDelete();
        objectPoolList.Destroy();
    }

    /// <summary>
    /// IDから現在アクティブなオブジェクトを取得する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetActiveObject<T>(int id) where T : BaseObject
    {
        for (int i = 0; i < updateList.Count; i++)
        {
            if (updateList[i].Id <= 0) continue;

            if (id == updateList[i].Id)
            {
                T obj = updateList[i] as T;
                if (obj == null) Debug.LogError($"id = {id}に一致するオブジェクトを{typeof(T).ToString()}に変換できませんでした。");
                return obj;
            }
        }

        Debug.LogError($"id = {id}に一致するオブジェクトは現在アクティブではありません");
        return null;
    }

    /// <summary>
    /// プールからオブジェクトを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <param name="deleteCallback"></param>
    /// <returns></returns>
    public T OnGet<T>(PoolType type, int id, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        if (id != 0 && CheckExitIdInActiveObject(id))
        {
            Debug.LogError("指定したIdが使われているため、取得できません。");
            return null;
        }

        T obj = objectPoolList.OnGet<T>(type);

        if (obj == null) return null;

        obj.Id = id;
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
    /// プールからオブジェクトを取得(idは0とする)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public T OnGet<T>(PoolType type, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        return OnGet<T>(type, 0, deleteCallback);
    }

    /// <summary>
    /// オブジェクトをロードして取得(idは0とする)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="id"></param>
    /// <param name="deleteCallback"></param>
    /// <returns></returns>

    /// <summary>
    /// オブジェクトをロードして取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="deleteCallback"></param>
    /// <returns></returns>
    public T OnAddressableLoad<T>(string path, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        return OnAddressableLoad<T>(path, 0, deleteCallback);
    }

    /// <summary>
    /// オブジェクトをロードして取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T OnResouceLoad<T>(string path, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        return OnResouceLoad<T>(path, 0, deleteCallback);
    }

    /// <summary>
    /// オブジェクトをロードして取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T OnAddressableLoad<T>(string path, int id, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        if (id != 0 && CheckExitIdInActiveObject(id))
        {
            Debug.LogError("指定したIdが使われているため、取得できません。");
            return null;
        }

        //T res = Resources.Load<T>(path);
        (T obj, int assetId) = AddressableAssetLoadUtility.LoadPrefab<T>(path);

        if (obj == null)
        {
            Debug.LogError($"リソースをロードできませんでした。(pathl:{path})");
            return null;
        }
        obj.transform.SetParent(this.transform);
        obj.Id = id;
        obj.onDeleteCallback = () =>
        {
            deleteCallback?.Invoke(obj);

            // オブジェクトの退会
            updateList.Remove(obj);

            // そのまま破棄
            Destroy(obj.gameObject);

            AddressableAssetLoadUtility.ReleasePrefab(assetId);
        };

        // オブジェクトの登録
        updateList.Add(obj);
        return obj;
        // return OnAddressableLoad<T>(path, 0, deleteCallback);
    }

    /// <summary>
    /// オブジェクトをResourcesからロードして取得(idは0とする)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="id"></param>
    /// <param name="deleteCallback"></param>
    /// <returns></returns>
    public T OnResouceLoad<T>(string path, int id, Action<T> deleteCallback = null) where T : BaseObject, IObjectInterpreter
    {
        if (id != 0 && CheckExitIdInActiveObject(id))
        {
            Debug.LogError("指定したIdが使われているため、取得できません。");
            return null;
        }

        //T res = Resources.Load<T>(path);
        T res = Resources.Load<T>(path);

        if (res == null)
        {
            Debug.LogError($"リソースをロードできませんでした。(path:{path})");
            return null;
        }

        T obj = Instantiate(res, this.transform);
        obj.Id = id;
        obj.onDeleteCallback = () =>
        {
            deleteCallback?.Invoke(obj);

            // オブジェクトの退会
            updateList.Remove(obj);

            // そのまま破棄
            Destroy(obj.gameObject);

            Resources.UnloadUnusedAssets();
        };

        // オブジェクトの登録
        updateList.Add(obj);
        return obj;
    }

    /// <summary>
    /// 現在アクティブのオブジェクト群にIDが存在するかスキャン
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool CheckExitIdInActiveObject(int id)
    {
        for (int i = 0; i < updateList.Count; i++)
        {
            if (updateList[i].Id <= 0) continue;

            if (id == updateList[i].Id)
            {
                return true;
            }
        }

        return false;
    }
}
