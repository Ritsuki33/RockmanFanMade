using UnityEngine;
using Cinemachine;

public class CameraBlendDetector : MonoBehaviour
{
    private CinemachineBrain _cinemachineBrain;

    private void Start()
    {
        _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        _cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }

    private void OnDestroy()
    {
        if (_cinemachineBrain != null)
        {
            _cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
        }
    }

    private void OnCameraActivated(ICinemachineCamera fromCamera, ICinemachineCamera toCamera)
    {
        if (_cinemachineBrain.IsBlending)
        {
            StartCoroutine(CheckBlendCompletion());
        }
    }

    private System.Collections.IEnumerator CheckBlendCompletion()
    {
        while (_cinemachineBrain.IsBlending)
        {
            yield return null;
        }

        Debug.Log("Camera blend completed!");
        // ここでブレンド完了後の処理を行う
    }
}