using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ゲームメニュー画面
/// </summary>
public class GameMenuScreen : BaseScreen<GameMenuScreen, GameMenuScreenPresenter, GameMainManager.UI>
{
    enum Gauge
    {
        PlayerHp,
        Other,
    }

    [SerializeField] GameMenuGaugeSelectController gaugeSelectController;

    public GameMenuGaugeSelectController GaugeSelectController => gaugeSelectController;

    public GameMenuGaugeBar PlayerHpBar => gaugeSelectController.Selects[(int)Gauge.PlayerHp].GaugeBar;
    public GameMenuGaugeBar OtherBar => gaugeSelectController.Selects[(int)Gauge.Other].GaugeBar;
}

public class GameMenuScreenPresenter : BaseScreenPresenter<GameMenuScreen, GameMenuScreenPresenter, GameMenuScreenViewModel, GameMainManager.UI>
{
    bool inputable = false;

    protected override void Initialize()
    {
        inputable = false;
        m_screen.GaugeSelectController.Init(0, Selected);

        if (m_viewModel.PlayerStatusParam != null)
        {
            m_viewModel.PlayerStatusParam.HpChangeCallback += SetPlayerHp;
            m_viewModel.PlayerStatusParam.OnRefresh();
        }
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
        else if (info.decide)
        {
            m_screen.GaugeSelectController.Selected();
        }
        else
        {
            var dir = GetInputDirection(info);
            m_screen.GaugeSelectController.InputUpdate(dir);
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }

    }
    private void SetPlayerHp(int hp, int maxHp)
    {
        m_screen.PlayerHpBar.SetParam((float)hp / maxHp);
    }
    protected override void Hide()
    {
        GameMainManager.Instance.OnPause(false, true);
    }

    protected override void Destroy()
    {
        if (m_viewModel.PlayerStatusParam != null)
        {
            m_viewModel.PlayerStatusParam.HpChangeCallback -= SetPlayerHp;
        }
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
        GameMainManager.Instance.TransitToGameMain();
        AudioManager.Instance.PlaySe(SECueIDs.menu);
    }
}

public class GameMenuScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    public IParamStatusSubject PlayerStatusParam => ProjectManager.Instance.RDH.PlayerInfo.StatusParam;

}
