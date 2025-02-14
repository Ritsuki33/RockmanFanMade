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
    protected override void Init()
    {
        screenContainer.Add(UI.BossSelect, m_bossSelectScreen);
        screenContainer.Add(UI.BossIntro, m_bossIntroScreen);

        screenContainer.TransitScreen(UI.BossSelect, false);
    }

    protected override void OnUpdate()
    {
        ScreenContainer.OnUpdate();
    }

    protected override void Terminate()
    {
        screenContainer.Clear();
    }
}
