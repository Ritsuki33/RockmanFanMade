using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StageEnemy : PhysicalObject
{
    [SerializeField] EnemyData enemyData = default;

    [SerializeField,Header("討伐後発生イベントを直接指定")] UnityEvent defeatEvent = default;

    private Material material;

    protected int currentHp = 0;

    public virtual int CurrentHp { get; set; }
    public int MaxHp => (enemyData != null) ? enemyData.Hp : 3;


    protected override void Init()
    {
        base.Init();
        MainMaterial.SetFloat(ShaderPropertyId.IsFadeColorID, 0);
        CurrentHp = MaxHp;
    }


    public virtual void Damaged(RockBusterDamage damageVal)
    {
        CurrentHp = Mathf.Clamp(CurrentHp - damageVal.baseDamageValue, 0, MaxHp);
        if (CurrentHp <= 0)
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

            AudioManager.Instance.PlaySe(SECueIDs.athit);
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    public void Dead()
    {
        OnDead();

        defeatEvent?.Invoke();
        EventTriggerManager.Instance.Notify(EnemyEventType.Defeated, this);
    }

    public virtual void OnDead()
    {
        var explode=ObjectManager.Instance.OnGet<Explode>(PoolType.Explode);
        explode.Setup(Explode.Layer.None, this.transform.position, 0);

        Delete();
       
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
                MainMaterial.SetFloat(ShaderPropertyId.IsFadeColorID, 1);

                yield return PauseManager.Instance.PausableWaitForSeconds(0.05f);

                MainMaterial.SetFloat(ShaderPropertyId.IsFadeColorID, 0);

                yield return PauseManager.Instance.PausableWaitForSeconds(0.05f);
            }
            yield return null;
        }
    }
}
