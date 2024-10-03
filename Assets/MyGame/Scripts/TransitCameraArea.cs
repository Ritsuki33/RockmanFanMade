using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitCameraArea : MonoBehaviour
{
    [SerializeField] private BoxCollider2D transitArea;
    [SerializeField] CameraControllArea nextCameraControllArea;

    [SerializeField]ActionChainExecuter eventController;
    public BoxCollider2D TransitArea => transitArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.MainCameraControll.Equal(nextCameraControllArea.VirtualCamera))
        {
            eventController?.StartEvent();
        }
    }
}
