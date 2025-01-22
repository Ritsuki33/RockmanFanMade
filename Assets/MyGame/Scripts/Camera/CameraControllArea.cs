using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllArea : MonoBehaviour
{
    enum DirType
    {
        Right, Left, Up, Down
    }

    [SerializeField] DirType _dirType;
    [SerializeField, Header("可動域")] float scrollRange = 1.0f;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject transitCameraAreaPrefab;
    [SerializeField] Transform transitCameraAreaRoot;
    [SerializeField] List<TransitCameraArea> transitCameraAreas;

    private Camera _camera;
    CinemachineLineLimit _limit;

    public CinemachineVirtualCamera VirtualCamera => virtualCamera;

    public Vector3 Direction
    {
        get
        {
            switch (_dirType)
            {
                case DirType.Right:
                    return Vector2.right;
                case DirType.Left:
                    return Vector2.left;
                case DirType.Up:
                    return Vector2.up;
                case DirType.Down:
                    return Vector2.down;
                default:
                    return Vector2.zero;
            }
        }
    }

    public Vector3 StartCameraCneter => this.transform.position;

    public Vector3 EndCameraCenter => StartCameraCneter + Direction * scrollRange;

    public void SetFollowTargetObject(Transform position)
    {

    }

    private void AddTransitCameraArea()
    {
        var obj = Instantiate(transitCameraAreaPrefab, transitCameraAreaRoot).GetComponent<TransitCameraArea>();
        transitCameraAreas.Add(Instantiate(transitCameraAreaPrefab, transitCameraAreaRoot).GetComponent<TransitCameraArea>());
    }

    private void OnDrawGizmos()
    {
        if (_camera == null) _camera = Camera.main;

        float height = virtualCamera.m_Lens.OrthographicSize * 2;
        float width = height * _camera.aspect;

        Gizmos.color = Color.red;

        Vector3 cameraSize = new Vector3(width, height);

        Vector3 startLeftUp = StartCameraCneter + new Vector3(-cameraSize.x / 2, cameraSize.y / 2);
        Vector3 startLeftDown = StartCameraCneter + new Vector3(-cameraSize.x / 2, -cameraSize.y / 2);
        Vector3 startRightTop = StartCameraCneter + new Vector3(cameraSize.x / 2, cameraSize.y / 2);
        Vector3 startRightDown = StartCameraCneter + new Vector3(cameraSize.x / 2, -cameraSize.y / 2);


        Vector3 endLeftUp = startLeftUp + Direction * scrollRange;
        Vector3 endLeftDown = startLeftDown + Direction * scrollRange;
        Vector3 endRightTop = startRightTop + Direction * scrollRange;
        Vector3 endRightDown = startRightDown + Direction * scrollRange;

        Gizmos.DrawWireCube(StartCameraCneter, cameraSize);
        Gizmos.DrawWireCube(EndCameraCenter, cameraSize);

        Gizmos.DrawLine(startLeftUp, endLeftUp);
        Gizmos.DrawLine(startLeftDown, endLeftDown);
        Gizmos.DrawLine(startRightTop, endRightTop);
        Gizmos.DrawLine(startRightDown, endRightDown);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(StartCameraCneter, 0.5f);
        Gizmos.DrawLine(StartCameraCneter, EndCameraCenter);
        Gizmos.DrawSphere(EndCameraCenter, 0.5f);


        foreach (var transitCameraArea in transitCameraAreas)
        {
            if (!transitCameraArea.gameObject.activeSelf) continue;
            Vector2 center = (Vector2)transitCameraArea.TransitArea.gameObject.transform.position + transitCameraArea.TransitArea.offset;
            Vector2 size = (Vector2)transitCameraArea.TransitArea.size;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
