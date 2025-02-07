using System.Collections.Generic;
using UnityEngine;

public partial class CachedCollide
{
    Dictionary<GameObject, IRbVisitable> cacheCollider = new Dictionary<GameObject, IRbVisitable>();

    public void CacheClear()
    {
        cacheCollider.Clear();
    }

    public void OnCollisionEnter(IRbVisitor visitor, Collision2D collision)
    {
        collision.gameObject.TryGetComponent(out IRbVisitable collide);

        // キャッシュ
        if (!cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Add(collision.gameObject, collide);
        collide?.AcceptOnCollisionEnter(visitor);
    }

    public void OnCollisionStay(IRbVisitor visitor, Collision2D collision)
    {
        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(collision.gameObject))
        {
            collide = cacheCollider[collision.gameObject];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            collision.gameObject.TryGetComponent(out collide);
            cacheCollider.Add(collision.gameObject, collide);
        }
        collide?.AcceptOnCollisionStay(visitor);
    }

    public void OnCollisionExit(IRbVisitor visitor, Collision2D collision)
    {
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
        collide?.AcceptOnCollisionExit(visitor);

        if (cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Remove(collision.gameObject);
    }

    public void OnTriggerEnter(IRbVisitor visitor, Collider2D collision)
    {
        var collide = collision.gameObject.GetComponent<IRbVisitable>();

        // キャッシュ
        if (!cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Add(collision.gameObject, collide);

        collide?.AcceptOnTriggerEnter(visitor);
    }

    public void OnTriggerStay(IRbVisitor visitor, Collider2D collision)
    {
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

        collide?.AcceptOnTriggerStay(visitor);
    }

    public void OnTriggerExit(IRbVisitor visitor, Collider2D collision)
    {
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
        collide?.AcceptOnTriggerExit(visitor);

        // キャッシュの削除
        if (cacheCollider.ContainsKey(collision.gameObject)) cacheCollider.Remove(collision.gameObject);
    }
}
