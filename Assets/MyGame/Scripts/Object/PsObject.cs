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

    protected override void OnPause(bool isPause)
    {
       if(isPause) m_particleSystem.Stop();
        else m_particleSystem.Play();
    }
}
