using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMainSubject
{
    ISubsribeOnlyReactiveProperty<StageBoss> BossHolder { get; }
}

public interface IPlayerSubject
{
    ISubsribeOnlyReactiveProperty<float> Hp { get; }
    ISubsribeOnlyReactiveProperty<int> Recovery { get; }
}

public interface IBossSubject
{
    ISubsribeOnlyReactiveProperty<float> Hp { get; }
    ISubsribeOnlyReactiveProperty<int> Recovery { get; }

    Action AnimationFinishCallback { get; }
}

public class UIObserver : Singleton<UIObserver>
{
    public IGameMainSubject GameMainObserver => GameMainManager.Instance;
    public IPlayerSubject playerObserver => WorldManager.Instance.Player;
    public IBossSubject bossObserver = null;
}
