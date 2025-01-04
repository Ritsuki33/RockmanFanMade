using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDog : EnemyObject
{
    [SerializeField] BigDogBehavior ctr;

    public override void Init()
    {
        //ctr.Init();
        base.Init();
    }
}
