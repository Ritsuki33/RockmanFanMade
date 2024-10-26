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


    public void FadeIn(float fadeTime, Color color, Action callback)
    {
        image.color = color;
        blackOut.DOFade(0, fadeTime).OnComplete(() => callback?.Invoke());
    }

    public void FadeOut(float fadeTime, Color color, Action callback)
    {
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
