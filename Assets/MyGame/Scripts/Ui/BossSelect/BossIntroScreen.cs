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

    private BossIntroManager bossIntro;

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

    public void SetBossIntro(BossIntroManager bossIntroManager, Animator model)
    {
        bossIntro = bossIntroManager;
        bossIntro.transform.SetParent(this.transform, true);
        var rectTransform = bossIntro.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero;
        rectTransform.offsetMin = Vector2.zero;  // 左・下（Left, Bottom）
        rectTransform.offsetMax = Vector2.zero;  // 右・上（Right, Top）
        bossIntro.Setup(model);
    }
}

public class BossIntroScreenPresenter : BaseScreenPresenter<BossIntroScreen, BossIntroScreenPresenter, BossIntroScreenViewModel, BossSelectManager.UI>
{
    protected override void Initialize()
    {
        m_screen.SetBossIntro(m_viewModel.bossIntroManager, m_viewModel.modelData);
    }

    protected override void InputUpdate(InputInfo info)
    {
        base.InputUpdate(info);

    }

    protected override void Destroy()
    {
        m_screen.BossIntro.Destroy();
    }
}

public class BossIntroScreenViewModel : BaseViewModel<BossSelectManager.UI>
{
    public string bossName { get; private set; }
    public Animator modelData { get; private set; }
    public int modelDataId { get; private set; }

    public BossIntroManager bossIntroManager { get; private set; }
    public int bossIntroManagerId { get; private set; }

    protected override IEnumerator Configure()
    {
        bossName = GameState.bossName;

        (modelData, modelDataId) = AddressableAssetLoadUtility.LoadPrefab<Animator>($"{bossName}Intro");

        (bossIntroManager, bossIntroManagerId) = AddressableAssetLoadUtility.LoadPrefab<BossIntroManager>("BossIntro");
        yield return null;
    }

    protected override IEnumerator Destroy()
    {
        AddressableAssetLoadUtility.ReleasePrefab(modelDataId);
        AddressableAssetLoadUtility.ReleasePrefab(bossIntroManagerId);
        yield return null;
    }
}
