using UnityEngine;

public class WorldManager : SingletonComponent<WorldManager>
{
    [SerializeField] TransferArea _playerTransferArea;
    [SerializeField] StagePlayer player;
    [SerializeField] PlayerBehavior playerController;

    [SerializeField] ActionChainExecuter startAction = default;

    [SerializeField] CheckPointData defaultCheckPoint;
    public StagePlayer Player => player;
    public PlayerBehavior PlayerController => playerController;

    private CheckPointData currentCheckPointData;
    public CheckPointData CurrentCheckPointData => currentCheckPointData;
    public TransferArea PlayerTransferArea => _playerTransferArea;

    public void Init()
    {
        currentCheckPointData = defaultCheckPoint;
        playerController.Init();
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
}
