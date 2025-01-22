using System;
using UnityEngine;
using UnityEngine.Pool;

public class Laser : BaseObject
{
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] BoxCollider2D m_boxCollider;
    [SerializeField] ScrollSpriteController scrollSpriteController;
    [SerializeField, Header("ターゲットレイヤー")] public LayerMask targetLayer;

    // スプライトの開始地点と終了地点を指定するための変数
    Vector2 startPoint;
    private Vector2 spriteOffset;

    bool isLaunch = false;

    Transform transform_start = null;
    float speed = 0;
    Vector2 laserDir = Vector2.zero;

    float currentLength = 0;
    float offset = 0;

    float offsetSpeed = 0;

    public bool IsLaunch => isLaunch;

    public IObjectPool<Laser> Pool { get; set; }

    Action deleteCallback;
    public void Launch(Transform start, float speed, Action deleteCallback, Vector2 laserDir = default, float offsetSpeed = 0)
    {
        transform_start = start;
        startPoint = transform_start.transform.position;
        this.speed = speed;
        this.laserDir = laserDir;
        this.offsetSpeed = offsetSpeed;
        this.deleteCallback = deleteCallback;

        offset = 0;
        isLaunch = true;
    }

    /// <summary>
    /// 停止する
    /// </summary>
    public void Cease()
    {
        isLaunch = false;
    }

    protected override void OnFixedUpdate()
    {
        Vector2 preStartPoint = startPoint;
        // 始点の決定
        if (isLaunch)
        {
            startPoint = transform_start.transform.position;
            currentLength += Time.fixedDeltaTime * speed;
            offset += Time.fixedDeltaTime * offsetSpeed;
            offset %= 1.0f;
        }
        else
        {
            startPoint += laserDir * Time.fixedDeltaTime * speed;
        }


        // 終点の決定
        Vector2 endPoint = startPoint + currentLength * laserDir;

        // 始点から終点までレイを飛ばして、衝突検出
        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint, targetLayer);
        if (hit)
        {
            endPoint = hit.point;
            currentLength = hit.distance;
        }

        hit = Physics2D.Linecast(preStartPoint, startPoint, targetLayer);
        if (hit)
        {
            Delete();
        }

        Shaped(startPoint, endPoint);
    }

    public void Launch(Transform start, float speed, Vector2 laserDir = default, float offsetSpeed = 0)
    {
        Launch(start, speed, Delete, laserDir, offsetSpeed);

    }

    public void Delete()
    {
        Pool.Release(this);
    }

    /// <summary>
    /// レーザー形成
    /// </summary>
    void Shaped(Vector2 startPoint, Vector2 endPoint)
    {
        // 開始地点と終了地点の中間点にスプライトを配置
        Vector2 middlePoint = (startPoint + endPoint) / 2;
        this.transform.position = new Vector3(middlePoint.x, middlePoint.y, -2);

        // 開始地点と終了地点の距離を計算して、スケールを調整
        float distance = Vector3.Distance(startPoint, endPoint);
        m_spriteRenderer.size = new Vector3(distance, m_spriteRenderer.size.y, m_spriteRenderer.transform.localScale.z);
        m_boxCollider.size = m_spriteRenderer.size;

        // 開始地点から終了地点に向けてスプライトが回転するように設定
        Vector3 direction = endPoint - startPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 回転をさせるため、スクロール方向はローカル（右前）で考慮してよい
        scrollSpriteController.Scroll(Vector2.left * offset);
    }
}
