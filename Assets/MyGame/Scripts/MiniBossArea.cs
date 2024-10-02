using System;
using UnityEngine;

public class MiniBossArea : BaseArea
{
    [SerializeField] TransitCameraArea transitCameraArea;

    [SerializeField] EventController enemyDefeatedEvent;

    //public override void Enter()
    //{
    //    ClosedArea();
    //    EventTriggerManager.Instance.Subscribe(EventType.EnemyDefeated, OpenArea);
    //}

    //public override void Exit()
    //{
    //    EventTriggerManager.Instance.Unsubscribe(EventType.EnemyDefeated, OpenArea);
    //}

    //private void ClosedArea()
    //{
    //    transitCameraArea.gameObject.SetActive(false);
    //}

    private void OpenArea()
    {
        enemyDefeatedEvent.StartEvent();
    }
}
