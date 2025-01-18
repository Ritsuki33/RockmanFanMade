using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] StageEnemy boss;

    private void OnEnable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.StartStage, Init);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.StartStage, Init);
    }

    public void Init()
    {
        //boss.Init();
        boss.gameObject.SetActive(false);
    }
}
