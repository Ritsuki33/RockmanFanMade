using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMask : EnemyObject
{
    [SerializeField] RoketMaskBehavior rocketMaskComtroller = default;

    protected override void Init()
    {
        base.Init();
        //rocketMaskComtroller.Init();
    }
}
