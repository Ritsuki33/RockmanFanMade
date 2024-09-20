using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossArea : BaseArea
{
    [SerializeField] TransitCameraArea transitCameraArea;

    private void OnEnable()
    {
        EventTriggerManager.Instance.Subscribe(EventType.EnterArea, ClosedArea);
        EventTriggerManager.Instance.Subscribe(EventType.EnemyDefeated, OpenArea);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.Unsubscribe(EventType.EnterArea, ClosedArea);
        EventTriggerManager.Instance.Unsubscribe(EventType.EnemyDefeated, OpenArea);
    }

    private void ClosedArea()
    {
        transitCameraArea.gameObject.SetActive(false);
    }

    private void OpenArea()
    {
        transitCameraArea.gameObject.SetActive(true);
    }
}
