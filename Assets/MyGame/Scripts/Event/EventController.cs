using Cinemachine;
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
        BossBattleStart,
        External,
        PlayerInputProhibit,
        PlayerInputPermit,
        PlayerRepatriate,
        CameraChange,
        PlayerForceMoveAccordingToCamera,
        PlayerForceMoveAccordingToCameraEnd,
        SubscribeAction,
        UnSubscribeAction,
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
        virtual public void OnValidate() { }
    }

    [Serializable]
    class ObjectActive : BaseAction
    {
        [SerializeField] GameObject[] objs;
        [SerializeField] bool isActive = false;
        public override void Execute(Action finishCallback)
        {
            if (objs == null) Debug.Log("オブジェクトが設定されていません。");
            foreach (var obj in objs)
            {
                obj.SetActive(isActive);
            }
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
            finishCallback.Invoke();
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
            GameManager.Instance.PlayerController.TransferPlayer(finishCallback);
        }
    }

    [Serializable]
    class PlayerWalkAction : BaseAction
    {
        [SerializeField] Transform _bamili;

        override public void Execute(Action finishCallback)
        {
            GameManager.Instance.PlayerController.AutoMoveTowards(_bamili, finishCallback);
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
    class BossBattleStart : BaseAction
    {
        [SerializeField] BossController ctr;
        override public void Execute(Action finishCallback)
        {
            ctr.ToBattleState();
            finishCallback.Invoke();
        }
    }

    [Serializable]
    class ExternalAction:BaseAction
    {
        [SerializeField,Header("※メソッドの登録は１つだけ")] UnityEvent<Action> action;

        public override void Execute(Action finishCallback)
        {
            action?.Invoke(finishCallback);
        }

        public override void OnValidate()
        {
            if (action == null) return;
            int listenerCount = action.GetPersistentEventCount();

            if (listenerCount > 1)
            {
                Debug.LogWarning("複数のメソッドが登録されているため、最初のメソッド以外は削除されます。");

                // 最初のリスナー以外を削除
                for (int i = listenerCount - 1; i >= 1; i--)
                {
                    UnityEditor.Events.UnityEventTools.RemovePersistentListener(action,i);
                }
            }
        }
    }

    [Serializable]
    class PlayerInputProhibit : BaseAction
    {
        override public void Execute(Action finishCallback)
        {
            GameManager.Instance.PlayerController.InputProhibit(finishCallback);
        }
    }

    [Serializable]
    class PlayerInputPermit : BaseAction
    {
        override public void Execute(Action finishCallback)
        {
            GameManager.Instance.PlayerController.InputPermission();
            finishCallback.Invoke();
        }
    }

    [SerializeField]
    class PlayerRepatriate : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            GameManager.Instance.PlayerController.RepatriatePlayer(finishCallback);
        }
    }


    [SerializeField]
    class CameraChange : BaseAction
    {
        [SerializeField] CinemachineVirtualCamera nextControllArea;
        [SerializeField] CinemachineBlendDefinition.Style style;
        [SerializeField] float blendTime = 0.4f;

        public override void Execute(Action finishCallback)
        {
            GameManager.Instance.MainCameraControll.ChangeCamera(nextControllArea, style, blendTime, finishCallback);
        }
    }

    [SerializeField]
    class PlayerForceMoveAccordingToCamera : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            GameManager.Instance.PlayerController.PlayerForceMoveAccordingToCamera(finishCallback);
        }
    }

    [SerializeField]
    class PlayerForceMoveAccordingToCameraEnd : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            GameManager.Instance.PlayerController.PlayerForceMoveAccordingToCameraEnd(finishCallback);
        }
    }

    [SerializeField]
    class SubscribeAction : BaseAction
    {
        [SerializeField] EventType type;
        [SerializeField] UnityEvent action;

        public override void Execute(Action finishCallback)
        {
            EventTriggerManager.Instance.Subscribe(type, action.Invoke);
            finishCallback.Invoke();
        }
    }

    [SerializeField]
    class UnSubscribeAction : BaseAction
    {
        [SerializeField] EventType type;
        [SerializeField] UnityEvent action;

        public override void Execute(Action finishCallback)
        {
            EventTriggerManager.Instance.Unsubscribe(type, action.Invoke);
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
                case ActionType.BossBattleStart:
                    if (ae.action is not BossBattleStart)
                    {
                        ae.action = new BossBattleStart();
                    }
                    break;
                case ActionType.External:
                    if (ae.action is not ExternalAction)
                    {
                        ae.action = new ExternalAction();
                    }
                    ae.action.OnValidate();
                    break;
                case ActionType.PlayerInputPermit:
                    if (ae.action is not PlayerInputPermit)
                    {
                        ae.action = new PlayerInputPermit();
                    }
                    break;
                case ActionType.PlayerInputProhibit:
                    if (ae.action is not PlayerInputProhibit)
                    {
                        ae.action = new PlayerInputProhibit();
                    }
                    break;
                case ActionType.PlayerRepatriate:
                    if (ae.action is not PlayerRepatriate)
                    {
                        ae.action = new PlayerRepatriate();
                    }
                    break;
                case ActionType.CameraChange:
                    if (ae.action is not CameraChange)
                    {
                        ae.action = new CameraChange();
                    }
                    break;
                case ActionType.PlayerForceMoveAccordingToCamera:
                    if (ae.action is not PlayerForceMoveAccordingToCamera)
                    {
                        ae.action = new PlayerForceMoveAccordingToCamera();
                    }
                    break;
                case ActionType.PlayerForceMoveAccordingToCameraEnd:
                    if (ae.action is not PlayerForceMoveAccordingToCameraEnd)
                    {
                        ae.action = new PlayerForceMoveAccordingToCameraEnd();
                    }
                    break;
                case ActionType.SubscribeAction:
                    if (ae.action is not SubscribeAction)
                    {
                        ae.action = new SubscribeAction();
                    }
                    break;
                case ActionType.UnSubscribeAction:
                    if (ae.action is not UnSubscribeAction)
                    {
                        ae.action = new UnSubscribeAction();
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

    public void StartEvent(Action finishCallback)
    {
        eventFinishCallback = finishCallback;
        element.Execute(this);
    }

    public void StartEvent()
    {
        StartEvent(null);
    }
}