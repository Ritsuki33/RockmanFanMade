using UnityEngine;

public partial interface IRbVisitor
{ }

public interface IRbVisitable
{
    void AcceptOnTriggerEnter(IRbVisitor visitor);
    void AcceptOnTriggerStay(IRbVisitor visitor);
    void AcceptOnTriggerExit(IRbVisitor visitor);

    void AcceptOnCollisionEnter(IRbVisitor visitor);
    void AcceptOnCollisionStay(IRbVisitor visitor);
    void AcceptOnCollisionExit(IRbVisitor visitor);
}

public partial interface IExRbVisitor
{
    void OnHitEnter(RaycastHit2D hit) { }
    void OnBottomHitEnter(RaycastHit2D hit) { }
    void OnTopHitEnter(RaycastHit2D hit) { }
    void OnLeftHitEnter(RaycastHit2D hit) { }
    void OnRightHitEnter(RaycastHit2D hit) { }
    void OnHitStay(RaycastHit2D hit) { }
    void OnBottomHitStay(RaycastHit2D hit) { }
    void OnTopHitStay(RaycastHit2D hit) { }
    void OnLeftHitStay(RaycastHit2D hit) { }
    void OnRightHitStay(RaycastHit2D hit) { }
    void OnHitExit(RaycastHit2D hit) { }
    void OnBottomHitExit(RaycastHit2D hit) { }
    void OnTopHitExit(RaycastHit2D hit) { }
    void OnLeftHitExit(RaycastHit2D hit) { }
    void OnRightHitExit(RaycastHit2D hit) { }
}

public interface IExRbVisitable
{
    void AcceptOnHitEnter(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnHitStay(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnHitExit(IExRbVisitor visitor, RaycastHit2D hit);

    void AcceptOnBottomHitEnter(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnBottomHitStay(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnBottomHitExit(IExRbVisitor visitor, RaycastHit2D hit);

    void AcceptOnTopHitEnter(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnTopHitStay(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnTopHitExit(IExRbVisitor visitor, RaycastHit2D hit);

    void AcceptOnLeftHitEnter(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnLeftHitStay(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnLeftHitExit(IExRbVisitor visitor, RaycastHit2D hit);

    void AcceptOnRightHitEnter(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnRightHitStay(IExRbVisitor visitor, RaycastHit2D hit);
    void AcceptOnRightHitExit(IExRbVisitor visitor, RaycastHit2D hit);
}


