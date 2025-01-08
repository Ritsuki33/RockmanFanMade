using UnityEngine;

public class EnemyAppearController : MonoBehaviour
{
    [SerializeField] StageEnemy enemy = default;

    public bool IsDeath => !enemy.gameObject.activeSelf;

    StateMachine<EnemyAppearController> stateMachine = new StateMachine<EnemyAppearController>();

    IUpdateListController _updateListController = null;
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
        stateMachine.AddState((int)StateID.Appering, new Appering());
        stateMachine.AddState((int)StateID.Disappearing, new Disappearing());

        enemy.gameObject.SetActive(false);
    }

    //private void OnEnable()
    //{
    //    EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
    //    EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);
    //}

    //private void OnDisable()
    //{
    //    EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Disabled);
    //    EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraEnd, Enabled);
    //}


    public void Init(IUpdateListController updateListController)
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);

        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);

        _updateListController = updateListController;
    }

    public void OnUpdate()
    {
        if (_updateListController == null)
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

        _updateListController = null;
    }

    class None : State<EnemyAppearController, None>
    {
        protected override void Enter(EnemyAppearController ctr, int preId, int subId)
        {
            ctr.enemy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// カメラ外
    /// </summary>
    class OutOfCamera : State<EnemyAppearController, OutOfCamera>
    {
        protected override void Enter(EnemyAppearController ctr, int preId, int subId)
        {
            ctr.enemy.transform.position = ctr.transform.position;
            ctr.enemy.gameObject.SetActive(false);
            ctr._updateListController.RemoveObject(ctr.enemy);
        }

        protected override void Update(EnemyAppearController ctr)
        {
            if (!GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.Appering);
            }
        }
    }

    /// <summary>
    /// 敵が見えている状態
    /// </summary>
    class Appering : State<EnemyAppearController, Appering>
    {
        protected override void Enter(EnemyAppearController ctr, int preId, int subId)
        {
            ctr.enemy.gameObject.SetActive(true);
            ctr._updateListController.AddObject(ctr.enemy);
        }

        protected override void Update(EnemyAppearController ctr)
        {
            if (ctr.IsDeath || GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.enemy.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.Disappearing);
            }
        }
    }

    /// <summary>
    /// 消えている状態
    /// </summary>
    class Disappearing : State<EnemyAppearController, Disappearing>
    {
        protected override void Enter(EnemyAppearController ctr, int preId, int subId)
        {
            ctr.enemy.gameObject.SetActive(false);
            ctr._updateListController.RemoveObject(ctr.enemy);
        }
        protected override void Update(EnemyAppearController ctr)
        {
            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }
}
