using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class CachedHit
{
    enum Hit
    {
        Top,
        Bottom,
        Right,
        Left,
        All
    }

    class HitCache
    {
        public RaycastHit2D hit;
        public IExRbVisitable visitable;
    }
    Dictionary<Hit, HitCache> hitCache = new Dictionary<Hit, HitCache>{
        {Hit.Top, new HitCache()},
        {Hit.Bottom, new HitCache()},
        {Hit.Right, new HitCache()},
        {Hit.Left, new HitCache()},
        {Hit.All, new HitCache()},
    };

    public void CacheClear()
    {
        hitCache = new Dictionary<Hit, HitCache>{
        {Hit.Top, new HitCache()},
        {Hit.Bottom, new HitCache()},
        {Hit.Right, new HitCache()},
        {Hit.Left, new HitCache()},
        {Hit.All, new HitCache()},
    };
    }

    public void OnHitEnter(IExRbVisitor visitor, RaycastHit2D hit)
    {
        var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnHitEnter(visitor, hit);
        // キャッシュ
        hitCache[Hit.All].hit = hit;
        hitCache[Hit.All].visitable = hitv;
    }

    public void OnHitStay(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.All].hit.collider != hit.collider)
        {
            hitCache[Hit.All].visitable?.AcceptOnHitExit(visitor, hitCache[Hit.All].hit);

            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnHitEnter(visitor, hit);
            hitCache[Hit.All].hit = hit;
            hitCache[Hit.All].visitable = hitv;
        }

        hitCache[Hit.All].visitable?.AcceptOnHitStay(visitor, hit);
    }

    public void OnHitExit(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.All].hit)
        {
            hitCache[Hit.All].visitable?.AcceptOnHitExit(visitor, hitCache[Hit.All].hit);
        }
        else
        {
            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnHitEnter(visitor, hit);
        }

        hitCache[Hit.All].hit = default;
        hitCache[Hit.All].visitable = default;

    }

    public void OnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit)
    {
        var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnBottomHitEnter(visitor, hit);
        // キャッシュ
        hitCache[Hit.Bottom].hit = hit;
        hitCache[Hit.Bottom].visitable = hitv;
    }

    public void OnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Bottom].hit.collider != hit.collider)
        {
            hitCache[Hit.Bottom].visitable?.AcceptOnBottomHitExit(visitor, hitCache[Hit.Bottom].hit);

            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnBottomHitEnter(visitor, hit);
            hitCache[Hit.Bottom].hit = hit;
            hitCache[Hit.Bottom].visitable = hitv;
        }

        hitCache[Hit.Bottom].visitable?.AcceptOnBottomHitStay(visitor, hit);
    }

    public void OnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Bottom].hit)
        {
            hitCache[Hit.Bottom].visitable?.AcceptOnBottomHitExit(visitor, hitCache[Hit.Bottom].hit);
        }
        else
        {
            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnBottomHitEnter(visitor, hit);
        }

        hitCache[Hit.Bottom].hit = default;
        hitCache[Hit.Bottom].visitable = default;

    }

    public void OnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit)
    {
        var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnTopHitEnter(visitor, hit);
        // キャッシュ
        hitCache[Hit.Top].hit = hit;
        hitCache[Hit.Top].visitable = hitv;
    }

    public void OnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Top].hit.collider != hit.collider)
        {
            hitCache[Hit.Top].visitable?.AcceptOnBottomHitExit(visitor, hitCache[Hit.Top].hit);

            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnTopHitEnter(visitor, hit);
            hitCache[Hit.Top].hit = hit;
            hitCache[Hit.Top].visitable = hitv;
        }

        hitCache[Hit.Bottom].visitable?.AcceptOnTopHitStay(visitor, hit);
    }

    public void OnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Top].hit)
        {
            hitCache[Hit.Top].visitable?.AcceptOnTopHitExit(visitor, hitCache[Hit.Top].hit);
        }
        else
        {
            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnTopHitEnter(visitor, hit);
        }

        hitCache[Hit.Top].hit = default;
        hitCache[Hit.Top].visitable = default;
    }

    public void OnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit)
    {
        var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnLeftHitEnter(visitor, hit);
        // キャッシュ
        hitCache[Hit.Left].hit = hit;
        hitCache[Hit.Left].visitable = hitv;
    }

    public void OnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Left].hit.collider != hit.collider)
        {
            hitCache[Hit.Left].visitable?.AcceptOnBottomHitExit(visitor, hitCache[Hit.Left].hit);

            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnLeftHitEnter(visitor, hit);
            hitCache[Hit.Left].hit = hit;
            hitCache[Hit.Left].visitable = hitv;
        }

        hitCache[Hit.Bottom].visitable?.AcceptOnLeftHitStay(visitor, hit);
    }

    public void OnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Left].hit)
        {
            hitCache[Hit.Left].visitable?.AcceptOnLeftHitExit(visitor, hitCache[Hit.Left].hit);
        }
        else
        {
            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnLeftHitEnter(visitor, hit);
        }

        hitCache[Hit.Left].hit = default;
        hitCache[Hit.Left].visitable = default;
    }

    public void OnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit)
    {
        var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnRightHitEnter(visitor, hit);
        // キャッシュ
        hitCache[Hit.Right].hit = hit;
        hitCache[Hit.Right].visitable = hitv;
    }

    public void OnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Right].hit.collider != hit.collider)
        {
            hitCache[Hit.Right].visitable?.AcceptOnBottomHitExit(visitor, hitCache[Hit.Right].hit);

            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnRightHitEnter(visitor, hit);
            hitCache[Hit.Right].hit = hit;
            hitCache[Hit.Right].visitable = hitv;
        }

        hitCache[Hit.Bottom].visitable?.AcceptOnRightHitStay(visitor, hit);
    }

    public void OnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit)
    {
        if (hitCache[Hit.Right].hit)
        {
            hitCache[Hit.Right].visitable?.AcceptOnRightHitExit(visitor, hitCache[Hit.Right].hit);
        }
        else
        {
            var hitv = hit.collider.transform.parent.GetComponent<IExRbVisitable>();
            hitv?.AcceptOnRightHitEnter(visitor, hit);
        }

        hitCache[Hit.Right].hit = default;
        hitCache[Hit.Right].visitable = default;
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
