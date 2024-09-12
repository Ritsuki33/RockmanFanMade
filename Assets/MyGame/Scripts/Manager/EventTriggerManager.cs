using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EventType
{
    ChangeCameraStart,
    ChangeCameraEnd,
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
