using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftYellowCircleMoveSpawn : Spawn<LiftYellowCircleMove>
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
    protected override LiftYellowCircleMove OnGetResource()
    {
        return ObjectManager.Instance.OnGet<LiftYellowCircleMove>(PoolType.LiftYellow_CircleMove, (obj) => this.obj = null);
    }
    protected override void InitializeObject()
    {
        base.InitializeObject();
        Obj.Setup(_center, speed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(this._center.position, 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this._center.position);
    }
}
