using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventControll2 : MonoBehaviour
{
    Action eventActionCallback = null;
    [Serializable]
    abstract class AElement
    {
        abstract public EventAction EventAction { get; }
        abstract public void Execute(EventControll2 eventControll);
    }

    [Serializable]
    class Element : AElement
    {
        public Event _event;
        [SerializeField] public EventAction _eventAction;
        [SerializeReference] public AElement _next;         // Element型だとシリアライズ化が永久ループするため抽象クラスを準備

        public override EventAction EventAction => _eventAction;

        override public void Execute(EventControll2 eventControll)
        {
            if (_next != null)
            {
                eventControll.Subscribe(() => _next.Execute(eventControll));
            }
            _eventAction.Execute(eventControll);
        }
    }

    [Serializable]
    class EventAction
    {
        [SerializeField] UnityEvent<Action> action;

        public void Execute(EventControll2 eventControll)
        {
            eventControll.StartCoroutine(EventActionCo(eventControll));

            IEnumerator EventActionCo(EventControll2 eventControll)
            {
                int methodCount = action.GetPersistentEventCount(); // 全メソッド数
                int completeCount = 0; //完了した数

                action.Invoke(() =>
                {
                    completeCount++;
                });

                while (completeCount != methodCount) yield return null;

                eventControll.NotifyAction();
            }
        }
    }

    [SerializeField] Element _element;

    void StartEvent()
    {
        _element.Execute(this);
    }

    public void Subscribe(Action action)
    {
        eventActionCallback = action;
    }

    public void NotifyAction()
    {
        eventActionCallback?.Invoke();
    }
}
