using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPooledObject<T> where T : MonoBehaviour
{
    public IObjectPool<T> Pool { get; set; }
    void OnGet() { }
    void OnRelease() { }
    void OnDispose() { }
}

[Serializable]
public class ObjectPoolWrapper<T> where T : MonoBehaviour, IPooledObject<T>
{
    [SerializeField] private T prefab;
    [SerializeField] private Transform root;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxSize = 10;
    public ObjectPool<T> Pool { get; private set; }
    private List<T> _cacheObjects = new List<T>();

    public void Init()
    {
        _cacheObjects.Clear();

        if (Pool != null)
        {
            Pool.Clear();
        }
        else
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
    }

    public void AllRelease()
    {
        foreach (var obj in _cacheObjects)
        {
            if (obj.gameObject.activeSelf) // アクティブ状態のオブジェクトをチェック
            {
                Pool.Release(obj); // プールに返却
            }
        }
    }

    public void Release(T obj)=> Pool.Release(obj);
    public void Clear() => Pool.Clear();

    T OnCreateToPool()
    {
        T gameObject = GameObject.Instantiate<T>(prefab, root);
        gameObject.Pool = Pool;

        _cacheObjects.Add(gameObject);
        return gameObject;
    }

    void OnGetFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.OnGet();
    }

    void OnRelaseToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.OnRelease();
    }

    void OnDestroyFromPool(T obj)
    {
        _cacheObjects.Remove(obj);
        obj.OnDispose();
    }
}


