using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSpawn : InCameraSpawn
{
    [SerializeField] Transform[] liftPoints;

    public new LiftYellow Obj
    {
        get
        {
            LiftYellow grenademan = null;
            if (grenademan = base.Obj as LiftYellow)
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
        Obj.Setup(liftPoints);
    }
}
