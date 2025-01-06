using System;
using UnityEngine;
using UnityEngine.Pool;

public class PlacedBomb : AnimObject, IPooledObject<PlacedBomb>
{
    [SerializeField] PlacedBomb placedBomb;
    [SerializeField] Animator _animator;

    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    ExpandRigidBody exRb;

    Action<ExpandRigidBody> orbitfixedUpdate;
    Action onExplodedFinishCallback;

    AmbiguousTimer timer = new AmbiguousTimer();

    ExplodePool exlodePool => EffectManager.Instance.Explode2Pool;

    IObjectPool<PlacedBomb> pool = null;

    IObjectPool<PlacedBomb> IPooledObject<PlacedBomb>.Pool { get => pool; set => pool = value; }

    Action deleteCallback;

    ExRbStateMachine<PlacedBomb> stateMachine = new ExRbStateMachine<PlacedBomb>();
    public void Delete()
    {
        pool.Release(this);
        this.deleteCallback?.Invoke();
    }

    void IPooledObject<PlacedBomb>.OnGet()
    {
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
    }

    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        stateMachine.AddState(0, new Orbit());
        stateMachine.AddState(1, new Boot());
    }

    public void Init(Action<ExpandRigidBody> orbitfixedUpdate, Action onExplodedFinishCallback)
    {
        this.orbitfixedUpdate = orbitfixedUpdate;
        this.onExplodedFinishCallback = onExplodedFinishCallback;

        stateMachine.TransitReady(0);
    }

    class Orbit : ExRbState<PlacedBomb, Orbit>
    {
        int animationHash = Animator.StringToHash("UnBoot");
        protected override void Enter(PlacedBomb ctr, int preId, int subId)
        {
            ctr._animator.Play(animationHash);
        }

        protected override void FixedUpdate(PlacedBomb ctr)
        {
            ctr.orbitfixedUpdate(ctr.exRb);
        }

        protected override void OnBottomHitEnter(PlacedBomb ctr, RaycastHit2D hit)
        {
            ctr.stateMachine.TransitReady(1);
            ctr.exRb.Bottom = hit.point.y;
        }
    }

    class Boot : ExRbState<PlacedBomb, Boot>
    {
        int animationHash = Animator.StringToHash("Boot");
        protected override void Enter(PlacedBomb ctr, int preId, int subId)
        {
            ctr._animator.Play(animationHash);
            ctr.timer.Start(3, 3);
        }

        protected override void Update(PlacedBomb ctr)
        {
            ctr.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                var explode = ctr.exlodePool.Pool.Get().GetComponent<Explode>();
                explode.transform.position = ctr.transform.position;

                explode.Init(Explode.Layer.EnemyAttack, 5);

                ctr.placedBomb.Delete();
                ctr.onExplodedFinishCallback?.Invoke();
            });
        }
    }
}
