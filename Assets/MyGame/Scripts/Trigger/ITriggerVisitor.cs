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

public interface ITriggerVisitor
    : ITriggerVisitor<PlayerTrigger>, ITriggerVisitor<DamageBase>, ITriggerVisitor<RockBusterDamage>
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
