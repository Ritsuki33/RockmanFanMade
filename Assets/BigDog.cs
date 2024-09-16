using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDog : EnemyObject
{
    [SerializeField] BigDogController ctr;


    private void OnEnable()
    {
        EventTriggerManager.Instance.Subscribe(EventType.EnemyDefeated, UnlockNextArea);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.Unsubscribe(EventType.EnemyDefeated, UnlockNextArea);
    }

    public override void Init()
    {
        ctr.Init();
        base.Init();
    }

    public void UnlockNextArea()
    {
        Debug.Log("UnlockNextArea");
    }
}
