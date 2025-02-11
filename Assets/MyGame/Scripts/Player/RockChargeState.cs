using CriWare;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StagePlayer
{
    [Header("チャージ")]
    [SerializeField] Animator m_charge_animator;
    [SerializeField] Transform launcher;

    bool isLaunchTrigger = false;
    int rimLightColorId = Shader.PropertyToID("_RimLightColor");
    int FadeLightId = Shader.PropertyToID("_FadeLight");

    Coroutine chargingCo = default;

    ObjectManager ObjectManager => ObjectManager.Instance;
    // 豆バスターの数
    int curMameNum = 0;

    float chargeAnimSpeed;

    CriAtomExPlayback chargePlayback = default;
    enum Chage_StateID
    {
        None,
        ChargeSmall,
        ChargeMiddle,
        ChargeBig
    }

    class None : State<StagePlayer, None>
    {
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.StopRimLight();
            player.m_charge_animator.gameObject.SetActive(false);
        }
    }

    class ChargeSmall : State<StagePlayer, ChargeSmall>
    {
        float chargeStartTime = 1.0f;
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            chargeStartTime = 1.0f;
        }

        protected override void Update(StagePlayer player)
        {
            if (player.isLaunchTrigger)
            {
                if (chargeStartTime < 0)
                {
                    player.m_chargeStateMachine.TransitReady((int)Chage_StateID.ChargeMiddle);
                }

                chargeStartTime -= Time.deltaTime;
            }
        }
    }

    class ChargeMiddle : State<StagePlayer, ChargeMiddle>
    {
        int animationHash = 0;
        float chargeStartTime = 1.0f;

        public ChargeMiddle() { animationHash = Animator.StringToHash("ChargingBlue"); }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.m_charge_animator.gameObject.SetActive(true);
            player.m_charge_animator.Play(animationHash);
            player.LimLightChaging();
            chargeStartTime = 1.0f;

            player.chargePlayback = AudioManager.Instance.PlaySe(SECueIDs.charge);
        }

        protected override void Update(StagePlayer player)
        {
            if (player.isLaunchTrigger)
            {
                if (chargeStartTime < 0)
                {
                    player.m_chargeStateMachine.TransitReady((int)Chage_StateID.ChargeBig);
                }

                chargeStartTime -= Time.deltaTime;
            }

        }
    }

    class ChargeBig : State<StagePlayer, ChargeBig>
    {
        int animationHash = 0;
        public ChargeBig() { animationHash = Animator.StringToHash("ChargingYellow"); }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.m_charge_animator.gameObject.SetActive(true);
            player.m_charge_animator.Play(animationHash);
        }

        protected override void Update(StagePlayer player)
        {
            if (!player.isLaunchTrigger)
            {
                player.m_chargeStateMachine.TransitReady((int)Chage_StateID.None);
            }
        }
    }

    void ChargeInit()
    {
        m_charge_animator.gameObject.SetActive(false);
        isLaunchTrigger = false;
        m_chargeStateMachine.TransitReady((int)Chage_StateID.None);
        if (chargePlayback.status == CriAtomExPlayback.Status.Playing) chargePlayback.Stop();
    }

    public void LaunchTrigger(bool trigger, Action callbackAfterLaunch)
    {
        this.isLaunchTrigger = trigger;

        switch ((Chage_StateID)m_chargeStateMachine.CurId)
        {
            case Chage_StateID.None:
                if (this.isLaunchTrigger && curMameNum < mameMax)
                {
                    LaunchMame(IsRight);
                    m_chargeStateMachine.TransitReady((int)Chage_StateID.ChargeSmall);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case Chage_StateID.ChargeSmall:
                if (!this.isLaunchTrigger)
                {
                    m_chargeStateMachine.TransitReady((int)Chage_StateID.None);
                }
                break;
            case Chage_StateID.ChargeMiddle:
                if (!this.isLaunchTrigger)
                {
                    LaunchMiddle(IsRight);
                    m_chargeStateMachine.TransitReady((int)Chage_StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case Chage_StateID.ChargeBig:
                if (!this.isLaunchTrigger)
                {
                    LaunchBig(IsRight);
                    m_chargeStateMachine.TransitReady((int)Chage_StateID.None);
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
            launcher.position, isRight, 1, null, (rb) => rb.velocity = direction * speed
            );
        curMameNum++;

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }

    void LaunchMiddle(bool isRight)
    {
        if (chargePlayback.status == CriAtomExPlayback.Status.Playing) chargePlayback.Stop();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 16;
        var projectile = ObjectManager.OnGet<Projectile>(PoolType.ChargeShotSmall);
        projectile.Setup(
           launcher.position, isRight, 2, null, (rb) => rb.velocity = direction * speed
           );

        AudioManager.Instance.PlaySe(SECueIDs.buster);
    }

    void LaunchBig(bool isRight)
    {
        if (chargePlayback.status == CriAtomExPlayback.Status.Playing) chargePlayback.Stop();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 24;

        var projectile = ObjectManager.OnGet<Projectile>(PoolType.ChargeShot);
        projectile.Setup(
           launcher.position, isRight, 3, null, (rb) => rb.velocity = direction * speed
           );

        AudioManager.Instance.PlaySe(SECueIDs.chargeshot);
    }

    public void StopRimLight()
    {
        if (chargingCo != null) StopCoroutine(chargingCo);
        MainMaterial.SetFloat(FadeLightId, 0);
        chargingCo = null;
    }

    public void LimLightChaging()
    {
        if (chargingCo != null) StopCoroutine(chargingCo);

        chargingCo = StartCoroutine(ChagingMiddleCo());
        IEnumerator ChagingMiddleCo()
        {
            if (ColorUtility.TryParseHtmlString("#81C3FF", out Color color))
            {
                MainMaterial.SetColor(rimLightColorId, color);
                while (true)
                {
                    MainMaterial.SetFloat(FadeLightId, 1);

                    yield return PauseManager.Instance.PausableWaitForSeconds(0.05f);

                    MainMaterial.SetFloat(FadeLightId, 0);

                    yield return PauseManager.Instance.PausableWaitForSeconds(0.05f);
                }
            }
        }
    }
}
