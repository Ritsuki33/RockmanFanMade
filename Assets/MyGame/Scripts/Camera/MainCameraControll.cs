﻿using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class MainCameraControll : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] CinemachineBrain m_cinemachineBrain = default;

    [SerializeField, Range(0, 1)] float outOfViewOffset = 1.0f;
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

    public void OnReset()
    {
        // アクティブによるオンオフによる切り替え
        if (m_cinemachineBrain.ActiveVirtualCamera != null)
        {
            m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
            m_cinemachineBrain.ActiveVirtualCamera.Follow = null;
        }
    }

    /// <summary>
    /// カメラの変更
    /// </summary>
    /// <param name="virtualCamera"></param>
    /// <param name="style"></param>
    /// <param name="blendTime"></param>
    /// <param name="finishCallback"></param>
    public void ChangePlayerCamera(CinemachineVirtualCamera virtualCamera, CinemachineBlendDefinition.Style style, float blendTime, Action finishCallback)
    {
        StartCoroutine(ChangeCameraCo());

        IEnumerator ChangeCameraCo()
        {
            if (virtualCamera != null && !Equal(virtualCamera))
            {
                m_cinemachineBrain.m_DefaultBlend.m_Style = style;
                m_cinemachineBrain.m_DefaultBlend.m_Time = blendTime;

                // カメラ変更開始を通知する
                EventTriggerManager.Instance.Notify(EventType.ChangeCameraStart);

                // アクティブによるオンオフによる切り替え
                if (m_cinemachineBrain.ActiveVirtualCamera != null)
                {
                    m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
                    m_cinemachineBrain.ActiveVirtualCamera.Follow = null;
                }

                virtualCamera.VirtualCameraGameObject.SetActive(true);
                virtualCamera.Follow = WorldManager.Instance.Player.transform;
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

            finishCallback?.Invoke();
        }
    }

    public bool Equal(CinemachineVirtualCamera virtualCamera) => (m_cinemachineBrain.ActiveVirtualCamera == null) ? false : virtualCamera.gameObject == m_cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;


    public enum OutOfViewType
    {
        None, Top, Bottom, Left, Right
    }

    public bool CheckInView(GameObject gameObject, float offset)
    {
        OutOfViewType outOfViewType = GetOutOfViewInfo(gameObject, offset);
        return outOfViewType != OutOfViewType.None;
    }

    public bool CheckOutOfView(GameObject gameObject)
    {
        OutOfViewType outOfViewType = GetOutOfViewInfo(gameObject);
        return outOfViewType != OutOfViewType.None;
    }

    public bool CheckOutOfView(GameObject gameObject, out OutOfViewType outOfViewType)
    {
        outOfViewType = GetOutOfViewInfo(gameObject);
        return outOfViewType != OutOfViewType.None;
    }

    private OutOfViewType GetOutOfViewInfo(GameObject gameObject, float offset = 0)
    {
        OutOfViewType outOfViewType = OutOfViewType.None;

        // 物体の位置をスクリーン座標に変換
        Vector3 screenPoint = _camera.WorldToViewportPoint(gameObject.transform.position);

        // ビュー範囲外かどうかを判定
        if (screenPoint.x + (outOfViewOffset - offset) < 0) outOfViewType = OutOfViewType.Left;
        else if (screenPoint.x - (outOfViewOffset - offset) > 1) outOfViewType = OutOfViewType.Right;
        else if (screenPoint.y + (outOfViewOffset - offset) < 0) outOfViewType = OutOfViewType.Bottom;
        else if (screenPoint.y - (outOfViewOffset - offset) > 1) outOfViewType = OutOfViewType.Top;

        return outOfViewType;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(this.transform.position, OutOfViewSize);
    }

}
