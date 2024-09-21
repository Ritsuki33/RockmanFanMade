using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventControll : MonoBehaviour
{
    enum Event{
        None,
        PlayerMove,
        BossPause,
        BossHpBarSet,
        BattleStart,
        ExternalCall,
    }

    [Serializable]
    class Element
    {
        public Event _event;
        [SerializeReference]public BaseAction gameEvent;
    }


    [Serializable]
    abstract class BaseAction
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
    class PlayerMoveAction : BaseAction
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
    class BosePauseAction : BaseAction
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
    class BossHpBarSetAction : BaseAction
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
    class BattleStartAction : BaseAction
    {
        [SerializeField] public Element _next;

        override public Element Next => _next;
        override public void Execute()
        {
            prev.gameEvent.Unsubscrive();
            GameManager.Instance.Player.InputPermission();
        }
    }
    [Serializable]
    class ExternalCallAction : BaseAction
    {
        [SerializeField] UnityEvent action;
        [SerializeField] public Element _next;

        override public Element Next => _next;
        override public void Execute()
        {
            prev.gameEvent.Unsubscrive();
            action?.Invoke();
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
                if (element.gameEvent is not PlayerMoveAction)
                {
                    element.gameEvent = new PlayerMoveAction();
                }
                break;
            case Event.BossPause:
                if (element.gameEvent is not BosePauseAction)
                {
                    element.gameEvent = new BosePauseAction();
                }
                break;
            case Event.BossHpBarSet:
                if (element.gameEvent is not BossHpBarSetAction)
                {
                    element.gameEvent = new BossHpBarSetAction();
                }
                break;

            case Event.BattleStart:
                if (element.gameEvent is not BattleStartAction)
                {
                    element.gameEvent = new BattleStartAction();
                }
                break;
            case Event.ExternalCall:
                if (element.gameEvent is not ExternalCallAction)
                {
                    element.gameEvent = new ExternalCallAction();
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
