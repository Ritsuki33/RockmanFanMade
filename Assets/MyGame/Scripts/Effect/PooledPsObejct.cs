using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledPsObejct : PsObject,IPooledObject<PooledPsObejct>
{
    public IObjectPool<PooledPsObejct> Pool { get; set; }
}
