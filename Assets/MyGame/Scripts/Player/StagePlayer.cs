using System;
using System.Collections;
using UnityEngine;

public partial class StagePlayer : PhysicalObject, IDirect
{
    [SerializeField] ExpandRigidBody exRb;
    [Header("プレイヤー情報")]
    [SerializeField] int maxHp = 27;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] int mameMax = 3;
    [SerializeField] Gravity gravity;
    [SerializeField] Move move;
    [SerializeField] OnTheGround onTheGround;
    [SerializeField] Jump jump;
    [SerializeField] Direct direct;

    ExRbStateMachine<StagePlayer> m_mainStateMachine = new ExRbStateMachine<StagePlayer>();
    StateMachine<StagePlayer> m_chargeStateMachine = new StateMachine<StagePlayer>();


    int currentHp = 0;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;


    public Action<float> hpChangeTrigger;


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

        exRbHit.onBottomHitStay += OnBottomHitStay;
        exRbHit.onTopHitStay += OnTopHitStay;
        exRbHit.onBottomTopHitStay += OnBottomTopHitEnter;
        exRbHit.onLeftRightHitEnter += OnLeftRightHitEnter;
        exRbHit.onHitEnter += OnHitEnter;

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

        ChargeInit();
    }

    protected override void OnFixedUpdate()
    {
        m_mainStateMachine.FixedUpdate(this);
        m_chargeStateMachine.FixedUpdate(this);

        exRb.FixedUpdate();
    }

    protected override void OnUpdate()
    {
        m_mainStateMachine.Update(this);
        m_chargeStateMachine.Update(this);
    }

    protected override void OnPause(bool isPause)
    {
        base.OnPause(isPause);
        if (isPause)
        {
            chargeAnimSpeed = m_charge_animator.speed;
            m_charge_animator.speed = 0.0f;
        }
        else
        {
            m_charge_animator.speed = chargeAnimSpeed;
        }
    }

    public void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        hpChangeTrigger?.Invoke((float)currentHp / maxHp);
    }

    public void UpdateInput(GameMainManager.InputInfo input)
    {
        inputInfo = input;
    }

    public void Dead()
    {
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
            RequestPause(true);
            while (IsPause)
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
        RequestPause(false);
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

    private void OnTriggerEnterBase(Collider2D collision)
    {
        m_mainStateMachine.OnTriggerEnter(this, collision);
    }

    void OnTriggerStayDamageBase(DamageBase damageBase)
    {
        m_mainStateMachine.OnTriggerStay(this, damageBase);
    }

    void OnBottomHitStay(RaycastHit2D hit)
    {
        m_mainStateMachine.OnBottomHitStay(this, hit);
    }

    void OnTopHitStay(RaycastHit2D hit)
    {
        m_mainStateMachine.OnTopHitStay(this, hit);
    }

    void OnBottomTopHitEnter(RaycastHit2D bottom, RaycastHit2D top)
    {
        m_mainStateMachine.OnBottomTopHitEnter(this, bottom, top);
    }

    void OnLeftRightHitEnter(RaycastHit2D bottom, RaycastHit2D top)
    {
        m_mainStateMachine.OnLeftRightHitEnter(this, bottom, top);
    }

    void OnHitEnter(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Door"))
        {
            var shutter = hit.collider.GetComponent<ShutterControll>();
            shutter.Enter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) => rbCollide.OnTriggerEnter(collision);
    private void OnTriggerStay2D(Collider2D collision) => rbCollide.OnTriggerEnter(collision);
    private void OnTriggerExit2D(Collider2D collision) => rbCollide.OnTriggerExit(collision);


    private void OnDrawGizmos()
    {
        onTheGround.OnDrawGizmos(transform.position, exRb.PhysicalBoxSize.x);
    }

    public bool IsRight => direct.IsRight;
    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => TurnToTarget(targetPos);
    public void TurnFace() => direct.TurnFace();
}