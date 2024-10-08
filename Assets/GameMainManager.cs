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
    [SerializeField] ReadyUi readyUi = default;
    [SerializeField] HpBar hpBar = default;

    private IInput InputController => InputManager.Instance;
    private CameraControllArea currentCameraControllArea;
    public MainCameraControll MainCameraControll => m_mainCameraControll;

    public ReadyUi ReadyUi => readyUi;
    public HpBar HpBar => hpBar;

    protected override void Init()
    {
        WorldManager.Instance.Init();
        StageStart();
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
        FadeInManager.Instance.FadeOut();
        while (FadeInManager.Instance.IsFade) yield return null;

        action.Invoke();
    }

   
}
