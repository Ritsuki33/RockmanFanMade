using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : ModelController
{
    [SerializeField] private Animator m_animator;

    float currentAnimSpeed = 0f;

    public override float Speed
    {
        get => m_animator.speed;
        set
        {
            m_animator.speed = value;
        }
    }

    public override bool IsPlaying(int hash)
    {
        return m_animator.IsPlayingCurrentAnimation(hash);
    }

    public override void OnPause(bool isPause)
    {
        if (isPause)
        {
            currentAnimSpeed = m_animator.speed;
            m_animator.speed = 0.0f;
        }
        else
        {
            m_animator.speed = currentAnimSpeed;
        }
    }

    public override void OnPlay(int hash)
    {
        m_animator.Play(hash);
    }
}
