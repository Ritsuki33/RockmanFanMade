using UnityEngine;

/// <summary>
/// オブジェクトインタプリター
/// </summary>
public interface IObjectInterpreter
{
    GameObject gameObject { get; }

    void Init();
    void OnFixedUpdate();
    void OnUpdate();

    void OnPause(bool isPause);

    void Destroy();

    void OnReset();
}


/// <summary>
/// ベースオブジェクト
/// </summary>
public class BaseObject : MonoBehaviour, IObjectInterpreter
{
    void IObjectInterpreter.Init() => Init();
    void IObjectInterpreter.OnFixedUpdate() => OnFixedUpdate();
    void IObjectInterpreter.OnUpdate() => OnUpdate();
    void IObjectInterpreter.OnPause(bool isPause) => OnPause(isPause);
    void IObjectInterpreter.Destroy() => Destroy();
    void IObjectInterpreter.OnReset() => OnReset();

    protected virtual void Init() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPause(bool isPause) { }
    protected virtual void Destroy() { }
    protected virtual void OnReset() { }
}
