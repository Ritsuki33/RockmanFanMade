using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Environment : Singleton<Environment>, IPlayerEnvironment
{
    /// <summary>
    /// プレイヤーの最大体力
    /// </summary>
    public int MaxHp { get; set; } = 27;

    /// <summary>
    /// プレイヤーの現在の体力
    /// </summary>
    int currentPlayerHp = 0;
    public int CurrentHp
    {
        get
        {
            return currentPlayerHp;
        }
        set
        {
            currentPlayerHp = Mathf.Clamp(value, 0, MaxHp);
            hp.Value = (float)currentPlayerHp / MaxHp;
        }
    }

    /// <summary>
    /// プレイヤーの体力（0~1）
    /// </summary>
    public ReactiveProperty<float> hp { get; set; } = new ReactiveProperty<float>(0);

    public ReactiveProperty<int> recovery { get; set; } = new ReactiveProperty<int>(0);

    private Action _hpRecoveryAnimationFinish = null;
    public Action hpRecoveryAnimationFinish { get => _hpRecoveryAnimationFinish; set => _hpRecoveryAnimationFinish = value; }
}
