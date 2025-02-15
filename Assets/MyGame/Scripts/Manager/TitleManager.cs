using Unity.VisualScripting;
using UnityEngine;

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
        screenContainer.OnUpdate();
    }

    protected override void Terminate()
    {
        screenContainer.Clear();
    }

    public void TransitToBossSelect()
    {
        screenContainer.Close(true, () =>
        {
            SceneManager.Instance.ChangeManager(ManagerType.BossSelect);
        });
    }
}
