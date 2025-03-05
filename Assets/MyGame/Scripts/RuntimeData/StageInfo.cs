using System;
using UnityEngine;

public interface IStageInfoSubject
{
    event Action OnSetBossHolder;
}

public class StageInfo : MonoBehaviour, IStageInfoSubject
{
    private StageBoss stageBossHolder;

    public ParamStatus StageBossParam => (stageBossHolder != null) ? stageBossHolder.statusParam : null;

    public event Action OnSetBossHolder = default;

    public void SetBossHolder(StageBoss stageBoss)
    {
        stageBossHolder = stageBoss;
        OnSetBossHolder?.Invoke();
    }

}
