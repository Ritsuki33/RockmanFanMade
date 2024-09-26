using Cinemachine;
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
    [SerializeField] PlayerController player = default;
    [SerializeField] Transform StartPos = default;

    [SerializeField] CameraControllArea defaultCameraControllArea;

    [SerializeField] EventController startEvent = default;
    public MainCameraControll MainCameraControll => m_mainCameraControll;

    public PlayerController Player => player;

    /// <summary>
    /// コントローラからの入力
    /// </summary>
    private IInput InputController => InputManager.Instance;

    private CameraControllArea currentCameraControllArea;

    private void Start()
    {
        StageStart();
        InitArea(defaultCameraControllArea);
    }

    private void Update()
    {
        InputInfo inputInfo = default;
        //inputInfo.right = true;
        inputInfo.SetInput(InputController);
        player.UpdateInput(inputInfo);

    }

    private void InitArea(CameraControllArea nextControllArea)
    {

        m_mainCameraControll.CinemachineBrain.ActiveVirtualCamera?.VirtualCameraGameObject.SetActive(false);
        currentCameraControllArea?.Area?.Exit();

        currentCameraControllArea?.Area?.gameObject.SetActive(false);

        // コントロールエリアの更新
        currentCameraControllArea = nextControllArea;
        currentCameraControllArea.VirtualCamera.gameObject.SetActive(true);
        currentCameraControllArea.Area?.gameObject.SetActive(true);

        currentCameraControllArea?.Area?.Enter();
    }

    public void ChangeCamera(CameraControllArea nextControllArea, IEnumerator start = null, IEnumerator end =null)
    {
        StartCoroutine(ChangeCameraCo(nextControllArea, start, end));

        IEnumerator ChangeCameraCo(CameraControllArea nextControllArea, IEnumerator start, IEnumerator end)
        {
            if (nextControllArea == null || nextControllArea.VirtualCamera.gameObject == m_mainCameraControll.CinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject) yield break;

            // 通知する
            EventTriggerManager.Instance.Notify(EventType.ChangeCameraStart);

            yield return start;

            InitArea(nextControllArea);

            // ブレンディングをスタートさせるため、次フレームまで待つ 
            yield return null;

            Vector3 pre_cameraPos = m_mainCameraControll.CinemachineBrain.transform.position;
            while (m_mainCameraControll.CinemachineBrain.IsBlending)
            {
                Vector3 delta = m_mainCameraControll.CinemachineBrain.transform.position - pre_cameraPos;
                player.transform.position += delta * 0.08f;
                pre_cameraPos = m_mainCameraControll.CinemachineBrain.transform.position;
                yield return null;
            }

            yield return end;

            // 通知する
            EventTriggerManager.Instance.Notify(EventType.ChangeCameraEnd);
        }
    }


    public void StageStart()
    {
        startEvent.StartEvent();
        //StartCoroutine(StageStartCo());
    }

    IEnumerator StageStartCo()
    {
        yield return new WaitForSeconds(1);
        player.Prepare(StartPos);
        UiManager.Instance.FadeInManager.FadeIn(0.4f);
        while (UiManager.Instance.FadeInManager.IsFade) yield return null;
        UiManager.Instance.ReadyUi.Play();
        while(UiManager.Instance.ReadyUi.IsPlaying) yield return null;
        player.TransferPlayer();
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
