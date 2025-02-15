using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// ボスイントロスクリーン
/// </summary>
public class BossIntroScreen : BaseScreen<BossIntroScreen, BossIntroScreenPresenter, BossIntroScreenViewModel, BossSelectManager.UI>
{
    //[SerializeField] Animator bossAnimator = default;
    //[SerializeField] LetterRevealText letterRevealText = default;

    [SerializeField] BossIntroManager bossIntro;

    protected override void Initialize(BossIntroScreenViewModel viewModel)
    {
        //letterRevealText.Init();
    }

    protected override void Open()
    {
        bossIntro.Play("Grenademan", () =>
        {
            BossSelectManager.Instance.TransitToGameMain();
        });
    }

    protected override void Hide()
    {
        FadeInManager.Instance.FadeOutImmediate();
        bossIntro.Terminate();
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

public class BossIntroScreenPresenter : BaseScreenPresenter<BossIntroScreen, BossIntroScreenPresenter, BossIntroScreenViewModel, BossSelectManager.UI>
{
    BossIntroScreen m_screen;
    BossIntroScreenViewModel m_viewModel;

    protected override void Initialize(BossIntroScreen screen, BossIntroScreenViewModel viewModel)
    {
        m_screen = screen;
        m_viewModel = viewModel;
    }

    protected override void InputUpdate(InputInfo info)
    {
        base.InputUpdate(info);

    }

}

public class BossIntroScreenViewModel : BaseViewModel<BossSelectManager.UI>
{

    protected override IEnumerator Configure()
    {
        yield return null;
    }
}
