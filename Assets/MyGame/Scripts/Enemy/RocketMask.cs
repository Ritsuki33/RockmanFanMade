using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMask : StageEnemy
{
    [SerializeField] RoketMaskBehavior rocketMaskComtroller = default;

    protected override void Init()
    {
        base.Init();
        //rocketMaskComtroller.Init();
    }
}
