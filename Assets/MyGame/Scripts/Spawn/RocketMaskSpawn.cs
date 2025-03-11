using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMastSpawn : InCameraSpawn
{
    [SerializeField] float distance = 5.0f;
    [SerializeField] float speed = 2.0f;

    public new RocketMask Obj
    {
        get
        {
            RocketMask rocketMask = null;
            if (rocketMask = base.Obj as RocketMask)
            {
                return rocketMask;
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
        Obj.Setup(distance, speed);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Vector3 halfVec = Vector3.right * distance / 2;
        Gizmos.DrawLine(this.transform.position - halfVec, this.transform.position + halfVec);
    }

}
