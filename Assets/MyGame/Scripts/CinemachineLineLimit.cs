using Cinemachine;
using UnityEngine;

public class CinemachineLineLimit : CinemachineExtension
{
    // �������ʂ�_
    [SerializeField]private Vector3 _origin = Vector3.up;

    // �����̌���
    [SerializeField] private Vector3 _direction = Vector3.right;

    private float _scrollRange = 0;

    [SerializeField] CameraControllArea _controlArea;
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetUp(Vector2 origin,Vector2 direction,float scrollRange)
    {
        _origin = origin;
        _direction = direction;
        _scrollRange = scrollRange;

    }

    // Extension�R�[���o�b�N
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        // �J�����ړ���̂ݏ��������s���邱�ƂƂ���
        if (stage != CinemachineCore.Stage.Body)
            return;

        // ���C���`
        var ray = new Ray(_controlArea.StartCameraCneter, _controlArea.Direction);
        // �v�Z���ꂽ�J�����ʒu
        var point = state.RawPosition;

        // ���C��ɓ��e�����J�����ʒu���v�Z
        point -= ray.origin;
        point = Vector3.Project(point, ray.direction);
        point += ray.origin;

        float max_x = 0;
        float min_x = 0;
        float max_y = 0;
        float min_y = 0;
        if (_controlArea.StartCameraCneter.x <= _controlArea.EndCameraCenter.x)
        {
            min_x = _controlArea.StartCameraCneter.x;
            max_x = _controlArea.EndCameraCenter.x;
        }
        else
        {
            min_x = _controlArea.EndCameraCenter.x;
            max_x = _controlArea.StartCameraCneter.x; 
        }

        if (_controlArea.StartCameraCneter.y <= _controlArea.EndCameraCenter.y)
        {
            min_y = _controlArea.StartCameraCneter.y;
            max_y = _controlArea.EndCameraCenter.y;
        }
        else
        {
            min_y = _controlArea.EndCameraCenter.y;
            max_y = _controlArea.StartCameraCneter.y;
        }

        point.x = Mathf.Clamp(point.x,min_x, max_x);
        point.y = Mathf.Clamp(point.y,min_y, max_y);
        // ���e�_���J�����ʒu�ɔ��f
        state.RawPosition = point;
    }

    #region DrawGizmos

    private const float GizmoLineLength = 1000;

    // �ړ��͈͂��G�f�B�^��ŕ\��(�m�F�p)
    //private void OnDrawGizmos()
    //{
    //    if (!isActiveAndEnabled) return;

    //    var ray = new Ray(_origin, _direction);

    //    Gizmos.DrawRay(
    //        ray.origin - ray.direction * GizmoLineLength / 2,
    //        ray.direction * GizmoLineLength
    //    );
    //}

    #endregion
}