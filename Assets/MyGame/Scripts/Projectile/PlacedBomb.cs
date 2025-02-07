using System;
using UnityEngine;
using UnityEngine.Pool;

public class PlacedBomb : AnimObject, IHitEvent
{
    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] ExpandRigidBody exRb;

    Action<ExpandRigidBody> orbitfixedUpdate;

    AmbiguousTimer timer = new AmbiguousTimer();

    IObjectPool<PlacedBomb> pool = null;



    ExRbStateMachine<PlacedBomb> stateMachine = new ExRbStateMachine<PlacedBomb>();

    CachedHit exRbHit = new CachedHit();

    protected override void Awake()
    {
        base.Awake();
        exRb.Init(this);
        stateMachine.AddState(0, new Orbit());
        stateMachine.AddState(1, new Boot());

        exRbHit.CacheClear();
    }

    protected override void Init()
    {
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
        stateMachine.TransitReady(0);
    }
    protected override void OnFixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    protected override void OnLateFixedUpdate()
    {
        exRb.FixedUpdate();
    }

    protected override void OnUpdate()
    {
        stateMachine.Update(this);
    }

    void IHitEvent.OnBottomHitEnter(RaycastHit2D hit)
    {
        stateMachine.OnBottomHitEnter(this, hit);
    }

    public void Setup(Vector3 position, Action<ExpandRigidBody> orbitfixedUpdate)
    {
        this.transform.position = position;
        this.orbitfixedUpdate = orbitfixedUpdate;

        stateMachine.TransitReady(0);
    }

    class Orbit : ExRbState<PlacedBomb, Orbit>
    {
        int animationHash = Animator.StringToHash("UnBoot");
        protected override void Enter(PlacedBomb ctr, int preId, int subId)
        {
            ctr.MainAnimator.Play(animationHash);
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
            ObjectManager ObjectManager => ObjectManager.Instance;
        int animationHash = Animator.StringToHash("Boot");
        protected override void Enter(PlacedBomb ctr, int preId, int subId)
        {
            ctr.MainAnimator.Play(animationHash);
            ctr.timer.Start(3, 3);
        }

        protected override void Update(PlacedBomb ctr)
        {
            ctr.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                var explode = ObjectManager.OnGet<Explode>(PoolType.Explode2);
                explode.Setup(Explode.Layer.EnemyAttack, ctr.transform.position, 5);

                ctr.Delete();
            });
        }
    }
}
