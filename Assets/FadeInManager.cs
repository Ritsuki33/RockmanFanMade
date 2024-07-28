using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeInManager : MonoBehaviour
{
    [SerializeField] CanvasGroup blackOut = default;

    private bool isFade = false;

    public bool IsFade => isFade;
    public void FadeIn()
    {
        isFade = true;
        blackOut.DOFade(1, 0.4f).OnComplete(() => isFade = false);
    }

    public void FadeOut()
    {
        isFade = true;
        blackOut.DOFade(0, 0.4f).OnComplete(() => isFade = false);
    }
}
