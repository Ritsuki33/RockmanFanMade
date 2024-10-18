using UnityEngine;

public interface ITriggerVisitor<T> where T: ITriggerVisitable
{
    void OnTriggerEnter(T collision);
    void OnTriggerStay(T collision);
    void OnTriggerExit(T collision);

    void OnCollisionEnter(T collision);
    void OnCollisionStay(T collision);
    void OnCollisionExit(T collision);
}

public partial interface ITriggerVisitor
{ }

public interface ITriggerVisitable
{
    void AcceptOnTriggerEnter(ITriggerVisitor visitor);
    void AcceptOnTriggerStay(ITriggerVisitor visitor);
    void AcceptOnTriggerExit(ITriggerVisitor visitor);

    void AcceptOnCollisionEnter(ITriggerVisitor visitor);
    void AcceptOnCollisionStay(ITriggerVisitor visitor);
    void AcceptOnCollisionExit(ITriggerVisitor visitor);
}


public partial interface IHitVisitor<T> where T : IHitVisitable
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

public partial interface IHitVisitor
{ }

public interface IHitVisitable
{
    void AcceptOnHitEnter(IHitVisitor visitor);
    void AcceptOnHitStay(IHitVisitor visitor);
    void AcceptOnHitExit(IHitVisitor visitor);

    void AcceptOnBottomHitEnter(IHitVisitor visitor);
    void AcceptOnBottomHitStay(IHitVisitor visitor);
    void AcceptOnBottomHitExit(IHitVisitor visitor);

    void AcceptOnTopHitEnter(IHitVisitor visitor);
    void AcceptOnTopHitStay(IHitVisitor visitor);
    void AcceptOnTopHitExit(IHitVisitor visitor);

    void AcceptOnLeftHitEnter(IHitVisitor visitor);
    void AcceptOnLeftHitStay(IHitVisitor visitor);
    void AcceptOnLeftHitExit(IHitVisitor visitor);

    void AcceptOnRightHitEnter(IHitVisitor visitor);
    void AcceptOnRightHitStay(IHitVisitor visitor);
    void AcceptOnRightHitExit(IHitVisitor visitor);
}