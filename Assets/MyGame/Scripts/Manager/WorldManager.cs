using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IUpdateListController
{
    public void AddPlayer(StagePlayer player);
    public void AddObject(IObjectInterpreter obj);
    public void RemoveObject(IObjectInterpreter obj);
}
public class WorldManager : SingletonComponent<WorldManager>, IUpdateListController
{
    [SerializeField] TransferArea _playerTransferArea;

    [SerializeField] ActionChainExecuter startAction = default;

    [SerializeField] CheckPointData defaultCheckPoint;

    [SerializeField] Transform enemyRoot;

    private StagePlayer _player;
    public StagePlayer Player => _player;

    private CheckPointData currentCheckPointData;
    public CheckPointData CurrentCheckPointData => currentCheckPointData;
    public TransferArea PlayerTransferArea => _playerTransferArea;

    UpdateList updateList = new UpdateList();

    bool isPause = false;

    List<EnemyAppearController> enemyAppearControllerList = new List<EnemyAppearController>();
    public void Init()
    {
        currentCheckPointData = defaultCheckPoint;
        isPause = false;

        enemyAppearControllerList = enemyRoot.GetComponentsInChildren<EnemyAppearController>().ToList();

        foreach (var e in enemyAppearControllerList)
        {
            e.Init(this);
        }
    }

    /// <summary>
    /// 物理演算等の動きはここからでいいか？？
    /// </summary>
    private void FixedUpdate()
    {
        if (isPause) return;
        updateList.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        if (isPause) return;

        updateList.OnUpdate();

        foreach(var element in enemyAppearControllerList)
        {
            element.OnUpdate();
        }
    }

    public void OnPause(bool isPause)
    {
        this.isPause = isPause;
        updateList.OnPause(isPause);
    }
    /// <summary>
    /// チェックポイントの保存
    /// </summary>
    /// <param name="checkPointData"></param>
    public void SaveCheckPoint(CheckPointData checkPointData)
    {
        currentCheckPointData = checkPointData;
    }

    public void StartStage()
    {
        EventTriggerManager.Instance.Notify(EventType.StartStage);
        startAction.StartEvent();
    }

    public Vector2 GetPlayerTransferPostion()
    {
        Vector2 appearPos = new Vector2(
               currentCheckPointData.position.position.x,
               GameMainManager.Instance.MainCameraControll.OutOfViewTop
               );

        return appearPos;
    }

    /// <summary>
    /// プレイヤーの登録
    /// </summary>
    /// <param name="player"></param>
    public void AddPlayer(StagePlayer player)
    {
        _player = player;
        AddObject(player);
    }

    public void AddObject(IObjectInterpreter obj)
    {
        obj.Init();
        updateList.Add(obj);
    }

    public void RemoveObject(IObjectInterpreter obj)
    {
        obj.Destroy();
        updateList.Remove(obj);
    }
}
