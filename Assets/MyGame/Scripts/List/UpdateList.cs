using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * オブジェクトをリストで管理し、オブジェクトの増減に応じてリストを更新する場合、
 * 直接リストを操作するとループ処理が中断されたりエラーが発生することがあります。
 * これは、リストが変更されると列挙（foreach ループなど）が破損するため
 * ループ中はisUpdatingで管理する。
 */

public class UpdateList
{
    enum ExecType
    {
        Add,
        Remove
    }
    class ExecTarget
    {
        public ExecTarget(IObjectInterpreter obj, ExecType execType)
        {
            this.obj = obj;
            this.execType = execType;
        }

        public IObjectInterpreter obj;
        public ExecType execType;
    }

    // 現在のリスト
    private List<IObjectInterpreter> list = new List<IObjectInterpreter>();

    private List<ExecTarget> execTargets = new List<ExecTarget>();

    public int Count => list.Count;

    // アップデート中か
    private bool isUpdating = true;

    public void OnFixedUpdate()
    {
        isUpdating = true;

        FixedList();
        int nullCount = 0;

        foreach (IObjectInterpreter e in list)
        {
            try
            {
                e.OnFixedUpdate();
            }
            catch (NullReferenceException ex)
            {
                Debug.LogError($"FixedUpdate中にNull参照を検知しました。{ex.Message}");
                nullCount++;
            }
        }

        // nullがあった場合は削除
        if (nullCount > 0) list.RemoveAll(item => item == null);

        isUpdating = false;
    }

    public void OnLateFixedUpdate()
    {
        isUpdating = true;
        foreach (IObjectInterpreter e in list)
        {
            e.OnLateFixedUpdate();
        }
        isUpdating = false;
    }


    public void OnUpdate()
    {
        isUpdating = true;

        FixedList();
        int nullCount = 0;
        foreach (IObjectInterpreter e in list)
        {
            try
            {
                e.OnUpdate();
            }
            catch (NullReferenceException ex)
            {
                Debug.LogError($"Update中にNull参照を検知しました。{ex.Message}");
                nullCount++;
            }
        }

        // nullがあった場合は削除
        if (nullCount > 0) list.RemoveAll(item => item == null);

        isUpdating = false;
    }

    public void OnLateUpdate()
    {
        isUpdating = true;
        foreach (IObjectInterpreter e in list)
        {
            e.OnLateUpdate();
        }
        isUpdating = false;
    }

    /// <summary>
    /// すべて削除
    /// </summary>
    public void AllDelete()
    {
        // 要素が減っていくので逆順で実行
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].Delete();
        }
    }

    public void OnPause(bool isPause)
    {
        foreach (IObjectInterpreter e in list)
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
            int index = execTargets.FindIndex(e => e.obj == obj);
            if (index >= 0 && execTargets[index].execType != ExecType.Add)
            {
                obj.Init();
                execTargets[index].execType = ExecType.Add;
            }
            else
            {
                obj.Init();
                execTargets.Add(new ExecTarget(obj, ExecType.Add));
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
            int index = execTargets.FindIndex(e => e.obj == obj);
            if (index >= 0 && execTargets[index].execType != ExecType.Remove)
            {
                obj.Destroy();
                execTargets[index].execType = ExecType.Remove;
            }
            else
            {
                obj.Destroy();
                execTargets.Add(new ExecTarget(obj, ExecType.Remove));
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
        foreach (var target in execTargets)
        {
            switch (target.execType)
            {
                case ExecType.Add:
                    if (target.execType == ExecType.Add)
                    {
                        if (!list.Contains(target.obj))
                        {
                            list.Add(target.obj);
                        }
                    }
                    break;
                case ExecType.Remove:
                    if (target.execType == ExecType.Add)
                    {
                        if (list.Contains(target.obj))
                        {
                            target.obj.Destroy();
                            list.Remove(target.obj);
                        }
                    }
                    break;
            }
        }

        if (execTargets.Count > 0) execTargets.Clear();
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
