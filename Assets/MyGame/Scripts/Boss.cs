using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyObject
{
    BaseObjectPool DeathEffectPool => EffectManager.Instance.DeathEffectPool;
    public override void Damaged(int val)
    {
        base.Damaged(val);
        GameMainManager.Instance.EmemyHpBar.SetParam((float)currentHp / maxHp);
    }

    public override void OnDead()
    {
        var deathEffect = DeathEffectPool.Pool.Get().GetComponent<ParticleSystem>();
        deathEffect.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -3);

        deathEffect.Play();

        this.gameObject.SetActive(false);
    }
}
