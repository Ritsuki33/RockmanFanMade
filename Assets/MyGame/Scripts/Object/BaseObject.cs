using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトインタプリター
/// </summary>
interface IObjectInterpreter
{
    void OnFixedUpdate();
    void OnUpdate();

    void OnPause(bool isPause);
}


/// <summary>
/// ベースオブジェクト
/// </summary>
public class BaseObject : MonoBehaviour, IObjectInterpreter
{
    void IObjectInterpreter.OnFixedUpdate() => OnFixedUpdate();

    void IObjectInterpreter.OnUpdate() => OnUpdate();

    void IObjectInterpreter.OnPause(bool isPause) => OnPause(isPause);

    protected virtual void OnFixedUpdate() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPause(bool isPause) { }
}
