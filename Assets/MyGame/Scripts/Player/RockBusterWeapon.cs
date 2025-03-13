using System;
using CriWare;
using UnityEngine;

public interface IPlayerWeapon
{
    void ChargeInit();
    void LaunchTrigger(bool trigger, Action callbackAfterLaunch);
    int Energy { get; }
    void Update();
    int EnergyMax { get; }

    event Action<int, int> EnergyChangeCallback;
    event Action<int, int, Action> EnergyRecoveryCallback;
    void RecoveryEnergy(int amount, Action callback);

    void OnRefresh();

    PlayerWeaponType Type { get; }

    void Lock(bool isLock);
}
public class BasePlayerWeapon
{
    public int Energy { get; protected set; } = 0;
    public int EnergyMax { get; protected set; } = 27;
    public event Action<int, int> EnergyChangeCallback;
    public event Action<int, int, Action> EnergyRecoveryCallback;
    protected bool isLock = false;
    public void ConsumeEnergy(int amount)
    {
        Energy -= amount;
        if (Energy < 0) Energy = 0;
        EnergyChangeCallback?.Invoke(Energy, EnergyMax);
    }

    public void RecoveryEnergy(int amount, Action callback)
    {
        Energy += amount;
        if (Energy > EnergyMax) Energy = EnergyMax;

        var hpPlayback = AudioManager.Instance.PlaySe(SECueIDs.hprecover);
        EnergyRecoveryCallback?.Invoke(Energy, EnergyMax, () =>
        {
            hpPlayback.Stop();
            callback?.Invoke();
        });
    }
    public void Lock(bool isLock)
    {
        this.isLock = isLock;
    }

    public void OnRefresh()
    {
        EnergyChangeCallback?.Invoke(Energy, EnergyMax);
    }
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

    public int Energy { get; private set; } = 0;
    public int EnergyMax { get; private set; } = 0;

    public event Action<int, int> EnergyChangeCallback;
    public event Action<int, int, Action> EnergyRecoveryCallback;
    protected bool isLock = false;

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

        isLock = false;
    }

    public void LaunchTrigger(bool trigger, Action callbackAfterLaunch)
    {
        this.isLaunchTrigger = trigger;

        switch ((Chage_StateID)m_stateMachine.CurId)
        {
            case Chage_StateID.None:
                if (this.isLaunchTrigger && curMameNum < mameMax)
                {
                    if (!isLock) LaunchMame();
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
                    if (!isLock) LaunchMiddle();
                    m_stateMachine.TransitReady((int)Chage_StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case Chage_StateID.ChargeBig:
                if (!this.isLaunchTrigger)
                {
                    if (!isLock) LaunchBig();
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
    public void Lock(bool isLock)
    {
        this.isLock = isLock;
    }

    public void ConsumeEnergy(int amount) { /*特になし*/ }


    public void RecoveryEnergy(int amount, Action callback) { /*特になし*/ }

    public void OnRefresh() { }

    ObjectManager ObjectManager => ObjectManager.Instance;
    public PlayerWeaponType Type => PlayerWeaponType.RockBuster;

}
