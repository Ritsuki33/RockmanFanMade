using System;
using System.Collections.Generic;
using UnityEngine;

public partial class RbCollide : IRbVisitor
{
    Dictionary<GameObject, IRbVisitable> cacheCollider = new Dictionary<GameObject, IRbVisitable>();

    public event Action<Collision2D> onCollisionEnter;
    public event Action<Collision2D> onCollisionStay;
    public event Action<Collision2D> onCollisionExit;
    public event Action<Collider2D> onTriggerEnter;
    public event Action<Collider2D> onTriggerStay;
    public event Action<Collider2D> onTriggerExit;

    public void Init()
    {
        cacheCollider.Clear();
    }

    public void OnCollisionEnter(Collision2D collision)
    {
        onCollisionEnter?.Invoke(collision);

        var collide = collision.gameObject.GetComponent<IRbVisitable>();
        // キャッシュ
        if (!cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Add(collision.gameObject, collide);
        collide?.AcceptOnCollisionEnter(this);
    }


    public void OnCollisionStay(Collision2D collision)
    {
        onCollisionStay?.Invoke(collision);

        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(collision.gameObject))
        {
            collide = cacheCollider[collision.gameObject];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            collide = collision.gameObject.GetComponent<IRbVisitable>();
            cacheCollider.Add(collision.gameObject, collide);
        }
        collide?.AcceptOnCollisionStay(this);
    }

    public void OnCollisionExit(Collision2D collision)
    {
        onCollisionExit?.Invoke(collision);

        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(collision.gameObject))
        {
            collide = cacheCollider[collision.gameObject];
        }
        else
        {
            // キャッシュがない場合は改めて取得
            collide = collision.gameObject.GetComponent<IRbVisitable>();
        }
        collide?.AcceptOnCollisionExit(this);

        if (cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Remove(collision.gameObject);
    }

    public void OnTriggerEnter(Collider2D collision)
    {
        onTriggerEnter?.Invoke(collision);
        var collide = collision.gameObject.GetComponent<IRbVisitable>();

        // キャッシュ
        if (!cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Add(collision.gameObject, collide);

        collide?.AcceptOnTriggerEnter(this);
    }

    public void OnTriggerStay(Collider2D collision)
    {
        onTriggerStay?.Invoke(collision);

        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(collision.gameObject))
        {
            collide = cacheCollider[collision.gameObject];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            collide = collision.gameObject.GetComponent<IRbVisitable>();
            cacheCollider.Add(collision.gameObject, collide);
        }

        collide?.AcceptOnTriggerStay(this);
    }

    public void OnTriggerExit(Collider2D collision)
    {
        onTriggerExit?.Invoke(collision);

        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(collision.gameObject))
        {
            collide = cacheCollider[collision.gameObject];
        }
        else
        {
            // キャッシュがない場合は改めて取得
            collide = collision.gameObject.GetComponent<IRbVisitable>();
        }
        collide?.AcceptOnTriggerExit(this);

        // キャッシュの削除
        if (cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Remove(collision.gameObject);
    }
}
