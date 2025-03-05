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
    [SerializeField] EkanSelectController ekanSelectController;

    public GameMenuGaugeSelectController GaugeSelectController => gaugeSelectController;
    public EkanSelectController EkanSelectController => ekanSelectController;

    public GameMenuGaugeBar PlayerHpBar => gaugeSelectController.Selects[(int)Gauge.PlayerHp].GaugeBar;
    public GameMenuGaugeBar OtherBar => gaugeSelectController.Selects[(int)Gauge.Other].GaugeBar;

    public void SetPlayerHp(float hp)
    {
        PlayerHpBar.SetParam(hp);
    }

    public void ParamChangeAnimation(float hp, Action callback)
    {
        PlayerHpBar.ParamChangeAnimation(hp, callback);
    }

    public void UpdateGaugeSelect(InputDirection dir)
    {
        gaugeSelectController.InputUpdate(dir);
    }

    public void UpdateEkanSelect(InputDirection dir)
    {
        ekanSelectController.InputUpdate(dir);
    }

    public void DisabledGaugeSelect(bool isDisabled)
    {
        gaugeSelectController.Disabled(isDisabled);
    }

    public void DisabledEkanSelect(bool isDisabled)
    {
        ekanSelectController.Disabled(isDisabled);
    }
}

public class GameMenuScreenPresenter : BaseScreenPresenter<GameMenuScreen, GameMenuScreenPresenter, GameMenuScreenViewModel, GameMainManager.UI>
{
    bool inputable = false;

    bool isCursorInWeapon = true;
    protected override void Initialize()
    {
        inputable = false;
        m_screen.GaugeSelectController.Init(0, SelectedWeapon);

        if (m_viewModel.PlayerStatusParam != null)
        {
            m_viewModel.PlayerStatusParam.HpChangeCallback += SetPlayerHp;
            m_viewModel.PlayerStatusParam.OnDamageCallback += SetPlayerHp;
            m_viewModel.PlayerStatusParam.OnRecoveryCallback += PlyaerParamChangeAnimation;
            m_viewModel.PlayerStatusParam.OnRefresh();
        }

        m_screen.EkanSelectController.Init(0, SelectedEkan);

        isCursorInWeapon = true;
        m_screen.GaugeSelectController.Disabled(!isCursorInWeapon);
        m_screen.EkanSelectController.Disabled(isCursorInWeapon);
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
            if (isCursorInWeapon) m_screen.GaugeSelectController.Selected();
            else m_screen.EkanSelectController.Selected();
        }
        else if (info.down || info.up)
        {
            var dir = GetInputDirection(info);
            m_screen.UpdateGaugeSelect(dir);
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }
        else if (info.left || info.right)
        {
            isCursorInWeapon = !isCursorInWeapon;
            m_screen.GaugeSelectController.Disabled(!isCursorInWeapon);
            m_screen.EkanSelectController.Disabled(isCursorInWeapon);
        }
    }
    private void SetPlayerHp(int hp, int maxHp)
    {
        m_screen.SetPlayerHp((float)hp / maxHp);
    }

    private void PlyaerParamChangeAnimation(int hp, int maxHp, Action callback)
    {
        // ポーズを掛けて回復アニメーションさせる
        var hpPlayback = AudioManager.Instance.PlaySe(SECueIDs.hprecover);
        m_screen.ParamChangeAnimation((float)hp / maxHp, () =>
        {
            hpPlayback.Stop();

            callback?.Invoke();
        });
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

    private void SelectedWeapon(SelectInfo info)
    {
        Debug.Log(info.id);
        GameMainManager.Instance.TransitToGameMain();
        AudioManager.Instance.PlaySe(SECueIDs.menu);
    }
    private void SelectedEkan(SelectInfo info)
    {
        if (info.id == 0)
        {
            inputable = false;
            m_viewModel.OnRecovery(m_viewModel.PlayerStatusParam.MaxHp, () =>
            {
                inputable = true;
            });
        }
    }
}

public class GameMenuScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    public IParamStatus PlayerStatusParam => ProjectManager.Instance.RDH.PlayerInfo.StatusParam;


    public void OnRecovery(int recovery, Action callback)
    {
        PlayerStatusParam.OnRecovery(recovery, callback);
    }
}
