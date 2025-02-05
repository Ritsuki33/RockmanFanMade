using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftYellowLineMoveSpawn : InCameraSpawn
{
    [SerializeField] Transform[] liftPoints;
    [SerializeField] float maxSpeed = 5.0f;     // 最大速度 (v)
    [SerializeField] float accelerate = 2.0f;   // 加速、減速(v/s)

    public new LiftYellowLineMove Obj
    {
        get
        {
            LiftYellowLineMove grenademan = null;
            if (grenademan = base.Obj as LiftYellowLineMove)
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
        Obj.Setup(liftPoints, maxSpeed, accelerate);
    }
}
