using System;
using UnityEngine;

public class PlacedBombBehavior : ExRbStateMachine<PlacedBombBehavior>
{
    [SerializeField] PlacedBomb placedBomb;
    [SerializeField] Animator _animator;

    ExpandRigidBody exRb;

    Action<ExpandRigidBody> orbitfixedUpdate;
    Action onExplodedFinishCallback;

    AmbiguousTimer timer = new AmbiguousTimer();

    BaseObjectPool exlodePool => EffectManager.Instance.Explode2Pool;

    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        this.AddState(0, new Orbit());    
        this.AddState(1, new Boot());
    }

    public void Init(Action<ExpandRigidBody> orbitfixedUpdate, Action onExplodedFinishCallback)
    {
        this.orbitfixedUpdate = orbitfixedUpdate;
        this.onExplodedFinishCallback = onExplodedFinishCallback;

        this.TransitReady(0);
    }

    class Orbit : ExRbState<PlacedBombBehavior, Orbit>
    {
        int animationHash = Animator.StringToHash("UnBoot");
        protected override void Enter(PlacedBombBehavior ctr, int preId, int subId)
        {
            ctr._animator.Play(animationHash);
        }

        protected override void FixedUpdate(PlacedBombBehavior ctr)
        {
            ctr.orbitfixedUpdate(ctr.exRb);
        }

        protected override void OnBottomHitEnter(PlacedBombBehavior ctr, RaycastHit2D hit)
        {
            ctr.TransitReady(1);
            ctr.exRb.Bottom = hit.point.y;
        }
    }

    class Boot : ExRbState<PlacedBombBehavior, Boot>
    {
        int animationHash = Animator.StringToHash("Boot");
        protected override void Enter(PlacedBombBehavior ctr, int preId, int subId)
        {
            ctr._animator.Play(animationHash);
            ctr.timer.Start(3, 3);
        }

        protected override void Update(PlacedBombBehavior ctr)
        {
            ctr.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                var explode = ctr.exlodePool.Pool.Get().GetComponent<ExplodeController>();
                explode.transform.position = ctr.transform.position;

                explode.Init(ExplodeController.Layer.EnemyAttack, 5);

                ctr.placedBomb.Delete();
                ctr.onExplodedFinishCallback?.Invoke();
            });
        }
    }
}
