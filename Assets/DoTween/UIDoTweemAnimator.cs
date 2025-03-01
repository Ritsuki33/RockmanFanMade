using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTweemAnimator : MonoBehaviour
{
    public abstract void PlayOpen(Action finishCallback = null);
    public abstract void PlayClose(Action finishCallback = null);

    public abstract void ResetStatus();
    public abstract Sequence CreateOpenSequence();
    public abstract Sequence CreateCloseSequence();
}

[RequireComponent(typeof(CanvasGroup))]
public class UIDoTweemAnimator : BaseTweemAnimator
{
    enum TweenType
    {
        Move,
        Rotate,
        Scale,
        Fade,
    }

    enum ConnectType
    {
        Join,
        Append
    }

    private void Awake()
    {
        CreateCache();
    }

    Sequence sequence = default;

    public override void PlayOpen(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = CreateOpenSequence();
        gameObject.SetActive(false);
        sequence.Play()
        .OnStart(() => gameObject.SetActive(true))
        .OnComplete(() => finishCallback?.Invoke())
        .OnKill(() => ResetStatus());
    }

    public override void PlayClose(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = CreateCloseSequence();

        sequence.Play()
        .OnStart(() => gameObject.SetActive(true))
        .OnComplete(() => finishCallback?.Invoke())
        .OnKill(() => { ResetStatus(); gameObject.SetActive(false); });
    }

    [SerializeField, Header("オープン　アニメーション")] List<TweenElement> openTweens = new List<TweenElement>();
    [SerializeField, Header("クローズ　アニメーション")] bool isReverse = false;
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

