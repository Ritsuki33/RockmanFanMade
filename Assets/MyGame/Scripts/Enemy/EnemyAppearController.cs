using UnityEngine;

public class EnemyAppearController : MonoBehaviour
{
    [SerializeField] EnemyObject enemy = default;

    public bool IsDeath => !enemy.gameObject.activeSelf;

    StateMachine<EnemyAppearController> stateMachine = new StateMachine<EnemyAppearController>();

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

    private void OnEnable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraEnd, Enabled);
    }

    public void Enabled()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera);
    }

    public void Disabled()
    {
        enemy.gameObject.SetActive(false);
        stateMachine.TransitReady((int)StateID.None);
    }

    public void Init()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);

        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);

    }

    public void Destroy()
    {
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraEnd, Enabled);
    }

    class None : State<EnemyAppearController, None>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId, int subId)
        {
            enemyAppearController.enemy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// カメラ外
    /// </summary>
    class OutOfCamera : State<EnemyAppearController, OutOfCamera>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId, int subId)
        {
            //enemyAppearController.enemy.Init();
            enemyAppearController.enemy.transform.position = enemyAppearController.transform.position;
            enemyAppearController.enemy.gameObject.SetActive(false);
        }
        protected override void Update(EnemyAppearController enemyAppearController)
        {
            if (!GameMainManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.gameObject))
            {
                enemyAppearController.stateMachine.TransitReady((int)StateID.Appering);
            }
        }
    }

    /// <summary>
    /// 敵が見えている状態
    /// </summary>
    class Appering : State<EnemyAppearController, Appering>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId, int subId)
        {
            enemyAppearController.enemy.gameObject.SetActive(true);
        }

        protected override void Update(EnemyAppearController enemyAppearController)
        {
            if (enemyAppearController.IsDeath || GameMainManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.enemy.gameObject))
            {
                enemyAppearController.stateMachine.TransitReady((int)StateID.Disappearing);
            }
        }
    }

    /// <summary>
    /// 消えている状態
    /// </summary>
    class Disappearing : State<EnemyAppearController, Disappearing>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId, int subId)
        {
            enemyAppearController.enemy.gameObject.SetActive(false);
        }
        protected override void Update(EnemyAppearController enemyAppearController)
        {
            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.gameObject))
            {
                enemyAppearController.stateMachine.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }
}
