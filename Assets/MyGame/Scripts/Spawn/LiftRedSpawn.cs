using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftRedSpawn : InCameraSpawn
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] float idleSpeed = 5f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float accelerate = 15f;

    public new LiftRed Obj
    {
        get
        {
            LiftRed lift = null;
            if (lift = base.Obj as LiftRed)
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
        Obj.Setup(start, end, idleSpeed, maxSpeed, accelerate);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(start.transform.position, end.transform.position);
    }
}
