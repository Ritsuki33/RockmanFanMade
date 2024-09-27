using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : StateMachine<LauncherController>
{
    [SerializeField] Player player;
    [SerializeField] Animator m_charge_animator;

    [SerializeField] GameObject launcher;

    [SerializeField] int mameMax = 3;

    BaseObjectPool RockBusterPool => EffectManager.Instance.RockBusterPool;
    BaseObjectPool RockBusterMiddlePool => EffectManager.Instance.RockBusterMiddlePool;
    BaseObjectPool RockBusterBigPool => EffectManager.Instance.RockBusterBigPool;

    bool isLaunchTrigger = false;
    int rimLightColorId = Shader.PropertyToID("_RimLightColor");
    int FadeLightId = Shader.PropertyToID("_FadeLight");

    Coroutine chargingCo = default;

    // 豆バスターの数
    int curMameNum = 0;
    enum StateID
    {
        None,
        ChargeSmall,
        ChargeMiddle,
        ChargeBig
    }

    private void Awake()
    {
        m_charge_animator.gameObject.SetActive(false);
        stateMachine.AddState((int)StateID.None, new None());
        stateMachine.AddState((int)StateID.ChargeSmall, new ChargeSmall());
        stateMachine.AddState((int)StateID.ChargeMiddle, new ChargeMiddle());
        stateMachine.AddState((int)StateID.ChargeBig, new ChargeBig());

        TransitReady((int)StateID.None);
    }

    class None : State<LauncherController>
    {
        protected override void Enter(LauncherController launcher, int preId, int subId)
        {
            launcher.StopRimLight();
            launcher.m_charge_animator.gameObject.SetActive(false);
        }
    }

    class ChargeSmall : State<LauncherController>
    {
        float chargeStartTime = 1.0f;
        protected override void Enter(LauncherController launcher, int preId, int subId)
        {
            chargeStartTime = 1.0f;
        }

        protected override void Update(LauncherController launcher, IParentState parent)
        {
            if (launcher.isLaunchTrigger)
            {
                if (chargeStartTime < 0)
                {
                    launcher.TransitReady((int)StateID.ChargeMiddle);
                }

                chargeStartTime -= Time.deltaTime;
            }
        }
    }

    class ChargeMiddle : State<LauncherController>
    {
        int animationHash = 0;
        float chargeStartTime = 1.0f;
        public ChargeMiddle() { animationHash = Animator.StringToHash("ChargingBlue"); }

        protected override void Enter(LauncherController launcher, int preId, int subId)
        {
            launcher.m_charge_animator.gameObject.SetActive(true);
            launcher.m_charge_animator.Play(animationHash);
            launcher.LimLightChaging();
            chargeStartTime = 1.0f;
        }

        protected override void Update(LauncherController launcher, IParentState parent)
        {
            if (launcher.isLaunchTrigger)
            {
                if (chargeStartTime < 0)
                {
                    launcher.TransitReady((int)StateID.ChargeBig);
                }

                chargeStartTime -= Time.deltaTime;
            }
            
        }
    }

    class ChargeBig : State<LauncherController>
    {
        int animationHash = 0;
        public ChargeBig() { animationHash = Animator.StringToHash("ChargingYellow"); }

        protected override void Enter(LauncherController launcher, int preId, int subId)
        {
            launcher.m_charge_animator.gameObject.SetActive(true);
            launcher.m_charge_animator.Play(animationHash);
        }

        protected override void Update(LauncherController launcher, IParentState parent)
        {
            if (!launcher.isLaunchTrigger)
            {
                launcher.TransitReady((int)StateID.None);
            }
        }
    }

    public void LaunchTrigger(bool trigger, Action callbackAfterLaunch)
    {
        this.isLaunchTrigger = trigger;

        switch ((StateID)stateMachine.CurId)
        {
            case StateID.None:
                if (this.isLaunchTrigger && curMameNum < mameMax)
                {
                    LaunchMame(player.IsRight);
                    TransitReady((int)StateID.ChargeSmall);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case StateID.ChargeSmall:
                if (!this.isLaunchTrigger)
                {
                    TransitReady((int)StateID.None);
                }
                break;
            case StateID.ChargeMiddle:
                if (!this.isLaunchTrigger)
                {
                    LaunchMiddle(player.IsRight);
                    TransitReady((int)StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
            case StateID.ChargeBig:
                if (!this.isLaunchTrigger)
                {
                    LaunchBig(player.IsRight);
                    TransitReady((int)StateID.None);
                    callbackAfterLaunch.Invoke();
                }
                break;
        }
    }

    void LaunchMame(bool isRight)
    {
        var rockBaster = RockBusterPool.Pool.Get();
        var projectile = rockBaster.GetComponent<Projectile>();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        float speed = 16;
        projectile.Init(
            1,
            null,
            (rb) =>
            {
                rb.velocity = direction * speed;
            },
            null,
            () =>
            {
                if (curMameNum > 0) curMameNum--;
            }
            );
        projectile.transform.position = new Vector3(launcher.transform.position.x, launcher.transform.position.y, -2);

        curMameNum++;
    }

    void LaunchMiddle(bool isRight)
    {
        var rockBaster = RockBusterMiddlePool.Pool.Get();
        var projectile = rockBaster.GetComponent<Projectile>();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        Vector2 localScale = projectile.transform.localScale;
        localScale.x = (isRight) ? 1 : -1;
        projectile.transform.localScale = localScale;
        float speed = 16;
        projectile.Init(
            2,
            null
,
            (rb) =>
            {
                rb.velocity = direction * speed;
            });
        projectile.transform.position = new Vector3(launcher.transform.position.x, launcher.transform.position.y, -2);
    }

    void LaunchBig(bool isRight)
    {
        var rockBaster = RockBusterBigPool.Pool.Get();

        var projectile = rockBaster.GetComponent<Projectile>();
        Vector2 direction = (isRight) ? Vector2.right : Vector2.left;
        Vector2 localScale = projectile.transform.localScale;
        localScale.x = (isRight) ? 1 : -1;
        projectile.transform.localScale = localScale;
        float speed = 24;
        projectile.Init(
            3,
            null
,
            (rb) =>
            {
                rb.velocity = direction * speed;
            });
        projectile.transform.position = new Vector3(launcher.transform.position.x, launcher.transform.position.y, -2);
    }

    public void StopRimLight()
    {
        if (chargingCo != null) StopCoroutine(chargingCo);
        player.SetMaterialParam(FadeLightId, 0);
        chargingCo = null;
    }

    public void LimLightChaging()
    {
        if(chargingCo!=null) StopCoroutine(chargingCo);

        chargingCo = StartCoroutine(ChagingMiddleCo());
        IEnumerator ChagingMiddleCo()
        {
            if (ColorUtility.TryParseHtmlString("#81C3FF", out Color color))
            {

                player.SetMaterialParam(rimLightColorId, color);

                while (true)
                {
                    player.SetMaterialParam(FadeLightId, 1);

                    yield return new WaitForSeconds(0.05f);

                    player.SetMaterialParam(FadeLightId, 0);

                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}
