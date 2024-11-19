using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadRollerBehavior : ExRbStateMachine<RoadRollerBehavior>
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

    class Float : ExRbState<RoadRollerBehavior, Float>
    {
        protected override void Enter(RoadRollerBehavior roller, int preId, int subId)
        {
            roller._animator.Play(Moving.animationHash);
        }

        protected override void FixedUpdate(RoadRollerBehavior roller)
        {
            roller.gravity.UpdateVelocity();
            roller.exRb.velocity = roller.gravity.CurrentVelocity;
        }

        protected override void OnBottomHitEnter(RoadRollerBehavior roller, RaycastHit2D hit)
        {
            roller.TransitReady((int)StateId.Move);
        }

        protected override void OnTriggerEnter(RoadRollerBehavior roller, RockBusterDamage collision)
        {
            roller.roadRoller.Damaged(collision);
        }
    }

    class Moving : ExRbState<RoadRollerBehavior, Moving>
    {
        public static int animationHash = Animator.StringToHash("Move");
        protected override void Enter(RoadRollerBehavior roller, int preId, int subId)
        {
            roller._animator.Play(animationHash);
        }

        protected override void FixedUpdate(RoadRollerBehavior roller)
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
        protected override void OnRightHitStay(RoadRollerBehavior roller, RaycastHit2D hit)
        {
            if (roller.IsRight) { roller.TransitReady((int)StateId.Turn); }
        }

        protected override void OnLeftHitStay(RoadRollerBehavior roller, RaycastHit2D hit)
        {
            if (!roller.IsRight) { roller.TransitReady((int)StateId.Turn); }
        }

        protected override void OnTriggerEnter(RoadRollerBehavior roller, RockBusterDamage collision)
        {
            roller.roadRoller.Damaged(collision);
        }
    }

    class Turn : ExRbState<RoadRollerBehavior, Turn>
    {
        static int animationHash = Animator.StringToHash("Turn");
        protected override void Enter(RoadRollerBehavior roller, int preId, int subId)
        {
            roller._animator.Play(animationHash);
        }

        protected override void Update(RoadRollerBehavior roller)
        {
            if (!roller._animator.IsPlayingCurrentAnimation(animationHash))
            {
                roller.TransitReady((int)StateId.Move);
            }
        }

        protected override void OnTriggerEnter(RoadRollerBehavior roller, RockBusterDamage collision)
        {
            roller.roadRoller.Damaged(collision);
        }
    }

    public void TurnFace() => roadRoller.TurnFace();
}
