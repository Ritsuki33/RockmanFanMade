using System;
using TMPro;
using UnityEngine;

public abstract class BaseSelect<TData> : MonoBehaviour
{
    protected Action<TData> selected = default;
    [SerializeField] protected TData data;

    public void Setup(Action<TData> selected)
    {
        this.selected = selected;

        OnSetup(data);
    }

    public void Setup(TData data, Action<TData> selected)
    {
        this.data = data;
        this.selected = selected;

        OnSetup(data);
    }

    protected abstract void OnSetup(TData data);

    public void Selected()
    {
        selected.Invoke(data);
    }
}

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

public class CursorSelector : BaseSelect<SelectInfo>
{
    [SerializeField] Transform cursorPtr = default;
    [SerializeField] TextMeshProUGUI text = default;

    protected override void OnSetup(SelectInfo data)
    {
        if (this.text) this.text.text = data.text;
    }

    public void SetCursor(Transform cursor)
    {
        cursor.SetParent(cursorPtr);

        cursor.localPosition = Vector3.zero;
    }
}
