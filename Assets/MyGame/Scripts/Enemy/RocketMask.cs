using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMask : StageEnemy, IDirect, IRbVisitor
{
    [SerializeField] float distance = 5.0f;
    [SerializeField] float speed = 2.0f;
    [SerializeField] Direct direct;
    [SerializeField] AnimationEnvetController animationEnvetController;
    Coroutine defense = null;

    RbStateMachine<RocketMask> m_stateMachine = new RbStateMachine<RocketMask>();

    public bool IsRight => direct.IsRight;

    CachedCollide rbCollide = new CachedCollide();
    enum StateID
    {
        Move,
        Turn,
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        m_stateMachine.AddState((int)StateID.Move, new Move());
        m_stateMachine.AddState((int)StateID.Turn, new Turn());
        animationEnvetController.animationEvents.Add(0, TurnFace);

        rbCollide.CacheClear();
    }

    protected override void Init()
    {
        base.Init();
        TurnTo(false);
        m_stateMachine.TransitReady((int)StateID.Move, true);
    }

    protected override void OnFixedUpdate()
    {
        m_stateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        m_stateMachine.Update(this);
    }

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

    class Move : RbState<RocketMask, Move>
    {
        int animationHash = 0;
        float currentDistance = 0;
        Vector2 startPos = Vector2.zero;
        Vector2 endPos = Vector2.zero;

        public Move() : base() { animationHash = Animator.StringToHash("Move"); }
        protected override void Enter(RocketMask rocketMask, int preId, int subId)
        {
            rocketMask.MainAnimator.Play(animationHash);
            currentDistance = 0;
            startPos = (Vector2)rocketMask.transform.position;
            endPos = (Vector2)rocketMask.transform.position + ((rocketMask.IsRight) ? Vector2.right : Vector2.left) * rocketMask.distance;
        }

        protected override void FixedUpdate(RocketMask rocketMask)
        {
            if (((Vector2)rocketMask.transform.position - endPos).sqrMagnitude < 0.05f)
            {
                rocketMask.m_stateMachine.TransitReady((int)StateID.Turn);
            }
            else
            {
                currentDistance += rocketMask.speed * Time.fixedDeltaTime;
                float ratio = currentDistance / rocketMask.distance;

                if (ratio > 1) { ratio = 1; }
                Vector2 currentPos = Vector2.Lerp(startPos, endPos, ratio);

                rocketMask.rb.MovePosition(currentPos);
            }
        }

        protected override void OnTriggerEnter(RocketMask rocketMask, RockBuster collision)
        {
            rocketMask.Atacked(collision);
        }

        protected override void OnTriggerEnter(RocketMask rocketMask, PlayerAttack collision)
        {
            rocketMask.Damaged(collision);
        }
    }

    class Turn : RbState<RocketMask, Turn>
    {
        int animationHash = 0;
        public Turn() : base() { animationHash = Animator.StringToHash("Turn"); }
        protected override void Enter(RocketMask rocketMask, int preId, int subId)
        {
            rocketMask.MainAnimator.Play(animationHash);
        }

        protected override void Update(RocketMask rocketMask)
        {
            if (!rocketMask.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                rocketMask.m_stateMachine.TransitReady((int)StateID.Move);
            }
        }

        protected override void OnTriggerEnter(RocketMask rocketMask, RockBuster collision)
        {
            rocketMask.Atacked(collision);
        }
        protected override void OnTriggerEnter(RocketMask rocketMask, PlayerAttack collision)
        {
            rocketMask.Damaged(collision);
        }
    }

    public void Atacked(RockBuster damage)
    {
        if ((!IsRight && (this.transform.position.x > damage.transform.position.x))
           || (IsRight && (this.transform.position.x < damage.transform.position.x)))
        {
            Defense(damage);
        }
        else
        {
            Damaged(damage);
        }
    }

    public void Defense(RockBuster rockBuster)
    {
        if (rockBuster.AttackPower == 1)
        {
            ReflectBuster(rockBuster);
        }
        else if (rockBuster.AttackPower > 1)
        {
            rockBuster.Delete();
        }
    }

    public void ReflectBuster(RockBuster rockBuster)
    {
        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(rockBuster));
    }

    IEnumerator DefenseRockBuster(RockBuster rockBuster)
    {
        Vector2 reflection = rockBuster.CurVelocity;
        float speed = rockBuster.CurSpeed;
        reflection.x *= -1;
        reflection = new Vector2(reflection.x, 0).normalized;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        rockBuster.ChangeBehavior(
            0,
            null,
            (rb) =>
            {
                rb.velocity = reflection * speed;
            });
        yield return new WaitForSeconds(1f);

        defense = null;
    }

    public void TurnTo(bool isRight) => direct.TurnTo(isRight);

    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);

    public void TurnFace() => direct.TurnFace();
}
