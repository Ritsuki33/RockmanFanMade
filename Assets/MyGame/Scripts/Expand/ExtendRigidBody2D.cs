using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static public class ExtendRigidBody2D 
{
    public static void SetVelocty(this Rigidbody2D rb, Vector2 nextPosition)
    {
        Vector2 diff = nextPosition - (Vector2)rb.transform.position;

        Vector2 velocity = diff / Time.fixedDeltaTime;

        rb.velocity = velocity;
    }
}
