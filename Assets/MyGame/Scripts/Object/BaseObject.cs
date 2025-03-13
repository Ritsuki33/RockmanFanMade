using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// オブジェクトインタプリター
/// </summary>
public interface IObjectInterpreter
{
    GameObject gameObject { get; }

    int Id { get; set; }
    void Init();
    void OnFixedUpdate();
    void OnLateFixedUpdate();
    void OnUpdate();
    void OnLateUpdate();

    void RequestPause(bool isPause);

    void Destroy();

    Action onDeleteCallback { set; }
    void Delete();
}



/// <summary>
/// ベースオブジェクト
/// </summary>
public class BaseObject : MonoBehaviour, IObjectInterpreter
{
    [SerializeField] bool outofCameraDelete = true;

    private int id = 0;
    private bool _isPause = false;
    public bool IsPause => _isPause;
    Action _onDeleteCallback;

    Action IObjectInterpreter.onDeleteCallback { set => _onDeleteCallback = value; }

    int IObjectInterpreter.Id { get => id; set => id = value; }
    public int ObjectId { get { return id; } }

    int pauseRequest = 0;


    void IObjectInterpreter.Init() => Init();
    void IObjectInterpreter.OnFixedUpdate() => OnFixedUpdate();
    void IObjectInterpreter.OnUpdate() => OnUpdate();
    void IObjectInterpreter.RequestPause(bool isPause) => RequestPause(isPause);
    void IObjectInterpreter.Destroy() => Destroy();
    void IObjectInterpreter.Delete() => Delete();
    void IObjectInterpreter.OnLateFixedUpdate() => OnLateFixedUpdate();
    void IObjectInterpreter.OnLateUpdate() => OnLateUpdate();

    protected virtual void OnFixedUpdate() { }
    protected virtual void OnLateFixedUpdate() { }
    protected virtual void OnLateUpdate() { }
    protected virtual void OnPause(bool isPause) { _isPause = isPause; }
    protected virtual void Destroy() { }
    protected virtual void OnReset() { }

    protected virtual void Init()
    {
        pauseRequest = 0;
        OnPause(false);
    }


    protected virtual void OnUpdate()
    {
        if (outofCameraDelete && GameMainManager.Instance.MainCameraControll.CheckOutOfView(gameObject))
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

    public virtual void Delete()
    {
        _onDeleteCallback?.Invoke();
    }
}
