using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererSpriteTiling : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float textureScale = 1f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // LineRendererの設定
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        // Texture ModeをTileに設定
        lineRenderer.textureMode = LineTextureMode.Tile;

        // 距離に応じてテクスチャをスケール
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        lineRenderer.material.mainTextureScale = new Vector2(distance / textureScale, 1);
    }

    void Update()
    {
        // 動的に位置が変わる場合も対応
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        float distance = Vector3.Distance(startPoint.position, endPoint.position);
    }
}