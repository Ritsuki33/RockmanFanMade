using UnityEngine;

public class PlayerTrigger : MonoBehaviour, ITriggerVisitable
{
    [SerializeField] PlayerController controller;

    public PlayerController PlayerController => controller;

    public void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    public void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    public void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    public void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    public void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    public void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);
}
