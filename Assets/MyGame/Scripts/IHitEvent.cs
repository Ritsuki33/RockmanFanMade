using UnityEngine;

public interface IHitEvent
{
    void HitEnter(RaycastHit2D hit) { }
    void BottomHitEnter(RaycastHit2D hit) { }
    void TopHitEnter(RaycastHit2D hit) { }
    void LeftHitEnter(RaycastHit2D hit) { }
    void RightHitEnter(RaycastHit2D hit) { }
    void HitStay(RaycastHit2D hit) { }
    void BottomHitStay(RaycastHit2D hit) { }
    void TopHitStay(RaycastHit2D hit) { }
    void LeftHitStay(RaycastHit2D hit) { }
    void RightHitStay(RaycastHit2D hit) {}
    void HitExit(RaycastHit2D hit) { }
    void BottomHitExit(RaycastHit2D hit) { }
    void TopHitExit(RaycastHit2D hit) { }
    void LeftHitExit(RaycastHit2D hit) { }
    void RightHitExit(RaycastHit2D hit) { }

    void LeftRightHitEnter(RaycastHit2D leftHit, RaycastHit2D rightHit) { }
    void BottomToptHitEnter(RaycastHit2D bottomHit, RaycastHit2D topHit) { }

    void LeftRightHitExit(RaycastHit2D leftHit, RaycastHit2D rightHit) { }
    void BottomToptHitExit(RaycastHit2D bottomHit, RaycastHit2D topHit) { }

    void LeftRightHitStay(RaycastHit2D leftHit, RaycastHit2D rightHit) { }
    void BottomToptHitStay(RaycastHit2D bottomHit, RaycastHit2D topHit) { }
}
