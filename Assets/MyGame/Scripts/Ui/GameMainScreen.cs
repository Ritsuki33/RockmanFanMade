using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// スクリーンのスクリプトテンプレート
/// Enum型はそれぞれ適切な列挙型を指定
/// </summary>
public class GameMainScreen : BaseScreen<GameMainScreen, GameMainScreenPresenter, GameMainScreenViewModel, GameMainManager.UI>
{
    [SerializeField] ReadyUi readyUi = default;
    [SerializeField] HpBar enemyHpBar = default;
    [SerializeField] HpBar hpBar = default;

    public ReadyUi ReadyUi => readyUi;
    public HpBar EnemyHpBar => enemyHpBar;
    public HpBar HpBar => hpBar;

    protected override void Initialize(GameMainScreenViewModel viewModel)
    {
        HpBar.SetParam(WorldManager.Instance.PlayerController.CurrentHp);
    }

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

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }
}

public class GameMainScreenPresenter : BaseScreenPresenter<GameMainScreen, GameMainScreenPresenter, GameMainScreenViewModel, GameMainManager.UI>
{
    GameMainScreen _screen;
    GameMainScreenViewModel _viewModel;
    protected override void Initialize(GameMainScreen screen, GameMainScreenViewModel viewModel)
    {
        _screen = screen;
        _viewModel = viewModel;

        _viewModel.Player.hpChangeTrigger = SetPlayerHp;
    }

    protected override void InputUpdate(InputInfo info)
    {
        base.InputUpdate(info);
    }

    public void SetPlayerHp(float hp)
    {
        _screen.HpBar.SetParam(hp);
    }

    public void SetEnemyHp(float hp)
    {
        _screen.EnemyHpBar.SetParam(hp);
    }

    public void PlayerHpActive(bool isActive)
    {
        _screen.HpBar.gameObject.SetActive(isActive);
    }

    public void EnemyHpActive(bool isActive)
    {
        _screen.EnemyHpBar.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// ボスの体力上昇アニメーション（敵体力の監視登録も行う）
    /// </summary>
    /// <param name="val"></param>
    /// <param name="hpChangeTrigger"></param>
    /// <param name="finishCallback"></param>
    public void EnemyHpIncrementAnimation(BossController ctr, Action finishCallback)
    {
        _screen.EnemyHpBar.gameObject.SetActive(true);
        _screen.EnemyHpBar.SetParam(0.0f);
        _screen.EnemyHpBar.ParamChangeAnimation((float)ctr.CurrentHp / ctr.MaxHp, finishCallback);

        ctr.HpChangeTrigger = SetEnemyHp;
    }

    public void ReadyUiPlay(Action finishCallback)
    {
        _screen.ReadyUi.Play(finishCallback);
    }

    protected override void Deinitialize()
    {
        _viewModel.Player.hpChangeTrigger -= SetPlayerHp;
    }
}

public class GameMainScreenViewModel : BaseViewModel<GameMainManager.UI>
{
    public Player Player => WorldManager.Instance.Player;

    protected override IEnumerator Configure()
    {
        yield return null;
    }
}
