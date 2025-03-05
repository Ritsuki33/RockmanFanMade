using System.Collections;
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

    private ScreenContainer<ScreenType> screenContainer = new ScreenContainer<ScreenType>();

    protected override IEnumerator Init()
    {
        screenContainer.Add(ScreenType.Top, title);

        screenContainer.TransitScreen(ScreenType.Top, true);
        yield return null;
    }

    protected override void OnUpdate()
    {
        screenContainer.OnUpdate();
    }

    protected override IEnumerator Dispose()
    {
        screenContainer.Clear();
        yield return null;
    }

    public void TransitToBossSelect()
    {
        screenContainer.Close(true, () =>
        {
            SceneManager.Instance.ChangeManager(ManagerType.BossSelect);
        });
    }
}