        public Tween Do(RectTransform rectTransform, CanvasGroup canvasGroup)
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    if (tween is MoveTween)
                    {
                        var moveTween = tween as MoveTween;
                        return moveTween.Do(rectTransform);
                    }
                    break;
                case TweenType.Rotate:
                    if (tween is RotateTween)
                    {
                        var rotateTween = tween as RotateTween;
                        return rotateTween.Do(rectTransform);
                    }
                    break;
                case TweenType.Scale:
                    if (tween is ScaleTween)
                    {
                        var scaleTween = tween as ScaleTween;
                        return scaleTween.Do(rectTransform);
                    }
                    break;
                case TweenType.Fade:
                    if (tween is FadeTween)
                    {
                        var fadeTween = tween as FadeTween;
                        return fadeTween.Do(canvasGroup);
                    }
                    break;
            }
            return null;
        }
        public Tween Reverse(RectTransform rectTransform, CanvasGroup canvasGroup)
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    if (tween is MoveTween)
                    {
                        var moveTween = tween as MoveTween;
                        return moveTween.Reverse(rectTransform);
                    }
                    break;
                case TweenType.Rotate:
                    if (tween is RotateTween)
                    {
                        var rotateTween = tween as RotateTween;
                        return rotateTween.Reverse(rectTransform);
                    }
                    break;
                case TweenType.Scale:
                    if (tween is ScaleTween)
                    {
                        var scaleTween = tween as ScaleTween;
                        return scaleTween.Reverse(rectTransform);
                    }
                    break;
                case TweenType.Fade:
                    if (tween is FadeTween)
                    {
                        var fadeTween = tween as FadeTween;
                        return fadeTween.Reverse(canvasGroup);
                    }
                    break;
            }

            return null;
        }

        public void OnValidate(RectTransform _rectTransform, CanvasGroup _canvasGroup)
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    if (tween is MoveTween) return;
                    tween = new MoveTween();
                    var moveTween = tween as MoveTween;
                    moveTween.OnValidate(_rectTransform);
                    break;
                case TweenType.Rotate:
                    if (tween is RotateTween) return;
                    tween = new RotateTween();
                    var rotateTween = tween as RotateTween;
                    rotateTween.OnValidate(_rectTransform);
                    break;
                case TweenType.Scale:
                    if (tween is ScaleTween) return;
                    tween = new ScaleTween();
                    var scaleTween = tween as ScaleTween;
                    scaleTween.OnValidate(_rectTransform);
                    break;
                case TweenType.Fade:
                    if (tween is FadeTween) return;
                    tween = new FadeTween();
                    var fadeTween = tween as FadeTween;
                    fadeTween.OnValidate(_canvasGroup);
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
    }

    [Serializable]
    class MoveTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] public Vector3 endValue;

        public void OnValidate(RectTransform _rectTransform)
        {
            startValue = _rectTransform.anchoredPosition;
            endValue = _rectTransform.anchoredPosition;
        }
        public Tween Do(RectTransform _rectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOAnchorPos(startValue, 0));
            sequence.Append(_rectTransform.DOAnchorPos(endValue, duration).SetEase(ease).SetDelay(delay));

            return sequence;
        }

        public Tween Reverse(RectTransform _rectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOAnchorPos(endValue, 0));
            sequence.Append(_rectTransform.DOAnchorPos(startValue, duration).SetEase(ease).SetDelay(delay));

            return sequence;
        }
    }

    [Serializable]
    class RotateTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] public Vector3 endValue;
        [SerializeField] RotateMode rotateMode;

        public void OnValidate(RectTransform _rectTransform)
        {
            startValue = _rectTransform.localRotation.eulerAngles;
            endValue = _rectTransform.localRotation.eulerAngles;
        }

        public Tween Do(RectTransform _rectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DORotate(startValue, 0));
            sequence.Append(_rectTransform.DORotate(endValue, duration, rotateMode).SetEase(ease).SetDelay(delay));
            return sequence;
        }
        public Tween Reverse(RectTransform _rectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DORotate(endValue, 0));
            sequence.Append(_rectTransform.DORotate(startValue, duration).SetEase(ease).SetDelay(delay));

            return sequence;
        }
    }

    [Serializable]
    class ScaleTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] public Vector3 endValue;

        public void OnValidate(RectTransform _rectTransform)
        {
            startValue = _rectTransform.localScale;
            endValue = _rectTransform.localScale;
        }

        public Tween Do(RectTransform _rectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOScale(startValue, 0));
            sequence.Append(_rectTransform.DOScale(endValue, duration).SetEase(ease).SetDelay(delay));
            return sequence;
        }

        public Tween Reverse(RectTransform _rectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOScale(endValue, 0));
            sequence.Append(_rectTransform.DOScale(startValue, duration).SetEase(ease).SetDelay(delay));

            return sequence;
        }
    }

    [Serializable]
    class FadeTween : BaseTween
    {
        [SerializeField] public float startValue;
        [SerializeField] public float endValue;

        public void OnValidate(CanvasGroup _canvasGroup)
        {
            startValue = _canvasGroup.alpha;
            endValue = _canvasGroup.alpha;
        }

        public Tween Do(CanvasGroup _canvasGroup)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(startValue, 0));
            sequence.Append(_canvasGroup.DOFade(endValue, duration).SetEase(ease).SetDelay(delay));
            return sequence;
        }

        public Tween Reverse(CanvasGroup _canvasGroup)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(endValue, 0));
            sequence.Append(_canvasGroup.DOFade(startValue, duration).SetEase(ease).SetDelay(delay));

            return sequence;
        }
    }

    private void OnValidate()
    {
        if (m_rectTransform == null) m_rectTransform = GetComponent<RectTransform>();
        if (m_canvasGroup == null) m_canvasGroup = GetComponent<CanvasGroup>();

        if (openTweens != null)
        {
            foreach (var tween in openTweens)
            {
                tween.OnValidate(m_rectTransform, m_canvasGroup);
            }
        }

        if (closeTweens != null)
        {
            foreach (var tween in closeTweens)
            {
                tween.OnValidate(m_rectTransform, m_canvasGroup);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweens"></param>
    /// <param name="isReverse"></param>
    /// <returns></returns>
    private Sequence CreateSequence(List<TweenElement> tweens, bool isReverse = false)
    {
        if (m_rectTransform == null) m_rectTransform = GetComponent<RectTransform>();
        if (m_canvasGroup == null) m_canvasGroup = GetComponent<CanvasGroup>();

        var seq = DOTween.Sequence();
        if (!isReverse)
        {
            foreach (var tween in tweens)
            {
                switch (tween.connectType)
                {
                    case ConnectType.Join:
                        seq.Join(tween.Do(m_rectTransform, m_canvasGroup));
                        break;
                    case ConnectType.Append:
                        seq.Append(tween.Do(m_rectTransform, m_canvasGroup));
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            foreach (var tween in tweens)
            {
                switch (tween.connectType)
                {
                    case ConnectType.Join:
                        seq.Join(tween.Reverse(m_rectTransform, m_canvasGroup));
                        break;
                    case ConnectType.Append:
                        seq.Append(tween.Reverse(m_rectTransform, m_canvasGroup));
                        break;
                    default:
                        break;
                }
            }
        }
        return seq;
    }

    public override Sequence CreateOpenSequence() => CreateSequence(openTweens);
    public override Sequence CreateCloseSequence()
    {
        if (isReverse)
        {
            return CreateSequence(openTweens, isReverse);
        }
        else
        {
            return CreateSequence(closeTweens);
        }
    }


    private Vector3 cacheLocalPostion = default;
    private Vector3 cacheLocalScale = default;
    private Quaternion cacheLocalRatote = default;
    private float cacheFade = default;

    /// <summary>
    /// 要素の位置を保存
    /// </summary>
    public void CreateCache()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        cacheLocalPostion = m_rectTransform.anchoredPosition;
        cacheLocalRatote = m_rectTransform.localRotation;
        cacheLocalScale = m_rectTransform.localScale;
        cacheFade = m_canvasGroup.alpha;
    }

    public override void ResetStatus()
    {
        m_rectTransform.anchoredPosition = cacheLocalPostion;
        m_rectTransform.localRotation = cacheLocalRatote;
        m_rectTransform.localScale = cacheLocalScale;
        m_canvasGroup.alpha = cacheFade;
    }
}