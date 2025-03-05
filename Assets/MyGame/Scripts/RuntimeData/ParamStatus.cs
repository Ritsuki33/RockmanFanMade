using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IParamStatus
{
    int Hp { get; }
    int MaxHp { get; }
    void OnRefresh();
    // イベントの正しい定義方法
    event Action<int, int> HpChangeCallback;
    event Action<int, int> OnDamageCallback;
    event Action<int, int, Action> OnRecoveryCallback;

    void SetMaxHp(int maxHp);
    void OnChangeHp(int hp);
    void OnDamage(int damage);
    void OnRecovery(int recovery, Action callback);
}

public class ParamStatus : IParamStatus
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
        if (m_hp > m_maxHp) m_hp = m_maxHp;
    }

    public void OnChangeHp(int hp)
    {
        m_hp = hp;
        if (m_hp > m_maxHp) m_hp = m_maxHp;
        HpChangeCallback?.Invoke(m_hp, m_maxHp);
    }

    // ダメージを受けた時
    public void OnDamage(int damage)
    {
        m_hp -= damage;
        if (m_hp < 0) m_hp = 0;
        OnDamageCallback?.Invoke(m_hp, m_maxHp);
    }

    // 回復した時
    public void OnRecovery(int recovery, Action callback)
    {
        m_hp += recovery;
        if (m_hp > m_maxHp) m_hp = m_maxHp;
        OnRecoveryCallback?.Invoke(m_hp, m_maxHp, callback);
    }

    public void OnRefresh()
    {
        OnChangeHp(m_hp);
    }
}
