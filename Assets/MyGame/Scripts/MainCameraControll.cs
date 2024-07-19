using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControll : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] CinemachineBrain m_cinemachineBrain = default;

    [SerializeField,Range(0,1)] float outOfViewOffset = 1.0f;
    public Camera MainCamera => _camera;
    public CinemachineBrain CinemachineBrain => m_cinemachineBrain;

    public bool CheckOutOfView(GameObject gameObject)
    {
        // �e�ۂ̈ʒu���X�N���[�����W�ɕϊ�
        Vector3 screenPoint = _camera.WorldToViewportPoint(gameObject.transform.position);

        // �r���[�͈͊O���ǂ����𔻒�
        bool isOutOfView = screenPoint.x + outOfViewOffset < 0 || screenPoint.x - outOfViewOffset > 1 || screenPoint.y + outOfViewOffset < 0 || screenPoint.y - outOfViewOffset > 1;

        return isOutOfView;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        float height = _camera.orthographicSize * 2;
        float width = height * _camera.aspect;

        Vector3 cameraSize = new Vector3(width * (1 + outOfViewOffset * 2), height * (1 + outOfViewOffset * 2));
        Gizmos.DrawWireCube(this.transform.position, cameraSize);
    }
}
