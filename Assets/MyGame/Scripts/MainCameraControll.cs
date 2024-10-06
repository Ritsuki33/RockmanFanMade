using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class MainCameraControll : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] CinemachineBrain m_cinemachineBrain = default;

    [SerializeField,Range(0,1)] float outOfViewOffset = 1.0f;
    public CinemachineBrain CinemachineBrain => m_cinemachineBrain;

    private Vector3 deltaMove = default;

    /// <summary>
    /// カメラの移動量
    /// </summary>
    public Vector3 DeltaMove => deltaMove;

    /// <summary>
    /// カメラ遷移中か
    /// </summary>
    public bool IsBlending => m_cinemachineBrain.IsBlending;

    public float Height => _camera.orthographicSize * 2;
    public float Width => Height * _camera.aspect;

    public Vector3 OutOfViewSize => new Vector3(Width * (1 + outOfViewOffset * 2), Height * (1 + outOfViewOffset * 2));

    public float OutOfViewLeft => transform.position.x - OutOfViewSize.x / 2;
    public float OutOfViewRight => transform.position.x + OutOfViewSize.x / 2;

    public float OutOfViewBottom => transform.position.y - OutOfViewSize.y / 2;
    public float OutOfViewTop => transform.position.y + OutOfViewSize.y / 2;

    public CinemachineVirtualCamera CurrrentVirtualCamera => m_cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;

    /// <summary>
    /// カメラの変更
    /// </summary>
    /// <param name="virtualCamera"></param>
    /// <param name="style"></param>
    /// <param name="blendTime"></param>
    /// <param name="finishCallback"></param>
    public void ChangeCamera(CinemachineVirtualCamera virtualCamera, CinemachineBlendDefinition.Style style,float blendTime, Action finishCallback)
    {
        StartCoroutine(ChangeCameraCo(virtualCamera, style, blendTime, finishCallback));
      
        IEnumerator ChangeCameraCo(CinemachineVirtualCamera nextVirtualCamera, CinemachineBlendDefinition.Style style, float blendTime, Action callback)
        {
            if (nextVirtualCamera != null && !Equal(nextVirtualCamera))
            {
                m_cinemachineBrain.m_DefaultBlend.m_Style = style;
                m_cinemachineBrain.m_DefaultBlend.m_Time = blendTime;

                // カメラ変更開始を通知する
                EventTriggerManager.Instance.Notify(EventType.ChangeCameraStart);

                // アクティブによるオンオフによる切り替え
                m_cinemachineBrain.ActiveVirtualCamera?.VirtualCameraGameObject.SetActive(false);
                virtualCamera.VirtualCameraGameObject.SetActive(true);

                // ブレンディングをスタートさせるため、次フレームまで待つ 
                yield return null;

                Vector3 pre_cameraPos = this.transform.position;
                while (m_cinemachineBrain.IsBlending)
                {
                    deltaMove = this.transform.position - pre_cameraPos;
                    pre_cameraPos = this.transform.position;
                    yield return null;
                }

                deltaMove = Vector3.zero;

                // カメラ変更終了を通知する
                EventTriggerManager.Instance.Notify(EventType.ChangeCameraEnd);
            }

            callback?.Invoke();
        }
    }

    public bool Equal(CinemachineVirtualCamera virtualCamera) => (m_cinemachineBrain.ActiveVirtualCamera == null) ? false : virtualCamera.gameObject == m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;

    public bool CheckOutOfView(GameObject gameObject)
    {
        // 物体の位置をスクリーン座標に変換
        Vector3 screenPoint = _camera.WorldToViewportPoint(gameObject.transform.position);

        // ビュー範囲外かどうかを判定
        bool isOutOfView = screenPoint.x + outOfViewOffset < 0 || screenPoint.x - outOfViewOffset > 1 || screenPoint.y + outOfViewOffset < 0 || screenPoint.y - outOfViewOffset > 1;

        return isOutOfView;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(this.transform.position, OutOfViewSize);
    }

}
