using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のスポーン制御
/// </summary>
public class InCameraSpawn : Spawn<BaseObject>, ISpawn
{
    [SerializeField] PoolType type;

    public bool IsDeath => Obj == null || !Obj.gameObject.activeSelf;

    StateMachine<InCameraSpawn> stateMachine = new StateMachine<InCameraSpawn>();

    enum StateID
    {
        OutOfCamera,
        InCamera,
    }

    private void Awake()
    {
        stateMachine.AddState((int)StateID.OutOfCamera, new OutOfCamera());
        stateMachine.AddState((int)StateID.InCamera, new InCamera());
    }

    void ISpawn.Initialize()
    {
        stateMachine.TransitReady((int)StateID.OutOfCamera, true);
    }

    void ISpawn.OnUpdate()
    {
        stateMachine.Update(this);
    }

    void ISpawn.Terminate() { }

    protected override BaseObject OnGetResource()
    {
        return ObjectManager.Instance.OnGet<BaseObject>(type, (obj) => this.obj = null);
    }

    /// <summary>
    /// スポーン位置がカメラ外
    /// </summary>
    class OutOfCamera : State<InCameraSpawn, OutOfCamera>
    {
        protected override void Update(InCameraSpawn ctr)
        {
            if (!GameMainManager.Instance.MainCameraControll.CheckInView(ctr.gameObject, 0.05f))
            {
                ctr.stateMachine.TransitReady((int)StateID.InCamera);
            }
        }
    }

    /// <summary>
    /// スポーン位置がカメラ内
    /// </summary>
    class InCamera : State<InCameraSpawn, InCamera>
    {
        protected override void Enter(InCameraSpawn ctr, int preId, int subId)
        {
            if (ctr.IsDeath) ctr.TrySpawnObject();
        }

        protected override void Update(InCameraSpawn ctr)
        {
            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.stateMachine.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, Vector3.one * 0.4f);
    }
}
