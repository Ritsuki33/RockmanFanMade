using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// スクリーンのスクリプトテンプレート
/// Enum型はそれぞれ適切な列挙型を指定
/// </summary>
public class GameMainScreen : BaseScreen<GameMainScreen, GameMainScreenPresenter, GameMainManager.UI>
{
    [SerializeField] ReadyUi readyUi = default;
    [SerializeField] GaugeBar enemyHpBar = default;
    [SerializeField] GaugeBar hpBar = default;

    public ReadyUi ReadyUi => readyUi;
    public GaugeBar EnemyHpBar => enemyHpBar;
    public GaugeBar HpBar => hpBar;

    protected override void Open()
    {
        base.Open();
    }

    protected override IEnumerator OpenCoroutine()
    {
        return base.OpenCoroutine();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void Hide()
    {
        base.Hide();
    }

    protected override IEnumerator HideCoroutine()
    {
        return base.HideCoroutine();
    }
}

public class GameMainScreenPresenter : BaseScreenPresenter<GameMainScreen, GameMainScreenPresenter, GameMainScreenViewModel, GameMainManager.UI>
{

    protected override void Initialize()
    {
        m_viewModel.PlayerStatusParam.HpChangeCallback += SetPlayerHp;
        m_viewModel.PlayerStatusParam.OnDamageCallback += SetPlayerHp;
        m_viewModel.PlayerStatusParam.OnRecoveryCallback += PlyaerParamChangeAnimation;

        m_viewModel.StageInfoSubject.OnSetBossHolder += SetBossSubject;

        if (m_viewModel.BossStatusParam != null)
        {
            m_viewModel.BossStatusParam.HpChangeCallback += SetEnemyHp;
            m_viewModel.BossStatusParam.OnDamageCallback += SetEnemyHp;
            m_viewModel.BossStatusParam.OnRecoveryCallback += BossParamChangeAnimation;
        }
    }

    protected override void Open()
    {
        m_viewModel.PlayerStatusParam.OnRefresh();
    }

    protected override void InputUpdate(InputInfo info)
    {
        if (info.start)
        {
            GameMainManager.Instance.TransitToPause();
            AudioManager.Instance.PlaySystem(SECueIDs.menu);
        }
        else if (info.select)
        {
            GameMainManager.Instance.TransitToGameMenu();
            AudioManager.Instance.PlaySystem(SECueIDs.menu);
        }
    }

    protected override void Destroy()
    {
        if (m_viewModel.PlayerStatusParam != null)
        {
            m_viewModel.PlayerStatusParam.HpChangeCallback -= SetPlayerHp;
            m_viewModel.PlayerStatusParam.OnDamageCallback -= SetPlayerHp;
            m_viewModel.PlayerStatusParam.OnRecoveryCallback -= PlyaerParamChangeAnimation;
        }

        m_viewModel.StageInfoSubject.OnSetBossHolder -= SetBossSubject;

        if (m_viewModel.BossStatusParam != null)
        {
            m_viewModel.BossStatusParam.HpChangeCallback -= SetEnemyHp;
            m_viewModel.BossStatusParam.OnDamageCallback -= SetEnemyHp;
            m_viewModel.BossStatusParam.OnRecoveryCallback -= BossParamChangeAnimation;
        }
    }

    private void SetPlayerHp(int hp, int maxHp)
    {
        m_screen.HpBar.SetParam((float)hp / maxHp);
    }

    public void SetEnemyHp(int hp, int maxHp)
    {
        m_screen.EnemyHpBar.SetParam((float)hp / maxHp);
    }

    public void PlayerHpActive(bool isActive)
    {
        m_screen.HpBar.gameObject.SetActive(isActive);
    }

    public void EnemyHpActive(bool isActive)
    {
        m_screen.EnemyHpBar.gameObject.SetActive(isActive);
    }

    private void PlyaerParamChangeAnimation(int hp, int maxHp, Action callback)
    {
        // ポーズを掛けて回復アニメーションさせる
        WorldManager.Instance.OnPause(true);
        var hpPlayback = AudioManager.Instance.PlaySe(SECueIDs.hprecover);
        m_screen.HpBar.ParamChangeAnimation((float)hp / maxHp, () =>
        {
            WorldManager.Instance.OnPause(false);
            hpPlayback.Stop();

            callback?.Invoke();
        });
    }

    private void BossParamChangeAnimation(int hp, int maxHp, Action callback)
    {
        m_screen.EnemyHpBar.gameObject.SetActive(true);
        var hpPlayback = AudioManager.Instance.PlaySe(SECueIDs.hprecover);
        m_screen.EnemyHpBar.ParamChangeAnimation((float)hp / maxHp, () =>
        {
            hpPlayback.Stop();

            callback?.Invoke();
        });
    }

    public void ReadyUiPlay(Action finishCallback)
    {
        m_screen.ReadyUi.Play(finishCallback);
    }


    private void SetBossSubject()
    {
        m_viewModel.BossStatusParam.HpChangeCallback += SetEnemyHp;
        m_viewModel.BossStatusParam.OnDamageCallback += SetEnemyHp;
        m_viewModel.BossStatusParam.OnRecoveryCallback += BossParamChangeAnimation;
    }

}

public class GameMainScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    public IParamStatus PlayerStatusParam => ProjectManager.Instance.RDH.PlayerInfo.StatusParam;
    public IParamStatus BossStatusParam => ProjectManager.Instance.RDH.StageInfo.StageBossParam;
    public IStageInfoSubject StageInfoSubject => ProjectManager.Instance.RDH.StageInfo;
}
