using UnityEngine;

public class Batman : EnemyObject
{
    ExRbStateMachine<Batman> mainStateMachine = new ExRbStateMachine<Batman>();
    ExpandRigidBody exRb;

    RaycastSensor sensor;

    RbCollide rbCollide = new RbCollide();
    ExRbHit exRbHit = new ExRbHit();

    enum StateID
    {
        Idle,
        ToMove,
        Move,
        ToIdle,
    }

    protected override void Awake()
    {
        base.Awake();

        exRb = GetComponent<ExpandRigidBody>();
        sensor = GetComponent<RaycastSensor>();

        mainStateMachine.Clear();
        mainStateMachine.AddState((int)StateID.Idle, new Idle());
        mainStateMachine.AddState((int)StateID.ToMove, new ToMove());
        mainStateMachine.AddState((int)StateID.Move, new Move());
        mainStateMachine.AddState((int)StateID.ToIdle, new ToIdle());

        exRbHit.Init(exRb);
        exRbHit.onTopHitEnter += OnTopHitEnter;
        rbCollide.onCollisionEnterRockBusterDamage += OnCollisionEnterRockBusterDamage;
        rbCollide.onCollisionEnterPlayerTrigger += OnCollisionEnterPlayerTrigger;
    }

    protected override void Init()
    {
        base.Init();
        mainStateMachine.TransitReady((int)StateID.Idle);
    }

    protected override void OnFixedUpdate()
    {
        mainStateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        mainStateMachine.Update(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(collision);
    }

    private void OnTopHitEnter(RaycastHit2D hit)
    {
        mainStateMachine.OnTopHitEnter(this, hit);
    }

    private void OnCollisionEnterRockBusterDamage(RockBusterDamage collision)
    {
        mainStateMachine.OnTriggerEnter(this, collision);
    }

    private void OnCollisionEnterPlayerTrigger(PlayerTrigger collision)
    {
        mainStateMachine.OnTriggerEnter(this, collision);
    }

    class Idle : ExRbState<Batman, Idle>
    {
        static int anmationHash = Animator.StringToHash("Idle");
        AmbiguousTimer timer = new AmbiguousTimer();
        protected override void Enter(Batman batman, int preId, int subId)
        {
            batman.MainAnimator.Play(anmationHash);
            timer.Start(1, 2);
        }

        protected override void Update(Batman batman)
        {
            timer.MoveAheadTime(Time.deltaTime, () =>
            {
                batman.sensor.SearchForTargetStay(Vector2.down, (hit) =>
                {
                    batman.mainStateMachine.TransitReady((int)StateID.ToMove);
                });
            }
            , true);
        }
        protected override void OnTriggerEnter(Batman batman, RockBusterDamage collision)
        {
            batman.Damaged(collision);
        }
    }

    class ToMove : ExRbState<Batman, ToMove>
    {
        static int anmationHash = Animator.StringToHash("ToMove");
        protected override void Enter(Batman batman, int preId, int subId)
        {
            batman.MainAnimator.Play(anmationHash);
        }

        protected override void FixedUpdate(Batman batman)
        {
            batman.exRb.velocity = 1.0f * Vector2.down;
        }

        protected override void Update(Batman batman)
        {
            if (!batman.MainAnimator.IsPlayingCurrentAnimation(anmationHash))
            {
                batman.mainStateMachine.TransitReady((int)StateID.Move);
            }
        }

        protected override void OnTriggerEnter(Batman batman, RockBusterDamage collision)
        {
            batman.Damaged(collision);
        }
    }

    class Move : ExRbState<Batman, Move>
    {
        static int anmationHash = Animator.StringToHash("Move");
        float speed = 1;

        Transform PlayerPos => WorldManager.Instance.Player.transform;
        protected override void Enter(Batman batman, int preId, int subId)
        {
            batman.MainAnimator.Play(anmationHash);
        }

        protected override void FixedUpdate(Batman batman)
        {
            Vector2 move = PlayerPos.position - batman.transform.position;
            batman.exRb.velocity = speed * move.normalized;
        }

        protected override void OnTriggerEnter(Batman batman, RockBusterDamage collision)
        {
            batman.Damaged(collision);
        }

        protected override void OnTriggerEnter(Batman batman, PlayerTrigger collision)
        {
            batman.mainStateMachine.TransitReady((int)StateID.ToIdle);
        }
    }

    class ToIdle : ExRbState<Batman, ToIdle>
    {
        static int anmationHash = Animator.StringToHash("ToIdle");
        float speed = 0f;

        protected override void Enter(Batman batman, int preId, int subId)
        {
            batman.MainAnimator.Play(anmationHash);
            speed = 0f;
        }

        protected override void FixedUpdate(Batman batman)
        {
            batman.exRb.velocity = speed * Vector2.up;

            if (speed < 10) speed += 0.5f;
        }

        protected override void OnTopHitEnter(Batman batman, RaycastHit2D hit)
        {
            batman.mainStateMachine.TransitReady((int)StateID.Idle);
        }

        protected override void OnTriggerEnter(Batman batman, RockBusterDamage collision)
        {
            batman.Damaged(collision);
        }
    }
}
