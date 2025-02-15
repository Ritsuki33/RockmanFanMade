using UnityEngine;

public class TitleScreen : BaseScreen<TitleScreen, TitleScreenPresenter, TitleScreenViewModel, TitleManager.ScreenType>
{
    [SerializeField] MainMenuColorSelect select;

    public MainMenuColorSelect Select => select;

    protected override void Initialize(TitleScreenViewModel viewModel)
    {
        select.Init(viewModel.Selects.ToArray(), viewModel.Selected);
    }

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
