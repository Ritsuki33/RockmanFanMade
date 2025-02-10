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

public abstract class SelectController<TSelect,TData> : MonoBehaviour where TSelect: BaseSelect<TData>
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

    public virtual void Init(int index, Action<TData> selectCallback)
    {
        foreach (var item in selects)
        {
            item.Setup(Selected);
        }

        UpdateCursor(index);

        this.selectCallback = selectCallback;
    }

    public virtual void Init(TData[] data, Action<TData> selectCallback)
    {
        for (int i = 0; i < data.Length; i++)
        {
            BaseSelect<TData> select = GameObject.Instantiate(selectPrefab, selectsRoot);
            select.Setup(data[i], Selected);
            selects.Add((TSelect)select);
        }

        UpdateCursor(currentIndex);

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

    public void OnDestroy()
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
        currentIndex = Mathf.Clamp(index, 0, selects.Count - 1);

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
