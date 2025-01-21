using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageBoss : StageEnemy
{
    protected Action<float> hpChangeTrigger = default;
    public Action<float> HpChangeTrigger { get { return hpChangeTrigger; } set { hpChangeTrigger = value; } }

    public abstract void Appeare(Action finishCallback);
    public abstract void ToBattleState();
}
