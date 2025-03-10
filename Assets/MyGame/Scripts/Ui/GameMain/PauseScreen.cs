using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : BaseScreen<PauseScreen, PauseScreenPresenter, GameMainManager.UI>
{
    protected override void Open()
    {
        FooterUI.Open();
    }

    protected override void Hide()
    {
        FooterUI.Close();
    }

    FooterUI FooterUI => ProjectManager.Instance.FooterUi;
}

public class PauseScreenPresenter : BaseScreenPresenter<PauseScreen, PauseScreenPresenter, PauseScreenViewModel, GameMainManager.UI>
{
    protected override void Initialize()
    {
        ProjectManager.Instance.FooterUi.Setup(m_viewModel.KeyGuides);
    }
    protected override void Open()
    {
        GameMainManager.Instance.OnPause(true);
    }

    protected override void InputUpdate(InputInfo info)
    {
        if (info.start)
        {
            GameMainManager.Instance.TransitToGameMain();
        }
    }

    protected override void Hide()
    {
        GameMainManager.Instance.OnPause(false);
    }
}

public class PauseScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    public (KeyGuideType, string)[] KeyGuides;

    protected override IEnumerator Configure()
    {
        KeyGuides = new (KeyGuideType, string)[] {
            (KeyGuideType.SPACE, "ポーズ解除"),
             };
        yield return null;
    }
}