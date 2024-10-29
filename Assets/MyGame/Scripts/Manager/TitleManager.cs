using UnityEngine;
public struct InputInfo
{
    public bool decide, up, down, left, right;
    public void SetInput(IInput input = null)
    {
        decide = input.GetDownInput(InputType.Decide);
        up = input.GetDownInput(InputType.Up);
        down = input.GetDownInput(InputType.Down);
        left = input.GetDownInput(InputType.Left);
        right = input.GetDownInput(InputType.Right);
    }

    public bool IsInput => decide | up | down | left | right;
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

    protected abstract void OnUpdate();

    protected abstract void Terminate();
}

public class TitleManager : BaseManager<TitleManager>
{
    [SerializeField] TitleScreen title;

    public enum ScreenType
    {
        Top
    }

    private IInput InputController => InputManager.Instance;

    private ScreenContainer<ScreenType> screenContainer= new ScreenContainer<ScreenType>();

    protected override void Init()
    {
        screenContainer.Add(ScreenType.Top, title);

        screenContainer.TransitScreen(ScreenType.Top, true);
    }

    protected override void OnUpdate()
    {
    }

    protected override void Terminate()
    {
        screenContainer.Clear();
    }
}
