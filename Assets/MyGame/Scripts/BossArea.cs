using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : BaseArea
{
    [SerializeField] BossController boss = default;
    [SerializeField] Transform bamili = default;
    [SerializeField] EventControll eventControll = default;

    public override void Enter()
    {
        eventControll.StartEvent();
    }
}
