using System;
using UnityEngine;

public class Boss : EnemyObject
{
    BaseObjectPool DeathEffectPool => EffectManager.Instance.DeathEffectPool;

    public Action<float> hpParamIncrementAnimation = default;
    public Action<float> hpChangeTrigger = default;

    public override void Damaged(int val)
    {
        base.Damaged(val);

        hpChangeTrigger?.Invoke((float)currentHp / MaxHp);
        //var presenter = GameMainManager.Instance.ScreenContainer.GetCurrentScreenPresenter<GameMainScreenPresenter>();
        //presenter?.SetEnemyHp((float)currentHp / MaxHp);
    }

    public override void OnDead()
    {
        var deathEffect = DeathEffectPool.Pool.Get().GetComponent<ParticleSystem>();
        deathEffect.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -3);

        deathEffect.Play();

        this.gameObject.SetActive(false);
    }
}
