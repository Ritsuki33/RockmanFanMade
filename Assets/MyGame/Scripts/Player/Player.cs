using UnityEngine;

public class Player : StageObject
{
    [SerializeField] int maxHp = 27;

    int currentHp = 0;
    public void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        GameMainManager.Instance.HpBar.SetParam((float)currentHp/maxHp);
    }
}
