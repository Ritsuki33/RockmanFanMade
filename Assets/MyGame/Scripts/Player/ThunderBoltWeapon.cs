using System;
using UnityEngine;

public class ThunderBoltWeapon : IPlayerWeapon
{
    private bool isLaunchTrigger;

    ObjectManager ObjectManager => ObjectManager.Instance;
    int num = 0;
    StagePlayer m_player;

    public ThunderBoltWeapon(StagePlayer player)
    {
        m_player = player;
    }

    public void ChargeInit()
    {
        num = 0;
    }

    public void LaunchTrigger(bool isTrigger, Action actionFinishCallback)
    {
        if (isTrigger && !isLaunchTrigger && num == 0)
        {
            OnLaunch(m_player.IsRight);
            actionFinishCallback?.Invoke();
        }

        isLaunchTrigger = isTrigger;
    }

    public void Update() { }

    private void OnLaunch(bool isRight)
    {
        var thunderBolt = ObjectManager.OnGet<ThunderBolt>(PoolType.ThunderBolt, (pjt) => { if (num > 0) num--; });

        thunderBolt.Setup(m_player.Launcher.position, isRight, 2);
        num++;

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }
}
