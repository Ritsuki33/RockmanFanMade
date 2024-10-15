using UnityEngine;

public class RockBusterDamage : DamageBase
{
    [SerializeField] public Projectile projectile;

    public void DeleteBuster() => projectile.Delete();

    public override void AcceptOnTriggerEnter(ITriggerVisitor visitor) => visitor.OnTriggerEnter(this);
    public override void AcceptOnCollisionEnter(ITriggerVisitor visitor) => visitor.OnCollisionEnter(this);
    public override void AcceptOnCollisionExit(ITriggerVisitor visitor) => visitor.OnCollisionExit(this);
    public override void AcceptOnCollisionStay(ITriggerVisitor visitor) => visitor.OnCollisionStay(this);
    public override void AcceptOnTriggerExit(ITriggerVisitor visitor) => visitor.OnTriggerExit(this);
    public override void AcceptOnTriggerStay(ITriggerVisitor visitor) => visitor.OnTriggerStay(this);
}
