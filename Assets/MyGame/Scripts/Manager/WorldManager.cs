using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IRegister
{
    public void OnRegist(IObjectInterpreter obj);
    public void OnUnregist(IObjectInterpreter obj);
}

public class WorldManager : SingletonComponent<WorldManager>
{
    [SerializeField] Transform _playerTransferArea;

    [SerializeField] ActionChainExecuter startAction = default;

    [SerializeField] CheckPointData defaultCheckPoint;

    [SerializeField] Transform spawnRoot;

    private StagePlayer _player;
    public StagePlayer Player
    {
        get
        {
            if (!_player) Debug.LogError("Playerがロードされていません");

            return _player;
        }
    }

    private CheckPointData currentCheckPointData;
    public CheckPointData CurrentCheckPointData => currentCheckPointData;
    public Transform PlayerTransferArea => _playerTransferArea;


    bool isPause = false;

    public bool IsPause => isPause;

    private List<ISpawn> _spawns;
    public void Init()
    {
        currentCheckPointData = defaultCheckPoint;
        isPause = false;

        _player = ObjectManager.Instance.OnLoad<StagePlayer>("Prefabs/Player");

        _spawns = spawnRoot.GetComponentsInChildren<ISpawn>()?.ToList();

    }

    public void OnReset()
    {
        ObjectManager.Instance.AllDelete();

        _player = ObjectManager.Instance.OnLoad<StagePlayer>("Prefabs/Player");

        _spawns.ForEach(spawn =>
        {
            spawn.Initialize();
        });
    }

    /// <summary>
    /// 物理演算等の動きはここからでいいか？？
    /// </summary>
    private void FixedUpdate()
    {
        if (isPause) return;
        ObjectManager.Instance.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        if (isPause) return;
        ObjectManager.Instance.OnUpdate();

        _spawns.ForEach(spawn =>
        {
            spawn.OnUpdate();
        });
    }

    public void OnPause(bool isPause)
    {
        this.isPause = isPause;
        ObjectManager.Instance.OnPause(isPause);
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
        EventTriggerManager.Instance.Init();

        GameMainManager.Instance.GameMainScreenPresenter.BindPlayerHp(Player.Hp);

        _spawns.ForEach(spawn =>
        {
            spawn.Initialize();
        });

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
