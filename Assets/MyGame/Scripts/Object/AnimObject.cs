using UnityEngine;

/// <summary>
/// アニメーションを持つオブジェクト
/// </summary>
public class AnimObject : BaseObject
{
    [SerializeField] private Animator m_mainAnimator;
    public Animator MainAnimator => m_mainAnimator;

    float currentAnimSpeed = 0f;
    private Material _mainMaterial;

    public Material MainMaterial
    {
        get
        {
            if (_mainMaterial == null)
            {
                _mainMaterial = m_mainAnimator.gameObject.GetComponent<Renderer>().material;
            }

            return _mainMaterial;
        }
    }

    protected virtual void Awake()
    {
        _mainMaterial = m_mainAnimator.gameObject.GetComponent<Renderer>().material;
    }

    protected override void Init()
    {
        currentAnimSpeed = m_mainAnimator.speed;
    }

    protected override void OnPause(bool isPause)
    {
        if (isPause)
        {
            currentAnimSpeed = m_mainAnimator.speed;
            m_mainAnimator.speed = 0.0f;
        }
        else
        {
            m_mainAnimator.speed = currentAnimSpeed;
        }
    }
}
