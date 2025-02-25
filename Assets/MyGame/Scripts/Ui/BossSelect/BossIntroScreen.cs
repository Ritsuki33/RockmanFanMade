using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// ボスイントロスクリーン
/// </summary>
public class BossIntroScreen : BaseScreen<BossIntroScreen, BossIntroScreenPresenter, BossSelectManager.UI>
{
    //[SerializeField] Animator bossAnimator = default;
    //[SerializeField] LetterRevealText letterRevealText = default;

    [SerializeField] BossIntroManager bossIntro;

    public BossIntroManager BossIntro => bossIntro;

    protected override void Open()
    {
        bossIntro.Play(() =>
        {
            BossSelectManager.Instance.TransitToGameMain();
        });
    }

    protected override void Hide()
    {
        FadeInManager.Instance.FadeOutImmediate();
    }

    protected override IEnumerator HideCoroutine()
    {
        return base.HideCoroutine();
    }

    protected override void Deinitialize()
    {
        bossIntro.Terminate();
    }
}

public class BossIntroScreenPresenter : BaseScreenPresenter<BossIntroScreen, BossIntroScreenPresenter, BossIntroScreenViewModel, BossSelectManager.UI>
{
    protected override void Initialize()
    {
        m_screen.BossIntro.Setup(m_viewModel.modelData);
    }

    protected override void InputUpdate(InputInfo info)
    {
        base.InputUpdate(info);

    }
    protected override void DeInitialize()
    {
        m_viewModel.Destroy();
    }
}

public class BossIntroScreenViewModel : BaseViewModel<BossSelectManager.UI>
{
    public string bossName { get; private set; }
    public Animator modelData { get; private set; }

    protected override IEnumerator Configure()
    {
        bossName = GameState.bossName;

        modelData = AddressableAssetLoadUtility.LoadPrefab<Animator>($"{bossName}Intro");
        yield return null;
    }

    public void Destroy()
    {
        AddressableAssetLoadUtility.Release(modelData);
    }
}
