using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * オブジェクトをリストで管理し、オブジェクトの増減に応じてリストも更新する場合、
 * 直接リストを操作するとループ処理が中断されたりエラーが発生することがあります。
 * これは、リストが変更されると列挙（foreach ループなど）が破損するため
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


    public void OnFixedUpdate()
    {
        FixedList();
        foreach (IObjectInterpreter e in list)
        {
            e.OnFixedUpdate();
        }
    }

    public void OnUpdate()
    {
        FixedList();
        foreach (IObjectInterpreter e in list)
        {
            e.OnUpdate();
        }
    }

    public void OnPause(bool isPause)
    {
        foreach(IObjectInterpreter e in list)
        {
            e.OnPause(isPause);
        }
    }

    public void Add(IObjectInterpreter obj)
    {
        if (!addList.Contains(obj)) addList.Add(obj);
    }

    public void Remove(IObjectInterpreter obj)
    {
        if (!removeList.Contains(obj)) removeList.Add(obj);
    }

    public void Clear()
    {
        list.Clear();
    }

    // リストの更新
    private void FixedList()
    {
        foreach(var e in addList)
        {
            e.Init();
            list.Add(e);
        }

        foreach(var e in removeList)
        {
            e.Destroy();
            list.Remove(e);
        }

        if (addList.Count > 0) addList.Clear();
        if (removeList.Count > 0) removeList.Clear();
    }

    public IObjectInterpreter this[int idndex]
    {
        get
        {
            return list[idndex];
        }
        set
        {
            list[idndex] = value;
        }
    }
}
