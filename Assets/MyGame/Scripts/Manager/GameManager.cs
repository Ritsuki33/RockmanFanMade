using System;
using System.Collections;
using UnityEngine;

public struct InputInfo
{
    public bool left, right, up, down, jump,jumping, fire;
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

[DefaultExecutionOrder(-100)]
public class GameManager : SingletonComponent<GameManager>
{
    [SerializeField] MainCameraControll m_mainCameraControll = default;

    public MainCameraControll MainCameraControll => m_mainCameraControll;

    /// <summary>
    /// コントローラからの入力
    /// </summary>
    private IInput InputController => InputManager.Instance;

    private CameraControllArea currentCameraControllArea;

    private void Start()
    {
        WorldManager.Instance.Init();
        StageStart();
    }

    private void Update()
    {
        InputInfo inputInfo = default;
        //inputInfo.right = true;
        inputInfo.SetInput(InputController);
        WorldManager.Instance.PlayerController.UpdateInput(inputInfo);
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
        UiManager.Instance.FadeInManager.FadeOut();
        while (UiManager.Instance.FadeInManager.IsFade) yield return null;

        action.Invoke();
    }
}
