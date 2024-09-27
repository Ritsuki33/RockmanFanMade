using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    Action<EventController> actionFinishCallback = default;

    Action eventFinishCallback = default;
    enum ActionType
    {
        ObjectActive,
        ObjectMove,
        StageObjectTurn,
        FadeInScreen,
        FadeOutScreen,
        StartSign,
        PlayerTransfer,
        PlayerMove,
        EnemyPause,
        BossHpBarSet,
        BattleStart,
        External,
    }

    [Serializable]
    abstract class AElement {
        abstract public void Execute(EventController eventControll);
        abstract public List<ActionElement> Actions { get; }
    }

    [Serializable]
    class Element : AElement
    {
        [SerializeField] float delayTime = 0.0f;
        [SerializeField] public List<ActionElement> actions;
        [SerializeReference] public AElement _next;

        override public List<ActionElement> Actions => actions;

        override public void Execute(EventController eventControll)
        {
            eventControll.StartCoroutine(ActionExecuteCo(eventControll));

            IEnumerator ActionExecuteCo(EventController eventControll)
            {
                if (actions == null || actions.Count == 0) yield break;

                if (delayTime > 0) yield return new WaitForSeconds(delayTime);

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

                // アクション終了の通知
                if (eventControll.actionFinishCallback != null) eventControll.actionFinishCallback.Invoke(eventControll);
                else
                {
                    // イベント終了の通知
                    eventControll.eventFinishCallback?.Invoke();
                    eventControll.eventFinishCallback = null;
                }
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
    class ObjectActive : BaseAction
    {

        [SerializeField] GameObject obj;
        [SerializeField] bool isActive = false;
        public override void Execute(Action finishCallback)
        {
            obj.SetActive(isActive);
            finishCallback();
        }
    }

    [Serializable]
    class ObjectMove : BaseAction
    {
        [SerializeField] Transform transform;
        [SerializeField] Transform targetPosition;

        public override void Execute(Action finishCallback)
        {
            transform.position = targetPosition.position;
            finishCallback.Invoke();
        }
    }

    [Serializable]
    class StageObjectTurn:BaseAction
    {
        [SerializeField] StageObject obj;
        [SerializeField] bool isRight = true;
        public override void Execute(Action finishCallback)
        {
            obj.TurnTo(isRight);
        }
    }

    [SerializeField]
    class FadeInScreen : BaseAction
    {
        [SerializeField] float fadeTime = 0.4f;
        [SerializeField] Color color = Color.black;
        public override void Execute(Action finishCallback)
        {
            UiManager.Instance.FadeInManager.FadeIn(fadeTime, color, finishCallback);
        }
    }

    [SerializeField]
    class FadeOutScreen : BaseAction
    {
        [SerializeField] float fadeTime = 0.4f;
        [SerializeField] Color color = Color.black;
        public override void Execute(Action finishCallback)
        {
            UiManager.Instance.FadeInManager.FadeOut(fadeTime, color,finishCallback);
        }
    }

    [SerializeField]
    class StartSign : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            UiManager.Instance.ReadyUi.Play(finishCallback);
        }
    }

    [SerializeField]
    class PlayerTransfer : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            GameManager.Instance.Player.TransferPlayer(finishCallback);
        }
    }

    [Serializable]
    class PlayerWalkAction : BaseAction
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
        [SerializeField] BossController ctr;
        override public void Execute(Action finishCallback)
        {
            ctr.ToBattleState();
            GameManager.Instance.Player.InputPermission();
            finishCallback.Invoke();
        }
    }

    [Serializable]
    class ExternalAction:BaseAction
    {
        [SerializeField] UnityEvent<Action> action;

        private EventController eventController;

        public ExternalAction(EventController eventController)
        {
            this.eventController = eventController;
        }

        public override void Execute(Action finishCallback)
        {
            eventController.StartCoroutine(ExecuteCo(finishCallback));
            IEnumerator ExecuteCo(Action finishCallback)
            {
                int methodNum = action.GetPersistentEventCount();
                int completed = 0;

                action.Invoke(() =>{
                    completed++;
                });

                while (completed != methodNum) yield return null;
                finishCallback.Invoke();
            }
        }
    }
    [SerializeField] Element element;

    private void OnValidate()
    {
        OnvalidateElement(element);
    }

    void OnvalidateElement(Element element)
    {
        if (element == null || element.actions == null) return;

        foreach (var ae in element.actions)
        {
            switch (ae.type)
            {
                case ActionType.ObjectActive:
                    if (ae.action is not ObjectActive)
                    {
                        ae.action = new ObjectActive();
                    }
                    break;
                case ActionType.ObjectMove:
                    if (ae.action is not ObjectMove)
                    {
                        ae.action = new ObjectMove();
                    }
                    break;
                case ActionType.StageObjectTurn:
                    if (ae.action is not StageObjectTurn)
                    {
                        ae.action = new StageObjectTurn();
                    }
                    break;
                case ActionType.FadeInScreen:
                    if (ae.action is not FadeInScreen)
                    {
                        ae.action = new FadeInScreen();
                    }
                    break;
                case ActionType.FadeOutScreen:
                    if (ae.action is not FadeOutScreen)
                    {
                        ae.action = new FadeOutScreen();
                    }
                    break;
                case ActionType.StartSign:
                    if (ae.action is not StartSign)
                    {
                        ae.action = new StartSign();
                    }
                    break;
                case ActionType.PlayerTransfer:
                    if (ae.action is not PlayerTransfer)
                    {
                        ae.action = new PlayerTransfer();
                    }
                    break;
                case ActionType.PlayerMove:
                    if (ae.action is not PlayerWalkAction)
                    {
                        ae.action = new PlayerWalkAction();
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
                case ActionType.External:
                    if (ae.action is not ExternalAction)
                    {
                        ae.action = new ExternalAction(this);
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

    public void StartEvent(Action finishCallback=null)
    {
        eventFinishCallback = finishCallback;
        element.Execute(this);
    }
}