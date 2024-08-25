using UnityEngine;

interface IBaseExRbHit : IHitEvent
{
    protected internal interface IExRbCallbackSet
    {
        void AddOnHitEventCallback(IHitEvent createVelocity);
        void RemoveOnHitEventCallback(IHitEvent createVelocity);
    }

    void AddOnHitEventCallback(IExRbCallbackSet exRbCallbackSet)
    {
        exRbCallbackSet?.AddOnHitEventCallback(this);
    }
    void RemoveOnHitEventCallback(IExRbCallbackSet exRbCallbackSet)
    {
        exRbCallbackSet?.RemoveOnHitEventCallback(this);
    }

    void OnEnable(GameObject obj)
    {
        ExpandRigidBody exRb = obj.GetComponent<ExpandRigidBody>();
        AddOnHitEventCallback(exRb);
    }

    void OnDisable(GameObject obj)
    {
        ExpandRigidBody exRb = obj.GetComponent<ExpandRigidBody>();
        RemoveOnHitEventCallback(exRb);
    }

    void IHitEvent.BottomHitEnter(RaycastHit2D hit) => OnBottomHitEnter(hit);
    void IHitEvent.TopHitEnter(RaycastHit2D hit) => OnTopHitEnter(hit);
    void IHitEvent.LeftHitEnter(RaycastHit2D hit) => OnLeftHitEnter(hit);
    void IHitEvent.RightHitEnter(RaycastHit2D hit) => OnRightHitEnter(hit);
    void IHitEvent.BottomHitStay(RaycastHit2D hit) => OnBottomHitStay(hit);
    void IHitEvent.TopHitStay(RaycastHit2D hit) => OnTopHitStay(hit);
    void IHitEvent.LeftHitStay(RaycastHit2D hit) => OnLeftHitStay(hit);
    void IHitEvent.RightHitStay(RaycastHit2D hit) => OnRightHitStay(hit);
    void IHitEvent.BottomHitExit(RaycastHit2D hit) => OnBottomHitExit(hit);
    void IHitEvent.TopHitExit(RaycastHit2D hit) => OnTopHitExit(hit);
    void IHitEvent.LeftHitExit(RaycastHit2D hit) => OnLeftHitExit(hit);
    void IHitEvent.RightHitExit(RaycastHit2D hit) => OnRightHitExit(hit);

    void OnBottomHitEnter(RaycastHit2D hit) { }
    void OnTopHitEnter(RaycastHit2D hit) { }
    void OnLeftHitEnter(RaycastHit2D hit) { }
    void OnRightHitEnter(RaycastHit2D hit) { }
    void OnBottomHitStay(RaycastHit2D hit) { }
    void OnTopHitStay(RaycastHit2D hit) { }
    void OnLeftHitStay(RaycastHit2D hit) { }
    void OnRightHitStay(RaycastHit2D hit) { }
    void OnBottomHitExit(RaycastHit2D hit) { }
    void OnTopHitExit(RaycastHit2D hit) { }
    void OnLeftHitExit(RaycastHit2D hit) { }
    void OnRightHitExit(RaycastHit2D hit) { }
}
