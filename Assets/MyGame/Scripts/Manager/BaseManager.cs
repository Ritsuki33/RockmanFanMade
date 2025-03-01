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
    public void Init();
    public void OnUpdate();
    public void Terminate();

    public void SetActive(bool active);
}

public abstract class BaseManager<T> : SingletonComponent<T>, IManager where T : MonoBehaviour
{
    void IManager.Init()
    {
        Init();
    }

    void IManager.OnUpdate()
    {
        OnUpdate();
    }

    void IManager.Terminate()
    {
        Terminate();
    }

    void IManager.SetActive(bool active)
    {
        gameObject.SetActive(active);
    }


    protected abstract void Init();

    protected virtual void OnFixedUpdate() { }
    protected abstract void OnUpdate();

    protected abstract void Terminate();
}
