using UnityEngine;

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
        stateMachine.AddState((int)StateID.Appering, new Appering());
        stateMachine.AddState((int)StateID.Disappearing, new Disappearing());

        enemy.gameObject.SetActive(false);
    }

    public void Init(IRegister register)
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);

        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);

        _register = register;
    }

    public void Reset()
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
    /// カメラ外
    /// </summary>
    class OutOfCamera : State<Spawn, OutOfCamera>
    {
        protected override void Enter(Spawn ctr, int preId, int subId)
        {
            ctr.enemy.transform.position = ctr.transform.position;
            ctr.enemy.gameObject.SetActive(false);
            ctr._register?.OnUnregist(ctr.enemy);
        }

        protected override void Update(Spawn ctr)
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
    class Appering : State<Spawn, Appering>
    {
        protected override void Enter(Spawn ctr, int preId, int subId)
        {
            ctr.enemy.gameObject.SetActive(true);
            ctr.enemy.Setup(ctr._register.OnUnregist);
            ctr._register?.OnRegist(ctr.enemy);
        }

        protected override void Update(Spawn ctr)
        {
            if (ctr.IsDeath || GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.enemy.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.Disappearing);
            }
        }
    }

    /// <summary>
    /// 画面に入っているが、敵が消えている状態
    /// </summary>
    class Disappearing : State<Spawn, Disappearing>
    {
        protected override void Enter(Spawn ctr, int preId, int subId)
        {
            ctr.enemy.gameObject.SetActive(false);
            ctr._register?.OnUnregist(ctr.enemy);
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
