using System;
using Unity.VisualScripting;
using UnityEngine;

public class EventControll : MonoBehaviour
{
    enum Event{
        None,
        PlayerMove,
        BossPause,
        BossHpBarSet,
        BattleStart,
    }

    [Serializable]
    class Element
    {
        public Event _event;
        [SerializeReference]public BaseEvent gameEvent;
    }


    [Serializable]
    abstract class BaseEvent
    {
        [NonSerialized] public Element prev;
        abstract public void Execute();
        abstract public Element Next { get; }

        public void SetSubscribeEvent(EventType type)
        {
            // 前のイベントは削除
            if (prev != null)
            {
                prev.gameEvent.Unsubscrive();
            }

            if (Next.gameEvent != null)
            {
                EventTriggerManager.Instance.Subscribe(type, Next.gameEvent.Execute);
            }
        }
        virtual public void Unsubscrive() { }
    }

    [Serializable]
    class PlayerMoveEvent : BaseEvent
    {
        [SerializeField] Transform _bamili;
        [SerializeField] public Element _next;

        override public Element Next => _next;


        override public void Execute()
        {
            SetSubscribeEvent(EventType.PlayerMoveEnd);
            GameManager.Instance.Player.AutoMoveTowards(_bamili);
        }

        override public void Unsubscrive()
        {
            EventTriggerManager.Instance.Unsubscribe(EventType.PlayerMoveEnd, Next.gameEvent.Execute);
        }
    }

    [Serializable]
    class BosePauseEvent : BaseEvent
    {
        [SerializeField] BossController ctr;
        [SerializeField] public Element _next;

        override public Element Next => _next;
        override public void Execute()
        {
            SetSubscribeEvent(EventType.EnemyPauseEnd);
            ctr.Appeare();
        }

        override public void Unsubscrive()
        {
            EventTriggerManager.Instance.Unsubscribe(EventType.EnemyPauseEnd, Next.gameEvent.Execute);
        }
    }

    [Serializable]
    class BossHpBarSetEvent : BaseEvent
    {
        [SerializeField] BossController ctr;
        [SerializeField] public Element _next;

        override public Element Next => _next;
        override public void Execute()
        {
            SetSubscribeEvent(EventType.HpBarSetEnd);

            UiManager.Instance.HpBar.gameObject.SetActive(true);
            UiManager.Instance.HpBar.SetParam(0.0f);
            UiManager.Instance.HpBar.ParamChangeAnimation(1.0f, () =>
            {
                EventTriggerManager.Instance.Notify(EventType.HpBarSetEnd);
            });
        }

        override public void Unsubscrive()
        {
            EventTriggerManager.Instance.Unsubscribe(EventType.HpBarSetEnd, Next.gameEvent.Execute);
        }
    }

    [Serializable]
    class BattleStartEvent : BaseEvent
    {
        [SerializeField] BossController ctr;
        [SerializeField] public Element _next;

        override public Element Next => _next;
        override public void Execute()
        {
            prev.gameEvent.Unsubscrive();
            GameManager.Instance.Player.InputPermission();
        }
    }

    [SerializeField] Element element;

    private void OnValidate()
    {
        Onvalidate(element);
    }

    public void StartEvent()
    {
        element.gameEvent.Execute();
    }

    void Onvalidate(Element element, Element prev=null)
    {
        if (element == null) return;

        switch (element._event)
        {
            case Event.None:
                element.gameEvent = null;
                break;
            case Event.PlayerMove:
                if (element.gameEvent is not PlayerMoveEvent)
                {
                    element.gameEvent = new PlayerMoveEvent();
                }
                break;
            case Event.BossPause:
                if (element.gameEvent is not BosePauseEvent)
                {
                    element.gameEvent = new BosePauseEvent();
                }
                break;
            case Event.BossHpBarSet:
                if (element.gameEvent is not BossHpBarSetEvent)
                {
                    element.gameEvent = new BossHpBarSetEvent();
                }
                break;

            case Event.BattleStart:
                if (element.gameEvent is not BattleStartEvent)
                {
                    element.gameEvent = new BattleStartEvent();
                }
                break;
        }

        if (element.gameEvent is not null)
        {
            element.gameEvent.prev = prev;
            Onvalidate(element.gameEvent.Next, element);
        }
    }
}
