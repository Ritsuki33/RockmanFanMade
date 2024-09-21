using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EventType
{
    ChangeCameraStart,
    ChangeCameraEnd,
    EnemyDefeated,
    EnemyPauseEnd,
    PlayerMoveEnd,
    HpBarSetEnd,
}

[DefaultExecutionOrder(-100)]
public class EventTriggerManager : SingletonComponent<EventTriggerManager>
{
    private Dictionary<EventType,Action> eventTriggers = new Dictionary<EventType, Action>();

    public Action ChangeCameraTrigger => this[EventType.ChangeCameraStart];

    protected override void Awake()
    {
        base.Awake();

        eventTriggers.Add(EventType.ChangeCameraStart, null);
        eventTriggers.Add(EventType.ChangeCameraEnd, null);
        eventTriggers.Add(EventType.EnemyDefeated, null);
        eventTriggers.Add(EventType.EnemyPauseEnd, null);
        eventTriggers.Add(EventType.PlayerMoveEnd, null);
        eventTriggers.Add(EventType.HpBarSetEnd, null);
    }
    
    public void Subscribe(EventType eventType, Action action)
    {
        eventTriggers[eventType] += action;
    }

    public void Unsubscribe(EventType eventType, Action action)
    {
        eventTriggers[eventType] -= action;
    }

    public void Notify(EventType eventType)
    {
        eventTriggers[eventType]?.Invoke();
    }

    /// <summary>
    /// インデクサーの定義
    /// </summary>
    /// <param name="eventType"></param>
    /// <returns></returns>
    public Action this[EventType eventType]
    {
        get
        {
            return eventTriggers[eventType];
        }
    }
}
