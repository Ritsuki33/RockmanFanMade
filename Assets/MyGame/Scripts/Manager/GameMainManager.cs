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
    }


    [SerializeField] MainCameraControll m_mainCameraControll = default;
    [SerializeField] GameMainScreen m_gameMainScreen = default;
    [SerializeField] Transform worldRoot= default;
    WorldManager worldManager = default;

    private IInput InputController => InputManager.Instance;
    private CameraControllArea currentCameraControllArea;
    public MainCameraControll MainCameraControll => m_mainCameraControll;


    private ScreenContainer<UI> screenContainer = new ScreenContainer<UI>();
    public ScreenContainer<UI> ScreenContainer => screenContainer;

    public GameMainScreenPresenter GameMainScreenPresenter => m_gameMainScreen.ScreenPresenter;

    private bool isPause = false;
    //ポーズ可否　
    public bool Pausable { get; set; } = true;
    protected override void Init()
    {
        StartCoroutine(Initialize());

        IEnumerator Initialize()
        {
            FadeInManager.Instance.FadeOutImmediate();

            worldManager = CreateWorldManager();
            if (worldManager == null)
            {
                Debug.LogError("ワールドの読み込みに失敗しました");
                yield break;
            }

            // ワールドの初期化
            worldManager.Init();

            // オブジェクトマネージャー初期化
            ObjectManager.Instance.Init();

            screenContainer.Add(UI.GameMain, m_gameMainScreen);
            yield return screenContainer.Initialize(UI.GameMain, true);

            OnPause(false);

            worldManager.StartStage();
        }
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

        if (Pausable && inputInfo.start)
        {
            isPause = !isPause;
            OnPause(isPause);
        }
    }

    protected override void Terminate()
    {
        screenContainer.Clear();
    }

    public void StageReStart()
    {
        StartCoroutine(RestartCo());

        IEnumerator RestartCo()
        {
            screenContainer.Close(true, null);
            MainCameraControll.OnReset();
            worldManager.OnReset();

            yield return screenContainer.Initialize(UI.GameMain, true);

            OnPause(false);

            worldManager.StartStage();
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
        FadeInManager.Instance.FadeOut(0.4f, Color.black, ()=>{ isfade = false; });
        while (isfade) yield return null;

        action.Invoke();
    }

    public void GameStageEnd()
    {
        ObjectManager.Instance.Destroy();
        DestroyWorld();
        SceneManager.Instance.ChangeManager(ManagerType.BossSelect);
    }

    void OnPause(bool isPause)
    {
        this.isPause = isPause;

        PauseManager.Instance.OnPause(isPause);
        GameMainScreenPresenter.OnOpenPauseUi(isPause);
        worldManager?.OnPause(isPause);

        AudioManager.Instance.OnPause(isPause);
    }


    WorldManager CreateWorldManager()
    {
        var res = Resources.Load<WorldManager>("Worlds/World");

        if (res != null)
        {
            return Instantiate(res, this.worldRoot);
        }
        else
        {
            return null;
        }
    }

    void DestroyWorld()
    {
        Destroy(worldManager.gameObject);
        worldManager = null;

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.StopSe();
    }
}
