using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スクリーンのスクリプトテンプレート
/// Enum型はそれぞれ適切な列挙型を指定
/// </summary>
public class BossSelectScreen : BaseScreen<BossSelectScreen, BossSelectScreenPresenter, BossSelectScreenViewModel, BossSelectManager.UI>
{
    [SerializeField] BossIntroScreen bossIntroScreen = default;
    [SerializeField] BossSelectController bossSelectController = default;
    [SerializeField] Image flash = default;

    public BossSelectController BossSelectController => bossSelectController;

    protected override void Initialize(BossSelectScreenViewModel viewModel)
    {
        base.Initialize(viewModel);

        flash.gameObject.SetActive(false);
    }

    public void OpenBossIntroScreen()
    {
        TransitScreen(BossSelectManager.UI.BossIntro, false);
    }

    public void Selected()
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


            OpenBossIntroScreen();
        }
    }
}

public class BossSelectScreenPresenter : BaseScreenPresenter<BossSelectScreen, BossSelectScreenPresenter, BossSelectScreenViewModel, BossSelectManager.UI>
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
        m_screen.Selected();
    }

 
}

public class BossSelectScreenViewModel : BaseViewModel<BossSelectManager.UI>
{

    protected override IEnumerator Configure()
    {
        yield return null;
    }
}
