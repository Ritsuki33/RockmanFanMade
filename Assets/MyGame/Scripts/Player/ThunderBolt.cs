using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : IPlayerWeapon
{
    private bool isLaunchTrigger;

    ObjectManager ObjectManager => ObjectManager.Instance;
    int num = 0;
    StagePlayer m_player;

    public ThunderBolt(StagePlayer player)
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
        Vector2 direction = isRight ? Vector2.right : Vector2.left;
        float speed = 24;
        var projectile = ObjectManager.OnGet<Projectile>(PoolType.ThunderBolt, (pjt) => { if (num > 0) num--; });

        projectile.Setup(
            m_player.Launcher.position,
            isRight,
            1,
            null,
            (rb) => rb.velocity = direction * speed,
            (pjt) =>
            {
                var pjt1 = ObjectManager.OnGet<Projectile>(PoolType.ThunderBoltMini);
                var pjt2 = ObjectManager.OnGet<Projectile>(PoolType.ThunderBoltMini);

                // 上下に飛散
                pjt1.Setup(pjt.transform.position, isRight, 1, null, (rb) => rb.velocity = Vector2.up * speed);
                pjt2.Setup(pjt.transform.position, isRight, 1, null, (rb) => rb.velocity = Vector2.down * speed);
            }
            );
        num++;

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }
}
