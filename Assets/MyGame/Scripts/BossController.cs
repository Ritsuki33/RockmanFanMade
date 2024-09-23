using System;
using UnityEngine;

public class BossController : ExRbStateMachine<BossController>
{
    [SerializeField] private Animator _animator;
    private Gravity _gravity;
    private ExpandRigidBody _exRb;

    private Action finishActionCallback;
    enum StateId
    {
        Idle,
        Float,
        Appearance,
    }

    private void Awake()
    {
        _gravity = GetComponent<Gravity>();
        _exRb = GetComponent<ExpandRigidBody>();

        AddState((int)StateId.Idle, new Idle());
        //AddState((int)StateId.Float, new Float());
        AddState((int)StateId.Appearance, new Appearance());

        TransitReady((int)StateId.Appearance);
    }

    class Appearance : ExRbState<BossController>
    {
        enum SubStateId
        {
            Float,
            Pause,
            Wait,
        }

        public Appearance()
        {
            this.AddSubState((int)SubStateId.Float, new Float());
            this.AddSubState((int)SubStateId.Pause, new Pause());
            this.AddSubState((int)SubStateId.Wait, new Wait());
        }

        protected override void Enter(BossController ctr, int preId)
        {
            this.TransitSubReady((int)SubStateId.Float, preId);
        }

        protected override void FixedUpdate(BossController ctr, IParentState parent)
        {
            ctr._gravity.UpdateVelocity();
            ctr._exRb.velocity = ctr._gravity.CurrentVelocity;
        }

        class Float : ExRbState<BossController>
        {
            static int animationHash = Animator.StringToHash("Float");
            protected override void Enter(BossController ctr, int preId)
            {
                ctr._animator.Play(animationHash);
            }

            protected override void OnBottomHitEnter(BossController ctr, RaycastHit2D hit, IParentState parent)
            {
                parent.TransitSubReady((int)SubStateId.Pause);
            }
        }

        class Pause : ExRbState<BossController>
        {
            static int animationHash = Animator.StringToHash("Pause");
            protected override void Enter(BossController ctr, int preId)
            {
                ctr._animator.Play(animationHash);
            }

            protected override void OnBottomHitEnter(BossController ctr, RaycastHit2D hit, IParentState parent)
            {
                parent.TransitSubReady((int)SubStateId.Pause);
            }

            protected override void Update(BossController ctr, IParentState parent)
            {
                if (!ctr._animator.IsPlayingCurrentAnimation(animationHash))
                {
                    parent.TransitSubReady((int)SubStateId.Wait);
                }
            }

            protected override void Exit(BossController ctr, int nextId)
            {
                // 通知を飛ばす
                ctr.finishActionCallback?.Invoke();
            }
        }

        class Wait : ExRbState<BossController> { }
    }

    class Idle : ExRbState<BossController>
    {

        static int animationHash = Animator.StringToHash("Idle");
        protected override void Enter(BossController ctr, int preId)
        {
            ctr._animator.Play(animationHash);
        }

        protected override void FixedUpdate(BossController ctr, IParentState parent)
        {

            ctr._gravity.UpdateVelocity();
            ctr._exRb.velocity += ctr._gravity.CurrentVelocity;
        }

        protected override void OnBottomHitExit(BossController ctr, RaycastHit2D hit, IParentState parent)
        {
            ctr.TransitReady((int)StateId.Float);
        }
    }


    public void Appeare(Action finishCallback)
    {
        gameObject.SetActive(true);
        finishActionCallback = finishCallback;
        this.TransitReady((int)StateId.Appearance);
    }
}
