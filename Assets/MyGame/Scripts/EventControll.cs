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
    }

    [Serializable]
    abstract class AElement
    {
        abstract public BaseAction GameAction { get; }
        abstract public void Execute();
    }

    [Serializable]
    class Element: AElement
    {
        public Event _event;
        [NonSerialized] public Element _prev;
        [SerializeReference] public BaseAction _gameAction;
        [SerializeReference] public AElement _next;         // Element型だとシリアライズ化が永久ループするため抽象クラスを準備

        public override BaseAction GameAction => _gameAction;

        private EventType _type;

        override public void Execute()
        {
            _gameAction.Execute(this);
        }

        public void SetSubscribeEvent(EventType type)
        {
            // 前のイベントは削除
            if (_prev != null)
            {
                _prev.Unsubscrive();
            }

            if (_next!= null)
            {
                _type = type;
                EventTriggerManager.Instance.Subscribe(_type, _next.Execute);
            }
        }

        public void Unsubscrive()
        {
            EventTriggerManager.Instance.Unsubscribe(_type, _next.Execute);
        }
    }

    [Serializable]
    abstract class BaseAction
    {
        abstract public void Execute(Element element);
    }

    [Serializable]
    class PlayerMoveAction : BaseAction
    {
        [SerializeField] Transform _bamili;

        override public void Execute(Element element)
        {
            element.SetSubscribeEvent(EventType.PlayerMoveEnd);
            GameManager.Instance.Player.AutoMoveTowards(_bamili, () =>
            {
                EventTriggerManager.Instance.Notify(EventType.PlayerMoveEnd);
            }
            );
        }
    }

    [Serializable]
    class BosePauseAction : BaseAction
    {
        [SerializeField] BossController ctr;

        override public void Execute(Element element)
        {
            element.SetSubscribeEvent(EventType.EnemyPauseEnd);
            ctr.Appeare(() => { EventTriggerManager.Instance.Notify(EventType.EnemyPauseEnd); });
        }
    }

    [Serializable]
    class BossHpBarSetAction : BaseAction
    {
        [SerializeField] BossController ctr;

        override public void Execute(Element element)
        {
            element.SetSubscribeEvent(EventType.HpBarSetEnd);

            UiManager.Instance.HpBar.gameObject.SetActive(true);
            UiManager.Instance.HpBar.SetParam(0.0f);
            UiManager.Instance.HpBar.ParamChangeAnimation(1.0f, () =>
            {
                EventTriggerManager.Instance.Notify(EventType.HpBarSetEnd);
            });
        }
    }

    [Serializable]
    class BattleStartAction : BaseAction
    {
        override public void Execute(Element element)
        {
            GameManager.Instance.Player.InputPermission();
        }
    }

    //[Serializable]
    //class ExternalCallAction : BaseAction
    //{
    //    [SerializeField] UnityEvent<Action> action;

    //    override public void Execute(Element element)
    //    {
    //        int methodCount = action.GetPersistentEventCount();
    //        int completeCount = 0;
    //        action.Invoke(() =>
    //        {
    //            completeCount++;
    //        });
    //    }
    //}


    [SerializeField] Element element;

    private void OnValidate()
    {
        OnValidateElement(element);
    }

    public void StartEvent()
    {
        element.Execute();
    }

    void OnValidateElement(Element element, Element prev=null)
    {
        if (element == null) return;

        switch (element._event)
        {
            case Event.None:
                element._gameAction = null;
                element._next = null;
                break;
            case Event.PlayerMove:
                if (element._gameAction is not PlayerMoveAction)
                {
                    element._gameAction = new PlayerMoveAction();
                }
                break;
            case Event.BossPause:
                if (element._gameAction is not BosePauseAction)
                {
                    element._gameAction = new BosePauseAction();
                }
                break;
            case Event.BossHpBarSet:
                if (element._gameAction is not BossHpBarSetAction)
                {
                    element._gameAction = new BossHpBarSetAction();
                }
                break;

            case Event.BattleStart:
                if (element._gameAction is not BattleStartAction)
                {
                    element._gameAction = new BattleStartAction();
                }
                break;
        }

        if (element._gameAction is not null)
        {
            if (element._next is null) element._next = new Element();

            element._prev = prev;
            OnValidateElement(element._next as Element, element);
        }
    }
}
