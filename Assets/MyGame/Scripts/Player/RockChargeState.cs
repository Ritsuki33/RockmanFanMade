using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StagePlayer
{
    [Header("チャージ")]
    [SerializeField] Animator m_charge_animator;
    [SerializeField] Transform launcher;

    ProjectilePool RockBusterPool => EffectManager.Instance.RockBusterPool;
    ProjectilePool RockBusterMiddlePool => EffectManager.Instance.RockBusterMiddlePool;
    ProjectilePool RockBusterBigPool => EffectManager.Instance.RockBusterBigPool;

    bool isLaunchTrigger = false;
    int rimLightColorId = Shader.PropertyToID("_RimLightColor");
    int FadeLightId = Shader.PropertyToID("_FadeLight");

    Coroutine chargingCo = default;

    // 豆バスターの数
    int curMameNum = 0;

    float chargeAnimSpeed;

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
        // チャージの状態セット
        m_chargeStateMachine.AddState((int)Chage_StateID.None, new None());
        m_chargeStateMachine.AddState((int)Chage_StateID.ChargeSmall, new ChargeSmall());
        m_chargeStateMachine.AddState((int)Chage_StateID.ChargeMiddle, new ChargeMiddle());
        m_chargeStateMachine.AddState((int)Chage_StateID.ChargeBig, new ChargeBig());

        chargeAnimSpeed = m_charge_animator.speed;
        m_charge_animator.gameObject.SetActive(false);
        isLaunchTrigger = false;
        m_chargeStateMachine.TransitReady((int)Chage_StateID.None);
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
        ObjectManager.Instance.CreateRockBuster(RockBuster.Small, launcher.position, 1, isRight, null, (rb) => rb.velocity = direction * speed, () => { if (curMameNum > 0) curMameNum--; });

        curMameNum++;
    }

    void LaunchMiddle(bool isRight)
    {
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 16;
        ObjectManager.Instance.CreateRockBuster(RockBuster.Middle, launcher.position, 2, isRight, null, (rb) => rb.velocity = direction * speed);
    }

    void LaunchBig(bool isRight)
    {
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 24;
        ObjectManager.Instance.CreateRockBuster(RockBuster.Big, launcher.position, 3, isRight, null, (rb) => rb.velocity = direction * speed);
    }

    public void StopRimLight()
    {
        if (chargingCo != null) StopCoroutine(chargingCo);
        SetMaterialParam(FadeLightId, 0);
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

                SetMaterialParam(rimLightColorId, color);

                while (true)
                {
                    SetMaterialParam(FadeLightId, 1);

                    yield return new WaitForSeconds(0.05f);

                    SetMaterialParam(FadeLightId, 0);

                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}
