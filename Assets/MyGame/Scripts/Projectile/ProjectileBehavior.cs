using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : RbStateMachine<ProjectileBehavior>
{
    [SerializeField] ProjectileReusable projectile;

    Action<Rigidbody2D> fixedUpdate;
    Action deleteCallback;
    private Rigidbody2D rb;

    Action<ProjectileReusable, Collision2D> onCollision;
    Action<ProjectileReusable, Collider2D> onTrigger;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        AddState(0, new Fire());    
        TransitReady(0);
    }

    class Fire : RbState<ProjectileBehavior, Fire>
    {
        protected override void FixedUpdate(ProjectileBehavior ctr)
        {
            ctr.projectile.FixedUpdateCallback.Invoke(ctr.rb);

            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.projectile.Delete();
            }
        }

        protected override void OnCollisionEnter(ProjectileBehavior ctr, Collision2D collision)
        {
            ctr.projectile.OnCollisionCallback?.Invoke(ctr.projectile);
        }

        protected override void OnTriggerEnter(ProjectileBehavior ctr, PlayerTrigger collision)
        {
            ctr.projectile.OnCollisionCallback?.Invoke(ctr.projectile);
        }
    }
}
