using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMan : StageEnemy
{
    [SerializeField] GreenManBehavior greenManController;
    protected override void Init()
    {
        base.Init();
        //greenManController.Init();
    }
}
