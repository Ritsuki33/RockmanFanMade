using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModelController
{
    float Speed { get; set; }
    void OnPlay(int hash);
    void OnPause(bool IsPause);
    bool IsPlaying(int hash);
}

public abstract class ModelController : MonoBehaviour, IModelController
{
    public abstract float Speed { get; set; }
    public abstract bool IsPlaying(int hash);
    public abstract void OnPause(bool IsPause);
    public abstract void OnPlay(int hash);
}
