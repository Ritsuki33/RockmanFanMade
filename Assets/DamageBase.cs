using UnityEngine;

public interface IDamageController
{
    void TakeDamage(DamageBase damage);
}

public class DamageBase : MonoBehaviour
{
    [SerializeField] public int baseDamageValue = 3;
    public  void Accept(IDamageController visitor)
    {
        visitor.TakeDamage(this);
    }
}
