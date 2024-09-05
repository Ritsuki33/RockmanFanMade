using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatmanController : ExRbStateMachine<BatmanController>
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

    class Idle : ExRbState<BatmanController>
    {
        static int anmationHash = Animator.StringToHash("Idle");
        AmbiguousTimer timer = new AmbiguousTimer();
        protected override void Enter(BatmanController batmanController, int preId)
        {
            batmanController._animator.Play(anmationHash);
            timer.Start(1, 2);
        }

        protected override void Update(BatmanController batmanController, IParentState parent)
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

        protected override void OnTriggerEnter2D(BatmanController batmanController, Collider2D collision, IParentState parent)
        {
            if (collision.gameObject.CompareTag("RockBuster"))
            {
                batmanController.batman.Attacked(collision);
            }
        }
    }

    class ToMove : ExRbState<BatmanController>
    {
        static int anmationHash = Animator.StringToHash("ToMove");
        protected override void Enter(BatmanController batmanController, int preId)
        {
            batmanController._animator.Play(anmationHash);
        }

        protected override void FixedUpdate(BatmanController batmanController, IParentState parent)
        {
            batmanController.exRb.velocity = 1.0f * Vector2.down;
        }

        protected override void Update(BatmanController batmanController, IParentState parent)
        {
            if (!batmanController._animator.IsPlayingCurrentAnimation(anmationHash))
            {
                batmanController.TransitReady((int)StateID.Move);
            }
        }

        protected override void OnTriggerEnter2D(BatmanController batmanController, Collider2D collision, IParentState parent)
        {
            batmanController.batman.Attacked(collision);
        }
    }

    class Move : ExRbState<BatmanController>
    {
        static int anmationHash = Animator.StringToHash("Move");
        float speed = 1;

        Transform PlayerPos => GameManager.Instance.Player.transform;
        protected override void Enter(BatmanController batmanController, int preId)
        {
            batmanController._animator.Play(anmationHash);
        }

        protected override void FixedUpdate(BatmanController batmanController, IParentState parent)
        {
            Vector2 move = PlayerPos.position - batmanController.transform.position;
            batmanController.exRb.velocity = speed * move.normalized;
        }

        protected override void OnTriggerEnter2D(BatmanController batmanController, Collider2D collision, IParentState parent)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                batmanController.TransitReady((int)StateID.ToIdle);

            }
            else
            {
                batmanController.batman.Attacked(collision);
            }
        }
    }

    class ToIdle : ExRbState<BatmanController>
    {
        static int anmationHash = Animator.StringToHash("ToIdle");
        float speed = 0f;

        protected override void Enter(BatmanController batmanController, int preId)
        {
            batmanController._animator.Play(anmationHash);
            speed = 0f;
        }

        protected override void FixedUpdate(BatmanController batmanController, IParentState parent)
        {
            batmanController.exRb.velocity = speed * Vector2.up;

            if (speed < 10) speed += 0.5f;
        }

        protected override void OnTopHitEnter(BatmanController batmanController, RaycastHit2D hit, IParentState parent)
        {
            batmanController.TransitReady((int)StateID.Idle);
        }

        protected override void OnTriggerEnter2D(BatmanController batmanController, Collider2D collision, IParentState parent)
        {
            batmanController.batman.Attacked(collision);
        }
    }
}
