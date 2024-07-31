using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaseObjectPool : MonoBehaviour
{
    [SerializeField] private ReusableObject prefab;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxSize = 10;
    public ObjectPool<ReusableObject> Pool { get; private set; }

    private void Awake()
    {
        // オブジェクトプールを作成します
        Pool = new ObjectPool<ReusableObject>
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

    ReusableObject OnCreateToPool()
    {
        var gameObject = Instantiate(prefab, this.transform);
        gameObject.Pool = Pool;

        return gameObject;
    }

    void OnGetFromPool(ReusableObject gameObject)
    {
        gameObject.gameObject.SetActive(true);
    }

    void OnRelaseToPool(ReusableObject gameObject)
    {
        gameObject.gameObject.SetActive(false);
    }
    void OnDestroyFromPool(ReusableObject gameObject)
    {
        Destroy(gameObject.gameObject);
    }

}
