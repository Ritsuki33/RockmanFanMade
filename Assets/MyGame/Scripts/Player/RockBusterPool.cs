using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RockBusterPool : MonoBehaviour
{
    [SerializeField] private RockBuster rockBusterPrefab;
    public ObjectPool<RockBuster> pool;

    private void Awake() { 
        // オブジェクトプールを作成します
        pool = new ObjectPool<RockBuster>
        (
            createFunc: CreateRockBuster,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnRelaseToPool,
            actionOnDestroy: OnDestroyFromPool,
            collectionCheck: true,
            defaultCapacity: 3,
            maxSize: 3
        );
    }

    RockBuster CreateRockBuster()
    {
        var rockBuster=Instantiate( rockBusterPrefab );
        rockBuster.Pool = pool;

        Debug.Log("CreateRockBuster");
        return rockBuster;
    }

    void OnGetFromPool(RockBuster rockBuster)
    {
        rockBuster.gameObject.SetActive(true);
    }

    void OnRelaseToPool(RockBuster rockBuster)
    {
        rockBuster.gameObject.SetActive(false);
    }
    void OnDestroyFromPool(RockBuster rockBuster)
    {
        Destroy(rockBuster.gameObject);
    }

}
