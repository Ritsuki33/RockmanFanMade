using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineBlendDefinition;

public class ActionChainExecuter : MonoBehaviour
{
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
        EnemyAppear,
        BossHpBarSet,
        BossBattleStart,
        External,
        PlayerInputProhibit,
        PlayerInputPermit,
        PlayerRepatriate,
        CameraChange,
        PlayerForceMoveAccordingToCamera,
        PlayerForceMoveAccordingToCameraEnd,
        SubscribeActionChain,
        UnSubscribeActionChain,
        SetupCheckPoint,
        SetPlayerHp,
        ChangeManager,
        DefeatEnemyCondition,
    }

    [Serializable]
    abstract class AElement {
        abstract public void Execute(ActionChainExecuter eventControll);
        abstract public List<ActionElement> Actions { get; }
    }

    [Serializable]
    class Element : AElement
    {
        [SerializeField] float delayTime = 0.0f;
        [SerializeField] public List<ActionElement> actions;
        [SerializeReference] public AElement _next;

        override public List<ActionElement> Actions => actions;

        override public void Execute(ActionChainExecuter eventControll)
        {
            eventControll.StartCoroutine(ActionExecuteCo(eventControll));

            IEnumerator ActionExecuteCo(ActionChainExecuter eventControll)
            {
                if (actions == null || actions.Count == 0) yield break;

                if (delayTime > 0) yield return new WaitForSeconds(delayTime);

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

                // 次のアクション
                if (_next != null && _next.Actions != null && _next.Actions.Count != 0) _next.Execute(eventControll);
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
        [SerializeField, Header("備考")] private string remarks;
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
        [SerializeField] StageDirectionalObject obj;
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
            FadeInManager.Instance.FadeIn(fadeTime, color, finishCallback);
        }
    }

    [SerializeField]
    class FadeOutScreen : BaseAction
    {
        [SerializeField] float fadeTime = 0.4f;
        [SerializeField] Color color = Color.black;
        public override void Execute(Action finishCallback)
        {
            FadeInManager.Instance.FadeOut(fadeTime, color,finishCallback);
        }
    }

    [SerializeField]
    class StartSign : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            var presenter = GameMainManager.Instance.ScreenContainer.GetCurrentScreenPresenter<GameMainScreenPresenter>();
            presenter.ReadyUiPlay(finishCallback);
        }
    }

    [SerializeField]
    class PlayerTransfer : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            Vector2 appearPos = WorldManager.Instance.GetPlayerTransferPostion();
            WorldManager.Instance.Player.transform.position_xy(appearPos);
            WorldManager.Instance.Player.TransferPlayer(finishCallback);
        }
    }

    [Serializable]
    class PlayerWalkAction : BaseAction
    {
        [SerializeField] Transform _bamili;

        override public void Execute(Action finishCallback)
        {
            WorldManager.Instance.Player.AutoMoveTowards(_bamili, finishCallback);
        }
    }

    [Serializable]
    class BoseAppearAction : BaseAction
    {
        [SerializeField] Grenademan ctr;
        [SerializeField] GameObject transferArea;
        override public void Execute(Action finishCallback)
        {
            Vector2 appearPos = new Vector3(
               transferArea.transform.position.x,
               GameMainManager.Instance.MainCameraControll.OutOfViewTop
               );
            ctr.transform.position_xy(appearPos);
            //ctr.Appeare(finishCallback);
        }
    }

    [Serializable]
    class BossHpBarSetAction : BaseAction
    {
        [SerializeField] Grenademan ctr;

        override public void Execute(Action finishCallback)
        {
            var presenter = GameMainManager.Instance.ScreenContainer.GetCurrentScreenPresenter<GameMainScreenPresenter>();
            presenter.EnemyHpIncrementAnimation(ctr, finishCallback);
        }
    }

    [Serializable]
    class BossBattleStart : BaseAction
    {
        [SerializeField] Grenademan ctr;
        override public void Execute(Action finishCallback)
        {
            //ctr.ToBattleState();
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
            WorldManager.Instance.Player.InputProhibit(finishCallback);
        }
    }

    [Serializable]
    class PlayerInputPermit : BaseAction
    {
        override public void Execute(Action finishCallback)
        {
            WorldManager.Instance.Player.InputPermission();
            finishCallback.Invoke();
        }
    }

    [Serializable]
    class PlayerRepatriate : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            WorldManager.Instance.Player.RepatriatePlayer(finishCallback);
        }
    }


    [Serializable]
    class CameraChange : BaseAction
    {
        [SerializeField] CinemachineVirtualCamera nextControllArea;
        [SerializeField] CinemachineBlendDefinition.Style style;
        [SerializeField] float blendTime = 0.8f;

        public override void Execute(Action finishCallback)
        {
            GameMainManager.Instance.MainCameraControll.ChangeCamera(nextControllArea, style, blendTime, finishCallback);
        }
    }

    [Serializable]
    class PlayerForceMoveAccordingToCamera : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            WorldManager.Instance.Player.PlayerForceMoveAccordingToCamera(finishCallback);
        }
    }

    [Serializable]
    class PlayerForceMoveAccordingToCameraEnd : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            WorldManager.Instance.Player.PlayerForceMoveAccordingToCameraEnd(finishCallback);
        }
    }

    [Serializable]
    class SubscribeActionChain : BaseAction
    {
        [SerializeField] EventType type;
        [SerializeField] ActionChainExecuter actionChainExecuter;

        public override void Execute(Action finishCallback)
        {
            EventTriggerManager.Instance.VoidEventTriggers.Subscribe(type, actionChainExecuter.StartEvent);
            finishCallback.Invoke();
        }
    }

    [Serializable]
    class UnSubscribeActionChain : BaseAction
    {
        [SerializeField] EventType type;
        [SerializeField] ActionChainExecuter actionChainExecuter;

        public override void Execute(Action finishCallback)
        {
            EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(type, actionChainExecuter.StartEvent);
            finishCallback.Invoke();
        }
    }

    [Serializable]
    class SetupCheckPoint : BaseAction
    {
        public override void Execute(Action finishCallback)
        {
            var checkPoint = WorldManager.Instance.CurrentCheckPointData;
            var nextControllArea = checkPoint.virtualCamera;
            WorldManager.Instance.PlayerTransferArea.transform.position_xy(checkPoint.position.position);
            GameMainManager.Instance.MainCameraControll.ChangeCamera(nextControllArea, Style.Cut, 0, finishCallback);
        }
    }

    [Serializable]
    class SetPlayerHp : BaseAction
    {
        [SerializeField, Range(0, 27)] int val;
        public override void Execute(Action finishCallback)
        {
            WorldManager.Instance.Player.SetHp(val);
            finishCallback();
        }
    }

    [Serializable]
    class ChangeManager : BaseAction
    {
        [SerializeField] ManagerType type;
        public override void Execute(Action finishCallback)
        {
            SceneManager.Instance.ChangeManager(type);
            finishCallback();
        }
    }

    [Serializable]
    class DefeatEnemyCondition : BaseAction
    {
        enum CondtionType
        {
            DefeatTargetEnemyCondition,
            DefeatEnemyNunCondition
        }

        [SerializeField, Header("条件タイプ")] CondtionType type;
        [SerializeReference] BaseEnemyCondition enemyCondition;

        public override void Execute(Action finishCallback)
        {
            EventTriggerManager.Instance.EenemyEventTriggers.Subscribe(EnemyEventType.Defeated, enemyCondition.Defeated);
            finishCallback.Invoke();
        }

        public void OnValidation()
        {
            switch (type)
            {
                case CondtionType.DefeatTargetEnemyCondition:
                    if(enemyCondition is not DefeatTargetEnemyCondition)
                    {
                        enemyCondition = new DefeatTargetEnemyCondition();
                    }
                    break;
                case CondtionType.DefeatEnemyNunCondition:
                    if (enemyCondition is not DefeatEnemyNunCondition)
                    {
                        enemyCondition = new DefeatEnemyNunCondition();
                    }
                    break;
                default:
                    break;
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
                case ActionType.EnemyAppear:
                    if (ae.action is not BoseAppearAction)
                    {
                        ae.action = new BoseAppearAction();
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
                case ActionType.SubscribeActionChain:
                    if (ae.action is not SubscribeActionChain)
                    {
                        ae.action = new SubscribeActionChain();
                    }
                    break;
                case ActionType.UnSubscribeActionChain:
                    if (ae.action is not UnSubscribeActionChain)
                    {
                        ae.action = new UnSubscribeActionChain();
                    }
                    break;
                case ActionType.SetupCheckPoint:
                    if (ae.action is not SetupCheckPoint)
                    {
                        ae.action = new SetupCheckPoint();
                    }
                    break;
                case ActionType.SetPlayerHp:
                    if (ae.action is not SetPlayerHp)
                    {
                        ae.action = new SetPlayerHp();
                    }
                    break;
                case ActionType.ChangeManager:
                    if (ae.action is not ChangeManager)
                    {
                        ae.action = new ChangeManager();
                    }
                    break;
                case ActionType.DefeatEnemyCondition:
                    if (ae.action is not DefeatEnemyCondition)
                    {
                        ae.action = new DefeatEnemyCondition();
                    }

                    (ae.action as DefeatEnemyCondition).OnValidation();
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