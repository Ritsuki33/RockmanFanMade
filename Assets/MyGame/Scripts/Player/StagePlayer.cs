using System;
using System.Collections;
using UnityEngine;

public partial class StagePlayer : PhysicalObject, IDirect,IBeltConveyorVelocity
{
    [SerializeField] ExpandRigidBody exRb;
    [Header("プレイヤー情報")]
    [SerializeField] int maxHp = 27;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] int mameMax = 3;
    [SerializeField] Gravity gravity;
    [SerializeField] Move move;
    [SerializeField] Jump jump;
    [SerializeField] Direct direct;

    ExRbStateMachine<StagePlayer> m_mainStateMachine = new ExRbStateMachine<StagePlayer>();
    StateMachine<StagePlayer> m_chargeStateMachine = new StateMachine<StagePlayer>();

    ReactiveProperty<float> hp = new ReactiveProperty<float>(0);
    public IReadOnlyReactiveProperty<float> Hp => hp;
    int currentHp = 0;

    public int CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            hp.Value = (float)currentHp / MaxHp;
        }
    }
    public int MaxHp => maxHp;


    //public Action<float> hpChangeTrigger;


    Collider2D bodyLadder = null;

    BoxCollider2D boxPhysicalCollider = null;
    bool isladderTop = false;

    GameMainManager.InputInfo inputInfo;

    Transform bamili = null;

    Action actionFinishCallback = null;

    AmbiguousTimer timer = new AmbiguousTimer();

    bool invincible = false;

    ExRbHit exRbHit = new ExRbHit();
    RbCollide rbCollide = new RbCollide();

    private bool isDead = false;

    RaycastHit2D bottomHit = default;

    Ground curGround = default;
    enum Main_StateID
    {
        Standing = 0,
        Floating,
        Running,
        Climb,
        Jumping,
        ClimbUp,
        ClimbDown,
        Death,
        Transfer,
        Transfered,
        AutoMove,
        Repatriate,
        Damaged,
    }

    protected override void Awake()
    {
        base.Awake();
        m_mainStateMachine.Clear();

        // メインの状態セット
        m_mainStateMachine.AddState((int)Main_StateID.Standing, new Standing());
        m_mainStateMachine.AddState((int)Main_StateID.Floating, new Floating());
        m_mainStateMachine.AddState((int)Main_StateID.Running, new Running());
        m_mainStateMachine.AddState((int)Main_StateID.Climb, new Climb());
        m_mainStateMachine.AddState((int)Main_StateID.Jumping, new Jumping());
        m_mainStateMachine.AddState((int)Main_StateID.ClimbUp, new ClimbUp());
        m_mainStateMachine.AddState((int)Main_StateID.ClimbDown, new ClimbDown());
        m_mainStateMachine.AddState((int)Main_StateID.Death, new Death());
        m_mainStateMachine.AddState((int)Main_StateID.Transfer, new Transfer());
        m_mainStateMachine.AddState((int)Main_StateID.Transfered, new Transfered());
        m_mainStateMachine.AddState((int)Main_StateID.AutoMove, new AutoMove());
        m_mainStateMachine.AddState((int)Main_StateID.Repatriate, new Repatriation());
        m_mainStateMachine.AddState((int)Main_StateID.Damaged, new DamagedState());

        boxPhysicalCollider = GetComponent<BoxCollider2D>();

        exRb.Init();

        exRbHit.Init(exRb);
        rbCollide.Init();

        rbCollide.onTriggerEnter += OnTriggerEnterBase;
        rbCollide.onTriggerEnterDamageBase += OnTriggerStayDamageBase;
        rbCollide.onTriggerEnterRecovery += OnTriggerStayRecovery;

        exRbHit.onBottomHitStay += OnBottomHitStay;
        exRbHit.onBottomHitExit += OnBottomHitExit;
        exRbHit.onTopHitEnterBlockTilemap += OnTopHitEnter;
        exRbHit.onTopHitStay += OnTopHitStay;
        exRbHit.onHitEnter += OnHitEnter;
        exRbHit.onHitStayDamageBase += OnHitStay;
        exRbHit.onBottomHitStayBeltConveyor += OnBottomHitStay;
        exRbHit.onBottomHitStayTire += OnBottmHitStay;
        exRbHit.onBottomHitStayGround += OnBottomHitStay;
        exRbHit.onBottomHitExitGround += OnBottomHitExit;

        m_chargeStateMachine.Clear();
        // チャージの状態セット
        m_chargeStateMachine.AddState((int)Chage_StateID.None, new None());
        m_chargeStateMachine.AddState((int)Chage_StateID.ChargeSmall, new ChargeSmall());
        m_chargeStateMachine.AddState((int)Chage_StateID.ChargeMiddle, new ChargeMiddle());
        m_chargeStateMachine.AddState((int)Chage_StateID.ChargeBig, new ChargeBig());
        chargeAnimSpeed = m_charge_animator.speed;
    }

    protected override void Init()
    {
        base.Init();

        invincible = false;
        isDead = false;
        ChargeInit();

        bottomHit = default;
        curGround = default;
    }

    protected override void Destroy()
    {
        exRb.DeleteCache();
        hp.Dispose();
    }

    protected override void OnFixedUpdate()
    {
        m_mainStateMachine.FixedUpdate(this);
        m_chargeStateMachine.FixedUpdate(this);
    }

    protected override void OnLateFixedUpdate()
    {
        exRb.FixedUpdate();

        if (CrashCheck(exRb.BoxColliderCenter, exRb.PhysicalBoxSize, exRb.PhysicalLayer))
        {
            Dead();
        }
    }

    protected override void OnUpdate()
    {
        m_mainStateMachine.Update(this);
        m_chargeStateMachine.Update(this);
    }

    protected override void OnPause(bool isPause)
    {
        bool current = IsPause;
        base.OnPause(isPause);
        if (isPause)
        {
            if (!current)
            {
                chargeAnimSpeed = m_charge_animator.speed;
                m_charge_animator.speed = 0.0f;
            }
        }
        else
        {
            m_charge_animator.speed = chargeAnimSpeed;
        }
    }

    public void SetHp(int hp)
    {
        this.CurrentHp = Mathf.Clamp(hp, 0, maxHp);
    }

    public void UpdateInput(GameMainManager.InputInfo input)
    {
        inputInfo = input;
    }

    public void Dead()
    {
        if (isDead) return;
        isDead = true;
        m_mainStateMachine.TransitReady((int)Main_StateID.Death);
        GameMainManager.Instance.DeathNotification();
    }

    public void TransferedAnimationEnd()
    {
        m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
    }

    public void Prepare(Transform tranform)
    {
        this.gameObject.SetActive(false);
        this.transform.position = tranform.position;
    }

    public void TransferPlayer(Action actionFinishCallback = null)
    {
        this.gameObject.SetActive(true);
        this.actionFinishCallback = actionFinishCallback;
        m_mainStateMachine.TransitReady((int)Main_StateID.Transfer);
    }

    public void RepatriatePlayer(Action actionFinishCallback = null)
    {
        this.actionFinishCallback = actionFinishCallback;
        m_mainStateMachine.TransitReady((int)Main_StateID.Repatriate);
    }

    public void AutoMoveTowards(Transform bamili, Action actionFinishCallback)
    {
        this.bamili = bamili;
        this.actionFinishCallback = actionFinishCallback;
        m_mainStateMachine.TransitReady((int)Main_StateID.AutoMove, true);
    }

    /// <summary>
    /// 入力の禁止
    /// </summary>
    /// <param name="actionFinishCallback"></param>
    public void InputProhibit(Action actionFinishCallback)
    {
        m_mainStateMachine.TransitReady((int)Main_StateID.AutoMove, (int)AutoMove.SubStateId.Wait);
    }

    /// <summary>
    /// 入力を許可
    /// </summary>
    public void InputPermission()
    {
        m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
    }

    public void ActionFinishNotify()
    {
        actionFinishCallback?.Invoke();
        actionFinishCallback = null;
    }

    /// <summary>
    /// カメラのブレンディングによる強制移動
    /// </summary>
    /// <param name="actionFinishCallback"></param>
    public void PlayerForceMoveAccordingToCamera(Action actionFinishCallback)
    {
        StartCoroutine(PlayerForceMoveAccordingToCameraCo());
        actionFinishCallback?.Invoke();

        IEnumerator PlayerForceMoveAccordingToCameraCo()
        {
            GameMainManager.Instance.Pausable = false;
            WorldManager.Instance.OnPause(true);
            while (WorldManager.Instance.IsPause)
            {
                this.transform.position += GameMainManager.Instance.MainCameraControll.DeltaMove * 0.08f;
                yield return null;
            }
        }
    }

    /// <summary>
    /// カメラのブレンディングによる強制移動 終了
    /// </summary>
    /// <param name="actionFinishCallback"></param>
    public void PlayerForceMoveAccordingToCameraEnd(Action actionFinishCallback)
    {
        GameMainManager.Instance.Pausable = true;
        WorldManager.Instance.OnPause(false);
        actionFinishCallback?.Invoke();
    }

    public void Damaged(int val)
    {
        SetHp(CurrentHp - val);

        if (CurrentHp <= 0)
        {
            Dead();
        }
        else
        {
            m_mainStateMachine.TransitReady((int)Main_StateID.Damaged);
        }
    }

    public void InvincibleState()
    {
        StartCoroutine(InvincibleStateCo());

        IEnumerator InvincibleStateCo()
        {
            invincible = true;
            int count = 5;

            for (int i = 0; i < count; i++)
            {
                spriteRenderer.enabled = false;

                yield return PauseManager.Instance.PausableWaitForSeconds(0.05f);

                spriteRenderer.enabled = true;

                yield return PauseManager.Instance.PausableWaitForSeconds(0.05f);
            }

            invincible = false;

            yield return null;
        }
    }

    public void RecoverHp(int val)
    {
        // GUIとの切り離し
        hp.Dispose();

        float startParam = hp.Value;
        SetHp(CurrentHp + val);

        // 無敵
        invincible = true;

        // ポーズを掛ける
        WorldManager.Instance.OnPause(true);
        GameMainManager.Instance.GameMainScreenPresenter.PlayerHpIncrementAnimation(startParam, hp.Value, Hp, () =>
        {
            WorldManager.Instance.OnPause(false);
            invincible = false;
        });
    }

    private void OnTriggerEnterBase(Collider2D collision)
    {
        m_mainStateMachine.OnTriggerEnter(this, collision);
    }

    private void OnTriggerStayRecovery(Recovery recovery)
    {
        RecoverHp(recovery.Amount);
        recovery.gameObject.SetActive(false);
    }

    void OnTriggerStayDamageBase(DamageBase damageBase)
    {
        m_mainStateMachine.OnTriggerStay(this, damageBase);
    }

    void OnBottomHitStay(RaycastHit2D hit)
    {
        m_mainStateMachine.OnBottomHitStay(this, hit);
    }

    void OnBottomHitExit(RaycastHit2D hit)
    {
        m_mainStateMachine.OnBottomHitExit(this, hit);
    }


    void OnTopHitStay(RaycastHit2D hit)
    {
        m_mainStateMachine.OnTopHitStay(this, hit);
    }

    void OnCrash(ContactPoint2D a, ContactPoint2D b)
    {
        Damaged(int.MaxValue);
    }

    void OnHitStay(DamageBase damage)
    {
        if (!invincible) Damaged(damage.baseDamageValue);
    }

    void OnHitEnter(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Door"))
        {
            var shutter = hit.collider.GetComponent<ShutterControll>();
            shutter.Enter();
        }
    }

    void OnBottomHitStay(Ground ground)
    {
        //m_mainStateMachine.OnBottomHitStay(this, ground);
        curGround = ground;
    }

    void OnBottomHitExit(Ground ground)
    {
        curGround = default;
    }

    void OnBottomHitStay(BeltConveyor beltConveyor)
    {
        beltConveyor.GetOn(this);
    }

    void OnBottmHitStay(Tire tire)
    {
        jump.Init(tire.JumpPower);
        m_mainStateMachine.TransitReady((int)Main_StateID.Jumping);
        tire.OnSteppedOn();
    }

    void OnTopHitEnter(BlockTilemap blockTilemap)
    {
        // ヒット位置情報が取ってこれないので、ブロックが壊せないｗ
        Debug.Log("トップヒット");
    }

    private void OnTriggerEnter2D(Collider2D collision) => rbCollide.OnTriggerEnter(collision);
    private void OnTriggerStay2D(Collider2D collision) => rbCollide.OnTriggerEnter(collision);
    private void OnTriggerExit2D(Collider2D collision) => rbCollide.OnTriggerExit(collision);


    private void OnDrawGizmos()
    {
        exRb.OnDrawGizmos();
    }

    public bool IsRight => direct.IsRight;

    Vector2 IBeltConveyorVelocity.velocity { get => exRb.velocity; set => exRb.velocity = value; }

    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();
}