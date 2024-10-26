using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ValueEventType
{
    ChangeCameraStart,
    ChangeCameraEnd,
    EnemyDefeated,
}

public class EventTrigger<T,S> where T : Enum where S : Delegate
{
    private Dictionary<T, S> eventTriggers = new Dictionary<T, S>();

    public void Init()
    {
        foreach (T type in Enum.GetValues(typeof(T)))
        {
            eventTriggers.Add(type, null);
        }
    }

    /// <summary>
    /// インデクサーの定義
    /// </summary>
    /// <param name="eventType"></param>
    /// <returns></returns>
    public S this[T eventType]
    {
        get
        {
            return eventTriggers[eventType];
        }
        set
        {
            eventTriggers[eventType] = value;
        }
    }
}

[DefaultExecutionOrder(-100)]
public class EventTriggerManager : SingletonComponent<EventTriggerManager>
{
    EventTrigger<ValueEventType, Action> valueEventTriggers = new EventTrigger<ValueEventType, Action>();

    protected override void Awake()
    {
        base.Awake();

        valueEventTriggers.Init();
    }
    
    public void Subscribe(ValueEventType eventType, Action action)=> valueEventTriggers[eventType] += action;

    public void Unsubscribe(ValueEventType eventType, Action action) => valueEventTriggers[eventType] -= action;

    public void Notify(ValueEventType eventType)=> valueEventTriggers[eventType]?.Invoke();
}
