using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIDoTweenSelecterAnimator : MonoBehaviour
{
    
    [SerializeField] private List<UIDoTweenSelecterElement> list = new List<UIDoTweenSelecterElement>();

    [SerializeField] private float space = 0.0f;
    [SerializeField] private float offsetStep = 0.0f;

    [SerializeField] float delay = 0.0f;

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
                    arrangement += new Vector2(obj.RectTransform.rect.width + space ,-offsetStep);
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

    [SerializeField] List<TweenElement> openTweens = new List<TweenElement>();
    [SerializeField] List<TweenElement> closeTweens = new List<TweenElement>();

    [Serializable]
    class TweenElement
    {
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
        [SerializeField] bool isFrom = true;
        [SerializeField] public Vector3 offset;

        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            var tween = _rectTransform.DOAnchorPos(_rectTransform.localPosition + offset, duration).SetDelay(delay);
            if (isFrom) tween = tween.From();
            return tween;
        }
    }

    [Serializable]
    class RotateTween : BaseTween
    {
        [SerializeField] public Vector3 startValue;
        [SerializeField] RotateMode rotateMode;

        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            return _rectTransform.DORotate(startValue, duration, rotateMode).SetEase(ease).SetDelay(delay).From();
        }
    }

    [Serializable]
    class ScaleTween : BaseTween
    {
        [SerializeField] bool isFrom = true;
        [SerializeField] public Vector3 startValue;

        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            var tween = _rectTransform.DOScale(startValue, duration).SetEase(ease).SetDelay(delay);
            if (isFrom) tween = tween.From();
            return tween;
        }
    }

    [Serializable]
    class FadeTween : BaseTween
    {
        [SerializeField] public float startValue;
        public override Tween Do(RectTransform _rectTransform, CanvasGroup m_canvasGroup)
        {
            return m_canvasGroup.DOFade(startValue, duration).SetEase(ease).SetDelay(delay).From();
        }
    }

    Sequence sequence = default;

    public void PlayOpen(Action finishCallback = null)
    {
        // 修正案:
        CreateCachePosition(); // スペル修正

        if (sequence != null) sequence.Kill(true);
        sequence = CreateOpenSequence();

        sequence.Play().OnComplete(() => finishCallback?.Invoke()); 
    }

    public void PlayClose(Action finishCallback = null)
    {

        if (sequence != null) sequence.Kill(true);
        sequence = CreateCloseSequence();

        sequence.Play().OnComplete(() =>
        {
            finishCallback?.Invoke();
            sequence.Kill(true);

        }).OnKill(ResetPosition);
    }


    public Sequence CreateOpenSequence() => CreateSequence(openTweens);
    public Sequence CreateCloseSequence() => CreateSequence(closeTweens);

    

    /// <summary>
    /// シーケンスの作成
    /// </summary>
    /// <param name="finishCallback"></param>
    private Sequence CreateSequence(List<TweenElement> tweens)
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

            foreach (var tween in tweens)
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
            seq.Join(subSequence.SetDelay(totalDelay));
            totalDelay += delay;
        }

        return seq;
    }

    /// <summary>
    /// 各要素の位置を保存
    /// </summary>
    private void CreateCachePosition()
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
            obj.Reset();
        }
    }
}

