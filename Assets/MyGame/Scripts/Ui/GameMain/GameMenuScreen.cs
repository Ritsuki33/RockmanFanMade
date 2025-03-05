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
    [SerializeField] ItemSelectController itemSelectController;

    public GameMenuGaugeSelectController GaugeSelectController => gaugeSelectController;
    public ItemSelectController ItemSelectController => itemSelectController;

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

    public void UpdateItemSelect(InputDirection dir)
    {
        itemSelectController.InputUpdate(dir);
    }

    public void DisabledGaugeSelect(bool isDisabled)
    {
        gaugeSelectController.Disabled(isDisabled);
    }

    public void DisabledEkanSelect(bool isDisabled)
    {
        itemSelectController.Disabled(isDisabled);
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

        m_screen.ItemSelectController.Init(0, SelectedEkan);

        isCursorInWeapon = true;
        m_screen.GaugeSelectController.Disabled(!isCursorInWeapon);
        m_screen.ItemSelectController.Disabled(isCursorInWeapon);
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
            else m_screen.ItemSelectController.Selected();
        }
        else if (info.down || info.up)
        {
            var dir = GetInputDirection(info);
            if (isCursorInWeapon)
            {
                m_screen.UpdateGaugeSelect(dir);
            }
            else
            {
                m_screen.UpdateItemSelect(dir);
            }
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }
        else if (info.left || info.right)
        {
            isCursorInWeapon = !isCursorInWeapon;
            m_screen.GaugeSelectController.Disabled(!isCursorInWeapon);
            m_screen.ItemSelectController.Disabled(isCursorInWeapon);
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
        else if (info.id == 1)
        {
            // ステージ離脱
            GameMainManager.Instance.GameStageEnd();
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
