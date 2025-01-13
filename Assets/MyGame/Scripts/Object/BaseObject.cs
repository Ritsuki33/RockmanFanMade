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

    void RequestPause(bool isPause);

    void Destroy();

    void OnReset();
}


/// <summary>
/// ベースオブジェクト
/// </summary>
public class BaseObject : MonoBehaviour, IObjectInterpreter
{
    private bool _isPause = false;
    public bool IsPause => _isPause;

    int pauseRequest = 0;
    void IObjectInterpreter.Init() => Init();
    void IObjectInterpreter.OnFixedUpdate() => OnFixedUpdate();
    void IObjectInterpreter.OnUpdate() => OnUpdate();
    void IObjectInterpreter.RequestPause(bool isPause) => RequestPause(isPause);
    void IObjectInterpreter.Destroy() => Destroy();
    void IObjectInterpreter.OnReset() => OnReset();

    protected virtual void Init() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPause(bool isPause) { _isPause = isPause; }
    protected virtual void Destroy() { }
    protected virtual void OnReset() { }

    /// <summary>
    /// ポーズのリクエスト
    /// </summary>
    /// <param name="isPause"></param>
    public void RequestPause(bool isPause) {
        if (isPause)
        {
            pauseRequest++;
        }
        else
        {
            pauseRequest--;
            if (pauseRequest < 0) pauseRequest = 0;
        }

        // リクエストが1つ以上ならポーズ
        OnPause(pauseRequest > 0);
    }


}
