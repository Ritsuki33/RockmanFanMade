using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeInManager : SingletonComponent<FadeInManager>
{
    [SerializeField] CanvasGroup blackOut = default;

    private bool isFade = false;

    public bool IsFade => isFade;

    protected override void Awake()
    {
        base.Awake();
        FadeOutImmediate();
    }

    public void FadeIn()
    {
        isFade = true;
        blackOut.DOFade(0, 0.4f).OnComplete(() => isFade = false);
    }

    public void FadeOut()
    {
        isFade = true;
        blackOut.DOFade(1, 0.4f).OnComplete(() => isFade = false);
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
