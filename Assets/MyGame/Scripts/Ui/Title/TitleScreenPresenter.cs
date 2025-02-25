public class TitleScreenPresenter : BaseScreenPresenter<TitleScreen, TitleScreenPresenter, TitleScreenViewModel, TitleManager.ScreenType>
{
    protected override void Initialize()
    {
        m_screen.Select.Init(0, m_viewModel.Selects.ToArray(), m_viewModel.Selected);
    }

    protected override void Open()
    {
        AudioManager.Instance.PlayBgm(BGMCueIDs.title8);
    }

    protected override void Hide()
    {
        AudioManager.Instance.StopBGM();
    }

    protected override void InputUpdate(InputInfo info)
    {

        var dir = GetInputDirection(info);
        if (dir != InputDirection.None)
        {
            m_screen.Select.InputUpdate(dir);
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }
        else if (info.decide)
        {
            m_screen.Select.Selected();
        }
    }

    private InputDirection GetInputDirection(InputInfo info)
    {
        if (info.up) return InputDirection.Up;
        else if (info.down) return InputDirection.Down;
        else return InputDirection.None;
    }
}