using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIDoTweenSelecterElement : MonoBehaviour
{
    [SerializeField] RectTransform m_rectTransform;
    [SerializeField] CanvasGroup m_canvasGroup;

    public RectTransform RectTransform => m_rectTransform;
    public CanvasGroup CanvasGroup => m_canvasGroup;

    private Vector3 cacheLocalPostion = default;
    private Vector3 cacheLocalScale = default;
    private Quaternion cacheLocalRatote = default;
    private float cacheFade = default;

    /// <summary>
    /// 各要素の位置を保存
    /// </summary>
    public void CreateCashe()
    {
        cacheLocalPostion = m_rectTransform.anchoredPosition;
        cacheLocalRatote = m_rectTransform.localRotation;
        cacheLocalScale = m_rectTransform.localScale;
        cacheFade = m_canvasGroup.alpha;
    }

    public void ResetStatus()
    {
        m_rectTransform.anchoredPosition = cacheLocalPostion;
        m_rectTransform.localRotation = cacheLocalRatote;
        m_rectTransform.localScale = cacheLocalScale;
        m_canvasGroup.alpha = cacheFade;
    }
}
