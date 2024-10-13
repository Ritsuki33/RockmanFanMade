using UnityEngine;

public interface IDamageController
{
    void TakeDamage(DamageBase damage);
    void TakeDamage(RockBusterDamage damage) { TakeDamage(damage as DamageBase); }
}

public class DamageBase : MonoBehaviour
{
    [SerializeField] public int baseDamageValue = 3;
    
    public virtual void Accept(IDamageController visitor)
    {
        visitor.TakeDamage(this);
    }

}
