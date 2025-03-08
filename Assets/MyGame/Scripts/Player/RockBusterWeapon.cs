using System;
using CriWare;
using UnityEngine;

public interface IPlayerWeapon
{
    void ChargeInit();
    void LaunchTrigger(bool trigger, Action callbackAfterLaunch);
    void Update();
}

public class RockBusterWeapon : IPlayerWeapon
{
    enum Chage_StateID
    {
        None,
        ChargeSmall,
        ChargeMiddle,
        ChargeBig
    }

    StateMachine<RockBusterWeapon> m_stateMachine = new StateMachine<RockBusterWeapon>();
    bool isLaunchTrigger = false;

    int curMameNum = 0;
    int mameMax = 3;

    class None : State<RockBusterWeapon, None>
    {
        protected override void Enter(RockBusterWeapon buster, int preId, int subId)
        {
            buster.m_player.StopRimLight();
            buster.m_player.ChargeAnimator.gameObject.SetActive(false);
        }
    }

    class ChargeSmall : State<RockBusterWeapon, ChargeSmall>
    {
        float chargeStartTime = 1.0f;
        protected override void Enter(RockBusterWeapon buster, int preId, int subId)
        {
            chargeStartTime = 1.0f;
        }

        protected override void Update(RockBusterWeapon buster)
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

    class ChargeMiddle : State<RockBusterWeapon, ChargeMiddle>
    {
        int animationHash = 0;
        float chargeStartTime = 1.0f;

        public ChargeMiddle() { animationHash = Animator.StringToHash("ChargingBlue"); }

        protected override void Enter(RockBusterWeapon buster, int preId, int subId)
        {
            buster.m_player.ChargeAnimator.gameObject.SetActive(true);
            buster.m_player.ChargeAnimator.Play(animationHash);
            buster.m_player.LimLightChaging();
            chargeStartTime = 1.0f;

            buster.m_player.chargePlayback = AudioManager.Instance.PlaySe(SECueIDs.charge);
        }

        protected override void Update(RockBusterWeapon buster)
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

    class ChargeBig : State<RockBusterWeapon, ChargeBig>
    {
        int animationHash = 0;
        public ChargeBig() { animationHash = Animator.StringToHash("ChargingYellow"); }

        protected override void Enter(RockBusterWeapon buster, int preId, int subId)
        {
            buster.m_player.ChargeAnimator.gameObject.SetActive(true);
            buster.m_player.ChargeAnimator.Play(animationHash);
        }

        protected override void Update(RockBusterWeapon buster)
        {
            if (!buster.isLaunchTrigger)
            {
                buster.m_stateMachine.TransitReady((int)Chage_StateID.None);
            }
        }
    }

    StagePlayer m_player;
    public RockBusterWeapon(StagePlayer player)
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
                    LaunchMame();
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
                    LaunchMiddle();
                    m_stateMachine.TransitReady((int)Chage_StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case Chage_StateID.ChargeBig:
                if (!this.isLaunchTrigger)
                {
                    LaunchBig();
                    m_stateMachine.TransitReady((int)Chage_StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
        }

    }

    void LaunchMame()
    {
        var projectile = ObjectManager.OnGet<RockBuster>(PoolType.RockBuster, (pjt) => { if (curMameNum > 0) curMameNum--; });
        projectile.Setup(m_player.Launcher.position, m_player.IsRight, 1, 16);
        curMameNum++;
        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }

    void LaunchMiddle()
    {
        if (m_player.chargePlayback.status == CriAtomExPlayback.Status.Playing) m_player.chargePlayback.Stop();
        var projectile = ObjectManager.OnGet<RockBuster>(PoolType.ChargeShotSmall);
        projectile.Setup(m_player.Launcher.position, m_player.IsRight, 2, 16);

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }

    void LaunchBig()
    {
        if (m_player.chargePlayback.status == CriAtomExPlayback.Status.Playing) m_player.chargePlayback.Stop();
        var projectile = ObjectManager.OnGet<RockBuster>(PoolType.ChargeShot);
        projectile.Setup(m_player.Launcher.position, m_player.IsRight, 3, 24);

        AudioManager.Instance.PlaySe(SECueIDs.chargeshot);
    }

    ObjectManager ObjectManager => ObjectManager.Instance;

}
