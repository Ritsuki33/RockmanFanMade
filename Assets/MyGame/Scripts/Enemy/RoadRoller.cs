using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadRoller : EnemyObject
{
    [SerializeField] RoadRollerController controller;
    public override void Init()
    {
        base.Init();
        controller.Init();
    }
}
