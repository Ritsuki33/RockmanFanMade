using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDog : EnemyObject
{
    [SerializeField] BigDogController ctr;

    public override void Init()
    {
        ctr.Init();
        base.Init();
    }
}
