using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIDoTweenSelecterAnimator : BaseTweemAnimator
{

    [SerializeField] private List<UIDoTweenSelecterElement> list = new List<UIDoTweenSelecterElement>();

    [SerializeField] private float space = 0.0f;
    [SerializeField] private float offsetStep = 0.0f;

    [SerializeField] float delay = 0.0f;

    void Awake()
    {
        CreateCache();
    }

    public void AutoRayout(bool isHorizonal)
    {

        if (isHorizonal)
        {
            // サイズの決定
            float width = 0, height = 0;

            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i];
                if (i == 0) height += obj.RectTransform.rect.height;
                if (i != list.Count - 1) height += offsetStep;

                width += obj.RectTransform.rect.width;
                if (i != list.Count - 1) width += space;
            }

            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            // 整列
            Vector2 arrangement = new Vector2(-width / 2, offsetStep * (list.Count - 1) / 2);   // 一番上端
            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i];
                if (i == 0) arrangement += new Vector2(obj.RectTransform.rect.width / 2, 0);
                else
                {
                    arrangement += new Vector2(obj.RectTransform.rect.width + space, -offsetStep);
                }
                obj.transform.localPosition = arrangement;
            }
        }
        else
        {
            // サイズの決定
            float width = 0, height = 0;

            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i];
                if (i == 0) width += obj.RectTransform.rect.width;
                if (i != list.Count - 1) width += offsetStep;

                height += obj.RectTransform.rect.height;
                if (i != list.Count - 1) height += space;
            }

            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            // 整列
            Vector2 arrangement = new Vector2(-offsetStep * (list.Count - 1) / 2, height / 2);   // 一番上端
            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i];
                if (i == 0) arrangement -= new Vector2(0, obj.RectTransform.rect.height / 2);
                else
                {
                    arrangement -= new Vector2(-offsetStep, obj.RectTransform.rect.height + space);
                }
                obj.transform.localPosition = arrangement;
            }
        }
    }

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

    [SerializeField, Header("オープン　アニメーション")] List<TweenElement> openTweens = new List<TweenElement>();

    [Serializable]
    class TweenElement
    {
        [SerializeField] public ConnectType connectType;
        [SerializeField] TweenType tweenType;
        [SerializeReference] BaseTween tween;

        public Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    if (tween is MoveTween)
                    {
                        var moveTween = tween as MoveTween;
                        return moveTween.Do(_rectTransform);
                    }

                    break;
                case TweenType.Rotate:
                    if (tween is RotateTween)
                    {
                        var rotateTween = tween as RotateTween;
                        return rotateTween.Do(_rectTransform);
                    }
                    break;
                case TweenType.Scale:
                    if (tween is ScaleTween)
                    {
                        var scaleTween = tween as ScaleTween;
                        return scaleTween.Do(_rectTransform);
                    }
                    break;
                case TweenType.Fade:
                    if (tween is FadeTween)
                    {
                        var fadeTween = tween as FadeTween;
                        return fadeTween.Do(m_canvasGroup);
                    }
                    break;
            }
            return null;
        }

        public Tween Reverse(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    if (tween is MoveTween)
                    {
                        var moveTween = tween as MoveTween;
                        return moveTween.Reverse(_rectTransform);
                    }

                    break;
                case TweenType.Rotate:
                    if (tween is RotateTween)
                    {
                        var rotateTween = tween as RotateTween;
                        return rotateTween.Reverse(_rectTransform);
                    }
                    break;
                case TweenType.Scale:
                    if (tween is ScaleTween)
                    {
                        var scaleTween = tween as ScaleTween;
                        return scaleTween.Reverse(_rectTransform);
                    }
                    break;
                case TweenType.Fade:
                    if (tween is FadeTween)
                    {
                        var fadeTween = tween as FadeTween;
                        return fadeTween.Reverse(m_canvasGroup);
                    }
                    break;
            }
            return null;
        }

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
    }


    [Serializable]
    class MoveTween : BaseTween
    {
        [SerializeField] public Vector3 offset;

        public Tween Do(RectTransform _rectTransform)
        {
            var tween = _rectTransform.DOAnchorPos(_rectTransform.localPosition + offset, duration).SetDelay(delay).From();
            return tween;
        }

        public Tween Reverse(RectTransform _rectTransform)
        {
            var tween = _rectTransform.DOAnchorPos(_rectTransform.localPosition + offset, duration).SetDelay(delay);
            return tween;
        }
    }

    [Serializable]
    class RotateTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] RotateMode rotateMode;

        public Tween Do(RectTransform _rectTransform)
        {
            return _rectTransform.DORotate(startValue, duration, rotateMode).SetEase(ease).SetDelay(delay).From();
        }

        public Tween Reverse(RectTransform _rectTransform)
        {
            var tween = _rectTransform.DORotate(startValue, duration, rotateMode).SetEase(ease).SetDelay(delay);
            return tween;
        }
    }

    [Serializable]
    class ScaleTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;

        public Tween Do(RectTransform _rectTransform)
        {
            var tween = _rectTransform.DOScale(startValue, duration).SetEase(ease).SetDelay(delay);
            return tween;
        }

        public Tween Reverse(RectTransform _rectTransform)
        {
            var tween = _rectTransform.DOScale(startValue, duration).SetEase(ease).SetDelay(delay);
            return tween;
        }
    }

    [Serializable]
    class FadeTween : BaseTween
    {
        [SerializeField] public float startValue;
        public Tween Do(CanvasGroup m_canvasGroup)
        {
            return m_canvasGroup.DOFade(startValue, duration).SetEase(ease).SetDelay(delay).From();
        }

        public Tween Reverse(CanvasGroup m_canvasGroup)
        {
            return m_canvasGroup.DOFade(startValue, duration).SetEase(ease).SetDelay(delay);
        }
    }

    Sequence sequence = default;

    public override void PlayOpen(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = CreateOpenSequence();

        sequence.Play()
        .OnStart(() => gameObject.SetActive(true))
        .OnComplete(() =>
        {
            finishCallback?.Invoke();
            sequence.Kill(true);
        })
        .OnKill(ResetPosition);
    }

    public override void PlayClose(Action finishCallback = null)
    {
        if (sequence != null) sequence.Kill(true);
        sequence = CreateCloseSequence();

        sequence.Play()
        .OnStart(() => gameObject.SetActive(true))
        .OnComplete(() =>
        {
            finishCallback?.Invoke();

        }).OnKill(() =>
        {
            ResetPosition();
            gameObject.SetActive(false);
        });
    }


    public Sequence CreateOpenSequence() => CreateSequence(false);

    public Sequence CreateCloseSequence() => CreateSequence(true);

    /// <summary>
    /// シーケンスの作成
    /// </summary>
    /// <param name="finishCallback"></param>
    private Sequence CreateSequence(bool isReverse)
    {
        var seq = DOTween.Sequence();
        float totalDelay = 0;
        foreach (var obj in list)
        {
            Sequence subSequence = DOTween.Sequence();
            var m_rectTransform = obj;
            var m_canvasGroup = obj.GetComponent<CanvasGroup>();

            if (subSequence != null) subSequence.Kill(true);
            subSequence = DOTween.Sequence();
            if (!isReverse)
            {
                foreach (var tween in openTweens)
                {
                    switch (tween.connectType)
                    {
                        case ConnectType.Join:
                            subSequence.Join(tween.Do(obj.RectTransform, obj.CanvasGroup));
                            break;
                        case ConnectType.Append:
                            subSequence.Append(tween.Do(obj.RectTransform, obj.CanvasGroup));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                foreach (var tween in openTweens)
                {
                    switch (tween.connectType)
                    {
                        case ConnectType.Join:
                            subSequence.Join(tween.Reverse(obj.RectTransform, obj.CanvasGroup));
                            break;
                        case ConnectType.Append:
                            subSequence.Append(tween.Reverse(obj.RectTransform, obj.CanvasGroup));
                            break;
                        default:
                            break;
                    }
                }
            }

            seq.Join(subSequence.SetDelay(totalDelay));
            totalDelay += delay;
        }

        return seq;
    }

    public void OnValidate()
    {
        foreach (var obj in openTweens)
        {
            obj.OnValidate();
        }
    }
    /// <summary>
    /// 各要素の位置を保存
    /// </summary>
    private void CreateCache()
    {
        foreach (var obj in list)
        {
            obj.CreateCashe();
        }
    }

    private void ResetPosition()
    {
        foreach (var obj in list)
        {
            obj.ResetStatus();
        }
    }
}

