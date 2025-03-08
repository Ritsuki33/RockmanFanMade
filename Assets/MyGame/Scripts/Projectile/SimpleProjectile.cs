using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleProjectile<T> where T : PhysicalObject
{
    private int attackPower = 1;

    Action<Rigidbody2D> fixedUpdate;

    Action<T> onCollision;

    public int AttackPower => attackPower;
    private T obj;
    private Rigidbody2D rb => obj.Rb;

    public Vector2 CurVelocity => rb.velocity;
    public float CurSpeed => rb.velocity.magnitude;


    public SimpleProjectile(T obj)
    {
        this.obj = obj;
    }

    public void Init(Action Delete)
    {
        EventTriggerManager.Instance.VoidEventTriggers.Subscribe(EventType.ChangeCameraStart, Delete);
    }

    public void Destory(Action Delete)
    {
        EventTriggerManager.Instance.VoidEventTriggers.Unsubscribe(EventType.ChangeCameraStart, Delete);
    }

    public void OnFixedUpdate()
    {
        fixedUpdate?.Invoke(rb);
    }

    public void Setup(Vector3 position, int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate, Action<T> onCollisionEnter = null)
    {
        this.rb.transform.position = position;
        start?.Invoke(rb);
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
        this.onCollision = onCollisionEnter;
    }

    public void ChangeBehavior(int attackPower, Action<Rigidbody2D> start, Action<Rigidbody2D> fixedUpdate)
    {
        start?.Invoke(rb);
        this.attackPower = attackPower;
        this.fixedUpdate = fixedUpdate;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        onCollision?.Invoke(obj);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        onCollision?.Invoke(obj);
    }
}
