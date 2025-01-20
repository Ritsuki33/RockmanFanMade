using System;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class ObjectPoolWrapper2<E> where E : Enum
{
    string addPath;
    Transform _root;

    public ObjectPool<BaseObject> Pool { get; private set; }
    BaseObject res = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(GenericPoolData<E> master,Transform root)
    {
        // リソースの読み込み
        string path = $"Prefabs/{master.addPath}{master.type.ToString()}";
        res = Resources.Load<BaseObject>(path);

        _root = root;
        if (res == null)
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
            Debug.LogError("型キャストができないため取得てきません");
            return null;
        }
    }

    public void Release(BaseObject obj)
    {
        Pool.Release(obj);
    }

    BaseObject OnCreateToPool()
    {
        BaseObject gameObject = GameObject.Instantiate(res, _root);
        return gameObject;
    }

    void OnGetFromPool(BaseObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    void OnRelaseToPool(BaseObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    void OnDestroyFromPool(BaseObject obj)
    {
        GameObject.Destroy(obj.gameObject);
    }
}
