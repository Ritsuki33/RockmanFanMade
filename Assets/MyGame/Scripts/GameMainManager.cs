using System;
using System.Collections;
using UnityEngine;

public class GameMainManager : BaseManager<GameMainManager>
{
    public struct InputInfo
    {
        public bool left, right, up, down, jump, jumping, fire;
        public void SetInput(IInput input = null)
        {
            left = input.GetInput(InputType.Left);
            right = input.GetInput(InputType.Right);
            up = input.GetInput(InputType.Up);
            down = input.GetInput(InputType.Down);
            jump = input.GetDownInput(InputType.Cancel);
            jumping = input.GetInput(InputType.Cancel);
            fire = input.GetInput(InputType.Decide);
        }
    }

    [SerializeField] MainCameraControll m_mainCameraControll = default;
    [SerializeField] BossSelectScreen m_bossSelectScreen = default;
    [SerializeField] GameMainScreen m_gameMainScreen = default;

    private IInput InputController => InputManager.Instance;
    private CameraControllArea currentCameraControllArea;
    public MainCameraControll MainCameraControll => m_mainCameraControll;

    public enum UI
    {
        BossSelect,
        GameMain,
    }
    private ScreenContainer<UI> screenContainer = new ScreenContainer<UI>();
    public ScreenContainer<UI> ScreenContainer => screenContainer;

    protected override void Init()
    {
        StartCoroutine(Initialize());

        IEnumerator Initialize()
        {
            FadeInManager.Instance.FadeOutImmediate();

            //WorldManager.Instance.Init();

            screenContainer.Add(UI.BossSelect, m_bossSelectScreen);
            screenContainer.Add(UI.GameMain, m_gameMainScreen);

            yield return screenContainer.Initialize(UI.BossSelect, true);

            //StageStart();

        }
    }

    protected override void OnUpdate()
    {
        InputInfo inputInfo = default;
        inputInfo.SetInput(InputController);
        WorldManager.Instance.PlayerController.UpdateInput(inputInfo);
    }

    protected override void Terminate()
    {
    }

    public void StageStart()
    {
        WorldManager.Instance.StartStage();
    }

    public void DeathNotification()
    {
        StartCoroutine(DeathExecuteCo(StageStart));
    }

    IEnumerator DeathExecuteCo(Action action)
    {
        yield return new WaitForSeconds(4.0f);

        bool isfade = true;
        FadeInManager.Instance.FadeOut(0.4f, Color.black, ()=>{ isfade = false; });
        while (isfade) yield return null;

        action.Invoke();
    }

   
}
