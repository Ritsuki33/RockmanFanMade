using UnityEngine;

public class EnemyAppearController : StateMachine<EnemyAppearController>
{
    [SerializeField] EnemyObject enemy = default;

    public bool IsDeath => !enemy.gameObject.activeSelf;

    enum StateID
    {
        None,
        OutOfCamera,
        Appering,
        Disappearing
    }
    private void Awake()
    {
        AddState((int)StateID.None, new None());
        AddState((int)StateID.OutOfCamera, new OutOfCamera());
        AddState((int)StateID.Appering, new Appering());
        AddState((int)StateID.Disappearing, new Disappearing());

        TransitReady((int)StateID.OutOfCamera, true);
    }

    private void Start()
    {
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
        TransitReady((int)StateID.OutOfCamera);
    }

    public void Disabled()
    {
        enemy.gameObject.SetActive(false);
        TransitReady((int)StateID.None);
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
            enemyAppearController.enemy.Init();
            enemyAppearController.enemy.transform.position = enemyAppearController.transform.position;
            enemyAppearController.enemy.gameObject.SetActive(false);
        }
        protected override void Update(EnemyAppearController enemyAppearController)
        {
            if (!GameMainManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.gameObject))
            {
                enemyAppearController.TransitReady((int)StateID.Appering);
            }
        }
    }

    /// <summary>
    /// 敵が見えている状態
    /// </summary>
    class Appering: State<EnemyAppearController, Appering>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId, int subId)
        {
            enemyAppearController.enemy.gameObject.SetActive(true);
        }

        protected override void Update(EnemyAppearController enemyAppearController)
        {
            if (enemyAppearController.IsDeath|| GameMainManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.enemy.gameObject))
            {
                enemyAppearController.TransitReady((int)StateID.Disappearing);
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
                enemyAppearController.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }
}
