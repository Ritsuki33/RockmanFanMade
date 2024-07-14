using UnityEngine;

public interface IHitEvent
{
    void OnBottomHitStay(RaycastHit2D hit) {}
    void OnTopHitStay(RaycastHit2D hit) { }
    void OnLeftHitStay(RaycastHit2D hit) { }
    void OnRightHitStay(RaycastHit2D hit) {}
    void OnBottomHitExit(RaycastHit2D hit) { }
    void OnTopHitExit(RaycastHit2D hit) { }
    void OnLeftHitExit(RaycastHit2D hit) { }
    void OnRightHitExit(RaycastHit2D hit) { }
}
