using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : RbStateMachine<ProjectileController>
{
    [SerializeField] Projectile projectile;

    Action<Rigidbody2D> fixedUpdate;
    Action deleteCallback;
    private Rigidbody2D rb;

    Action<Projectile, Collision2D> onCollision;
    Action<Projectile, Collider2D> onTrigger;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        AddState(0, new Fire());    
        TransitReady(0);
    }

    class Fire : RbState<ProjectileController, Fire>
    {
        protected override void FixedUpdate(ProjectileController ctr)
        {
            ctr.projectile.FixedUpdateCallback.Invoke(ctr.rb);

            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject))
            {
                ctr.projectile.Delete();
            }
        }

        protected override void OnCollisionEnter(ProjectileController ctr, Collision2D collision)
        {
            ctr.projectile.OnCollisionCallback?.Invoke(ctr.projectile);
        }

        protected override void OnTriggerEnter(ProjectileController ctr, PlayerTrigger collision)
        {
            ctr.projectile.OnCollisionCallback?.Invoke(ctr.projectile);
        }
    }
}
