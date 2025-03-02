using System;
using System.Collections.Generic;
using UnityEngine;

public enum InputDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}
static class InputDirectionExtensions
{
    public static InputDirection GetInputDirection(this InputDirection dir, InputInfo info)
    {
        if (info.up) return InputDirection.Up;
        else if (info.down) return InputDirection.Down;
        else return InputDirection.None;
    }
}

public abstract class SelectController<TSelect, TData> : MonoBehaviour where TSelect : BaseSelector<TData>
{
    [Header("動的生成用 入力")]
    [SerializeField] Transform selectsRoot = default;
    [SerializeField] TSelect selectPrefab = default;

    [Header("静的生成用 入力")]
    [SerializeField] protected List<TSelect> selects = new List<TSelect>();
    protected int currentIndex = 0;
    protected int preIndex = 0;

    public int CurrentIndex => currentIndex;

    Action<TData> selectCallback;

    public List<TSelect> Selects => selects;

    /// <summary>
    /// 静的に要素がある場合はこちらで初期化
    /// </summary>
    /// <param name="index"></param>
    /// <param name="selectCallback"></param>
    public virtual void Init(int index, Action<TData> selectCallback)
    {
        foreach (var item in selects)
        {
            item.Setup(Selected);
            item.OnCursorExit();
        }

        UpdateCursor(index);

        this.selectCallback = selectCallback;
    }

    /// <summary>
    /// 動的に要素がある場合はこちらで初期化
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="data"></param>
    /// <param name="selectCallback"></param>
    public virtual void Init(int startIndex, TData[] data, Action<TData> selectCallback)
    {
        // 静的に生成した要素を削除
        foreach (var item in selects)
        {
            Destroy(item.gameObject);
        }
        selects.Clear();

        for (int i = 0; i < data.Length; i++)
        {
            BaseSelector<TData> select = GameObject.Instantiate(selectPrefab, selectsRoot);
            select.OnCursorExit();
            select.Setup(data[i], Selected);
            selects.Add((TSelect)select);
        }

        UpdateCursor(startIndex);

        this.selectCallback = selectCallback;
    }

    public void Clear()
    {
        foreach (var select in selects)
        {
            Destroy(select.gameObject);
        }

        selects.Clear();
    }

    public virtual void OnDestroy()
    {
        Clear();
    }

    /// <summary>
    /// コントローラー
    /// </summary>
    /// <param name="info"></param>
    public abstract void InputUpdate(InputDirection info);

    /// <summary>
    /// モデル
    /// </summary>
    /// <param name="index"></param>
    protected void UpdateCursor(int index)
    {
        preIndex = currentIndex;
        selects[preIndex].OnCursorExit();
        currentIndex = Mathf.Clamp(index, 0, selects.Count - 1);

        selects[currentIndex].OnCursorEnter();
        DisplayCursor();
    }

    private void Selected(TData data)
    {
        this.selectCallback.Invoke(data);
    }

    /// <summary>
    /// ビュー
    /// </summary>
    public abstract void DisplayCursor();

}
