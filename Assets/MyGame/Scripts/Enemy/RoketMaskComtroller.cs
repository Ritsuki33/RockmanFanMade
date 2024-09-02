using System.Collections;
using UnityEngine;

public class RoketMaskComtroller : RbStateMachine<RoketMaskComtroller>
{
    [SerializeField] RocketMask rocketMask = default;
    [SerializeField] Animator _animator;
    [SerializeField] float distance = 5.0f;
    [SerializeField] float speed = 2.0f;
    [SerializeField] AnimationEnvetController animationEnvetController;
    Coroutine defense = null;
    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;

    bool isRight => this.transform.localScale.x < 0;

    enum StateID
    {
        Move,
        Turn,
    }

    Rigidbody2D rb = default;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        AddState((int)StateID.Move, new Move());
        AddState((int)StateID.Turn, new Turn());

        animationEnvetController.animationEvents.Add(0, rocketMask.TurnFace);
        Init();
    }

    public void Init()
    {
        TransitReady((int)StateID.Move, true);
    }

    class Move : RbState<RoketMaskComtroller>
    {
        int animationHash = 0;
        float currentDistance = 0;
        Vector2 startPos = Vector2.zero;
        Vector2 endPos = Vector2.zero;

        public Move() : base() { animationHash = Animator.StringToHash("Move"); }
        protected override void Enter(RoketMaskComtroller rocketMask, int preId)
        {
            rocketMask._animator.Play(animationHash);
            currentDistance = 0;
            startPos = (Vector2)rocketMask.transform.position;
            endPos = (Vector2)rocketMask.transform.position + ((rocketMask.isRight) ? Vector2.right : Vector2.left) * rocketMask.distance;
        }

        protected override void FixedUpdate(RoketMaskComtroller rocketMask, IParentState parent)
        {
            if(((Vector2)rocketMask.transform.position-endPos).sqrMagnitude<0.05f)
            {
                rocketMask.TransitReady((int)StateID.Turn);
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

        protected override void OnTriggerEnter2D(RoketMaskComtroller rocketMask, Collider2D collision, IParentState parent)
        {
            rocketMask.rocketMask.Attacked(collision);
        }
    }

    class Turn : RbState<RoketMaskComtroller>
    {
        int animationHash = 0;
        public Turn() : base() { animationHash = Animator.StringToHash("Turn"); }
        protected override void Enter(RoketMaskComtroller rocketMask, int preId)
        {
            rocketMask._animator.Play(animationHash);
        }

        protected override void Update(RoketMaskComtroller rocketMask, IParentState parent)
        {
            if (!rocketMask._animator.IsPlayingCurrentAnimation(animationHash))
            {
                rocketMask.TransitReady((int)StateID.Move);
            }
        }

        protected override void OnTriggerEnter2D(RoketMaskComtroller rocketMask, Collider2D collision, IParentState parent)
        {
            rocketMask.rocketMask.Attacked(collision);
        }
    }

    public void Defense(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster"))
        {
            ReflectBuster(collision);
        }
        else if (collision.gameObject.CompareTag("ChargeShot"))
        {
            var rockBuster = collision.gameObject.GetComponent<Projectile>();
            rockBuster.Delete();
        }
    }

    public void ReflectBuster(Collider2D collision)
    {
        var rockBuster = collision.gameObject.GetComponent<Projectile>();

        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(rockBuster));
    }

    IEnumerator DefenseRockBuster(Projectile projectile)
    {
        projectile.DisableDamageDetection();
        Vector2 reflection = projectile.Direction;
        reflection.x *= -1;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        projectile.ChangeDirection(reflection);
        yield return new WaitForSeconds(1f);

        defense = null;
    }
}
