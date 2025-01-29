using UnityEngine;

public interface IHitEvent
{
    void OnHitEnter(RaycastHit2D hit);
    void OnBottomHitEnter(RaycastHit2D hit);
    void OnTopHitEnter(RaycastHit2D hit);
    void OnLeftHitEnter(RaycastHit2D hit);
    void OnRightHitEnter(RaycastHit2D hit);
    void OnHitStay(RaycastHit2D hit);
    void OnBottomHitStay(RaycastHit2D hit);
    void OnTopHitStay(RaycastHit2D hit);
    void OnLeftHitStay(RaycastHit2D hit);
    void OnRightHitStay(RaycastHit2D hit);
    void OnHitExit(RaycastHit2D hit);
    void OnBottomHitExit(RaycastHit2D hit);
    void OnTopHitExit(RaycastHit2D hit);
    void OnLeftHitExit(RaycastHit2D hit);
    void OnRightHitExit(RaycastHit2D hit);
}

public interface IExRbCallbackSet
{
    void AddOnHitEventCallback(IHitEvent hitEvent);
    void RemoveOnHitEventCallback(IHitEvent hitEvent);
}
