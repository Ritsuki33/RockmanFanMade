using System;
using UnityEngine;

public class Player : StageObject
{
    [SerializeField] int maxHp = 27;

    int currentHp = 0;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    public Action<float> hpChangeTrigger;
    public void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        hpChangeTrigger?.Invoke((float)currentHp / maxHp);
    }

}
