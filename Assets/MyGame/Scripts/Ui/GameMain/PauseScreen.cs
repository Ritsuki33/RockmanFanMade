using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : BaseScreen<PauseScreen, PauseScreenPresenter, GameMainManager.UI>
{

}

public class PauseScreenPresenter : BaseScreenPresenter<PauseScreen, PauseScreenPresenter, PauseScreenViewModel, GameMainManager.UI>
{
    protected override void Open()
    {
        GameMainManager.Instance.OnPause(true);
    }

    protected override void InputUpdate(InputInfo info)
    {
        if (info.decide)
        {
            GameMainManager.Instance.GameStageEnd();
        }
        else if (info.start)
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

}