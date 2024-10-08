using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class FadeInManager : SingletonComponent<FadeInManager>
{
    [SerializeField] CanvasGroup blackOut = default;
    [SerializeField] Image image = default;

    private bool isFade = false;

    public bool IsFade => isFade;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    FadeOutImmediate();
    //}

    public void FadeIn(float fadeTime = 0.4f)
    {
        isFade = true;
        image.color = Color.black;
        blackOut.DOFade(0, fadeTime).OnComplete(() => isFade = false);
    }

    public void FadeIn()
    {
        isFade = true;
        blackOut.DOFade(0, 0.4f).OnComplete(() => isFade = false);
    }

    public void FadeIn(float fadeTime, Color color, Action callback)
    {
        isFade = true;
        image.color = color;
        blackOut.DOFade(0, fadeTime).OnComplete(() => callback?.Invoke());
    }

    public void FadeOut()
    {
        isFade = true;
        blackOut.DOFade(1, 0.4f).OnComplete(() => isFade = false);
    }

    public void FadeOut(float fadeTime, Color color, Action callback)
    {
        isFade = true;
        image.color = color;
        blackOut.DOFade(1, fadeTime).OnComplete(() => callback?.Invoke());
    }

    public void FadeInImmediate()
    {
        blackOut.alpha = 0f;
    }

    public void FadeOutImmediate()
    {
        blackOut.alpha = 1f;
    }
}
