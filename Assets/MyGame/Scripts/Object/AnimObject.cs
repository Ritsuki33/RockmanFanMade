using UnityEngine;

/// <summary>
/// アニメーションを持つオブジェクト
/// </summary>
public class AnimObject : BaseObject
{
    [SerializeField] private Animator m_mainAnimator;
    public Animator MainAnimator => m_mainAnimator;

    float cacheAnimSpeed = 0f;
    private Material _mainMaterial;

    public Material MainMaterial
    {
        get
        {
            if (m_mainAnimator != null && _mainMaterial == null)
            {
                _mainMaterial = m_mainAnimator?.gameObject.GetComponent<Renderer>().material;
            }

            return _mainMaterial;
        }
    }

    protected virtual void Awake()
    {
        if (m_mainAnimator != null) _mainMaterial = m_mainAnimator.gameObject.GetComponent<Renderer>().material;
    }

    protected override void Init()
    {
        if (m_mainAnimator != null) cacheAnimSpeed = m_mainAnimator.speed;
        base.Init();
    }

    protected override void OnPause(bool isPause)
    {

        if (m_mainAnimator == null) return;

        if (isPause)
        {
            if (!IsPause)
            {
                cacheAnimSpeed = m_mainAnimator.speed;
                m_mainAnimator.speed = 0.0f;
            }
        }
        else
        {
            m_mainAnimator.speed = cacheAnimSpeed;
        }

        base.OnPause(isPause);
    }
}
