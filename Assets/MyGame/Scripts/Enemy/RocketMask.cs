using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMask : EnemyObject
{
    [SerializeField] RoketMaskComtroller rocketMaskComtroller = default;

    public override void Init()
    {
        base.Init();
        rocketMaskComtroller.Init();
    }
}
