using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageBoss : StageEnemy
{
    protected Action animationFinishCallback = null;

    public abstract void Appeare(Action finishCallback);
    public abstract void ToBattleState();

    public void SetHp(int hp)
    {
        statusParam.OnChangeHp(hp);
    }

    public void MaxRecovery(Action callback)
    {
        statusParam.OnRecovery(MaxHp, callback);
    }
}
