using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    Action<EventController> actionFinishCallback = default;

    enum ActionType
    {
        PlayerMove,
        EnemyPause,
        BossHpBarSet,
        BattleStart,
    }

    [Serializable]
    abstract class AElement {
        abstract public void Execute(EventController eventControll);
        abstract public List<ActionElement> Actions { get; }
    }

    [Serializable]
    class Element : AElement
    {
        [SerializeField] public List<ActionElement> actions;
        [SerializeReference] public AElement _next;

        override public List<ActionElement> Actions => actions;

        override public void Execute(EventController eventControll)
        {
            eventControll.StartCoroutine(ActionExecuteCo(eventControll));

            IEnumerator ActionExecuteCo(EventController eventControll)
            {
                if (actions == null || actions.Count == 0) yield break;

                // 次イベント登録
                eventControll.actionFinishCallback = (_next != null && _next.Actions != null && _next.Actions.Count != 0) ? _next.Execute : null;

                int actionNum = actions.Count;
                int completed = 0;

                foreach (var a in actions)
                {
                    a.Execute(() =>
                    {
                        completed++;
                    });
                }

                while (completed != actionNum) yield return null;

                // イベント終了の通知
                eventControll.actionFinishCallback?.Invoke(eventControll);
            }
        }
    }

    [Serializable]
    class ActionElement
    {
        [SerializeField] public ActionType type;
        [SerializeReference] public BaseAction action;

        public void Execute(Action finishCallback) => action.Execute(finishCallback);
    }

    [Serializable]
    abstract class BaseAction
    {
        abstract public void Execute(Action finishCallback);
    }

    [Serializable]
    class PlayerMoveAction : BaseAction
    {
        [SerializeField] Transform _bamili;

        override public void Execute(Action finishCallback)
        {
            GameManager.Instance.Player.AutoMoveTowards(_bamili, finishCallback);
        }
    }

    [Serializable]
    class BosePauseAction : BaseAction
    {
        [SerializeField] BossController ctr;

        override public void Execute(Action finishCallback)
        {
            ctr.Appeare(finishCallback);
        }
    }

    [Serializable]
    class BossHpBarSetAction : BaseAction
    {
        [SerializeField] BossController ctr;

        override public void Execute(Action finishCallback)
        {
            UiManager.Instance.HpBar.gameObject.SetActive(true);
            UiManager.Instance.HpBar.SetParam(0.0f);
            UiManager.Instance.HpBar.ParamChangeAnimation(1.0f, finishCallback);
        }
    }

    [Serializable]
    class BattleStartAction : BaseAction
    {
        override public void Execute(Action finishCallback)
        {
            GameManager.Instance.Player.InputPermission();
            finishCallback.Invoke();
        }
    }


    [SerializeField] Element element;

    private void OnValidate()
    {
        OnvalidateElement(element);
    }

    void OnvalidateElement(Element element)
    {
        if (element.actions == null) return;

        foreach (var ae in element.actions)
        {
            switch (ae.type)
            {
                case ActionType.PlayerMove:
                    if (ae.action is not PlayerMoveAction)
                    {
                        ae.action = new PlayerMoveAction();
                    }
                    break;
                case ActionType.EnemyPause:
                    if (ae.action is not BosePauseAction)
                    {
                        ae.action = new BosePauseAction();
                    }
                    break;
                case ActionType.BossHpBarSet:
                    if (ae.action is not BossHpBarSetAction)
                    {
                        ae.action = new BossHpBarSetAction();
                    }
                    break;
                case ActionType.BattleStart:
                    if (ae.action is not BattleStartAction)
                    {
                        ae.action = new BattleStartAction();
                    }
                    break;
                default:
                    break;
            }
        }

        if (element.actions.Count > 0)
        {
            if (element._next is null) element._next = new Element();
            OnvalidateElement(element._next as Element);
        }
        else
        {
            element._next = null;
        }
    }

    public void StartEvent()
    {
        element.Execute(this);
    }
}