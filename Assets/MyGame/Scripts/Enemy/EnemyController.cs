using UnityEngine;

public class EnemyController<T,E> : ExRbStateMachine<T>, IDamageController where T: ExRbStateMachine<T> where E: EnemyObject
{
    [SerializeField] protected E enemy;

    void IDamageController.TakeDamage(DamageBase damage)
    {
        enemy.Damaged(damage.baseDamageValue);
    }

    void IDamageController.TakeDamage(RockBusterDamage damage)
    {
        TakeDamage(damage);
    }

    protected virtual void TakeDamage(DamageBase damage) {
        enemy.Damaged(damage.baseDamageValue);
    }

    protected virtual void TakeDamage(RockBusterDamage damage)
    {
        enemy.Damaged(damage.baseDamageValue);

        if (damage.baseDamageValue == 1 || enemy.CurrentHp > 0) damage.DeleteBuster();
    }
}
