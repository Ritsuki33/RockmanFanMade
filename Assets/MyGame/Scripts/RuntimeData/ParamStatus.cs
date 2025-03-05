using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IParamStatusSubject
{
    void OnRefresh();
    // イベントの正しい定義方法
    event Action<int, int> HpChangeCallback;
    event Action<int, int> OnDamageCallback;
    event Action<int, int, Action> OnRecoveryCallback;
}

public class ParamStatus : IParamStatusSubject
{
    private int m_hp;
    private int m_maxHp;

    public int Hp => m_hp;
    public int MaxHp => m_maxHp;

    public event Action<int, int> HpChangeCallback = default;
    public event Action<int, int> OnDamageCallback = default;
    public event Action<int, int, Action> OnRecoveryCallback = default;

    public ParamStatus(int hp, int maxHp)
    {
        m_hp = hp;
        m_maxHp = maxHp;
    }

    public void SetMaxHp(int maxHp)
    {
        m_maxHp = maxHp;
    }

    public void OnChangeHp(int hp)
    {
        m_hp = hp;
        HpChangeCallback?.Invoke(m_hp, m_maxHp);
    }

    // ダメージを受けた時
    public void OnDamage(int damage)
    {
        m_hp -= damage;
        OnDamageCallback?.Invoke(m_hp, m_maxHp);
    }

    // 回復した時
    public void OnRecovery(int recovery, Action callback)
    {
        m_hp += recovery;
        OnRecoveryCallback?.Invoke(m_hp, m_maxHp, callback);
    }

    public void OnRefresh()
    {
        OnChangeHp(m_hp);
    }
}
