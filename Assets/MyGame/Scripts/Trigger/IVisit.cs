using UnityEngine;

public interface IRbVisitor<T> where T : IRbVisitable
{
    void OnTriggerEnter(T collision) { }
    void OnTriggerStay(T collision) { }
    void OnTriggerExit(T collision) { }

    void OnCollisionEnter(T collision) { }
    void OnCollisionStay(T collision) { }
    void OnCollisionExit(T collision) { }
}

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


public partial interface IExRbVisitor<T>
{
    void OnHitEnter(T obj, RaycastHit2D hit){}
    void OnBottomHitEnter(T obj, RaycastHit2D hit){}
    void OnTopHitEnter(T obj, RaycastHit2D hit){}
    void OnLeftHitEnter(T obj, RaycastHit2D hit){}
    void OnRightHitEnter(T obj, RaycastHit2D hit){}
    void OnHitStay(T obj, RaycastHit2D hit){}
    void OnBottomHitStay(T obj, RaycastHit2D hit){}
    void OnTopHitStay(T obj, RaycastHit2D hit){}
    void OnLeftHitStay(T obj, RaycastHit2D hit){}
    void OnRightHitStay(T obj, RaycastHit2D hit){}
    void OnHitExit(T obj, RaycastHit2D hit){}
    void OnBottomHitExit(T obj, RaycastHit2D hit){}
    void OnTopHitExit(T obj, RaycastHit2D hit){}
    void OnLeftHitExit(T obj, RaycastHit2D hit){}
    void OnRightHitExit(T obj, RaycastHit2D hit){}
}

public partial interface IExRbVisitor
{ }

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


