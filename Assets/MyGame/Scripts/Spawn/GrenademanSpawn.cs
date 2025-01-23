using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenademanSpawn : BossSpawn
{
    [SerializeField] Transform[] _placeBombPosArray = null;
    
    public new Grenademan Obj
    {
        get
        {
            Grenademan grenademan = null;
            if (grenademan = base.Obj as Grenademan)
            {
                return grenademan;
            }
            else
            {
                Debug.Log("Grenademan型を取得できませんでした。");
                return null;
            }
        }
    }

    protected override void InitializeObject()
    {
        base.InitializeObject();
        Obj?.Setup(_placeBombPosArray);
    }
}
