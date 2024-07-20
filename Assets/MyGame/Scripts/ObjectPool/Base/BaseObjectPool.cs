using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaseObjectPool<T> : MonoBehaviour where T: ReusableObject<T>
{
    [SerializeField] private T prefab;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxSize = 10;
    public ObjectPool<T> Pool { get; private set; }

    private void Awake()
    {
        // オブジェクトプールを作成します
        Pool = new ObjectPool<T>
        (
            createFunc: OnCreateToPool,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnRelaseToPool,
            actionOnDestroy: OnDestroyFromPool,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    T OnCreateToPool()
    {
        var gameObject = Instantiate(prefab, this.transform);
        gameObject.Pool = Pool;

        return gameObject;
    }

    void OnGetFromPool(T gameObject)
    {
        gameObject.gameObject.SetActive(true);
    }

    void OnRelaseToPool(T gameObject)
    {
        gameObject.gameObject.SetActive(false);
    }
    void OnDestroyFromPool(T gameObject)
    {
        Destroy(gameObject.gameObject);
    }

}
