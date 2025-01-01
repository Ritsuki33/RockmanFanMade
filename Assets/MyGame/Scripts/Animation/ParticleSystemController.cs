using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : ModelController
{
    [SerializeField]private ParticleSystem m_particleSystem;

    public override float Speed
    {
        get => m_particleSystem.main.startSpeedMultiplier;
        set
        {
            var ma = m_particleSystem.main;
            ma.startSpeedMultiplier = value;
        }
    }

    public override bool IsPlaying(int hash)
    {
        return m_particleSystem.IsAlive();
    }

    public override void OnPause(bool IsPause)
    {
        m_particleSystem.Stop();
    }

    public override void OnPlay(int hash)
    {
        m_particleSystem.Play();
    }
}
