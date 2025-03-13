using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LiftYellowLineMoveSpawn : Spawn<LiftYellowLineMove>
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

    protected override LiftYellowLineMove OnGetResource()
    {
        return ObjectManager.Instance.OnGet<LiftYellowLineMove>(PoolType.LiftYellow_LineMove, (obj) => this.obj = null);
    }

    protected override void InitializeObject()
    {
        base.InitializeObject();
        Obj.Setup(liftPoints, maxSpeed, accelerate);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < liftPoints.Length && i + 1 < liftPoints.Length; i++)
        {
            Gizmos.DrawLine(liftPoints[i].transform.position, liftPoints[i + 1].transform.position);
        }
    }
}
