using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using UnityEngine;

public interface IPlayerWeapon
{
    void ChargeInit();
    void LaunchTrigger(bool trigger, Action callbackAfterLaunch);
    void Update();
}

public class RockBuster : IPlayerWeapon
{
    enum Chage_StateID
    {
        None,
        ChargeSmall,
        ChargeMiddle,
        ChargeBig
    }

    StateMachine<RockBuster> m_stateMachine = new StateMachine<RockBuster>();
    bool isLaunchTrigger = false;

    int curMameNum = 0;
    int mameMax = 3;

    class None : State<RockBuster, None>
    {
        protected override void Enter(RockBuster buster, int preId, int subId)
        {
            buster.m_player.StopRimLight();
            buster.m_player.ChargeAnimator.gameObject.SetActive(false);
        }
    }

    class ChargeSmall : State<RockBuster, ChargeSmall>
    {
        float chargeStartTime = 1.0f;
        protected override void Enter(RockBuster buster, int preId, int subId)
        {
            chargeStartTime = 1.0f;
        }

        protected override void Update(RockBuster buster)
        {
            if (buster.isLaunchTrigger)
            {
                if (chargeStartTime < 0)
                {
                    buster.m_stateMachine.TransitReady((int)Chage_StateID.ChargeMiddle);
                }

                chargeStartTime -= Time.deltaTime;
            }
        }
    }

    class ChargeMiddle : State<RockBuster, ChargeMiddle>
    {
        int animationHash = 0;
        float chargeStartTime = 1.0f;

        public ChargeMiddle() { animationHash = Animator.StringToHash("ChargingBlue"); }

        protected override void Enter(RockBuster buster, int preId, int subId)
        {
            buster.m_player.ChargeAnimator.gameObject.SetActive(true);
            buster.m_player.ChargeAnimator.Play(animationHash);
            buster.m_player.LimLightChaging();
            chargeStartTime = 1.0f;

            buster.m_player.chargePlayback = AudioManager.Instance.PlaySe(SECueIDs.charge);
        }

        protected override void Update(RockBuster buster)
        {
            if (buster.isLaunchTrigger)
            {
                if (chargeStartTime < 0)
                {
                    buster.m_stateMachine.TransitReady((int)Chage_StateID.ChargeBig);
                }

                chargeStartTime -= Time.deltaTime;
            }

        }
    }

    class ChargeBig : State<RockBuster, ChargeBig>
    {
        int animationHash = 0;
        public ChargeBig() { animationHash = Animator.StringToHash("ChargingYellow"); }

        protected override void Enter(RockBuster buster, int preId, int subId)
        {
            buster.m_player.ChargeAnimator.gameObject.SetActive(true);
            buster.m_player.ChargeAnimator.Play(animationHash);
        }

        protected override void Update(RockBuster buster)
        {
            if (!buster.isLaunchTrigger)
            {
                buster.m_stateMachine.TransitReady((int)Chage_StateID.None);
            }
        }
    }

    StagePlayer m_player;
    public RockBuster(StagePlayer player)
    {
        m_player = player;
        m_stateMachine.AddState((int)Chage_StateID.None, new None());
        m_stateMachine.AddState((int)Chage_StateID.ChargeSmall, new ChargeSmall());
        m_stateMachine.AddState((int)Chage_StateID.ChargeMiddle, new ChargeMiddle());
        m_stateMachine.AddState((int)Chage_StateID.ChargeBig, new ChargeBig());
    }

    public void Update()
    {
        m_stateMachine.Update(this);
    }

    public void ChargeInit()
    {
        m_player.ChargeAnimator.gameObject.SetActive(false);
        isLaunchTrigger = false;
        m_stateMachine.TransitReady((int)Chage_StateID.None);
        if (m_player.chargePlayback.status == CriAtomExPlayback.Status.Playing) m_player.chargePlayback.Stop();
    }

    public void LaunchTrigger(bool trigger, Action callbackAfterLaunch)
    {
        this.isLaunchTrigger = trigger;

        switch ((Chage_StateID)m_stateMachine.CurId)
        {
            case Chage_StateID.None:
                if (this.isLaunchTrigger && curMameNum < mameMax)
                {
                    LaunchMame(m_player.IsRight);
                    m_stateMachine.TransitReady((int)Chage_StateID.ChargeSmall);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case Chage_StateID.ChargeSmall:
                if (!this.isLaunchTrigger)
                {
                    m_stateMachine.TransitReady((int)Chage_StateID.None);
                }
                break;
            case Chage_StateID.ChargeMiddle:
                if (!this.isLaunchTrigger)
                {
                    LaunchMiddle(m_player.IsRight);
                    m_stateMachine.TransitReady((int)Chage_StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case Chage_StateID.ChargeBig:
                if (!this.isLaunchTrigger)
                {
                    LaunchBig(m_player.IsRight);
                    m_stateMachine.TransitReady((int)Chage_StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
        }

    }


    void LaunchMame(bool isRight)
    {
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 16;
        var projectile = ObjectManager.OnGet<Projectile>(PoolType.RockBuster, (pjt) => { if (curMameNum > 0) curMameNum--; });

        projectile.Setup(
            m_player.Launcher.position, isRight, 1, null, (rb) => rb.velocity = direction * speed
            );
        curMameNum++;

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }

    void LaunchMiddle(bool isRight)
    {
        if (m_player.chargePlayback.status == CriAtomExPlayback.Status.Playing) m_player.chargePlayback.Stop();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 16;
        var projectile = ObjectManager.OnGet<Projectile>(PoolType.ChargeShotSmall);
        projectile.Setup(
           m_player.Launcher.position, isRight, 2, null, (rb) => rb.velocity = direction * speed
           );

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }

    void LaunchBig(bool isRight)
    {
        if (m_player.chargePlayback.status == CriAtomExPlayback.Status.Playing) m_player.chargePlayback.Stop();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 24;

        var projectile = ObjectManager.OnGet<Projectile>(PoolType.ChargeShot);
        projectile.Setup(
           m_player.Launcher.position, isRight, 3, null, (rb) => rb.velocity = direction * speed
           );

        AudioManager.Instance.PlaySe(SECueIDs.chargeshot);
    }

    ObjectManager ObjectManager => ObjectManager.Instance;

}
