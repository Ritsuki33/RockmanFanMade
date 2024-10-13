using UnityEngine;

public class RockBusterDamage : DamageBase
{
    [SerializeField] public Projectile projectile;

    public void DeleteBuster() => projectile.Delete();

    public override void Accept(IDamageController visitor)=> visitor.TakeDamage(this);
}
