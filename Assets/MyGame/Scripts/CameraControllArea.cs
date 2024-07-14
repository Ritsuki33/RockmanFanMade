//using Cinemachine;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class CameraControllArea : MonoBehaviour
//{
//    [SerializeField] BoxCollider2D cameraCollider;
//    [SerializeField] CinemachineVirtualCamera virtualCamera;
//    [SerializeField] List<TransitCameraArea> transitCameraAreas;
//    [SerializeField] GameObject transitCameraAreaPrefab;
//    [SerializeField] Transform transitCameraAreaRoot;
//    [SerializeField] bool drawGizmos = true;


//    Camera cam=null;
//    private void AddTransitCameraArea()
//    {
//        transitCameraAreas.Add(Instantiate(transitCameraAreaPrefab, transitCameraAreaRoot).GetComponent<TransitCameraArea>());
//    }

//    private void OnDrawGizmos()
//    {
//        if (!drawGizmos) return;
//        Vector2 center = (Vector2)cameraCollider.gameObject.transform.position + cameraCollider.offset;
//        Vector2 size = (Vector2)cameraCollider.size;

//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireCube(center, size);

//        foreach (var transitCameraArea in transitCameraAreas)
//        {
//            center = (Vector2)transitCameraArea.TransitArea.gameObject.transform.position + transitCameraArea.TransitArea.offset;
//            size = (Vector2)transitCameraArea.TransitArea.size;
//        }

//        Gizmos.color = Color.green;
//        Gizmos.DrawWireCube(center, size);


//        // âºëzÉJÉÅÉâÇ©ÇÁé¿ç€ÇÃÉJÉÅÉâÇéÊìæ
//        if (cam == null) cam = Camera.main;
//    }
//}
