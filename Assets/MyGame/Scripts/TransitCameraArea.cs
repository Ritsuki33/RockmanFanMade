using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitCameraArea : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_nextVirtualCamera = default;
    [SerializeField] private BoxCollider2D transitArea;


    public BoxCollider2D TransitArea => transitArea;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.ChangeCamera(m_nextVirtualCamera);
        
    }
   
}
