using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateList
{
    private List<IObjectInterpreter> list= new List<IObjectInterpreter>();


    public void OnFixedUpdate()
    {
        foreach (IObjectInterpreter e in list)
        {
            e.OnFixedUpdate();
        }
    }

    public void OnUpdate()
    {
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
        if (!list.Contains(obj))
        list.Add(obj);
    }

    public void Remove(IObjectInterpreter obj)
    {
        list.Remove(obj);
    }

    public void Clear()
    {

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
