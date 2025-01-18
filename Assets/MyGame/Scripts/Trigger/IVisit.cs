using UnityEngine;

public interface IRbVisitor<T> where T : IRbVisitable
{
    void OnTriggerEnter(T collision);
    void OnTriggerStay(T collision);
    void OnTriggerExit(T collision);

    void OnCollisionEnter(T collision);
    void OnCollisionStay(T collision);
    void OnCollisionExit(T collision);
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
    void OnHitEnter(T hit);
    void OnBottomHitEnter(T hit);
    void OnTopHitEnter(T hit);
    void OnLeftHitEnter(T hit);
    void OnRightHitEnter(T hit);
    void OnHitStay(T hit);
    void OnBottomHitStay(T hit);
    void OnTopHitStay(T hit);
    void OnLeftHitStay(T hit);
    void OnRightHitStay(T hit);
    void OnHitExit(T hit);
    void OnBottomHitExit(T hit);
    void OnTopHitExit(T hit);
    void OnLeftHitExit(T hit);
    void OnRightHitExit(T hit);
}

public partial interface IExRbVisitor
{ }

public interface IExRbVisitable
{
    void AcceptOnHitEnter(IExRbVisitor visitor);
    void AcceptOnHitStay(IExRbVisitor visitor);
    void AcceptOnHitExit(IExRbVisitor visitor);

    void AcceptOnBottomHitEnter(IExRbVisitor visitor);
    void AcceptOnBottomHitStay(IExRbVisitor visitor);
    void AcceptOnBottomHitExit(IExRbVisitor visitor);

    void AcceptOnTopHitEnter(IExRbVisitor visitor);
    void AcceptOnTopHitStay(IExRbVisitor visitor);
    void AcceptOnTopHitExit(IExRbVisitor visitor);

    void AcceptOnLeftHitEnter(IExRbVisitor visitor);
    void AcceptOnLeftHitStay(IExRbVisitor visitor);
    void AcceptOnLeftHitExit(IExRbVisitor visitor);

    void AcceptOnRightHitEnter(IExRbVisitor visitor);
    void AcceptOnRightHitStay(IExRbVisitor visitor);
    void AcceptOnRightHitExit(IExRbVisitor visitor);
}


