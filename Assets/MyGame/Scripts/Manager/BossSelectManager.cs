using System.Collections;
using UnityEngine;

public class BossSelectManager : BaseManager<BossSelectManager>
{
    public enum UI
    {
        BossSelect,
        BossIntro
    }

    [SerializeField] BossSelectScreen m_bossSelectScreen = default;
    [SerializeField] BossIntroScreen m_bossIntroScreen = default;

    private ScreenContainer<UI> screenContainer = new ScreenContainer<UI>();
    public ScreenContainer<UI> ScreenContainer => screenContainer;
    protected override IEnumerator Init()
    {
        screenContainer.Add(UI.BossSelect, m_bossSelectScreen);
        screenContainer.Add(UI.BossIntro, m_bossIntroScreen);

        screenContainer.TransitScreen(UI.BossSelect, true);
        yield return null;
    }

    protected override void OnUpdate()
    {
        ScreenContainer.OnUpdate();
    }

    protected override IEnumerator Dispose()
    {
        screenContainer.Clear();
        yield return null;
    }

    public void TransitToGameMain()
    {
        screenContainer.Close(true, () =>
        {
            SceneManager.Instance.ChangeManager(ManagerType.GameMain);
        });
    }
}
