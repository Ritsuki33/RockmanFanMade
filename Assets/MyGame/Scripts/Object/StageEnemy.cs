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

    public int CurrentHp => currentHp;
    public int MaxHp => (enemyData != null) ? enemyData.Hp : 3;

    Action<StageEnemy> _onDeleteCallback;

    protected override void Init()
    {
        base.Init();
        MainMaterial.SetFloat(ShaderPropertyId.IsFadeColorID, 0);
        currentHp = MaxHp;
    }

    public void Setup(Action<StageEnemy> onDeleteCallback)
    {
        _onDeleteCallback = onDeleteCallback;
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
        OnDead();

        _onDeleteCallback.Invoke(this);
        defeatEvent?.Invoke();
        EventTriggerManager.Instance.Notify(EnemyEventType.Defeated, this);
    }

    public virtual void OnDead()
    {
        ObjectManager.Instance.Create(ExplodeType.Explode1, Explode.Layer.None, 0, this.transform.position);
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
                MainMaterial.SetFloat(ShaderPropertyId.IsFadeColorID, 1);

                yield return CoroutineManager.Instance.PausableWaitForSeconds(0.05f);

                MainMaterial.SetFloat(ShaderPropertyId.IsFadeColorID, 0);

                yield return CoroutineManager.Instance.PausableWaitForSeconds(0.05f);
            }
            yield return null;
        }
    }
}
