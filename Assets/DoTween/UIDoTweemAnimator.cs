using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDoTweemAnimator : MonoBehaviour
{
    enum TweenType
    {
        Move,
        Rotate,
        Scale,
        Fade,
    }

    public enum ConnectType
    {
        Join,
        Append
    }

    public void PlayOpen(Action finishCallback = null)
    {
        var seq = CreateOpenSequence();

        seq.Play().onComplete += () =>
        {
            finishCallback?.Invoke();
        };
    }

    public void PlayClose(Action finishCallback = null)
    {
        var seq = CreateCloseSequence();

        seq.Play().onComplete += () =>
        {
            finishCallback?.Invoke();
        };
    }

    [SerializeField] List<TweenElement> openTweens = new List<TweenElement>();
    [SerializeField] List<TweenElement> closeTweens = new List<TweenElement>();

    RectTransform m_rectTransform = default;
    CanvasGroup m_canvasGroup = default;

    [Serializable]
    class TweenElement
    {
        [SerializeField] float interval;
        [SerializeField] public ConnectType connectType;
        [SerializeField] TweenType tweenType;
        [SerializeReference] BaseTween tween;

        public Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup) => tween.Do(_rectTransform, m_canvasGroup);
        public void OnValidate()
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    if (tween is MoveTween) return;
                    tween = new MoveTween();
                    break;
                case TweenType.Rotate:
                    if (tween is RotateTween) return;
                    tween = new RotateTween();
                    break;
                case TweenType.Scale:
                    if (tween is ScaleTween) return;
                    tween = new ScaleTween();
                    break;
                case TweenType.Fade:
                    if (tween is FadeTween) return;
                    tween = new FadeTween();
                    break;
                default:
                    break;
            }
        }
    }

    abstract class BaseTween
    {
        [SerializeField] public float delay;
        [SerializeField] public float duration = 1.0f;
        [SerializeField] public Ease ease = Ease.OutQuad;
        abstract public Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup);
    }

    [Serializable]
    class MoveTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] public Vector3 endValue;

        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOAnchorPos(startValue, 0));
            sequence.Append(_rectTransform.DOAnchorPos(endValue, duration).SetEase(ease).SetDelay(delay));

            return sequence;
        }
    }

    [Serializable]
    class RotateTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] public Vector3 endValue;
        [SerializeField] RotateMode rotateMode;

        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DORotate(startValue, 0));
            sequence.Append(_rectTransform.DORotate(endValue, duration, rotateMode).SetEase(ease).SetDelay(delay));
            return sequence;
        }

    }

    [Serializable]
    class ScaleTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] public Vector3 endValue;

        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOScale(startValue, 0));
            sequence.Append(_rectTransform.DOScale(endValue, duration).SetEase(ease));
            return sequence;
        }
    }

    [Serializable]
    class FadeTween : BaseTween
    {
        [SerializeField] public float startValue;
        [SerializeField] public float endValue;
        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(m_canvasGroup.DOFade(startValue, 0));
            sequence.Append(m_canvasGroup.DOFade(endValue, duration).SetEase(ease));
            return sequence;
        }
    }

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnValidate()
    {
        if (openTweens != null)
        {
            foreach (var tween in openTweens)
            {
                tween.OnValidate();
            }
        }

        if (closeTweens != null)
        {
            foreach (var tween in closeTweens)
            {
                tween.OnValidate();
            }
        }
    }

    Sequence sequence = default;

    /// <summary>
    /// シーケンスの作成
    /// </summary>
    /// <param name="finishCallback"></param>
    private Sequence CreateSequence(List<TweenElement> tweens)
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();

        foreach (var tween in tweens)
        {
            switch (tween.connectType)
            {
                case ConnectType.Join:
                    sequence.Join(tween.Do(m_rectTransform, m_canvasGroup));
                    break;
                case ConnectType.Append:
                    sequence.Append(tween.Do(m_rectTransform, m_canvasGroup));
                    break;
                default:
                    break;
            }
        }

        return sequence;
    }

    public Sequence CreateOpenSequence() => CreateSequence(openTweens);
    public Sequence CreateCloseSequence() => CreateSequence(closeTweens);
}