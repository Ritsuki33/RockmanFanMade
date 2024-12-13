using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyObject : StageObject
{
    [SerializeField] EnemyData enemyData = default;

    [SerializeField,Header("討伐後発生イベントを直接指定")] UnityEvent defeatEvent = default;

    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;

    private Material material;

    protected int currentHp = 0;

    public int CurrentHp => currentHp;
    public int MaxHp => (enemyData != null) ? enemyData.Hp : 3;

    
    public virtual void Init()
    {
        SetMaterialParam(ShaderPropertyId.IsFadeColorID, 0);
        currentHp = MaxHp;
    }

    public virtual void Damaged(RockBusterDamage damageVal)
    {
        currentHp = Mathf.Clamp(currentHp - damageVal.baseDamageValue, 0, MaxHp);
        if (currentHp <= 0)
        {
            Dead();

            if (damageVal.baseDamageValue == 1)
            {
                damageVal.DeleteBuster();
            }
        }
        else
        {
            DamagedEffect();
            damageVal.DeleteBuster();
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    public void Dead()
    {
        //if (projectile.AttackPower < 3) projectile?.Delete();

        OnDead();

        defeatEvent?.Invoke();
        EventTriggerManager.Instance.Notify(EnemyEventType.Defeated, this);
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
    public virtual void DamagedEffect()
    {
        StartCoroutine(DamagedEffectCo());

        IEnumerator DamagedEffectCo()
        {
            //projectile?.Delete();
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
