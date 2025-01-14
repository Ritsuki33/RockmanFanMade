using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// 敵のスポーン制御
/// </summary>
public class Spawn : MonoBehaviour
{
    [SerializeField] StageEnemy enemy = default;

    public bool IsDeath => !enemy.gameObject.activeSelf;

    StateMachine<Spawn> stateMachine = new StateMachine<Spawn>();

    IRegister _register = null;
    enum StateID
    {
        None,
        OutOfCamera,
        Appering,
        Disappearing
    }

    private void Awake()
    {
        stateMachine.AddState((int)StateID.None, new None());
        stateMachine.AddState((int)StateID.OutOfCamera, new OutOfCamera());
        stateMachine.AddState((int)StateID.Appering, new InCamera());

        enemy.gameObject.SetActive(false);
    }

    public void Init(IRegister register)
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);

        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);

        _register = register;
    }

    /// <summary>
    /// 敵をスポーン
    /// </summary>
    public void SpawnEnemy()
    {
        enemy.gameObject.SetActive(true);
        enemy.Setup(() =>
        {
            _register.OnUnregist(enemy);
            enemy.gameObject.SetActive(false);
        });

        _register?.OnRegist(enemy);
    }

    public void OnReset()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);
    }

    public void OnUpdate()
    {
        if (_register == null)
        {
            Debug.Log("オブジェクト管理用インターフェイスが設定されていないため、更新処理が出来ません");
            return;
        }
        stateMachine.Update(this);
    }

    private void Enabled()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera);
    }

    private void Disabled()
    {
        enemy.gameObject.SetActive(false);
        stateMachine.TransitReady((int)StateID.None);
    }

    public void Destroy()
    {
        stateMachine.TransitReady((int)StateID.None);
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraEnd, Enabled);

        _register = null;
    }

    class None : State<Spawn, None>
    {
        protected override void Enter(Spawn ctr, int preId, int subId)
        {
            ctr.enemy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// スポーン位置がカメラ外
    /// </summary>
    class OutOfCamera : State<Spawn, OutOfCamera>
    {
        protected override void Update(Spawn ctr)
        {
            if (!GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.Appering);
            }
        }
    }

    /// <summary>
    /// スポーン位置がカメラ内
    /// </summary>
    class InCamera : State<Spawn, InCamera>
    {
        protected override void Enter(Spawn ctr, int preId, int subId)
        {
            if (ctr.IsDeath) ctr.SpawnEnemy();
        }

        protected override void Update(Spawn ctr)
        {
            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }
}
