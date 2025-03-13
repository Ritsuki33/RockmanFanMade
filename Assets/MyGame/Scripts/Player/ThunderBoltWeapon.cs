using System;
using UnityEngine;

public class ThunderBoltWeapon : BasePlayerWeapon, IPlayerWeapon
{
    private bool isLaunchTrigger;

    int num = 0;
    StagePlayer m_player;

    public ThunderBoltWeapon(StagePlayer player)
    {
        m_player = player;
        Energy = EnergyMax;
    }

    public void ChargeInit()
    {
        num = 0;
    }

    public void LaunchTrigger(bool isTrigger, Action actionFinishCallback)
    {
        if (isTrigger && !isLaunchTrigger && num == 0)
        {
            if (!isLock) OnLaunch(m_player.IsRight);
            actionFinishCallback?.Invoke();
        }

        isLaunchTrigger = isTrigger;
    }

    public void Update() { }

    private void OnLaunch(bool isRight)
    {
        if (Energy <= 0) return;
        ConsumeEnergy(1);
        var thunderBolt = ObjectManager.OnGet<ThunderBolt>(PoolType.ThunderBolt, (pjt) => { if (num > 0) num--; });

        thunderBolt.Setup(m_player.Launcher.position, isRight, 2);
        num++;

        AudioManager.Instance.PlaySe(SECueIDs.thunder);
    }


    ObjectManager ObjectManager => ObjectManager.Instance;
    public PlayerWeaponType Type => PlayerWeaponType.ThunderBolt;
}
