using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CachedHit
{
    Dictionary<RaycastHit2D, IExRbVisitable> onHitCache = new Dictionary<RaycastHit2D, IExRbVisitable>();

    public void CacheClear()
    {
        onHitCache.Clear();
    }

    public void OnHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnHitEnter(visitor, hit);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }

    public void OnHitStay(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;
        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();

            // キャッシュ
            onHitCache.Add(hit, hitv);
        }

        hitv?.AcceptOnHitStay(visitor, hit);
    }

    public void OnHitExit(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];

            // キャッシュを削除
            onHitCache.Remove(hit);
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();
        }

        hitv?.AcceptOnHitExit(visitor, hit);
    }

    public void OnBottomHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnBottomHitEnter(visitor, hit);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }


    public void OnBottomHitStay(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();

            // キャッシュ
            onHitCache.Add(hit, hitv);
        }

        hitv?.AcceptOnBottomHitStay(visitor, hit);
    }

    public void OnBottomHitExit(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];

            // キャッシュを削除
            onHitCache.Remove(hit);
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();
        }

        hitv?.AcceptOnBottomHitExit(visitor, hit);

        // キャッシュを削除
        if (onHitCache.ContainsKey(hit)) onHitCache.Remove(hit);
    }

    public void OnTopHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnTopHitEnter(visitor, hit);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }

    public void OnTopHitStay(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();

            // キャッシュ
            onHitCache.Add(hit, hitv);
        }

        hitv?.AcceptOnTopHitStay(visitor, hit);
    }

    public void OnTopHitExit(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];

            // キャッシュを削除
            onHitCache.Remove(hit);
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();
        }

        hitv?.AcceptOnTopHitExit(visitor, hit);
    }

    public void OnLeftHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnLeftHitEnter(visitor, hit);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }

    public void OnLeftHitStay(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();

            // キャッシュ
            onHitCache.Add(hit, hitv);
        }

        hitv?.AcceptOnLeftHitStay(visitor, hit);
    }

    public void OnLeftHitExit(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];

            // キャッシュを削除
            onHitCache.Remove(hit);
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();
        }

        hitv?.AcceptOnLeftHitExit(visitor, hit);
    }

    public void OnRightHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnRightHitEnter(visitor, hit);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }


    public void OnRightHitStay(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();

            // キャッシュ
            onHitCache.Add(hit, hitv);
        }

        hitv?.AcceptOnRightHitStay(visitor, hit);
    }

    public void OnRightHitExit(IExRbVisitor visitor,RaycastHit2D hit)
    {
        IExRbVisitable hitv = null;

        if (onHitCache.ContainsKey(hit))
        {
            hitv = onHitCache[hit];

            // キャッシュを削除
            onHitCache.Remove(hit);
        }
        else
        {
            hitv = hit.collider.GetComponent<IExRbVisitable>();
        }

        hitv?.AcceptOnRightHitExit(visitor, hit);
    }
}

public partial interface IHitInterpreter<T>
{
    void OnHitEnter(T hit) { }
    void OnBottomHitEnter(T hit) { }
    void OnTopHitEnter(T hit) { }
    void OnLeftHitEnter(T hit) { }
    void OnRightHitEnter(T hit) { }
    void OnHitStay(T hit) { }
    void OnBottomHitStay(T hit) { }
    void OnTopHitStay(T hit) { }
    void OnLeftHitStay(T hit) { }
    void OnRightHitStay(T hit) { }
    void OnHitExit(T hit) { }
    void OnBottomHitExit(T hit) { }
    void OnTopHitExit(T hit) { }
    void OnLeftHitExit(T hit) { }
    void OnRightHitExit(T hit) { }
}
