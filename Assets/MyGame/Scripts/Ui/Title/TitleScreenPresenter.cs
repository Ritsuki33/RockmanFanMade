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

        var dir = GetInputDirection(info);
        if (dir != InputDirection.None)
        {
            _screen.Select.InputUpdate(dir);
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }
        else if (info.decide)
        {
            _screen.Select.Selected();
        }
    }

    private InputDirection GetInputDirection(InputInfo info)
    {
        if (info.up) return InputDirection.Up;
        else if (info.down) return InputDirection.Down;
        else return InputDirection.None;
    }
}