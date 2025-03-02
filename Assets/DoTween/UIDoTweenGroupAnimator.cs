using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIDoTweenGroupAnimator : BaseTweemAnimator
{
    [SerializeField, Header("オープン　アニメーション")] private List<SeqItem> m_openSeq;

    [SerializeField, Header("クローズ　アニメーション")] private bool isReverse;
    [SerializeField] private List<SeqItem> m_closeSeq;

    Sequence sequence = default;

    public enum SeqType
    {
        Join,
        Append,
        AppendCallback,
        JoinCallback
    }

    [Serializable]
    class SeqItem
    {
        [SerializeField] public SeqType m_ConnectType;
        [SerializeReference] public BaseSeq baseSeq;
        public void ResetStatus() => baseSeq.ResetStatus();

        public void OnValidate()
        {
            switch (m_ConnectType)
            {
                case SeqType.Join:
                    if (baseSeq is JoinSeq) return;
                    baseSeq = new JoinSeq();
                    break;
                case SeqType.Append:
                    if (baseSeq is AppendSeq) return;
                    baseSeq = new AppendSeq();
                    break;
                case SeqType.AppendCallback:
                    if (baseSeq is AppendCallbackSeq) return;
                    baseSeq = new AppendCallbackSeq();
                    break;
                case SeqType.JoinCallback:
                    if (baseSeq is AppendCallbackSeq) return;
                    baseSeq = new AppendCallbackSeq();
                    break;
            }
        }

        public void SetOpenSequence(Sequence sequence) => baseSeq.SetOpenSequence(sequence);
        public void SetCloseSequence(Sequence sequence) => baseSeq.SetCloseSequence(sequence);
    }

    [Serializable]
    abstract class BaseSeq
    {
        [SerializeField] public float delay;
        public virtual void ResetStatus() { }
        public abstract void SetOpenSequence(Sequence sequence);
        public abstract void SetCloseSequence(Sequence sequence);
    }

    [Serializable]
    class JoinSeq : BaseSeq
    {
        [SerializeField] public BaseTweemAnimator m_Animator;

        public override void ResetStatus() => m_Animator.ResetStatus();

        public override void SetOpenSequence(Sequence sequence)
        {
            m_Animator.gameObject.SetActive(false);
            sequence.Join
            (
                m_Animator.CreateOpenSequence()
                .PrependCallback(() => m_Animator.gameObject.SetActive(true))
                .SetDelay(delay)
            );
        }

        public override void SetCloseSequence(Sequence sequence)
        {
            m_Animator.gameObject.SetActive(true);
            sequence.Join(
                m_Animator.CreateCloseSequence()
                .SetDelay(delay)
                .AppendCallback(() => m_Animator.gameObject.SetActive(false))
                );
        }
    }

    [Serializable]
    class AppendSeq : BaseSeq
    {
        [SerializeField] public BaseTweemAnimator m_Animator;
        public override void ResetStatus() => m_Animator.ResetStatus();

        public override void SetOpenSequence(Sequence sequence)
        {
            m_Animator.gameObject.SetActive(false);
            sequence.Append
            (
                m_Animator.CreateOpenSequence()
                .PrependCallback(() => m_Animator.gameObject.SetActive(true))
                .SetDelay(delay)
            );
        }

        public override void SetCloseSequence(Sequence sequence)
        {
            m_Animator.gameObject.SetActive(true);
            sequence.Append(
                m_Animator.CreateCloseSequence()
                .SetDelay(delay)
                .AppendCallback(() => m_Animator.gameObject.SetActive(false))
                );
        }
    }

    [Serializable]
    class AppendCallbackSeq : BaseSeq
    {
        [SerializeField] public UnityEvent m_Event;
        public override void SetOpenSequence(Sequence sequence)
        {
            sequence.AppendCallback(() => { m_Event.Invoke(); }).SetDelay(delay);
        }

        public override void SetCloseSequence(Sequence sequence)
        {
            sequence.AppendCallback(() => { m_Event.Invoke(); }).SetDelay(delay);
        }
    }

    [Serializable]
    class JoinCallbackSeq : BaseSeq
    {
        [SerializeField] public UnityEvent m_Event;
        public override void SetOpenSequence(Sequence sequence)
        {
            sequence.JoinCallback(() => { m_Event.Invoke(); }).SetDelay(delay);
        }

        public override void SetCloseSequence(Sequence sequence)
        {
            sequence.JoinCallback(() => { m_Event.Invoke(); }).SetDelay(delay);
        }
    }

    private void OnValidate()
    {
        if (m_openSeq != null)
        {
            foreach (var item in m_openSeq)
            {
                item.OnValidate();
            }
        }

        if (m_closeSeq != null)
        {
            foreach (var item in m_closeSeq)
            {
                item.OnValidate();
            }
        }
    }

    public override Sequence CreateOpenSequence()
    {
        var sequence = DOTween.Sequence();
        foreach (var item in m_openSeq)
        {
            item.SetOpenSequence(sequence);
        }
        return sequence;
    }

    public override Sequence CreateCloseSequence()
    {
        var sequence = DOTween.Sequence();

        if (isReverse)
        {
            foreach (var item in m_openSeq)
            {
                item.SetCloseSequence(sequence);
            }
        }
        else
        {
            foreach (var item in m_closeSeq)
            {
                item.SetCloseSequence(sequence);
            }
        }
        return sequence;
    }

    public override void PlayOpen(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = CreateOpenSequence();

        sequence.Play()
        .OnComplete(() => { finishCallback?.Invoke(); })
        .OnKill(() => { ResetStatus(); });
    }

    public override void PlayClose(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = CreateCloseSequence();

        sequence.Play()
        .OnComplete(() => { finishCallback?.Invoke(); })
        .OnKill(() => { ResetStatus(); });
    }

    public override void ResetStatus()
    {
        foreach (var item in m_openSeq)
        {
            item.ResetStatus();
        }
    }
}
