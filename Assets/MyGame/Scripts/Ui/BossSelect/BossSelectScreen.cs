using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

/// <summary>
/// スクリーンのスクリプトテンプレート
/// Enum型はそれぞれ適切な列挙型を指定
/// </summary>
public class BossSelectScreen : BaseScreen<BossSelectScreen, BossSelectScreenPresenter, BossSelectManager.UI>
{
    [SerializeField] BossSelectController bossSelectController = default;
    [SerializeField] Image flash = default;

    public BossSelectController BossSelectController => bossSelectController;

    protected override void Open()
    {
        flash.gameObject.SetActive(false);
        FadeInManager.Instance.FadeInImmediate();
    }

    public void FlashEffect(Action callback)
    {
        StartCoroutine(FlashEffectCo());
        IEnumerator FlashEffectCo()
        {
            int count = 0;
            do
            {

                flash.gameObject.SetActive(true);

                yield return new WaitForSeconds(0.1f);
                flash.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                count++;
            }
            while (count < 3);

            callback?.Invoke();
        }
    }
}

public class BossSelectScreenPresenter : BaseScreenPresenter<BossSelectScreen, BossSelectScreenPresenter, BossSelectScreenViewModel, BossSelectManager.UI>
{

    protected override void Initialize()
    {
        m_screen.BossSelectController.Init(4, m_viewModel.bossSelectInfos.ToArray(), Selected);
        ProjectManager.Instance.FooterUi.Setup(m_viewModel.KeyGuides);
    }

    protected override void Open()
    {
        AudioManager.Instance.OnPause(false);
        AudioManager.Instance.PlayBgm(BGMCueIDs.bossselect);
        ProjectManager.Instance.FooterUi.Open();
    }

    protected override void Hide()
    {
        AudioManager.Instance.StopBGM();
        ProjectManager.Instance.FooterUi.Close();
    }

    protected override void InputUpdate(InputInfo info)
    {
        var dir = GetInputDirection(info);
        if (dir != InputDirection.None)
        {
            m_screen.BossSelectController.InputUpdate(dir);
            AudioManager.Instance.PlaySystem(SECueIDs.select);
        }
        else if (info.decide)
        {
            m_screen.BossSelectController.Selected();
        }
    }

    private void Selected(BossSelectInfo info)
    {
        if (info.selectable)
        {
            AudioManager.Instance.PlaySystem(SECueIDs.start);
            m_screen.FlashEffect(() =>
            {
                m_viewModel.SetBossId(info.id, info.panelName);
                OpenBossIntroScreen();
            });
        }
        else
        {
            AudioManager.Instance.PlaySystem(SECueIDs.error);
        }
    }


    private InputDirection GetInputDirection(InputInfo info)
    {
        if (info.up) return InputDirection.Up;
        else if (info.down) return InputDirection.Down;
        else if (info.left) return InputDirection.Left;
        else if (info.right) return InputDirection.Right;
        else return InputDirection.None;
    }
    private void OpenBossIntroScreen()
    {
        TransitToScreen(BossSelectManager.UI.BossIntro, true);
    }

    protected override void Destroy()
    {
        m_screen.BossSelectController.OnDestroy();
    }
}

public class BossSelectScreenViewModel : BaseViewModel<BossSelectManager.UI>
{
    public List<BossSelectInfo> bossSelectInfos = new List<BossSelectInfo>();
    BossSelectData bossSelectData;
    SpriteAtlas spriteAtlas;
    private readonly string addressableBossSelectDataPath = "BossSelectData";
    private readonly string addressableSpriteAtlasPath = "BossSelectPanel";

    public (KeyGuideType, string)[] KeyGuides;

    protected override IEnumerator Configure()
    {
        bossSelectData = AddressableAssetLoadUtility.LoadAsset<BossSelectData>(addressableBossSelectDataPath);
        spriteAtlas = AddressableAssetLoadUtility.LoadAsset<SpriteAtlas>(addressableSpriteAtlasPath);

        foreach (var item in bossSelectData.bossInfoList)
        {
            var info = new BossSelectInfo();
            info.id = item.id;
            info.panelName = item.panelName;
            info.panelSprite = spriteAtlas.GetSprite(item.panelSprite);
            info.selectable = item.selectable;
            bossSelectInfos.Add(info);
        }

        // ボス未選択は‐1とする
        GameState.bossId = -1;
        GameState.bossName = "";
        yield return null;

        KeyGuides = new (KeyGuideType, string)[] {
            (KeyGuideType.WASD, "移動"),
            (KeyGuideType.L, "決定"),
             };
    }

    public void SetBossId(int id, string name)
    {
        GameState.bossId = id;
        GameState.bossName = name;
    }

    protected override IEnumerator Destroy()
    {
        AddressableAssetLoadUtility.ReleaseAsset(bossSelectData);
        AddressableAssetLoadUtility.ReleaseAsset(spriteAtlas);
        yield return null;
    }
}
