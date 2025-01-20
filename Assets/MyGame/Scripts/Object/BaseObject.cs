using System;
using System.Runtime.ConstrainedExecution;
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

    Action onDeleteCallback { set; }
    void Delete();
}



/// <summary>
/// ベースオブジェクト
/// </summary>
public class BaseObject : MonoBehaviour, IObjectInterpreter
{
    [SerializeField] bool outofCameraDelete = true;

    private bool _isPause = false;
    public bool IsPause => _isPause;
    Action _onDeleteCallback;

    Action IObjectInterpreter.onDeleteCallback { set => _onDeleteCallback = value; }

    int pauseRequest = 0;

    void IObjectInterpreter.Init() => Init();
    void IObjectInterpreter.OnFixedUpdate() => OnFixedUpdate();
    void IObjectInterpreter.OnUpdate() => OnUpdate();
    void IObjectInterpreter.RequestPause(bool isPause) => RequestPause(isPause);
    void IObjectInterpreter.Destroy() => Destroy();
    void IObjectInterpreter.OnReset() => OnReset();
    void IObjectInterpreter.Delete() => Delete();

    protected virtual void Init() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnPause(bool isPause) { _isPause = isPause; }
    protected virtual void Destroy() { }
    protected virtual void OnReset() { }

    protected virtual void OnUpdate()
    {
        if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(gameObject))
        {
            Delete();
        }
    }

    /// <summary>
    /// ポーズのリクエスト
    /// </summary>
    /// <param name="isPause"></param>
    public void RequestPause(bool isPause)
    {
        if (isPause)
        {
            if (pauseRequest == 0) OnPause(true);
            pauseRequest++;
        }
        else
        {
            pauseRequest--;
            if (pauseRequest <= 0)
            {
                pauseRequest = 0;
                OnPause(false);
            }
        }
    }

    public void Setup(Action onDeleteCallback)
    {
        _onDeleteCallback = onDeleteCallback;
    }

    public virtual void Delete()
    {
        _onDeleteCallback?.Invoke();
    }

}
