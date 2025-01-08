using System;
using UnityEngine;
using UnityEngine.Pool;

public class PlacedBomb : AnimObject, IPooledObject<PlacedBomb>
{
    [SerializeField] Animator _animator;

    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    ExpandRigidBody exRb;

    Action<ExpandRigidBody> orbitfixedUpdate;
    Action<PlacedBomb> onExplodedFinishCallback;

    AmbiguousTimer timer = new AmbiguousTimer();

    IObjectPool<PlacedBomb> pool = null;

    IObjectPool<PlacedBomb> IPooledObject<PlacedBomb>.Pool { get => pool; set => pool = value; }


    ExRbStateMachine<PlacedBomb> stateMachine = new ExRbStateMachine<PlacedBomb>();

    public void Delete()
    {
        onExplodedFinishCallback?.Invoke(this);
    }

    void IPooledObject<PlacedBomb>.OnGet()
    {
        if (boxCollider) boxCollider.enabled = true;
        if (boxTrigger) boxTrigger.enabled = true;
    }

    protected override void Awake()
    {
        base.Awake();
        exRb = GetComponent<ExpandRigidBody>();
        stateMachine.AddState(0, new Orbit());
        stateMachine.AddState(1, new Boot());
    }

    protected override void Init()
    {
        stateMachine.TransitReady(0);
    }
    protected override void OnFixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    protected override void OnUpdate()
    {
        stateMachine.Update(this);
    }

    public void Setup(Action<ExpandRigidBody> orbitfixedUpdate, Action<PlacedBomb> onExplodedFinishCallback)
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
            ObjectManager ObjectManager => ObjectManager.Instance;
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
                ObjectManager.Create(ExplodeType.Explode2, Explode.Layer.EnemyAttack, 5, ctr.transform.position);

                ctr.Delete();
            });
        }
    }
}
