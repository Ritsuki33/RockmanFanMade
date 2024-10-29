using System;
using TMPro;
using UnityEngine;

public abstract class BaseSelect<D> : MonoBehaviour
{
    protected Action<D> selected = default;
    [SerializeField] protected D data;

    public void Setup(Action<D> selected)
    {
        this.selected = selected;

        OnSetup(data);
    }

    public void Setup(D data, Action<D> selected)
    {
        this.data = data;
        this.selected = selected;

        OnSetup(data);
    }

    protected abstract void OnSetup(D data);

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
