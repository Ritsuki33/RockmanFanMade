using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ExRbHit
{
    Dictionary<RaycastHit2D, IExRbVisitable> onHitCache = new Dictionary<RaycastHit2D, IExRbVisitable>();

    public void CacheClear()
    {
        onHitCache.Clear();
    }

    public void OnHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnHitEnter(visitor);

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

        hitv?.AcceptOnHitStay(visitor);
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

        hitv?.AcceptOnHitExit(visitor);
    }

    public void OnBottomHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnBottomHitEnter(visitor);

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

        hitv?.AcceptOnBottomHitStay(visitor);
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

        hitv?.AcceptOnBottomHitExit(visitor);

        // キャッシュを削除
        if (onHitCache.ContainsKey(hit)) onHitCache.Remove(hit);
    }

    public void OnTopHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnTopHitEnter(visitor);

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

        hitv?.AcceptOnTopHitStay(visitor);
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

        hitv?.AcceptOnTopHitExit(visitor);
    }

    public void OnLeftHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnLeftHitEnter(visitor);

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

        hitv?.AcceptOnLeftHitStay(visitor);
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

        hitv?.AcceptOnLeftHitExit(visitor);
    }

    public void OnRightHitEnter(IExRbVisitor visitor,RaycastHit2D hit)
    {
        var hitv = hit.collider.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnRightHitEnter(visitor);

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

        hitv?.AcceptOnRightHitStay(visitor);
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

        hitv?.AcceptOnRightHitExit(visitor);
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
