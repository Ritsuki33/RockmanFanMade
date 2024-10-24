public class TitleScreenPresenter : BaseScreenPresenter<TitleScreen, TitleScreenPresenter, TitleScreenViewModel, TitleManager.ScreenType>
{
    private TitleScreen _screen;
    private TitleScreenViewModel _model;

    protected override void Initialize(TitleScreen screen, TitleScreenViewModel viewModel)
    {
        _screen = screen;
        _model = viewModel;
    }

    protected override void InputUpdate(InputInfo info)
    {
        _screen.Select.InputUpdate(info);
    }

}