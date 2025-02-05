using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum DropItemType
{
    Recovery = 1 << 0,
    Recovery_Big = 1 << 2,
}

/// <summary>
/// 敵のスポーン制御
/// </summary>
public class InCameraSpawn : Spawn<BaseObject>, ISpawn
{
    [SerializeField] PoolType type;
    [SerializeField] DropItemType dropItem = DropItemType.Recovery | DropItemType.Recovery_Big;

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
        return ObjectManager.Instance.OnGet<BaseObject>(type, (obj)=> { OnDelete(); });
    }

    /// <summary>
    /// オブジェクト消滅時
    /// </summary>
    private void OnDelete()
    {
        if (dropItem == 0) return;
        Probability.BranchMethods(
            (90, null),
            (10, OnDropItem)
            );
    }

    /// <summary>
    /// ドロップアイテムの選定
    /// </summary>
    private void OnDropItem()
    {
        List<DropItemType> targets = new List<DropItemType>();

        foreach (DropItemType item in Enum.GetValues(typeof(DropItemType)))
        {
            if ((dropItem & item) != 0)
            {
                targets.Add(item);
            }
        }

        DropItemType type = targets[(int)UnityEngine.Random.Range(0, targets.Count)];

        switch (type)
        {
            case DropItemType.Recovery:
                Recovery recovery = ObjectManager.Instance.OnGet<Recovery>(PoolType.Recovery);
                recovery.transform.position = Obj.transform.position;
                break;
            case DropItemType.Recovery_Big:
                Recovery recoveryBig = ObjectManager.Instance.OnGet<Recovery>(PoolType.Recovery_Big);
                recoveryBig.transform.position = Obj.transform.position;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// スポーン位置がカメラ外
    /// </summary>
    class OutOfCamera : State<InCameraSpawn, OutOfCamera>
    {
        protected override void Update(InCameraSpawn ctr)
        {
            if (!GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
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
}
