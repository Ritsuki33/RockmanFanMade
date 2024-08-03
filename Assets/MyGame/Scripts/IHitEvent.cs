using UnityEngine;

public interface IHitEvent
{
    void BottomHitStay(RaycastHit2D hit) { }
    void TopHitStay(RaycastHit2D hit) { }
    void LeftHitStay(RaycastHit2D hit) { }
    void RightHitStay(RaycastHit2D hit) {}
    void BottomHitExit(RaycastHit2D hit) { }
    void TopHitExit(RaycastHit2D hit) { }
    void LeftHitExit(RaycastHit2D hit) { }
    void RightHitExit(RaycastHit2D hit) { }
}
