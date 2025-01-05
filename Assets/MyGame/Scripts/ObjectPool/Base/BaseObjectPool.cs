using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaseObjectPool : MonoBehaviour
{
    [SerializeField] private Reusable prefab;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxSize = 10;
    public ObjectPool<Reusable> Pool { get; private set; }

    private List<Reusable> _cacheObjects = new List<Reusable>();

    public void Init()
    {
        // オブジェクトプールを作成します
        Pool = new ObjectPool<Reusable>
        (
            createFunc: OnCreateToPool,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnRelaseToPool,
            actionOnDestroy: OnDestroyFromPool,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );

        foreach (var obj in _cacheObjects)
        {
            if (obj.gameObject.activeSelf) // アクティブ状態のオブジェクトをチェック
            {
                Pool.Release(obj); // プールに返却
            }
        }

        Pool.Clear();
    }

    Reusable OnCreateToPool()
    {
        var gameObject = Instantiate(prefab, this.transform);
        gameObject.Pool = Pool;

        _cacheObjects.Add(gameObject);
        return gameObject;
    }

    void OnGetFromPool(IResuable obj)
    {
        obj.OnGet();
    }

    void OnRelaseToPool(IResuable obj)
    {
        obj.OnRelease();
    }

    void OnDestroyFromPool(IResuable obj)
    {
        _cacheObjects.Remove(obj as Reusable);
        obj.OnDispose();
    }
}
