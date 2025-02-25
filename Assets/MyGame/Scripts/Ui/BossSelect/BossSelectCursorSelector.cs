using System.Collections;
using System.Collections.Generic;
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
    protected override void OnSetup(BossSelectInfo data)
    {
        if (this.image) this.image.sprite = data.panelSprite;
    }
}
