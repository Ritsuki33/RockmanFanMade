using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : StageObject
{
    [SerializeField] EnemyData enemyData = default;

    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;

    private Material material;

    protected int maxHp=> (enemyData != null) ? enemyData.Hp : 3;
    protected int currentHp = 0;

    public virtual void Init() {
        SetMaterialParam(ShaderPropertyId.IsFadeColorID, 0);
        currentHp = maxHp;
    }

    public void Attacked(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster") || collision.gameObject.CompareTag("ChargeShot"))
        {
            OnAttacked(collision);
        }
    }

    public virtual void OnAttacked(Collider2D collision)
    {
        var projectile = collision.gameObject.transform.parent.GetComponent<Projectile>();
        currentHp = Mathf.Clamp(currentHp - projectile.AttackPower, 0, maxHp);
        if (currentHp <= 0)
        {
            Dead(projectile);
        }
        else
        {
            Damaged(projectile);
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    public void Dead(Projectile projectile)
    {
        if (projectile.AttackPower < 3) projectile?.Delete();

        OnDead();

        EventTriggerManager.Instance.Notify(EventType.EnemyDefeated);
    }

    public virtual void OnDead()
    {
        var explode = ExplodePool.Pool.Get();
        explode.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// ダメージ演出
    /// </summary>
    /// <param name="collision"></param>
    public virtual void Damaged(Projectile projectile)
    {
        StartCoroutine(DamagedCo(projectile));

        IEnumerator DamagedCo(Projectile projectile)
        {
            projectile?.Delete();
            int count = 5;

            for (int i = 0; i < count; i++)
            {
                SetMaterialParam(ShaderPropertyId.IsFadeColorID, 1);

                yield return new WaitForSeconds(0.05f);

                SetMaterialParam(ShaderPropertyId.IsFadeColorID, 0);

                yield return new WaitForSeconds(0.05f);
            }
            yield return null;
        }
    }
}
