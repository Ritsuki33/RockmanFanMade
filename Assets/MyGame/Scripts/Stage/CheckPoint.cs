using Cinemachine;
using System;
using UnityEngine;

[Serializable]
public struct CheckPointData
{
    public Transform position;
    public CinemachineVirtualCamera virtualCamera;
}

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPointData data = new CheckPointData();
        data.position = this.transform;
        data.virtualCamera = GameMainManager.Instance.MainCameraControll.CurrrentVirtualCamera;

        // CheckPoint
        WorldManager.Instance.SaveCheckPoint(data);
    }
}
