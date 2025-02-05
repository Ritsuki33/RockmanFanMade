using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadRoller : StageEnemy,IDirect
{
    [SerializeField] AnimationEnvetController aECtr;
    [SerializeField] Gravity gravity;
    [SerializeField] Move move;
    [SerializeField] ExpandRigidBody exRb;
    [SerializeField] Direct direct;
    [SerializeField] GroundChecker groundChecker;

    bool IsRight => this.transform.localScale.x < 0;

    bool IDirect.IsRight => direct.IsRight;

    ExRbStateMachine<RoadRoller> m_stateMachine = new ExRbStateMachine<RoadRoller>();

    RbCollide rbCollide = new RbCollide();
    ExRbHit exRbHit = new ExRbHit();

    enum StateId
    {
        Move,
        Turn,
        Float
    }

    protected override void Awake()
    {
        m_stateMachine.AddState((int)StateId.Move, new Moving());
        m_stateMachine.AddState((int)StateId.Turn, new Turn());
        m_stateMachine.AddState((int)StateId.Float, new Float());
        aECtr.animationEvents.Add(0, TurnFace);

        exRb.Init();
        rbCollide.Init();

        rbCollide.onTriggerEnterRockBusterDamage += OnTriggerEnterRockBusterDamage;
        exRbHit.Init(exRb);

        exRbHit.onBottomHitEnter += OnBotomHitEnter;
        exRbHit.onRightHitStay += OnRightHitStay;
        exRbHit.onLeftHitStay += OnLeftHitStay;

    }

    protected override void Init()
    {
        base.Init();
        m_stateMachine.TransitReady((int)StateId.Float);
    }

    protected override void OnFixedUpdate()
    {
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

    private void OnBotomHitEnter(RaycastHit2D hit)
    {
        m_stateMachine.OnBottomHitEnter(this, hit);
    }

    private void OnRightHitStay(RaycastHit2D hit)
    {
        m_stateMachine.OnRightHitStay(this, hit);
    }

    private void OnLeftHitStay(RaycastHit2D hit)
    {
        m_stateMachine.OnLeftHitStay(this, hit);
    }

    private void OnTriggerEnterRockBusterDamage(RockBusterDamage damage)
    {
        m_stateMachine.OnTriggerEnter(this,damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(collision);
    }

    class Float : ExRbState<RoadRoller, Float>
    {
        protected override void Enter(RoadRoller roller, int preId, int subId)
        {
            roller.MainAnimator.Play(Moving.animationHash);
        }

        protected override void FixedUpdate(RoadRoller roller)
        {
            roller.gravity.OnUpdate();
            roller.exRb.velocity = roller.gravity.CurrentVelocity;
        }

        protected override void OnBottomHitEnter(RoadRoller roller, RaycastHit2D hit)
        {
            roller.m_stateMachine.TransitReady((int)StateId.Move);
        }

        protected override void OnTriggerEnter(RoadRoller roller, RockBusterDamage collision)
        {
            roller.Damaged(collision);
        }
    }

    class Moving : ExRbState<RoadRoller, Moving>
    {
        public static int animationHash = Animator.StringToHash("Move");
        protected override void Enter(RoadRoller roller, int preId, int subId)
        {
            roller.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(RoadRoller roller)
        {
            roller.gravity.OnUpdate();
            roller.move.OnUpdate(Vector2.right, (roller.IsRight) ? Move.InputType.Right : Move.InputType.Left);
            roller.exRb.velocity = roller.gravity.CurrentVelocity;
            roller.exRb.velocity += roller.move.CurrentVelocity;

            if (!roller.groundChecker.CheckGround(roller.transform.position, roller.exRb.PhysicalBoxSize, roller.IsRight))
            {
                roller.m_stateMachine.TransitReady((int)StateId.Turn);
            }
        }
        protected override void OnRightHitStay(RoadRoller roller, RaycastHit2D hit)
        {
            if (roller.IsRight) { roller.m_stateMachine.TransitReady((int)StateId.Turn); }
        }

        protected override void OnLeftHitStay(RoadRoller roller, RaycastHit2D hit)
        {
            if (!roller.IsRight) { roller.m_stateMachine.TransitReady((int)StateId.Turn); }
        }

        protected override void OnTriggerEnter(RoadRoller roller, RockBusterDamage collision)
        {
            roller.Damaged(collision);
        }
    }

    class Turn : ExRbState<RoadRoller, Turn>
    {
        static int animationHash = Animator.StringToHash("Turn");
        protected override void Enter(RoadRoller roller, int preId, int subId)
        {
            roller.MainAnimator.Play(animationHash);
        }

        protected override void Update(RoadRoller roller)
        {
            if (!roller.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                roller.m_stateMachine.TransitReady((int)StateId.Move);
            }
        }

        protected override void OnTriggerEnter(RoadRoller roller, RockBusterDamage collision)
        {
            roller.Damaged(collision);
        }
    }

    public void TurnFace() => direct.TurnFace();

    void IDirect.TurnTo(bool isRight) => direct.TurnTo(isRight);

    void IDirect.TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);

    private void OnDrawGizmos()
    {
        exRb.OnDrawGizmos();
    }
}
