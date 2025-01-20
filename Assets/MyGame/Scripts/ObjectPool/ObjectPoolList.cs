using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolMaster<E> where E : Enum
{
    [SerializeField] public E type;
    [SerializeField] public string addPath;
    [SerializeField] public Transform root;
    [SerializeField] public int defaultCapacity = 10;
    [SerializeField] public int maxSize = 10;
}

[Serializable]
public class ObjectPoolList<E> where E : struct, Enum
{
    [SerializeField, Header("プール設定")] GenericPoolData<E>[] poolMaster;

    Dictionary<E, ObjectPoolWrapper2<E>> poolDic = new Dictionary<E, ObjectPoolWrapper2<E>>();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Transform _root)
    {
        foreach (GenericPoolData<E> master in poolMaster)
        {
            ObjectPoolWrapper2<E> pool = new ObjectPoolWrapper2<E>();

            pool.Init(master, _root);

            if (poolDic.ContainsKey(master.type))
            {
                Debug.LogWarning($"{master.type}は既に登録されています。");
            }

            poolDic.Add(master.type, pool);
        }
    }


    /// <summary>
    /// オブジェクトをプールから取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T OnGet<T>(E obj) where T : BaseObject
    {
        //E? type = ConvertClassNameToEnum(typeof(T).Name);

        //if (!type.HasValue)
        //{
        //    Debug.Log("クラス名に該当するPooledObjectTypeが存在しません。");
        //    return null;
        //}

        return this[obj].OnGet<T>();
    }

    public void OnRelease(E type, BaseObject obj)
    {
        this[type].Release(obj);
    }

    private static E? ConvertClassNameToEnum(string className)
    {
        if (Enum.TryParse(className, out E result))
        {
            return result; // 成功時はEnum値を返す
        }
        return null; // 失敗時はnullを返す
    }

    public ObjectPoolWrapper2<E> this[E type]
    {
        get
        {
            return poolDic[type];
        }
    }
}
