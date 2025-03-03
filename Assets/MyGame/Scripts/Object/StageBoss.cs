using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageBoss : StageEnemy
{
    ReactiveProperty<float> hp = new ReactiveProperty<float>(0);
    public ISubsribeOnlyReactiveProperty<float> Hp => hp;

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

    protected override void Destroy()
    {
        hp.Dispose();
    }

    public abstract void Appeare(Action finishCallback);
    public abstract void ToBattleState();
}
