using UnityEngine;

public class DamageBase : MonoBehaviour, ITriggerVisitable
{
    [SerializeField] public int baseDamageValue = 3;

    public virtual void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    public virtual void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    public virtual void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    public virtual void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    public virtual void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    public virtual void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);

}
