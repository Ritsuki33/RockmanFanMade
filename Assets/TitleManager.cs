using UnityEngine;
using UnityEngine.InputSystem.XInput;

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
    public struct InputInfo
    {
        public bool decide;
        public void SetInput(IInput input = null)
        {
            decide = input.GetInput(InputType.Decide);
        }
    }

    [SerializeField] TitleScreen title;
    private IInput InputController => InputManager.Instance;

    protected override void Init()
    {
    }

    protected override void OnUpdate()
    {
        InputInfo inputInfo = default;
        inputInfo.SetInput(InputController);
        title.InputUpdate(inputInfo);
    }

    protected override void Terminate()
    {

    }
}
