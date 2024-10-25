using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : SingletonComponent<WorldManager>
{
    [SerializeField] TransferArea _playerTransferArea;
    [SerializeField] Player player;
    [SerializeField] PlayerController playerController;

    [SerializeField] ActionChainExecuter startAction = default;

    [SerializeField] CheckPointData defaultCheckPoint;
    public Player Player => player;
    public PlayerController PlayerController => playerController;

    private CheckPointData currentCheckPointData;
    public CheckPointData CurrentCheckPointData => currentCheckPointData;
    public TransferArea PlayerTransferArea => _playerTransferArea;

    public void Init()
    {
        currentCheckPointData = defaultCheckPoint;
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
}
