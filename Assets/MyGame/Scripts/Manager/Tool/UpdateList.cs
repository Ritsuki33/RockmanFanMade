using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * オブジェクトをリストで管理し、オブジェクトの増減に応じてリストを更新する場合、
 * 直接リストを操作するとループ処理が中断されたりエラーが発生することがあります。
 * これは、リストが変更されると列挙（foreach ループなど）が破損するため
 * ループ中はisUpdatingで管理する。
 */

public class UpdateList
{
    // 現在のリスト
    private List<IObjectInterpreter> list= new List<IObjectInterpreter>();

    // Update中にリストの増減が発生するとループ分がエラーになるため、Update終了後にlistの更新に入る
    // 追加分のリスト
    private List<IObjectInterpreter> addList= new List<IObjectInterpreter>();

    // 削除分のリスト
    private List<IObjectInterpreter> removeList= new List<IObjectInterpreter>();

    public int Count=>list.Count;

    // アップデート中か
    private bool isUpdating = true;

    public void OnFixedUpdate()
    {
        isUpdating = true;

        FixedList();
        int nullCount = 0;

        foreach (IObjectInterpreter e in list)
        {
            if (e.gameObject == null) nullCount++; 
            else e.OnFixedUpdate();
        }

        // nullがあった場合は削除
        if (nullCount > 0) list.RemoveAll(item => item == null);

        isUpdating = false;
    }

    public void OnUpdate()
    {
        isUpdating = true;

        FixedList();
        int nullCount = 0;
        foreach (IObjectInterpreter e in list)
        {
            if (e.gameObject == null) nullCount++; 
            else e.OnUpdate();
        }

        // nullがあった場合は削除
        if (nullCount > 0) list.RemoveAll(item => item == null);

        isUpdating = false;
    }

    /// <summary>
    /// すべて削除
    /// </summary>
    public void AllDelete()
    {
        // 要素が減っていくので逆順で実行
        for(int i = list.Count - 1; i >= 0; i--)
        {
            list[i].Delete();
        }
    }

    public void OnPause(bool isPause)
    {
        foreach(IObjectInterpreter e in list)
        {
            e.RequestPause(isPause);
        }
    }

    /// <summary>
    /// オブジェクトの登録
    /// </summary>
    /// <param name="obj"></param>
    public void Add(IObjectInterpreter obj)
    {
        if (isUpdating)
        {
            // アップデートの場合は予約
            if (!addList.Contains(obj))
            {
                obj.Init();
                addList.Add(obj);
            }
        }
        else
        {
            // アップデート外ならばその場で登録
            if (!list.Contains(obj))
            {
                obj.Init();
                list.Add(obj);
            }
        }
        
    }

    /// <summary>
    /// オブジェクトの削除
    /// </summary>
    /// <param name="obj"></param>
    public void Remove(IObjectInterpreter obj)
    {
        if (isUpdating)
        {
            // アップデートの場合は予約
            if (!removeList.Contains(obj))
            {
                obj.Destroy();
                removeList.Add(obj);
            }
        }
        else
        {
            // アップデート外ならばその場で削除
            if (list.Contains(obj))
            {
                obj.Destroy();
                list.Remove(obj);
            }
        }
    }

    /// <summary>
    /// リストの更新
    /// </summary>
    private void FixedList()
    {
        foreach(var e in addList)
        {
            if (!list.Contains(e))
            {
                list.Add(e);
            }
        }

        foreach(var e in removeList)
        {
            if (list.Contains(e))
            {
                list.Remove(e);
            }
        }

        if (addList.Count > 0) addList.Clear();
        if (removeList.Count > 0) removeList.Clear();
    }

    /// <summary>
    /// インデクサー
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IObjectInterpreter this[int index]
    {
        get
        {
            return list[index];
        }
        set
        {
            list[index] = value;
        }
    }
}
