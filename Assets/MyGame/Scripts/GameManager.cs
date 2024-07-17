using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public struct InputInfo
{
    public bool left, right, up, down, jump;
    public void SetInput(IInput input = null)
    {
        left = input.GetInput(InputType.Left);
        right = input.GetInput(InputType.Right);
        up = input.GetInput(InputType.Up);
        down = input.GetInput(InputType.Down);
        jump = input.GetInput(InputType.Jump);
    }
}

public class GameManager : SingletonComponent<GameManager>
{
    [SerializeField] Camera m_mainCamera = default;
    [SerializeField] CinemachineBrain m_cinemachineBrain = default;

    [SerializeField] Player player = default;

    public Camera MainCamera => m_mainCamera;
    public CinemachineBrain CinemachineBrain => m_cinemachineBrain;

    public Player Player => player;

    /// <summary>
    /// コントローラからの入力
    /// </summary>
    private IInput InputController => InputManager.Instance;

    Coroutine coroutine = null;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        InputInfo inputInfo = default;
        //inputInfo.right = true;
        inputInfo.SetInput(InputController);
        player.UpdateInput(inputInfo);

    }

    public void ChangeCamera(CinemachineVirtualCamera nextVirtualCamera)
    {

        StartCoroutine(ChangeCameraCo(nextVirtualCamera));
    }

    IEnumerator ChangeCameraCo(CinemachineVirtualCamera nextVirtualCamera)
    {
        if (nextVirtualCamera.gameObject == m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject) yield break;
        m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        nextVirtualCamera.gameObject.SetActive(true);
        // プレイヤーの動きを止める
        player.PlayerPause();
        // ブレンディングをスタートさせるため、次フレームまで待つ 
        yield return null;
        Vector3 pre_cameraPos= m_cinemachineBrain.transform.position;
        while (m_cinemachineBrain.IsBlending)
        {
            Vector3 delta = m_cinemachineBrain.transform.position - pre_cameraPos;
            player.transform.position += delta * 0.08f;
            pre_cameraPos = m_cinemachineBrain.transform.position;
            yield return null;
        }
        player.PlayerPuaseCancel();
    }

    IEnumerator PlayerForceMove()
    {
        yield return new WaitForSeconds(1f);
        player.PlayerPause();
    }
  }
