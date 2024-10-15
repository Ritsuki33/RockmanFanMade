//using UnityEngine;

//public class EnemyController<T,E> : ExRbStateMachine<T>, ITriggerVisitor where T: ExRbStateMachine<T> where E: EnemyObject
//{
//    [SerializeField] protected E enemy;

//    void ITriggerVisitor<DamageBase>.OnTriggerEnter(DamageBase damage)
//    {
//        enemy.Damaged(damage.baseDamageValue);
//    }

//    void ITriggerVisitor<RockBusterDamage>.OnTriggerEnter(RockBusterDamage damage)
//    {
//        Take(damage);
//    }

//    protected virtual void Take(DamageBase damage) {
//        enemy.Damaged(damage.baseDamageValue);
//    }

//    protected virtual void Take(RockBusterDamage damage)
//    {
//        enemy.Damaged(damage.baseDamageValue);

//        if (damage.baseDamageValue == 1 || enemy.CurrentHp > 0) damage.DeleteBuster();
//    }
//}
