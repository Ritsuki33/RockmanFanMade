using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// スクリーンのスクリプトテンプレート
/// Enum型はそれぞれ適切な列挙型を指定
/// </summary>
public class BossSelectScreen : BaseScreen<BossSelectScreen, BossSelectScreenPresenter, BossSelectScreenViewModel, GameMainManager.UI>
{
    [SerializeField] BossSelectController bossSelectController;

    public BossSelectController BossSelectController => bossSelectController;
    protected override void Initialize(BossSelectScreenViewModel viewModel)
    {
        base.Initialize(viewModel);
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

public class BossSelectScreenPresenter : BaseScreenPresenter<BossSelectScreen, BossSelectScreenPresenter, BossSelectScreenViewModel, GameMainManager.UI>
{
    BossSelectScreen m_screen;
    BossSelectScreenViewModel m_viewModel;

    protected override void Initialize(BossSelectScreen screen, BossSelectScreenViewModel viewModel)
    {
        m_screen = screen;
        m_viewModel = viewModel;

        m_screen.BossSelectController.Init(4, Selected);
    }

    protected override void InputUpdate(InputInfo info)
    {
        m_screen.BossSelectController.InputUpdate(info);
    }

    private void Selected(SelectInfo info)
    {

    }
}

public class BossSelectScreenViewModel : BaseViewModel<GameMainManager.UI>
{

    protected override IEnumerator Configure()
    {
        yield return null;
    }
}
