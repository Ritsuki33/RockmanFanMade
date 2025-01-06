using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMan : EnemyObject
{
    [SerializeField] GreenManBehavior greenManController;
    protected override void Init()
    {
        base.Init();
        //greenManController.Init();
    }
}
