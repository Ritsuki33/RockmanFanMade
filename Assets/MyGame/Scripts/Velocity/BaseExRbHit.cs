using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseExRbHit : MonoBehaviour, IHitEvent
{
    protected internal interface IExRbCallbackSet
    {
        void AddOnHitEventCallback(IHitEvent createVelocity);
        void RemoveOnHitEventCallback(IHitEvent createVelocity);
    }

    protected virtual void OnEnable()
    {
        ExpandRigidBody exRb = GetComponent<ExpandRigidBody>();
        AddOnHitEventCallback(exRb);
    }

    protected virtual void OnDisable()
    {
        ExpandRigidBody exRb = GetComponent<ExpandRigidBody>();
        RemoveOnHitEventCallback(exRb);
    }

    void AddOnHitEventCallback(IExRbCallbackSet exRbCallbackSet)
    {
        exRbCallbackSet?.AddOnHitEventCallback(this);
    }
    void RemoveOnHitEventCallback(IExRbCallbackSet exRbCallbackSet)
    {
        exRbCallbackSet?.RemoveOnHitEventCallback(this);
    }

    void IHitEvent.BottomHitEnter(RaycastHit2D hit) { OnBottomHitEnter(hit); }
    void IHitEvent.TopHitEnter(RaycastHit2D hit) { OnTopHitEnter(hit); }
    void IHitEvent.LeftHitEnter(RaycastHit2D hit) { OnLeftHitEnter(hit); }
    void IHitEvent.RightHitEnter(RaycastHit2D hit) { OnRightHitEnter(hit); }
    void IHitEvent.BottomHitStay(RaycastHit2D hit) { OnBottomHitStay(hit); }
    void IHitEvent.TopHitStay(RaycastHit2D hit) { OnTopHitStay(hit); }
    void IHitEvent.LeftHitStay(RaycastHit2D hit) { OnLeftHitStay(hit); }
    void IHitEvent.RightHitStay(RaycastHit2D hit) { OnRightHitStay(hit); }
    void IHitEvent.BottomHitExit(RaycastHit2D hit) { OnBottomHitExit(hit); }
    void IHitEvent.TopHitExit(RaycastHit2D hit) { OnTopHitExit(hit); }
    void IHitEvent.LeftHitExit(RaycastHit2D hit) { OnLeftHitExit(hit); }
    void IHitEvent.RightHitExit(RaycastHit2D hit) { OnRightHitExit(hit); }

    protected virtual void OnBottomHitEnter(RaycastHit2D hit) { }
    protected virtual void OnTopHitEnter(RaycastHit2D hit) { }
    protected virtual void OnLeftHitEnter(RaycastHit2D hit) { }
    protected virtual void OnRightHitEnter(RaycastHit2D hit) { }
    protected virtual void OnBottomHitStay(RaycastHit2D hit) { }
    protected virtual void OnTopHitStay(RaycastHit2D hit) { }
    protected virtual void OnLeftHitStay(RaycastHit2D hit) { }
    protected virtual void OnRightHitStay(RaycastHit2D hit) { }
    protected virtual void OnBottomHitExit(RaycastHit2D hit) { }
    protected virtual void OnTopHitExit(RaycastHit2D hit) { }
    protected virtual void OnLeftHitExit(RaycastHit2D hit) { }
    protected virtual void OnRightHitExit(RaycastHit2D hit) { }

}
