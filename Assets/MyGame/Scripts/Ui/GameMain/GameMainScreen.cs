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
    private IBossSubject m_bossObserver { get; set; } = null;


    protected override void Initialize()
    {
        // m_screen.HpBar.SetParam(WorldManager.Instance.Player.CurrentHp);
        // m_screen.HpBar.SetParam(m_viewModel.PlayerEnv.hp.Value);
        m_viewModel.PlayerObserver.Hp.Subscribe(SetPlayerHp);
        m_viewModel.PlayerObserver.Recovery.Subscribe(OnPlayerGetRecovery);

        m_viewModel.PlayerObserver.Hp.Refresh();

        m_viewModel.GameMainSubject.BossHolder.Subscribe(SetBossSubject);
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
        m_viewModel.PlayerObserver.Hp.Dispose(SetPlayerHp);
        m_viewModel.PlayerObserver.Recovery.Dispose(OnPlayerGetRecovery);
    }
    /// <summary>
    /// プレイヤーHpの監視登録
    /// </summary>
    /// <param name="hp"></param>
    public void BindPlayerHp(ISubsribeOnlyReactiveProperty<float> hp)
    {
        hp.Subscribe(SetPlayerHp);
    }

    private void SetPlayerHp(float hp)
    {
        m_screen.HpBar.SetParam(hp);
    }

    public void SetEnemyHp(float hp)
    {
        m_screen.EnemyHpBar.SetParam(hp);
    }

    public void PlayerHpActive(bool isActive)
    {
        m_screen.HpBar.gameObject.SetActive(isActive);
    }

    public void EnemyHpActive(bool isActive)
    {
        m_screen.EnemyHpBar.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 回復アイテム取得時
    /// </summary>
    /// <param name="recovery"></param>
    private void OnPlayerGetRecovery(int recovery)
    {
        // GUIとの切り離し
        m_viewModel.PlayerObserver.Hp.Dispose(SetPlayerHp);
        m_viewModel.PlayerObserver.Hp.Subscribe(PlyaerParamChangeAnimation);
        // PlayerHpIncrementAnimation(startParam, m_viewModel.PlayerEnv.hp.Value, m_viewModel.PlayerEnv.hp, () =>
        // {
        //     WorldManager.Instance.OnPause(false);
        //     hpPlayback.Stop();

        //     // 回復アニメーション終了コールバック
        //     m_viewModel.NotifyPlayerHpRecoveryAnimationFinish();
        // });
    }

    private void OnBossGetRecovery(int recovery)
    {
        // GUIとの切り離し
        this.m_bossObserver.Hp.Dispose(SetEnemyHp);
        this.m_bossObserver.Hp.Subscribe(BossParamChangeAnimation);
    }

    private void PlyaerParamChangeAnimation(float hp)
    {
        // ポーズを掛けて回復アニメーションさせる
        WorldManager.Instance.OnPause(true);
        var hpPlayback = AudioManager.Instance.PlaySe(SECueIDs.hprecover);
        m_screen.HpBar.ParamChangeAnimation(hp, () =>
        {
            WorldManager.Instance.OnPause(false);
            hpPlayback.Stop();

            m_viewModel.PlayerObserver.Hp.Dispose(PlyaerParamChangeAnimation);
            m_viewModel.PlayerObserver.Hp.Subscribe(SetPlayerHp);
        });
    }

    private void BossParamChangeAnimation(float hp)
    {
        m_screen.EnemyHpBar.gameObject.SetActive(true);
        var hpPlayback = AudioManager.Instance.PlaySe(SECueIDs.hprecover);
        m_screen.EnemyHpBar.ParamChangeAnimation(hp, () =>
        {
            hpPlayback.Stop();

            this.m_bossObserver.Hp.Dispose(BossParamChangeAnimation);
            this.m_bossObserver.Hp.Dispose(SetEnemyHp);

            this.m_bossObserver.AnimationFinishCallback?.Invoke();
        });
    }

    public void ReadyUiPlay(Action finishCallback)
    {
        m_screen.ReadyUi.Play(finishCallback);
    }

    // public void HpIncrementAnimation(GaugeBar hpbar, float startParam, float endParam, Action finishCallback)
    // {
    //     hpbar.gameObject.SetActive(true);
    //     hpbar.SetParam(startParam);
    //     hpbar.ParamChangeAnimation(endParam, finishCallback);
    // }

    // /// <summary>
    // /// プレイヤーの体力
    // /// </summary>
    // /// <param name="param"></param>
    // /// <param name="hp"></param>
    // /// <param name="finishCallback"></param>
    // public void PlayerHpIncrementAnimation(float startParam, float endParam, ISubsribeOnlyReactiveProperty<float> hp, Action finishCallback)
    // {
    //     HpIncrementAnimation(m_screen.HpBar, startParam, endParam, () =>
    //     {
    //         // プレイヤーHpの監視登録
    //         hp.Subscribe(SetPlayerHp);

    //         finishCallback.Invoke();
    //     });
    // }

    // /// <summary>
    // /// ボスの体力上昇アニメーション（敵体力の監視登録も行う）
    // /// </summary>
    // /// <param name="val"></param>
    // /// <param name="hpChangeTrigger"></param>
    // /// <param name="finishCallback"></param>
    // public void EnemyHpIncrementAnimation(float startParam, float endParam, ISubsribeOnlyReactiveProperty<float> hp, Action finishCallback)
    // {
    //     HpIncrementAnimation(m_screen.EnemyHpBar, startParam, endParam, () =>
    //     {
    //         // 敵Hpの監視登録
    //         hp.Subscribe(SetEnemyHp);

    //         finishCallback.Invoke();
    //     });
    // }

    private void SetBossSubject(IBossSubject bossObserver)
    {
        this.m_bossObserver = bossObserver;

        this.m_bossObserver.Hp.Subscribe(SetEnemyHp);
        this.m_bossObserver.Recovery.Subscribe(OnBossGetRecovery);
    }

}

public class GameMainScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    private IPlayerSubject playerSubject { get; set; } = null;
    private IGameMainSubject gameMainSubject;

    public IPlayerSubject PlayerObserver => playerSubject;

    public IGameMainSubject GameMainSubject => gameMainSubject;
    protected override IEnumerator Configure()
    {
        playerSubject = UIObserver.Instance.playerObserver;
        gameMainSubject = GameMainManager.Instance;
        yield return null;
    }
}
