using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ゲームメニュー画面
/// </summary>
public class GameMenuScreen : BaseScreen<GameMenuScreen, GameMenuScreenPresenter, GameMainManager.UI>
{
    [SerializeField] MenuGaugeSelectController gaugeSelectController;

    public MenuGaugeSelectController GaugeSelectController => gaugeSelectController;
}

public class GameMenuScreenPresenter : BaseScreenPresenter<GameMenuScreen, GameMenuScreenPresenter, GameMenuScreenViewModel, GameMainManager.UI>
{
    bool inputable = false;

    protected override void Initialize()
    {
        inputable = false;
        m_screen.GaugeSelectController.Init(0, Selected);
    }

    protected override void Open()
    {
        GameMainManager.Instance.OnPause(true, true);
    }

    protected override void AfterOpen()
    {
        inputable = true;
    }

    protected override void InputUpdate(InputInfo info)
    {
        if (!inputable) return;

        if (info.select)
        {
            GameMainManager.Instance.TransitToGameMain();
            AudioManager.Instance.PlaySe(SECueIDs.menu);
        }
        else
        {
            var dir = GetInputDirection(info);
            m_screen.GaugeSelectController.InputUpdate(dir);
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }

    }

    protected override void Hide()
    {
        GameMainManager.Instance.OnPause(false, true);
    }

    private InputDirection GetInputDirection(InputInfo info)
    {
        if (info.up) return InputDirection.Up;
        else if (info.down) return InputDirection.Down;
        else return InputDirection.None;
    }

    private void Selected(SelectInfo info)
    {
        Debug.Log(info.id);
    }
}

public class GameMenuScreenViewModel : BaseViewModel<GameMainManager.UI>
{

}
