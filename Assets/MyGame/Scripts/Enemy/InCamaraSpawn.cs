using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// 敵のスポーン制御
/// </summary>
public class InCamaraSpawn : Spawn
{
    public bool IsDeath => !Obj.gameObject.activeSelf;

    StateMachine<InCamaraSpawn> stateMachine = new StateMachine<InCamaraSpawn>();

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

        Obj.gameObject.SetActive(false);
    }

    public override void Init(IRegister register)
    {
        base.Init(register);
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);

        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraEnd, Enabled);
    }

    public void OnReset()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);
    }

    public void OnUpdate()
    {
        stateMachine.Update(this);
    }

    private void Enabled()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera);
    }

    private void Disabled()
    {
        stateMachine.TransitReady((int)StateID.None);
    }

    public override void Destroy()
    {
        base.Destroy();
        stateMachine.TransitReady((int)StateID.None);
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Disabled);
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraEnd, Enabled);
    }

    class None : State<InCamaraSpawn, None>
    {
        protected override void Enter(InCamaraSpawn ctr, int preId, int subId)
        {
            ctr.Obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// スポーン位置がカメラ外
    /// </summary>
    class OutOfCamera : State<InCamaraSpawn, OutOfCamera>
    {
        protected override void Update(InCamaraSpawn ctr)
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
    class InCamera : State<InCamaraSpawn, InCamera>
    {
        protected override void Enter(InCamaraSpawn ctr, int preId, int subId)
        {
            if (ctr.IsDeath) ctr.SpawnObject();
        }

        protected override void Update(InCamaraSpawn ctr)
        {
            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }
}
