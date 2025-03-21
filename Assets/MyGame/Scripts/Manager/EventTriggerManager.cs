﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EventType
{
    ChangeCameraStart,
    ChangeCameraEnd,
}

public enum EnemyEventType
{
    Defeated
}

public interface IEventTrigger<T, S>
{
    public void Subscribe(T eventType, S action);
    public void Unsubscribe(T eventType, S action);
}

public class EventTrigger<T,S>: IEventTrigger<T, S> where T : Enum where S : Delegate
{
    private Dictionary<T, S> eventTriggers = new Dictionary<T, S>();

    public void Init()
    {
        eventTriggers.Clear();
        foreach (T type in Enum.GetValues(typeof(T)))
        {
            eventTriggers.Add(type, null);
        }
    }

    public void Subscribe(T eventType, S action)
    {
        if (eventTriggers[eventType] != null)
        {
            eventTriggers[eventType] = (S)Delegate.Combine(eventTriggers[eventType], action);
        }
        else
        {
            eventTriggers[eventType] = action;
        }
    }

    public void Unsubscribe(T eventType, S action)
    {
        if (eventTriggers[eventType] != null)
        {
            eventTriggers[eventType] = (S)Delegate.Remove(eventTriggers[eventType], action);
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
    EventTrigger<EventType, Action> voidEventTriggers = new EventTrigger<EventType, Action>();
    EventTrigger<EnemyEventType, Action<StageEnemy>> enemyEventTriggers = new EventTrigger<EnemyEventType, Action<StageEnemy>>();

    public IEventTrigger<EventType, Action> VoidEventTriggers => voidEventTriggers;
    public IEventTrigger<EnemyEventType, Action<StageEnemy>> EenemyEventTriggers => enemyEventTriggers;

    public void Init()
    {
        voidEventTriggers.Init();
        enemyEventTriggers.Init();
    }

    public void Notify(EventType eventType) => voidEventTriggers[eventType]?.Invoke();
    public void Notify(EnemyEventType eventType, StageEnemy enemy) => enemyEventTriggers[eventType]?.Invoke(enemy);


}
