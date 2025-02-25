using UnityEngine;

public class TitleScreen : BaseScreen<TitleScreen, TitleScreenPresenter, TitleManager.ScreenType>
{
    [SerializeField] MainMenuColorSelect select;

    public MainMenuColorSelect Select => select;

    protected override void Open()
    {
        FadeInManager.Instance.FadeInImmediate();
    }

    protected override void Hide()
    {
        FadeInManager.Instance.FadeOutImmediate();
    }

    protected override void Deinitialize()
    {
        select.Clear();
    }
}
