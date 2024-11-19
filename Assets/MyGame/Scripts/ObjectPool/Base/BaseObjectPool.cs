using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BaseObjectPool : MonoBehaviour
{
    [SerializeField] private ReusableObject prefab;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxSize = 10;
    public ObjectPool<ReusableObject> Pool { get; private set; }

    private List<ReusableObject> _cacheObjects = new List<ReusableObject>();
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

    private void OnEnable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.StartStage, Init);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.StartStage, Init);
    }

    public void Init()
    {
        foreach(var obj in _cacheObjects)
        {
            if (obj.gameObject.activeSelf) // アクティブ状態のオブジェクトをチェック
            {
                Pool.Release(obj); // プールに返却
            }
        }

        Pool.Clear();
    }

    ReusableObject OnCreateToPool()
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
        _cacheObjects.Remove(obj as ReusableObject);
        obj.OnDispose();
    }
}
