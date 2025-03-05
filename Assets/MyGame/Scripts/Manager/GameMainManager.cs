using System;
using System.Collections;
using UnityEngine;

public class GameMainManager : BaseManager<GameMainManager>
{
    public struct InputInfo
    {
        public bool left, right, up, down, jump, jumping, fire, start;
        public void SetInput(IInput input = null)
        {
            left = input.GetInput(InputType.Left);
            right = input.GetInput(InputType.Right);
            up = input.GetInput(InputType.Up);
            down = input.GetInput(InputType.Down);
            jump = input.GetDownInput(InputType.Cancel);
            jumping = input.GetInput(InputType.Cancel);
            fire = input.GetInput(InputType.Decide);
            start = input.GetDownInput(InputType.Start);
        }
    }

    public enum UI
    {
        GameMain,
        Pause,
        GameMenu,
    }


    [SerializeField] MainCameraControll m_mainCameraControll = default;
    [SerializeField] GameMainScreen m_gameMainScreen = default;
    [SerializeField] PauseScreen m_pauseScreen = default;
    [SerializeField] GameMenuScreen m_gameMenuScreen = default;
    [SerializeField] Transform worldRoot = default;
    WorldManager worldManager = default;
    int worldInstanceId = 0;

    private IInput InputController => InputManager.Instance;
    public MainCameraControll MainCameraControll => m_mainCameraControll;


    private ScreenContainer<UI> screenContainer = new ScreenContainer<UI>();
    public ScreenContainer<UI> ScreenContainer => screenContainer;

    public GameMainScreenPresenter GameMainScreenPresenter => m_gameMainScreen.ScreenPresenter;

    private bool isPause = false;
    //ポーズ可否　
    public bool Pausable { get; set; } = true;

    protected override IEnumerator Init()
    {
        FadeInManager.Instance.FadeOutImmediate();

        var res = AddressableAssetLoadUtility.LoadPrefab<WorldManager>("GrenademanStage", this.worldRoot);

        worldManager = res.Item1;
        if (worldManager == null)
        {
            Debug.LogError("ワールドの読み込みに失敗しました");
            yield break;
        }

        worldInstanceId = res.Item2;

        // ワールドの初期化
        worldManager.Init();

        // オブジェクトマネージャー初期化
        ObjectManager.Instance.Init();

        screenContainer.Add(UI.GameMain, m_gameMainScreen);
        screenContainer.Add(UI.Pause, m_pauseScreen);
        screenContainer.Add(UI.GameMenu, m_gameMenuScreen);

        OnPause(false);
    }

    protected override IEnumerator OnStart()
    {
        yield return screenContainer.Initialize(UI.GameMain, true);
        worldManager.StartStage();
        yield return null;
    }

    protected override void OnUpdate()
    {
        ScreenContainer.OnUpdate();

        InputInfo inputInfo = default;
        inputInfo.SetInput(InputController);

        if (!isPause)
        {
            worldManager.Player?.UpdateInput(inputInfo);
            worldManager.OnUpdate();
        }
    }

    protected override IEnumerator Dispose()
    {
        screenContainer.Clear();
        yield return null;
    }

    public void StageReStart()
    {
        StartCoroutine(RestartCo());

        IEnumerator RestartCo()
        {
            MainCameraControll.OnReset();

            bool isClose = false;
            screenContainer.Close(true, () => { isClose = true; });
            while (!isClose) yield return null;


            worldManager.OnReset();

            yield return screenContainer.Initialize(UI.GameMain, true);
            worldManager.StartStage();
            OnPause(false);
        }
    }



    public void DeathNotification()
    {
        StartCoroutine(DeathExecuteCo(StageReStart));
    }

    IEnumerator DeathExecuteCo(Action action)
    {

        AudioManager.Instance.StopBGM();
        yield return new WaitForSeconds(4.0f);

        bool isfade = true;
        FadeInManager.Instance.FadeOut(0.4f, Color.black, () => { isfade = false; });
        while (isfade) yield return null;

        action.Invoke();
    }

    public void GameStageEnd()
    {
        ObjectManager.Instance.Destroy();
        DestroyWorld();
        SceneManager.Instance.ChangeManager(ManagerType.BossSelect);
    }

    public void TransitToGameMenu()
    {
        screenContainer.TransitScreen(UI.GameMenu, true);
    }

    public void TransitToPause()
    {
        screenContainer.TransitScreen(UI.Pause, true);
    }

    public void TransitToGameMain()
    {
        screenContainer.TransitScreen(UI.GameMain, true);
    }

    public void OnPause(bool isPause, bool isGameMenu = false)
    {
        this.isPause = isPause;

        PauseManager.Instance.OnPause(isPause);
        worldManager?.OnPause(isPause);

        if (!isGameMenu) AudioManager.Instance.OnPause(isPause);
        else AudioManager.Instance.OnPauseSe(isPause);
    }



    void DestroyWorld()
    {
        Destroy(worldManager.gameObject);
        AddressableAssetLoadUtility.ReleasePrefab(worldInstanceId);
        worldManager = null;

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.StopSe();
    }
}
