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
        var parent = collision.gameObject.transform.parent.gameObject;
        parent.TryGetComponent(out IRbVisitable collide);

        // キャッシュ
        if (!cacheCollider.ContainsKey(parent)) cacheCollider.Add(parent, collide);
        collide?.AcceptOnCollisionEnter(visitor);
    }

    public void OnCollisionStay(IRbVisitor visitor, Collision2D collision)
    {
        var parent = collision.gameObject.transform.parent.gameObject;
        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(parent))
        {
            collide = cacheCollider[parent];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            parent.TryGetComponent(out collide);
            cacheCollider.Add(parent, collide);
        }
        collide?.AcceptOnCollisionStay(visitor);
    }

    public void OnCollisionExit(IRbVisitor visitor, Collision2D collision)
    {
        var parent = collision.gameObject.transform.parent.gameObject;
        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(parent))
        {
            collide = cacheCollider[parent];
        }
        else
        {
            // キャッシュがない場合は改めて取得
            collision.gameObject.TryGetComponent(out collide);
        }
        collide?.AcceptOnCollisionExit(visitor);

        if (cacheCollider.ContainsKey(parent)) cacheCollider.Remove(parent);
    }

    public void OnTriggerEnter(IRbVisitor visitor, Collider2D collision)
    {
        var parent = collision.gameObject.transform.parent.gameObject;
        parent.TryGetComponent(out IRbVisitable collide);

        // キャッシュ
        if (!cacheCollider.ContainsKey(parent)) cacheCollider.Add(parent, collide);

        collide?.AcceptOnTriggerEnter(visitor);
    }

    public void OnTriggerStay(IRbVisitor visitor, Collider2D collision)
    {
        var parent = collision.gameObject.transform.parent.gameObject;
        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(parent))
        {
            collide = cacheCollider[parent];
        }
        else
        {
            // キャッシュがない場合は改めて取得して再キャッシュ
            parent.TryGetComponent(out collide);
            cacheCollider.Add(parent, collide);
        }

        collide?.AcceptOnTriggerStay(visitor);
    }

    public void OnTriggerExit(IRbVisitor visitor, Collider2D collision)
    {
        var parent = collision.gameObject.transform.parent.gameObject;
        IRbVisitable collide = null;

        if (cacheCollider.ContainsKey(parent))
        {
            collide = cacheCollider[parent];
        }
        else
        {
            // キャッシュがない場合は改めて取得
            parent.TryGetComponent(out collide);
        }
        collide?.AcceptOnTriggerExit(visitor);

        // キャッシュの削除
        if (cacheCollider.ContainsKey(parent)) cacheCollider.Remove(parent);
    }
}
