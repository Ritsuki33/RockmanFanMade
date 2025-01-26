using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ExRbHit : IHitEvent, IExRbVisitor
{
    Dictionary<RaycastHit2D, IExRbVisitable> onHitCache = new Dictionary<RaycastHit2D, IExRbVisitable>();

    public event Action<RaycastHit2D> onHitEnter;
    public event Action<RaycastHit2D> onBottomHitEnter;
    public event Action<RaycastHit2D> onTopHitEnter;
    public event Action<RaycastHit2D> onLeftHitEnter;
    public event Action<RaycastHit2D> onRightHitEnter;
    public event Action<RaycastHit2D> onHitStay;
    public event Action<RaycastHit2D> onBottomHitStay;
    public event Action<RaycastHit2D> onTopHitStay;
    public event Action<RaycastHit2D> onLeftHitStay;
    public event Action<RaycastHit2D> onRightHitStay;
    public event Action<RaycastHit2D> onHitExit;
    public event Action<RaycastHit2D> onBottomHitExit;
    public event Action<RaycastHit2D> onTopHitExit;
    public event Action<RaycastHit2D> onLeftHitExit;
    public event Action<RaycastHit2D> onRightHitExit;
    public event Action<RaycastHit2D, RaycastHit2D> onBottomTopHitEnter;
    public event Action<RaycastHit2D, RaycastHit2D> onBottomTopHitExit;
    public event Action<RaycastHit2D, RaycastHit2D> onBottomTopHitStay;
    public event Action<RaycastHit2D, RaycastHit2D> onLeftRightHitEnter;
    public event Action<RaycastHit2D, RaycastHit2D> onLeftRightHitExit;
    public event Action<RaycastHit2D, RaycastHit2D> onLeftRightHitStay;

    public void Init(IExRbCallbackSet exRbCallbackSet)
    {
        onHitCache.Clear();
        exRbCallbackSet.AddOnHitEventCallback(this);
    }


    public void Destory(IExRbCallbackSet exRbCallbackSet)
    {
        onHitCache.Clear();
        exRbCallbackSet.RemoveOnHitEventCallback(this);
    }

    void IHitEvent.OnHitEnter(RaycastHit2D hit)
    {
        onHitEnter?.Invoke(hit);

        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnHitEnter(this);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }

    void IHitEvent.OnHitStay(RaycastHit2D hit)
    {
        onHitStay?.Invoke(hit);

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

        hitv?.AcceptOnHitStay(this);
    }

    void IHitEvent.OnHitExit(RaycastHit2D hit)
    {
        onHitExit?.Invoke(hit);

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

        hitv?.AcceptOnHitExit(this);
    }

    void IHitEvent.OnBottomHitEnter(RaycastHit2D hit)
    {
        onBottomHitEnter?.Invoke(hit);

        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnBottomHitEnter(this);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }


    void IHitEvent.OnBottomHitStay(RaycastHit2D hit)
    {
        onBottomHitStay?.Invoke(hit);

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

        hitv?.AcceptOnBottomHitStay(this);
    }

    void IHitEvent.OnBottomHitExit(RaycastHit2D hit)
    {
        onBottomHitExit?.Invoke(hit);

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

        hitv?.AcceptOnBottomHitExit(this);

        // キャッシュを削除
        if (onHitCache.ContainsKey(hit)) onHitCache.Remove(hit);
    }

    void IHitEvent.OnTopHitEnter(RaycastHit2D hit)
    {
        onTopHitEnter?.Invoke(hit);

        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnTopHitEnter(this);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }

    void IHitEvent.OnTopHitStay(RaycastHit2D hit)
    {
        onTopHitStay?.Invoke(hit);
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

        hitv?.AcceptOnTopHitStay(this);
    }

    void IHitEvent.OnTopHitExit(RaycastHit2D hit)
    {
        onTopHitExit?.Invoke(hit);
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

        hitv?.AcceptOnTopHitExit(this);
    }

    void IHitEvent.OnLeftHitEnter(RaycastHit2D hit)
    {
        onLeftHitEnter?.Invoke(hit);

        var hitv = hit.collider.GetComponent<IExRbVisitable>();

        hitv?.AcceptOnLeftHitEnter(this);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }

    void IHitEvent.OnLeftHitStay(RaycastHit2D hit)
    {
        onLeftHitStay?.Invoke(hit);
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

        hitv?.AcceptOnLeftHitStay(this);
    }

    void IHitEvent.OnLeftHitExit(RaycastHit2D hit)
    {
        onLeftHitExit?.Invoke(hit);
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

        hitv?.AcceptOnLeftHitExit(this);
    }

    void IHitEvent.OnRightHitEnter(RaycastHit2D hit)
    {
        onRightHitEnter?.Invoke(hit);

        var hitv = hit.collider.GetComponent<IExRbVisitable>();
        hitv?.AcceptOnRightHitEnter(this);

        // キャッシュ
        if (!onHitCache.ContainsKey(hit)) onHitCache.Add(hit, hitv);
    }


    void IHitEvent.OnRightHitStay(RaycastHit2D hit)
    {
        onRightHitStay?.Invoke(hit);
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

        hitv?.AcceptOnRightHitStay(this);
    }

    void IHitEvent.OnRightHitExit(RaycastHit2D hit)
    {
        onRightHitExit?.Invoke(hit);

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

        hitv?.AcceptOnRightHitExit(this);
    }

    void IHitEvent.OnLeftRightHitEnter(RaycastHit2D leftHit, RaycastHit2D rightHit) => onLeftRightHitEnter?.Invoke(leftHit, rightHit);
    void IHitEvent.OnBottomTopHitEnter(RaycastHit2D bottomHit, RaycastHit2D topHit) => onBottomTopHitEnter?.Invoke(bottomHit, topHit);
    void IHitEvent.OnLeftRightHitExit(RaycastHit2D leftHit, RaycastHit2D rightHit) => onLeftRightHitExit?.Invoke(leftHit, rightHit);
    void IHitEvent.OnBottomTopHitExit(RaycastHit2D bottomHit, RaycastHit2D topHit) => onBottomTopHitExit?.Invoke(bottomHit, topHit);
    void IHitEvent.OnLeftRightHitStay(RaycastHit2D leftHit, RaycastHit2D rightHit) => onLeftRightHitStay?.Invoke(leftHit, rightHit);
    void IHitEvent.OnBottomTopHitStay(RaycastHit2D bottomHit, RaycastHit2D topHit) => onBottomTopHitStay?.Invoke(bottomHit, topHit);


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

public partial interface IHitInterpreter : IHitInterpreter<RaycastHit2D>
{
    public void OnLeftRightHitEnter(RaycastHit2D leftHit, RaycastHit2D rightHit) { }
    public void OnBottomTopHitEnter(RaycastHit2D bottomHit, RaycastHit2D topHit) { }
    public void OnLeftRightHitExit(RaycastHit2D leftHit, RaycastHit2D rightHit) { }
    public void OnBottomToptHitExit(RaycastHit2D bottomHit, RaycastHit2D topHit) { }
    public void OnLeftRightHitStay(RaycastHit2D leftHit, RaycastHit2D rightHit) { }
    public void OnBottomToptHitStay(RaycastHit2D bottomHit, RaycastHit2D topHit) { }
}