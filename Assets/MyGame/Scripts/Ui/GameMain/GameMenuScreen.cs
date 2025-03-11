using System;
using System.Collections;
using System.Collections.Generic;
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

    public GameMenuGaugeBar PlayerHpBar => gaugeSelectController.Selects[(int)PlayerWeaponType.RockBuster].GaugeBar;
    public List<MenuGuageSelector> MenuGuageSelectorList => gaugeSelectController.Selects;

    protected override void Open()
    {
        ProjectManager.Instance.FooterUi.Open();
    }


    protected override void Hide()
    {
        ProjectManager.Instance.FooterUi.Close();
    }
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
        ProjectManager.Instance.FooterUi.Setup(m_viewModel.KeyGuides);

        inputable = false;
        m_screen.GaugeSelectController.Init(DefaultIndex, SelectedWeapon);

        if (m_viewModel.PlayerParamStatus != null)
        {
            m_viewModel.PlayerParamStatus.HpChangeCallback += SetPlayerHp;
            m_viewModel.PlayerParamStatus.OnDamageCallback += SetPlayerHp;
            m_viewModel.PlayerParamStatus.OnRecoveryCallback += PlyaerParamChangeAnimation;

            if (m_viewModel.PlayerWeaponStatus != null)
            {
                foreach (var gauge in m_screen.MenuGuageSelectorList)
                {
                    if (gauge.GaugeBar.Type == PlayerWeaponType.RockBuster) continue;

                    var weapon = m_viewModel.PlayerWeaponStatus.GetPlayerWeapon(gauge.GaugeBar.Type);
                    if (weapon != null)
                    {
                        weapon.EnergyChangeCallback += gauge.GaugeBar.SetParam;
                        weapon.EnergyRecoveryCallback += gauge.GaugeBar.ParamChangeAnimation;

                        gauge.gameObject.SetActive(true);
                    }
                    else
                    {
                        gauge.gameObject.SetActive(false);
                    }
                }
            }
            m_viewModel.PlayerParamStatus.OnRefresh();
        }

        m_screen.ItemSelectController.Init(0, SelectedItem);

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
            m_viewModel.PlayerParamStatus.OnDamageCallback -= SetPlayerHp;
            m_viewModel.PlayerParamStatus.OnRecoveryCallback -= PlyaerParamChangeAnimation;

            m_viewModel.PlayerParamStatus.OnRefresh();
        }

        if (m_viewModel.PlayerWeaponStatus != null)
        {
            foreach (var gauge in m_screen.MenuGuageSelectorList)
            {
                if (gauge.GaugeBar.Type == PlayerWeaponType.RockBuster) continue;

                var weapon = m_viewModel.PlayerWeaponStatus.GetPlayerWeapon(gauge.GaugeBar.Type);
                if (weapon != null)
                {
                    weapon.EnergyChangeCallback -= gauge.GaugeBar.SetParam;
                    weapon.EnergyRecoveryCallback -= gauge.GaugeBar.ParamChangeAnimation;

                    gauge.gameObject.SetActive(true);
                }
                else
                {
                    gauge.gameObject.SetActive(false);
                }
            }
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
        m_viewModel.PlayerParamStatus.OnChangeWeapon(info.weaponType);


        GameMainManager.Instance.TransitToGameMain();
        AudioManager.Instance.PlaySe(SECueIDs.menu);
    }
    private void SelectedItem(SelectInfo info)
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

    private int DefaultIndex
    {
        get
        {
            int index = 0;
            for (index = 0; index < m_screen.MenuGuageSelectorList.Count; index++)
            {
                var gauge = m_screen.MenuGuageSelectorList[index];
                if (gauge.GaugeBar.Type == m_viewModel.PlayerParamStatus.CurrentWeapon.Type)
                {
                    return index;
                }
            }

            return 0;
        }
    }
}

public class GameMenuScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    public IPlayerParamStatus PlayerParamStatus => ProjectManager.Instance.RDH.PlayerInfo.StatusParam;

    public IPlayerWeaponStatus PlayerWeaponStatus => ProjectManager.Instance.RDH.PlayerInfo.PlayerWeaponStatus;

    public PlayerWeaponInfo PlayerWeaponInfo => ProjectManager.Instance.RDH.PlayerWeaponInfo;

    public (KeyGuideType, string)[] KeyGuides;
    protected override IEnumerator Configure()
    {
        KeyGuides = new (KeyGuideType, string)[] {
            (KeyGuideType.WASD, "移動"),
            (KeyGuideType.L, "決定"),
            (KeyGuideType.TAB, "閉じる"),
             };
        yield return null;
    }
    public void OnRecovery(int recovery, Action callback)
    {
        PlayerParamStatus.OnRecovery(recovery, callback);
    }
}
