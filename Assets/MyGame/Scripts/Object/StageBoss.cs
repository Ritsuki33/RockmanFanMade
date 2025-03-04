using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageBoss : StageEnemy, IBossSubject
{
    ReactiveProperty<float> hp = new ReactiveProperty<float>(0);
    ReactiveProperty<int> recovery = new ReactiveProperty<int>(0);
    protected Action animationFinishCallback = null;
    ISubsribeOnlyReactiveProperty<float> IBossSubject.Hp => hp;
    ISubsribeOnlyReactiveProperty<int> IBossSubject.Recovery => recovery;
    public override int CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            hp.Value = (float)currentHp / MaxHp;
        }
    }

    Action IBossSubject.AnimationFinishCallback => animationFinishCallback;

    protected override void Destroy()
    {
        hp.Dispose();
    }

    public abstract void Appeare(Action finishCallback);
    public abstract void ToBattleState();

    public void SetHp(int hp)
    {
        CurrentHp = hp;
    }

    public void MaxRecovery(Action finishCallback)
    {
        this.recovery.Value = MaxHp;
        this.CurrentHp += MaxHp;

        this.animationFinishCallback = finishCallback;
    }
}
