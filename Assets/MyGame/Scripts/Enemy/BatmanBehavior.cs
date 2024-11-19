using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatmanBehavior : ExRbStateMachine<BatmanBehavior>
{
    [SerializeField] Batman batman;
    [SerializeField] Animator _animator;
    ExpandRigidBody exRb;

    RaycastSensor sensor;
    enum StateID
    {
        Idle,
        ToMove,
        Move,
        ToIdle,
    }

    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        sensor = GetComponent<RaycastSensor>();

        AddState((int)StateID.Idle, new Idle());
        AddState((int)StateID.ToMove, new ToMove());
        AddState((int)StateID.Move, new Move());
        AddState((int)StateID.ToIdle, new ToIdle());

        Init();
    }

    public void Init()
    {
        TransitReady((int)StateID.Idle);
    }

    class Idle : ExRbState<BatmanBehavior, Idle>
    {
        static int anmationHash = Animator.StringToHash("Idle");
        AmbiguousTimer timer = new AmbiguousTimer();
        protected override void Enter(BatmanBehavior batmanController, int preId, int subId)
        {
            batmanController._animator.Play(anmationHash);
            timer.Start(1, 2);
        }

        protected override void Update(BatmanBehavior batmanController)
        {
            timer.MoveAheadTime(Time.deltaTime, () =>
            {
                batmanController.sensor.SearchForTargetStay(Vector2.down, (hit) =>
                {
                    batmanController.TransitReady((int)StateID.ToMove);
                });
            }
            , true);
        }
        protected override void OnTriggerEnter(BatmanBehavior batmanController, RockBusterDamage collision)
        {
            batmanController.batman.Damaged(collision);
        }
    }

    class ToMove : ExRbState<BatmanBehavior, ToMove>
    {
        static int anmationHash = Animator.StringToHash("ToMove");
        protected override void Enter(BatmanBehavior batmanController, int preId, int subId)
        {
            batmanController._animator.Play(anmationHash);
        }

        protected override void FixedUpdate(BatmanBehavior batmanController)
        {
            batmanController.exRb.velocity = 1.0f * Vector2.down;
        }

        protected override void Update(BatmanBehavior batmanController)
        {
            if (!batmanController._animator.IsPlayingCurrentAnimation(anmationHash))
            {
                batmanController.TransitReady((int)StateID.Move);
            }
        }

        protected override void OnTriggerEnter(BatmanBehavior batmanController, RockBusterDamage collision)
        {
            batmanController.batman.Damaged(collision);
        }
    }

    class Move : ExRbState<BatmanBehavior, Move>
    {
        static int anmationHash = Animator.StringToHash("Move");
        float speed = 1;

        Transform PlayerPos => WorldManager.Instance.PlayerController.transform;
        protected override void Enter(BatmanBehavior batmanController, int preId, int subId)
        {
            batmanController._animator.Play(anmationHash);
        }

        protected override void FixedUpdate(BatmanBehavior batmanController)
        {
            Vector2 move = PlayerPos.position - batmanController.transform.position;
            batmanController.exRb.velocity = speed * move.normalized;
        }

        protected override void OnTriggerEnter(BatmanBehavior batmanController, RockBusterDamage collision)
        {
            batmanController.batman.Damaged(collision);
        }

        protected override void OnTriggerEnter(BatmanBehavior batmanController, PlayerTrigger collision)
        {
            batmanController.TransitReady((int)StateID.ToIdle);
        }
    }

    class ToIdle : ExRbState<BatmanBehavior, ToIdle>
    {
        static int anmationHash = Animator.StringToHash("ToIdle");
        float speed = 0f;

        protected override void Enter(BatmanBehavior batmanController, int preId, int subId)
        {
            batmanController._animator.Play(anmationHash);
            speed = 0f;
        }

        protected override void FixedUpdate(BatmanBehavior batmanController)
        {
            batmanController.exRb.velocity = speed * Vector2.up;

            if (speed < 10) speed += 0.5f;
        }

        protected override void OnTopHitEnter(BatmanBehavior batmanController, RaycastHit2D hit)
        {
            batmanController.TransitReady((int)StateID.Idle);
        }

        protected override void OnTriggerEnter(BatmanBehavior batmanController, RockBusterDamage collision)
        {
            batmanController.batman.Damaged(collision);
        }
    }
}
