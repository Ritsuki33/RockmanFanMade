using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ゲームメニュー画面
/// </summary>
public class GameMenuScreen : BaseScreen<GameMenuScreen, GameMenuScreenPresenter, GameMainManager.UI>
{

}

public class GameMenuScreenPresenter : BaseScreenPresenter<GameMenuScreen, GameMenuScreenPresenter, GameMenuScreenViewModel, GameMainManager.UI>
{
    protected override void Open()
    {
        GameMainManager.Instance.OnPause(true, true);
    }

    protected override void InputUpdate(InputInfo info)
    {
        if (info.select)
        {
            GameMainManager.Instance.TransitToGameMain();
        }
    }

    protected override void Hide()
    {
        GameMainManager.Instance.OnPause(false, true);
    }
}

public class GameMenuScreenViewModel : BaseViewModel<GameMainManager.UI>
{

}
