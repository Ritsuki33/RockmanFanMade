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
public class InCameraEnemySpawn : Spawn<StageEnemy>, ISpawn
{
    [SerializeField] PoolType type;
    [SerializeField] DropItemType dropItem = DropItemType.Recovery | DropItemType.Recovery_Big;

    public bool IsDeath => Obj == null || !Obj.gameObject.activeSelf;

    StateMachine<InCameraEnemySpawn> stateMachine = new StateMachine<InCameraEnemySpawn>();

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

    protected override StageEnemy OnGetResource()
    {
        var enemy = ObjectManager.Instance.OnGet<StageEnemy>(type, (obj) => this.obj = null);
        enemy.onDeadCallback = OnDefeated;
        return enemy;
    }

    /// <summary>
    /// オブジェクト消滅時
    /// </summary>
    private void OnDefeated()
    {
        if (dropItem == 0) return;
        Probability.BranchMethods(
            (100, null),
            (100, OnDropItem)
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
    class OutOfCamera : State<InCameraEnemySpawn, OutOfCamera>
    {
        protected override void Update(InCameraEnemySpawn ctr)
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
    class InCamera : State<InCameraEnemySpawn, InCamera>
    {
        protected override void Enter(InCameraEnemySpawn ctr, int preId, int subId)
        {
            if (ctr.IsDeath) ctr.TrySpawnObject();
        }

        protected override void Update(InCameraEnemySpawn ctr)
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
