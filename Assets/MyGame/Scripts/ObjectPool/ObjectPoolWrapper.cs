using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

[Serializable]
public class ObjectPoolWrapper<E> where E : Enum
{
    string addPath;
    Transform _root;

    public ObjectPool<BaseObject> Pool { get; private set; }
    BaseObject asset = null;

    List<BaseObject> caches = new List<BaseObject>();

    IAssetLoad assetLoad = null;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(GenericPoolData<E> master, Transform root)
    {
        string path = default;

        // アセットの読み込み
        if (master.isAddressables)
        {
            path = $"{master.addPath}{master.type.ToString()}";
            //res = Resources.Load<BaseObject>(path);
            assetLoad = new AddressableLoad();
        }
        else
        {
            path = $"Prefabs/{master.addPath}{master.type.ToString()}";
            assetLoad = new ResoucesLoad();
        }

        asset = assetLoad.Load<BaseObject>(path);
        _root = root;
        if (asset == null)
        {
            Debug.LogError($"リソースのロードに失敗しました(path:{path})");
            return;
        }

        if (Pool != null)
        {
            Pool.Clear();
        }
        else
        {
            // オブジェクトプールを作成します
            Pool = new ObjectPool<BaseObject>
            (
                createFunc: OnCreateToPool,
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnRelaseToPool,
                actionOnDestroy: OnDestroyFromPool,
                collectionCheck: true,
                defaultCapacity: master.defaultCapacity,
                maxSize: master.maxSize
            );
        }
    }

    public void Destory()
    {
        // 削除だけなので逆ループでおｋ
        for (int i = caches.Count - 1; i >= 0; i--)
        {
            Release(caches[i]);
        }

        Pool.Clear();

        // アセットのリリース
        assetLoad.Release(asset);
    }

    /// <summary>
    /// オブジェクトの取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T OnGet<T>() where T : BaseObject
    {
        T obj = null;
        if (obj = Pool.Get() as T)
        {
            return obj;
        }
        else
        {
            Debug.LogError($"型キャスト({typeof(T).ToString()})ができないため取得てきません");
            return null;
        }
    }

    public void Release(BaseObject obj)
    {
        if (caches.Contains(obj)) Pool.Release(obj);
    }

    BaseObject OnCreateToPool()
    {
        BaseObject gameObject = GameObject.Instantiate(asset, _root);
        return gameObject;
    }

    void OnGetFromPool(BaseObject obj)
    {
        caches.Add(obj);
        obj.gameObject.SetActive(true);
    }

    void OnRelaseToPool(BaseObject obj)
    {
        caches.Remove(obj);
        obj.gameObject.SetActive(false);
    }

    void OnDestroyFromPool(BaseObject obj)
    {
        GameObject.Destroy(obj.gameObject);
    }

    interface IAssetLoad
    {
        T Load<T>(string str) where T : BaseObject;
        void Release(BaseObject asset);
    }

    class ResoucesLoad : IAssetLoad
    {
        public T Load<T>(string str) where T : BaseObject
        {
            return Resources.Load<T>(str);
        }

        public void Release(BaseObject asset)
        {
            // オブジェクトやコンポーネントに対しては使えない
            //Resources.UnloadAsset(asset);
            Resources.UnloadUnusedAssets();
        }
    }

    class AddressableLoad : IAssetLoad
    {
        public T Load<T>(string str) where T : BaseObject
        {
            return AddressableAssetLoadUtility.LoadPrefab<T>(str);
        }

        public void Release(BaseObject asset)
        {
            AddressableAssetLoadUtility.Release(asset);
        }
    }
}
