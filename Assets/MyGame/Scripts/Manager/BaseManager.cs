using System.Collections;
using UnityEngine;

public struct InputInfo
{
    public bool decide, up, down, left, right, start, select;
    public void SetInput(IInput input = null)
    {
        decide = input.GetDownInput(InputType.Decide);
        up = input.GetDownInput(InputType.Up);
        down = input.GetDownInput(InputType.Down);
        left = input.GetDownInput(InputType.Left);
        right = input.GetDownInput(InputType.Right);
        start = input.GetDownInput(InputType.Start);
        select = input.GetDownInput(InputType.Select);
    }

    public bool IsInput => decide | up | down | left | right | start | select;
}
public interface IManager
{
    IEnumerator Init();
    IEnumerator OnStart();
    void OnUpdate();
    IEnumerator Dispose();
    IEnumerator OnEnd();
    void SetActive(bool active);
}

public abstract class BaseManager<T> : SingletonComponent<T>, IManager where T : MonoBehaviour
{
    IEnumerator IManager.Init() { yield return Init(); }
    IEnumerator IManager.OnStart() { yield return OnStart(); }
    void IManager.OnUpdate() => OnUpdate();
    IEnumerator IManager.OnEnd() { yield return OnEnd(); }
    IEnumerator IManager.Dispose() { yield return Dispose(); }
    void IManager.SetActive(bool active) => gameObject.SetActive(active);


    protected abstract IEnumerator Init();
    protected virtual IEnumerator OnStart() { yield return null; }
    protected virtual void OnFixedUpdate() { }
    protected abstract void OnUpdate();
    protected abstract IEnumerator Dispose();
    protected virtual IEnumerator OnEnd() { yield return null; }
}
