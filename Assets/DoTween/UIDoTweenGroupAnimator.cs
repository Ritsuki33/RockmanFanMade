using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIDoTweenGroupAnimator : MonoBehaviour
{
    [SerializeField] private List<SeqItem> m_openSeq;
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
        public abstract void SetOpenSequence(Sequence sequence);
        public abstract void SetCloseSequence(Sequence sequence);
    }

    [Serializable]
    class JoinSeq : BaseSeq
    {
        [SerializeField] public UIDoTweemAnimator m_Animator;

        public override void SetOpenSequence(Sequence sequence)
        {
            sequence.Join(m_Animator.CreateOpenSequence()).SetDelay(delay);
        }

        public override void SetCloseSequence(Sequence sequence)
        {
            sequence.Join(m_Animator.CreateCloseSequence()).SetDelay(delay);
        }
    }

    [Serializable]
    class AppendSeq : BaseSeq
    {
        [SerializeField] public UIDoTweemAnimator m_Animator;
        public override void SetOpenSequence(Sequence sequence)
        {
            sequence.Append(m_Animator.CreateOpenSequence()).SetDelay(delay);
        }

        public override void SetCloseSequence(Sequence sequence)
        {
            sequence.Append(m_Animator.CreateCloseSequence()).SetDelay(delay);
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

    public void PlayOpen(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();

        foreach (var item in m_openSeq)
        {
            item.SetOpenSequence(sequence);
        }

        sequence.Play().onComplete += () => { finishCallback?.Invoke(); };
    }

    public void PlayClose(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();

        foreach (var item in m_closeSeq)
        {
            item.SetCloseSequence(sequence);
        }

        sequence.Play().onComplete += () => { finishCallback?.Invoke(); };
    }

    public void Test()
    {
        Debug.Log("Test");
    }
}
