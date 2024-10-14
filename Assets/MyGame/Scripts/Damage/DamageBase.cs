using UnityEngine;

public class DamageBase : MonoBehaviour, ITriggerVisitable
{
    [SerializeField] public int baseDamageValue = 3;
    
    public virtual void Accept(ITriggerVisitor visitor)
    {
        visitor.Take(this);
    }
}
