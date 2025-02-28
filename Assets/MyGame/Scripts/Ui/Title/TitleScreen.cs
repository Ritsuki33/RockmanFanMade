using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : BaseScreen<TitleScreen, TitleScreenPresenter, TitleManager.ScreenType>
{
    [SerializeField] MainMenuColorSelect select;
    [SerializeField] private Image flashImage;

    [SerializeField] private CanvasGroup guide;
    public MainMenuColorSelect Select => select;

    protected override void Open()
    {
        FadeInManager.Instance.FadeInImmediate();
    }

    protected override void Hide()
    {
        FadeInManager.Instance.FadeOutImmediate();
    }

    public void Flash()
    {
        flashImage.color = new Color(1, 1, 1, 0); // 初期状態を透明に
        flashImage.DOFade(1, 0.5f) // フェードイン
            .OnComplete(() => flashImage.DOFade(0, 0.5f).Play())
            .Play(); // フェードアウト

        AudioManager.Instance.PlaySe(SECueIDs.shakin);
    }

    /// <summary>
    ///  ガイドの点滅
    /// </summary>
    public void StartBlinking()
    {
        // α値を使って点滅
        guide.DOFade(0f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .Play();
    }

    /// <summary>
    /// ガイドの点滅を停止
    /// </summary>
    public void StopBlinking()
    {
        DOTween.Kill(guide);
        guide.alpha = 1f;
    }
}
