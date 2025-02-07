using System;
using UnityEngine;

public class PsObject : BaseObject
{
    [SerializeField] private ParticleSystem m_particleSystem;

    public ParticleSystem ParticleSystem => m_particleSystem;

    private Material _material;

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

    public void Setup(Vector3 position)
    {
        this.transform.position = position;
    }

    protected override void OnUpdate()
    {
        if (!m_particleSystem.isPlaying)
        {
            Delete();
        }
    }
    protected override void OnPause(bool isPause)
    {
       if(isPause) m_particleSystem.Pause();
        else m_particleSystem.Play();
    }
}
