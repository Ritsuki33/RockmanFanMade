using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : BaseArea
{
    [SerializeField] BossController boss = default;
    [SerializeField] Transform bamili = default;
    [SerializeField] EventControll eventControll = default;
    private void OnEnable()
    {
        EventTriggerManager.Instance.Subscribe(EventType.EnterArea, eventControll.StartEvent);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.Unsubscribe(EventType.EnterArea, eventControll.StartEvent);
    }
}
