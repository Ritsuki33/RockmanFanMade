using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDog : StageEnemy
{
    [SerializeField] BigDogBehavior ctr;

    protected override void Init()
    {
        //ctr.Init();
        base.Init();
    }
}
