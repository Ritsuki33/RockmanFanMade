using Cinemachine;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonComponent<GameManager>
{
    [SerializeField] Camera m_mainCamera = default;
    [SerializeField] CinemachineBrain m_cinemachineBrain = default;

    [SerializeField] Player player = default;

    public Camera MainCamera => m_mainCamera;
    public CinemachineBrain CinemachineBrain => m_cinemachineBrain;

    public Player Player => player;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ChangeCamera(CinemachineVirtualCamera nextVirtualCamera)
    {

        StartCoroutine(ChangeCameraCo(nextVirtualCamera));
    }

    IEnumerator ChangeCameraCo(CinemachineVirtualCamera nextVirtualCamera)
    {
        yield return null;
        if (nextVirtualCamera.gameObject == m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject) yield return null;
        m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        nextVirtualCamera.gameObject.SetActive(true);

    }
  }
