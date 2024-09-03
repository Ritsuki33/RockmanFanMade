using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMan : EnemyObject
{
    [SerializeField] GreenManController greenManController;
    public override void Init()
    {
        base.Init();
        greenManController.Init();
    }
}
