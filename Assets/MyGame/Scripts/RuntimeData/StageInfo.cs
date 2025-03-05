using System;
using UnityEngine;

public interface IStageInfoSubject
{
    event Action<StageBoss> OnSetBossHolder;
}

public class StageInfo : MonoBehaviour, IStageInfoSubject
{
    private StageBoss stageBossHolder;

    public ParamStatus StageBossParam => stageBossHolder.statusParam;

    public event Action<StageBoss> OnSetBossHolder = default;

    public void SetBossHolder(StageBoss stageBoss)
    {
        stageBossHolder = stageBoss;
        OnSetBossHolder?.Invoke(stageBoss);
    }

}
