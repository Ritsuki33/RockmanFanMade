using Cinemachine;
using System.Collections;
using UnityEngine;

public struct InputInfo
{
    public bool left, right, up, down, jump, fire;
    public void SetInput(IInput input = null)
    {
        left = input.GetInput(InputType.Left);
        right = input.GetInput(InputType.Right);
        up = input.GetInput(InputType.Up);
        down = input.GetInput(InputType.Down);
        jump = input.GetInput(InputType.Cancel);
        fire = input.GetInput(InputType.Decide);
    }
}

public class GameManager : SingletonComponent<GameManager>
{
    [SerializeField] MainCameraControll m_mainCameraControll = default;

    [SerializeField] Player player = default;

    public MainCameraControll MainCameraControll => m_mainCameraControll;

    public Player Player => player;

    /// <summary>
    /// コントローラからの入力
    /// </summary>
    private IInput InputController => InputManager.Instance;

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
        if (nextVirtualCamera.gameObject == m_mainCameraControll.CinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject) yield break;
        m_mainCameraControll.CinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        nextVirtualCamera.gameObject.SetActive(true);
        // プレイヤーの動きを止める
        player.PlayerPause();
        // ブレンディングをスタートさせるため、次フレームまで待つ 
        yield return null;
        Vector3 pre_cameraPos= m_mainCameraControll.CinemachineBrain.transform.position;
        while (m_mainCameraControll.CinemachineBrain.IsBlending)
        {
            Vector3 delta = m_mainCameraControll.CinemachineBrain.transform.position - pre_cameraPos;
            player.transform.position += delta * 0.08f;
            pre_cameraPos = m_mainCameraControll.CinemachineBrain.transform.position;
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
