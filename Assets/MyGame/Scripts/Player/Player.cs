using UnityEngine;

public class Player : StageObject
{
    [SerializeField] int maxHp = 27;

    int currentHp = 0;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        EventTriggerManager.Instance.Notify(FloatEventType.PlayerDamaged, (float)currentHp / maxHp);

        var controller = GameMainManager.Instance.ScreenContainer.GetCurrentScreenPresenter<GameMainScreenPresenter>();

        controller?.SetPlayerHp((float)currentHp / maxHp);
        //GameMainManager.Instance.HpBar.SetParam((float)currentHp/maxHp);
    }

}
