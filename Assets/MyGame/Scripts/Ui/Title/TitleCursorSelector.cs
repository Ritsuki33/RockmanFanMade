using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct SelectInfo
{
    public int id;
    public string text;

    public SelectInfo(int id, string text)
    {
        this.id = id;
        this.text = text;
    }
}

public class TitleCursorSelector : CursorSelector<SelectInfo>
{
    [SerializeField] TextMeshProUGUI text = default;

    protected override void OnSetup(SelectInfo data)
    {
        if (this.text) this.text.text = data.text;
    }
}
