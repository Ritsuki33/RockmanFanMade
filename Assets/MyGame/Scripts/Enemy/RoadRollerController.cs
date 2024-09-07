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

    class Float : ExRbState<RoadRollerController>
    {
        protected override void Enter(RoadRollerController roller, int preId)
        {
            roller._animator.Play(Moving.animationHash);
        }

        protected override void FixedUpdate(RoadRollerController roller, IParentState parent)
        {
            roller.gravity.UpdateVelocity();
            roller.exRb.velocity = roller.gravity.CurrentVelocity;
        }

        protected override void OnBottomHitEnter(RoadRollerController roller, RaycastHit2D hit, IParentState parent)
        {
            roller.TransitReady((int)StateId.Move);
        }

        protected override void OnTriggerEnter2D(RoadRollerController roller, Collider2D collision, IParentState parent)
        {
            roller.roadRoller.Attacked(collision);
        }
    }

    class Moving : ExRbState<RoadRollerController>
    {
        public static int animationHash = Animator.StringToHash("Move");
        protected override void Enter(RoadRollerController roller, int preId)
        {
            roller._animator.Play(animationHash);
        }

        protected override void FixedUpdate(RoadRollerController roller, IParentState parent)
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
        protected override void OnRightHitStay(RoadRollerController roller, RaycastHit2D hit, IParentState parent)
        {
            if (roller.IsRight) { roller.TransitReady((int)StateId.Turn); }
        }

        protected override void OnLeftHitStay(RoadRollerController roller, RaycastHit2D hit, IParentState parent)
        {
            if (!roller.IsRight) { roller.TransitReady((int)StateId.Turn); }
        }

        protected override void OnTriggerEnter2D(RoadRollerController roller, Collider2D collision, IParentState parent)
        {
            roller.roadRoller.Attacked(collision);
        }
    }

    class Turn : ExRbState<RoadRollerController>
    {
        static int animationHash = Animator.StringToHash("Turn");
        protected override void Enter(RoadRollerController roller, int preId)
        {
            roller._animator.Play(animationHash);
        }

        protected override void Update(RoadRollerController roller, IParentState parent)
        {
            if (!roller._animator.IsPlayingCurrentAnimation(animationHash))
            {
                roller.TransitReady((int)StateId.Move);
            }
        }

        protected override void OnTriggerEnter2D(RoadRollerController roller, Collider2D collision, IParentState parent)
        {
            roller.roadRoller.Attacked(collision);
        }
    }

    public void TurnFace() => roadRoller.TurnFace();
}
