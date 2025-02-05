using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftYellowCircleMoveSpawn : InCameraSpawn
{
    [SerializeField] Transform _center;
    [SerializeField] float speed = 2.0f;

    public new LiftYellowCircleMove Obj
    {
        get
        {
            LiftYellowCircleMove lift = null;
            if (lift = base.Obj as LiftYellowCircleMove)
            {
                return lift;
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
        Obj.Setup(_center, speed);
    }
}
