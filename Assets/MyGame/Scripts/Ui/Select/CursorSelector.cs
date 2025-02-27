using System;
using TMPro;
using UnityEngine;

public abstract class BaseSelector<TData> : MonoBehaviour
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

    /// <summary>
    /// カーソルが合わさったとき
    /// </summary>
    public virtual void OnCursorEnter() { }

    public virtual void OnCursorExit() { }
}


public abstract class CursorSelector<TData> : BaseSelector<TData>
{
    [SerializeField] Transform cursorPtr = default;

    public void SetCursor(Transform cursor)
    {
        cursor.SetParent(cursorPtr, false);
        cursor.localPosition = Vector3.zero;
    }
}
