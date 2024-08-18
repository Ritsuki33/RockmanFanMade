using Cinemachine;
using UnityEngine;

public class CinemachineLineLimit : CinemachineExtension
{
    // 直線が通る点
    [SerializeField]private Vector3 _origin = Vector3.up;

    // 直線の向き
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

    // Extensionコールバック
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        // カメラ移動後のみ処理を実行することとする
        if (stage != CinemachineCore.Stage.Body)
            return;

        // レイを定義
        var ray = new Ray(_controlArea.StartCameraCneter, _controlArea.Direction);
        // 計算されたカメラ位置
        var point = state.RawPosition;

        // レイ上に投影したカメラ位置を計算
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
        // 投影点をカメラ位置に反映
        state.RawPosition = point;
    }

    #region DrawGizmos

    private const float GizmoLineLength = 1000;

    // 移動範囲をエディタ上で表示(確認用)
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