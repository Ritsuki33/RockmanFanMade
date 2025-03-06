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
        ThunderBolt,
    }


    [SerializeField] GameMenuGaugeSelectController gaugeSelectController;
    [SerializeField] ItemSelectController itemSelectController;

    public GameMenuGaugeSelectController GaugeSelectController => gaugeSelectController;
    public ItemSelectController ItemSelectController => itemSelectController;

    public GameMenuGaugeBar PlayerHpBar => gaugeSelectController.Selects[(int)Gauge.PlayerHp].GaugeBar;
    public GameMenuGaugeBar ThunderBoltBar => gaugeSelectController.Selects[(int)Gauge.ThunderBolt].GaugeBar;

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

        if (m_viewModel.PlayerParamStatus != null)
        {
            m_viewModel.PlayerParamStatus.HpChangeCallback += SetPlayerHp;
            m_viewModel.PlayerParamStatus.OnDamageCallback += SetPlayerHp;
            m_viewModel.PlayerParamStatus.OnRecoveryCallback += PlyaerParamChangeAnimation;
            m_viewModel.PlayerParamStatus.OnRefresh();
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
        if (m_viewModel.PlayerParamStatus != null)
        {
            m_viewModel.PlayerParamStatus.HpChangeCallback -= SetPlayerHp;
        }
    }
    private InputDirection GetInputDirection(InputInfo info)
    {
        if (info.up) return InputDirection.Up;
        else if (info.down) return InputDirection.Down;
        else return InputDirection.None;
    }

    private void SelectedWeapon(GameMenuGaugeInfo info)
    {
        for (int i = 0; i < m_viewModel.PlayerWeaponInfo.PlayerWeaponListData.Count; i++)
        {
            var weapon = m_viewModel.PlayerWeaponInfo.PlayerWeaponListData[i];
            if (weapon.WeaponType == info.weaponType)
            {
                m_viewModel.PlayerParamStatus.OnChangeWeapon(weapon);
                break;
            }
        }

        GameMainManager.Instance.TransitToGameMain();
        AudioManager.Instance.PlaySe(SECueIDs.menu);
    }
    private void SelectedEkan(SelectInfo info)
    {
        if (info.id == 0)
        {
            inputable = false;
            m_viewModel.OnRecovery(m_viewModel.PlayerParamStatus.MaxHp, () =>
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
    public IPlayerParamStatus PlayerParamStatus => ProjectManager.Instance.RDH.PlayerInfo.StatusParam;

    public PlayerWeaponInfo PlayerWeaponInfo => ProjectManager.Instance.RDH.PlayerWeaponInfo;

    public void OnRecovery(int recovery, Action callback)
    {
        PlayerParamStatus.OnRecovery(recovery, callback);
    }
}
