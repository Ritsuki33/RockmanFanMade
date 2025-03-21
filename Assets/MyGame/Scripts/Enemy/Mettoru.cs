﻿using System.Collections;
using UnityEngine;

public partial class Mettoru : StageEnemy, IDirect, IHitEvent, IRbVisitor, IExRbVisitor
{
    [SerializeField] Direct direct;
    [SerializeField] Gravity gravity;
    [SerializeField] Move move;
    [SerializeField] RaycastSensor raycastSensor;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] Jump jump;
    [SerializeField] bool walk = false;

    [SerializeField] ExpandRigidBody exRb;

    enum StateID
    {
        Idle = 0,
        Walk,
        Hide,
        Hiding,
        Appear,
        LookIn,
        Jump,
        JumpFloating,
    }

    public bool IsRight => direct.IsRight;

    Coroutine defense = null;


    int isFadeColorID = Shader.PropertyToID("_IsFadeColor");

    ExRbStateMachine<Mettoru> m_stateMachine = new ExRbStateMachine<Mettoru>();

    StagePlayer Player => WorldManager.Instance.Player;

    CachedCollide rbCollide = new CachedCollide();

    CachedHit exRbHit = new CachedHit();

    Vector3 targetPos = default;
    protected override void Awake()
    {
        m_stateMachine.AddState((int)StateID.Idle, new Idle());
        m_stateMachine.AddState((int)StateID.Walk, new Walk());
        m_stateMachine.AddState((int)StateID.Hide, new Hide());
        m_stateMachine.AddState((int)StateID.Hiding, new Hiding());
        m_stateMachine.AddState((int)StateID.Appear, new Appear());
        m_stateMachine.AddState((int)StateID.LookIn, new LookIn());
        m_stateMachine.AddState((int)StateID.Jump, new Jumping());
        m_stateMachine.AddState((int)StateID.JumpFloating, new JumpFloating());

        m_stateMachine.TransitReady((int)StateID.Hide);

        exRb.Init(this);

        rbCollide.CacheClear();
        exRbHit.CacheClear();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Init()
    {
        base.Init();
        m_stateMachine.TransitReady((int)StateID.Idle, true);
    }

    protected override void OnFixedUpdate()
    {
        if (Player) targetPos = Player.transform.position;
        m_stateMachine.FixedUpdate(this);
    }

    protected override void OnLateFixedUpdate()
    {
        exRb.FixedUpdate();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        m_stateMachine.Update(this);
    }

    IEnumerator DefenseRockBuster(RockBuster projectile)
    {
        Vector2 reflection = projectile.CurVelocity;
        float speed = projectile.CurSpeed;
        reflection.x *= -1;
        reflection = new Vector2(reflection.x, 0).normalized;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        projectile.ChangeBehavior(
            0,
            null
,
            (rb) =>
            {
                rb.velocity = reflection * speed;
            });
        yield return new WaitForSeconds(1f);

        defense = null;
    }

    /// <summary>
    /// 弾をうつ
    /// </summary>
    public void Fire()
    {
        var fire = ObjectManager.Instance.OnGet<SimpleProjectileComponent>(PoolType.MettoruFire);
        Vector2 direction = IsRight ? Vector2.right : Vector2.left;
        float speed = 10;
        fire.Setup(
           this.transform.position,
           IsRight,
           3,
           null,
           (rb) =>
           {
               rb.velocity = direction * speed;
           }
           );
    }

    public void Defense(RockBuster collision)
    {
        AudioManager.Instance.PlaySe(SECueIDs.kin);

        if (collision.AttackPower == 1)
        {
            ReflectBuster(collision);
        }
        else if (collision.AttackPower > 1)
        {
            collision.Delete();
        }
    }


    public void ReflectBuster(RockBuster collision)
    {
        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(collision));
    }

    public void TurnFace() => direct.TurnFace();
    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(this, collision);
    }

    void IRbVisitor.OnTriggerEnter(PlayerAttack damage)
    {
        m_stateMachine.OnTriggerEnter(this, damage);
    }

    void IRbVisitor.OnTriggerEnter(RockBuster damage)
    {
        m_stateMachine.OnTriggerEnter(this, damage);
    }

    void IHitEvent.OnBottomHitStay(RaycastHit2D hit)
    {
        m_stateMachine.OnBottomHitStay(this, hit);
    }

    void IHitEvent.OnLeftHitStay(RaycastHit2D hit)
    {
        m_stateMachine.OnLeftHitStay(this, hit);
    }

    void IHitEvent.OnRightHitStay(RaycastHit2D hit)
    {
        m_stateMachine.OnRightHitStay(this, hit);
    }
}
