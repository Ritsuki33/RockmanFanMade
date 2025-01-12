using System;
using UnityEngine;

public class PsObject : BaseObject
{
    [SerializeField] private ParticleSystem m_particleSystem;

    public ParticleSystem ParticleSystem => m_particleSystem;

    private Material _material;

    Action<PsObject> _finishedcallback;
    public  Material Material
    {
        get
        {
            if (_material == null)
            {
                m_particleSystem.gameObject.GetComponent<Renderer>().material = _material;
            }

            return _material;
        }
    }

    public void Setup(Action<PsObject> finishedcallback)
    {
        _finishedcallback = finishedcallback;
    }

    protected override void OnUpdate()
    {
        if (!m_particleSystem.isPlaying)
        {
            _finishedcallback?.Invoke(this);
        }
    }
    protected override void OnPause(bool isPause)
    {
       if(isPause) m_particleSystem.Pause();
        else m_particleSystem.Play();
    }

    protected override void OnReset()
    {
        _finishedcallback?.Invoke(this);
    }
}
