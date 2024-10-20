using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectController<T,D> : MonoBehaviour where T: BaseSelect<D>
{
    [SerializeField] Transform selectsRoot = default;
    [SerializeField] T selectPrefab = default;
    
    protected List<T> selects = new List<T>();
    protected int currentIndex = 0;
    protected int preIndex = 0;

    public int CurrentIndex => currentIndex;

    Action<D> selectCallback;

    public virtual void Init(D[] data, Action<D> selectCallback)
    {
        for (int i = 0; i < data.Length; i++)
        {
            BaseSelect<D> select = GameObject.Instantiate(selectPrefab, selectsRoot);
            select.Setup(data[i], Selected);
            selects.Add((T)select);
        }

        UpdateCursor(currentIndex);

        this.selectCallback = selectCallback;
    }

    public void OnDestroy()
    {
        foreach (var select in selects)
        {
            Destroy(select.gameObject);
        }

        selects.Clear();
    }

    /// <summary>
    /// コントローラー
    /// </summary>
    /// <param name="info"></param>
    public void InputUpdate(TitleManager.InputInfo info)
    {
        if (info.up)
        {
            UpdateCursor(currentIndex - 1);
        }
        else if (info.down)
        {
            UpdateCursor(currentIndex + 1);
        }

        if (info.decide)
        {
            selects[currentIndex].Selected();
        }
    }
   
    /// <summary>
    /// モデル
    /// </summary>
    /// <param name="index"></param>
    void UpdateCursor(int index)
    {
        preIndex = currentIndex;
        currentIndex = Mathf.Clamp(index, 0, selects.Count - 1);

        DisplayCursor();
    }

    public  void Selected(D data)
    {
        this.selectCallback.Invoke(data);
    }


    /// <summary>
    /// ビュー
    /// </summary>
    public abstract void DisplayCursor();

}
