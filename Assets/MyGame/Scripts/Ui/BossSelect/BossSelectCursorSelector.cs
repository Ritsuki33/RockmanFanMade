using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public struct BossSelectInfo
{
    public int id;
    public string panelName;
    public Sprite panelSprite;

    public bool selectable;
}

public class BossSelectCursorSelector : CursorSelector<BossSelectInfo>
{
    [SerializeField] Image image = default;

    [SerializeField] RectTransform m_rectTransform;
    protected override void OnSetup(BossSelectInfo data)
    {
        if (this.image) this.image.sprite = data.panelSprite;
    }

    public override void OnCursorEnter()
    {
        m_rectTransform.DOScale(1.4f, 0.1f).SetEase(Ease.InOutSine).Play();
    }

    public override void OnCursorExit()
    {
        m_rectTransform.DOScale(1.0f, 0.1f).SetEase(Ease.InOutSine).Play();
    }
}
