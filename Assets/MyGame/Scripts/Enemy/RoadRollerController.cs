using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadRollerController : ExRbStateMachine<RoadRollerController>
{
    [SerializeField] RoadRoller roadRoller;
    [SerializeField] Animator _animator;
    [SerializeField] AnimationEnvetController aECtr;
    Gravity gravity;
    Move move;
    GroundChecker groundChecker;

    ExpandRigidBody exRb;
    bool IsRight => this.transform.localScale.x < 0;

    enum StateId
    {
        Move,
        Turn,
        Float
    }

    private void Awake()
    {
        gravity = GetComponent<Gravity>();
        move = GetComponent<Move>();
        groundChecker = GetComponent<GroundChecker>();
        exRb = GetComponent<ExpandRigidBody>();


        AddState((int)StateId.Move, new Moving());
        AddState((int)StateId.Turn, new Turn());
        AddState((int)StateId.Float, new Float());
        aECtr.animationEvents.Add(0, TurnFace);

        Init();
    }

    public void Init()
    {
        TransitReady((int)StateId.Float);
    }

    class Float : ExRbState<RoadRollerController, Float>
    {
        protected override void Enter(RoadRollerController roller, int preId, int subId)
        {
            roller._animator.Play(Moving.animationHash);
        }

        protected override void FixedUpdate(RoadRollerController roller)
        {
            roller.gravity.UpdateVelocity();
            roller.exRb.velocity = roller.gravity.CurrentVelocity;
        }

        protected override void OnBottomHitEnter(RoadRollerController roller, RaycastHit2D hit)
        {
            roller.TransitReady((int)StateId.Move);
        }

        protected override void OnTriggerEnter(RoadRollerController roller, RockBusterDamage collision)
        {
            roller.roadRoller.Damaged(collision.baseDamageValue);
        }
    }

    class Moving : ExRbState<RoadRollerController, Moving>
    {
        public static int animationHash = Animator.StringToHash("Move");
        protected override void Enter(RoadRollerController roller, int preId, int subId)
        {
            roller._animator.Play(animationHash);
        }

        protected override void FixedUpdate(RoadRollerController roller)
        {
            roller.gravity.UpdateVelocity();
            roller.move.UpdateVelocity(Vector2.right, (roller.IsRight) ? Move.InputType.Right : Move.InputType.Left);
            roller.exRb.velocity = roller.gravity.CurrentVelocity;
            roller.exRb.velocity += roller.move.CurrentVelocity;

            if (!roller.groundChecker.CheckGround(roller.IsRight))
            {
                roller.TransitReady((int)StateId.Turn);
            }
        }
        protected override void OnRightHitStay(RoadRollerController roller, RaycastHit2D hit)
        {
            if (roller.IsRight) { roller.TransitReady((int)StateId.Turn); }
        }

        protected override void OnLeftHitStay(RoadRollerController roller, RaycastHit2D hit)
        {
            if (!roller.IsRight) { roller.TransitReady((int)StateId.Turn); }
        }

        protected override void OnTriggerEnter(RoadRollerController roller, RockBusterDamage collision)
        {
            roller.roadRoller.Damaged(collision.baseDamageValue);
        }
    }

    class Turn : ExRbState<RoadRollerController, Turn>
    {
        static int animationHash = Animator.StringToHash("Turn");
        protected override void Enter(RoadRollerController roller, int preId, int subId)
        {
            roller._animator.Play(animationHash);
        }

        protected override void Update(RoadRollerController roller)
        {
            if (!roller._animator.IsPlayingCurrentAnimation(animationHash))
            {
                roller.TransitReady((int)StateId.Move);
            }
        }

        protected override void OnTriggerEnter(RoadRollerController roller, RockBusterDamage collision)
        {
            roller.roadRoller.Damaged(collision.baseDamageValue);
        }
    }

    public void TurnFace() => roadRoller.TurnFace();
}
