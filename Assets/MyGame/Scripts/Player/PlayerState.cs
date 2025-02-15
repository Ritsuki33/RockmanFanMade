using CriWare;
using UnityEngine;

public partial class StagePlayer
{
    class Standing : ExRbState<StagePlayer, Standing>
    {
        enum SubStateID
        {
            Basic,
            Shoot
        }

        public Standing()
        {
            AddSubState((int)SubStateID.Basic, new Basic());
            AddSubState((int)SubStateID.Shoot, new Shoot());
        }

        protected override void Enter(StagePlayer obj, int preId, int subId)
        {
            TransitSubReady((int)SubStateID.Basic);
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.gravity.OnUpdate();
            player.exRb.velocity = player.gravity.CurrentVelocity;

            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.OnUpdate(player.bottomHit.normal.Verticalize(), type, (player.curGround == null) ? 1.0f : player.curGround.Friction);
            Vector2 moveV = player.move.CurrentVelocity;
            player.exRb.velocity += moveV;

        }

        protected override void Update(StagePlayer player)
        {
            if (player.inputInfo.left)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Running);
            }
            else if (player.inputInfo.right)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Running);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                Vector2 pos = player.exRb.position;
                pos.y += 0.1f;
                player.exRb.SetPosition(pos);
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Climb);
            }
            else if (player.bottomHit && player.inputInfo.down)
            {
                if (player.bottomHit.collider.gameObject.CompareTag("Ladder"))
                {
                    player.bodyLadder = player.bottomHit.collider;
                    player.m_mainStateMachine.TransitReady((int)Main_StateID.ClimbDown);
                }
            }

            if (player.inputInfo.jump)
            {
                player.jump.Init(30);
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Jumping);
            }
        }

        protected override void OnBottomHitStay(StagePlayer player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
            player.bottomHit = hit;
        }

        protected override void OnBottomHitExit(StagePlayer player, RaycastHit2D hit)
        {
            player.m_mainStateMachine.TransitReady((int)Main_StateID.Floating);
        }

        protected override void OnTriggerEnter(StagePlayer player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        // ==========================================
        class Basic : ExRbSubState<StagePlayer, Basic, Standing>
        {
            int animationHash = 0;
            public Basic() { animationHash = Animator.StringToHash("Idle"); }

            protected override void Enter(StagePlayer player, Standing parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
            }

            protected override void Update(StagePlayer player, Standing parent)
            {
                player.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<StagePlayer, Shoot, Standing>
        {

            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("Fire"); }

            float time = 0.3f;

            protected override void Enter(StagePlayer player, Standing parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
                time = 0.3f;
            }

            protected override void Update(StagePlayer player, Standing parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Basic);
                }

                player.LaunchTrigger(player.inputInfo.fire, () => { time = 0.3f; });
                time -= Time.deltaTime;
            }
        }
        // ==========================================
    }

    class Running : ExRbState<StagePlayer, Running>
    {
        enum SubStateID
        {
            Run,
            Shoot,
        }
        public Running()
        {
            AddSubState((int)SubStateID.Run, new Run());
            AddSubState((int)SubStateID.Shoot, new Shoot());
        }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            TransitSubReady((int)SubStateID.Run);
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.gravity.OnUpdate();
            player.exRb.velocity += player.gravity.CurrentVelocity;
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.OnUpdate(player.bottomHit.normal.Verticalize(), type, (player.curGround == null) ? 1.0f : player.curGround.Friction);
            player.exRb.velocity += player.move.CurrentVelocity;

            if (type == Move.InputType.Right)
            {
                player.TurnTo(true);
            }
            else if (type == Move.InputType.Left)
            {
                player.TurnTo(false);
            }
        }

        protected override void Update(StagePlayer player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
            }
            else if (player.inputInfo.jump)
            {
                player.jump.Init(30);
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Jumping);
            }
        }

        protected override void OnBottomHitStay(StagePlayer player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
            player.bottomHit = hit;
        }

        protected override void OnBottomHitExit(StagePlayer player, RaycastHit2D hit)
        {
            player.m_mainStateMachine.TransitReady((int)Main_StateID.Floating);
        }

        protected override void OnTriggerEnter(StagePlayer player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        // ============================================================
        class Run : ExRbSubState<StagePlayer, Run, Running>
        {
            int animationHash = 0;
            public Run() { animationHash = Animator.StringToHash("Run"); }

            protected override void Enter(StagePlayer player, Running parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
            }

            protected override void Update(StagePlayer player, Running parent)
            {
                player.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<StagePlayer, Shoot, Running>
        {
            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("RunBuster"); }
            float time = 0.3f;

            protected override void Enter(StagePlayer player, Running parent, int preId, int subId)
            {
                time = 0.3f;
                player.MainAnimator.Play(animationHash);
            }

            protected override void Update(StagePlayer player, Running parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Run);
                }

                player.LaunchTrigger(player.inputInfo.fire, () => { time = 0.3f; });
                time -= Time.deltaTime;
            }
        }
    }

    class Floating : ExRbState<StagePlayer, Floating>
    {
        enum SubStateID
        {
            Basic,
            Shoot
        }

        public Floating()
        {
            AddSubState((int)SubStateID.Basic, new Basic());
            AddSubState((int)SubStateID.Shoot, new Shoot());
        }
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.bottomHit = default;
            TransitSubReady((int)SubStateID.Basic);
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.gravity.OnUpdate();
            player.exRb.velocity = player.gravity.CurrentVelocity;
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.OnUpdate(Vector2.right, type);
            player.exRb.velocity += player.move.CurrentVelocity;

            if (type == Move.InputType.Right)
            {
                player.TurnTo(true);
            }
            else if (type == Move.InputType.Left)
            {
                player.TurnTo(false);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Climb);
            }
        }

        protected override void Update(StagePlayer ctr)
        {
            if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject, out MainCameraControll.OutOfViewType outOfViewType))
            {
                if (outOfViewType == MainCameraControll.OutOfViewType.Bottom)
                {
                    ctr.Dead();
                }
            }
        }

        protected override void OnBottomHitEnter(StagePlayer obj, RaycastHit2D collision)
        {
            AudioManager.Instance.PlaySe(SECueIDs.tyakuti);
        }

        protected override void OnBottomHitStay(StagePlayer player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
            player.m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
        }

        protected override void OnTriggerEnter(StagePlayer player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        class Basic : ExRbSubState<StagePlayer, Basic, Floating>
        {
            int animationHash = 0;
            public Basic() { animationHash = Animator.StringToHash("Float"); }

            protected override void Enter(StagePlayer player, Floating parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
            }

            protected override void Update(StagePlayer player, Floating parent)
            {
                player.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<StagePlayer, Shoot, Floating>
        {
            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("FloatBuster"); }
            float time = 0.3f;

            protected override void Enter(StagePlayer player, Floating parent, int preId, int subId)
            {
                time = 0.3f;
                player.MainAnimator.Play(animationHash);
            }

            protected override void Update(StagePlayer player, Floating parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Basic);
                }

                player.LaunchTrigger(player.inputInfo.fire, () => { time = 0.15f; });

                time -= Time.deltaTime;
            }
        }
    }

    class Jumping : ExRbState<StagePlayer, Jumping>
    {
        bool isJumping = true;

        enum SubStateID
        {
            Basic,
            Shoot,
        }

        public Jumping()
        {
            AddSubState((int)SubStateID.Basic, new Basic());
            AddSubState((int)SubStateID.Shoot, new Shoot());
        }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            TransitSubReady((int)SubStateID.Basic);
            player.bottomHit = default;
            isJumping = true;
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.jump.OnUpdate(player.gravity.GravityScale);
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.OnUpdate(Vector2.right, type);
            player.exRb.velocity = player.move.CurrentVelocity;

            if (type == Move.InputType.Right)
            {
                player.TurnTo(true);
            }
            else if (type == Move.InputType.Left)
            {
                player.TurnTo(false);
            }

            player.exRb.velocity += player.jump.CurrentVelocity;

            if (player.jump.CurrentVelocity.sqrMagnitude <= 0.001f)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Floating);
            }
        }

        protected override void Update(StagePlayer player)
        {
            if (isJumping && !player.inputInfo.jumping)
            {
                player.jump.SetSpeed(player.jump.CurrentSpeed / 2);
                isJumping = false;
            }
        }

        protected override void OnTopHitStay(StagePlayer player, RaycastHit2D hit)
        {
            player.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(StagePlayer player, RaycastHit2D hit)
        {
            player.jump.SetSpeed(0);
        }

        protected override void OnTriggerEnter(StagePlayer player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        class Basic : ExRbSubState<StagePlayer, Basic, Jumping>
        {
            int animationHash = 0;
            public Basic() { animationHash = Animator.StringToHash("Float"); }

            protected override void Enter(StagePlayer player, Jumping parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
            }

            protected override void Update(StagePlayer player, Jumping parent)
            {
                player.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<StagePlayer, Shoot, Jumping>
        {
            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("FloatBuster"); }
            float time = 0.3f;

            protected override void Enter(StagePlayer player, Jumping parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
                time = 0.3f;
            }

            protected override void Update(StagePlayer player, Jumping parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Basic);
                }

                player.LaunchTrigger(player.inputInfo.fire, () => { time = 0.3f; });

                time -= Time.deltaTime;
            }
        }
    }

    class Climb : ExRbState<StagePlayer, Climb>
    {
        int animationHash = 0;
        public Climb() { animationHash = Animator.StringToHash("Climb"); }

        enum Dir
        {
            Up,
            Down,
            None
        }
        Dir input = Dir.None;
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.MainAnimator.Play(animationHash);
            player.MainAnimator.speed = 0;
            Vector2 pos = player.exRb.BoxColliderCenter;
            pos.x = player.bodyLadder.transform.position.x;
            player.exRb.SetPosition(pos);

            player.bottomHit = default;
            player.gravity.Reset();
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            switch (input)
            {
                case Dir.Up:
                    player.exRb.velocity = new Vector2(0, 5);
                    break;
                case Dir.Down:
                    player.exRb.velocity = new Vector2(0, -5);
                    break;
                case Dir.None:
                    break;
                default:
                    break;
            }
        }

        protected override void Update(StagePlayer player)
        {

            if (player.bodyLadder == null)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
                return;
            }

            player.isladderTop = player.transform.position.y > player.bodyLadder.bounds.max.y;
            if (player.isladderTop && player.inputInfo.up)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.ClimbUp);
            }
            else if (player.inputInfo.down)
            {
                player.MainAnimator.speed = 1;
                input = Dir.Down;

            }
            else if (player.inputInfo.up)
            {
                player.MainAnimator.speed = 1;
                input = Dir.Up;
            }
            else
            {
                input = Dir.None;
                player.MainAnimator.speed = 0;
            }

            if (player.inputInfo.jump)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Floating);
            }
        }

        protected override void Exit(StagePlayer player, int nextId)
        {
            player.MainAnimator.speed = 1;
        }

        protected override void OnBottomHitStay(StagePlayer player, RaycastHit2D hit)
        {
            player.m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
        }
    }

    class ClimbUp : ExRbState<StagePlayer, ClimbUp>
    {
        int animationHash = 0;
        public ClimbUp() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.MainAnimator.Play(animationHash);
            time = 0;
        }
        protected override void FixedUpdate(StagePlayer player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.exRb.Bottom = player.bodyLadder.bounds.max.y;
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
            }
        }
    }

    class ClimbDown : ExRbState<StagePlayer, ClimbDown>
    {
        int animationHash = 0;
        public ClimbDown() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.MainAnimator.Play(animationHash);
            Vector2 nextPos = player.exRb.BoxColliderCenter;
            nextPos.x = player.bodyLadder.transform.position.x;
            nextPos.y = player.bodyLadder.bounds.max.y;
            player.exRb.SetPosition(nextPos);
            time = 0;
        }
        protected override void FixedUpdate(StagePlayer player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Climb);
            }
        }
    }

    class Death : ExRbState<StagePlayer, Death>
    {
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.Delete();
            var deathEffect = ObjectManager.Instance.OnGet<PsObject>(PoolType.PlayerDeathEffect);
            deathEffect.Setup(player.transform.position);

            if (player.chargePlayback.status == CriAtomExPlayback.Status.Playing) player.chargePlayback.Stop();
            AudioManager.Instance.PlaySe(SECueIDs.thiun);
        }
    }

    class Transfer : ExRbState<StagePlayer, Transfer>
    {
        int animationHash = 0;
        public Transfer() { animationHash = Animator.StringToHash("Transfer"); }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.boxPhysicalCollider.enabled = false;
            player.MainAnimator.Play(animationHash);
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.exRb.velocity = Vector2.down * 26;
        }
        protected override void OnBottomHitStay(StagePlayer player, RaycastHit2D hit)
        {
            player.m_mainStateMachine.TransitReady((int)Main_StateID.Transfered);
        }

        protected override void OnTriggerEnter(StagePlayer player, Collider2D collision)
        {
            player.boxPhysicalCollider.enabled = true;
        }
    }

    class Transfered : ExRbState<StagePlayer, Transfered>
    {
        int animationHash = 0;
        public Transfered() { animationHash = Animator.StringToHash("Transfered"); }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.MainAnimator.Play(animationHash);
            AudioManager.Instance.PlaySe(SECueIDs.teleportin);
        }

        protected override void Update(StagePlayer player)
        {
            if (!player.MainAnimator.IsPlayingCurrentAnimation(animationHash))
            {
                player.m_mainStateMachine.TransitReady((int)(Main_StateID.Standing));
                player.ActionFinishNotify();
            }
        }
    }

    class Repatriation : ExRbState<StagePlayer, Repatriation>
    {
        int animationHash = 0;
        public Repatriation()
        {
            animationHash = Animator.StringToHash("Repatriation");
            AddSubState(0, new Start());
            AddSubState(1, new Rise());

        }

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.boxPhysicalCollider.enabled = false;
            player.MainAnimator.Play(animationHash);

            TransitSubReady(0);

            AudioManager.Instance.PlaySe(SECueIDs.teleportout);

        }

        class Start : ExRbSubState<StagePlayer, Start, Repatriation>
        {
            protected override void Update(StagePlayer player, Repatriation parent)
            {
                if (!player.MainAnimator.IsPlayingCurrentAnimation(parent.animationHash))
                {
                    parent.TransitSubReady(1);
                }
            }
        }

        class Rise : ExRbSubState<StagePlayer, Rise, Repatriation>
        {
            protected override void FixedUpdate(StagePlayer player, Repatriation parent)
            {
                player.exRb.velocity = Vector2.up * 26;

                if (GameMainManager.Instance.MainCameraControll.CheckOutOfView(player.gameObject))
                {
                    player.gameObject.SetActive(false);

                    // 通知する
                    player.ActionFinishNotify();
                }
            }
        }
    }


    class AutoMove : ExRbState<StagePlayer, AutoMove>
    {
        public enum SubStateId
        {
            Float,
            Run,
            Wait
        }

        public AutoMove()
        {
            AddSubState((int)SubStateId.Float, new Float());
            AddSubState((int)SubStateId.Run, new Run());
            AddSubState((int)SubStateId.Wait, new Wait());
        }
        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.bottomHit = default;

            if (subId >= 0)
            {
                this.TransitSubReady(subId);
            }
            else
            {
                var preStateId = (Main_StateID)preId;

                if (preStateId == Main_StateID.Standing || preStateId == Main_StateID.Running)
                {
                    this.TransitSubReady((int)SubStateId.Run);
                }
                else
                {
                    this.TransitSubReady((int)SubStateId.Float);
                }
            }
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.gravity.OnUpdate();
            player.exRb.velocity = player.gravity.CurrentVelocity;
        }


        class Float : ExRbSubState<StagePlayer, Float, AutoMove>
        {
            int animationHash = Animator.StringToHash("Float");
            bool isWait = false;

            protected override void Enter(StagePlayer player, AutoMove parent, int preId, int subId)
            {
                player.MainAnimator.Play(animationHash);
                if (preId == (int)SubStateId.Wait)
                {
                    isWait = true;
                }
            }

            protected override void OnBottomHitStay(StagePlayer player, AutoMove parent, RaycastHit2D hit)
            {
                if (isWait)
                {
                    parent.TransitSubReady((int)SubStateId.Wait);
                }
                else
                {
                    parent.TransitSubReady((int)SubStateId.Run);
                }
            }
        }

        class Run : ExRbSubState<StagePlayer, Run, AutoMove>
        {
            float bamili_x; // x軸だけ利用
            int animationHash = Animator.StringToHash("Run");

            float preViousX = 0;

            protected override void Enter(StagePlayer player, AutoMove parent, int preId, int subId)
            {
                if (player.bamili)
                {
                    bamili_x = player.bamili.position.x;
                }
                preViousX = player.transform.position.x;

                player.MainAnimator.Play(animationHash);
            }

            protected override void OnBottomHitStay(StagePlayer player, AutoMove parent, RaycastHit2D hit)
            {
                player.gravity.OnGround(hit.normal);
                player.bottomHit = hit;
            }

            protected override void OnBottomHitExit(StagePlayer player, AutoMove parent, RaycastHit2D hit)
            {
                parent.TransitSubReady((int)SubStateId.Float);
            }

            protected override void FixedUpdate(StagePlayer player, AutoMove parent)
            {
                player.gravity.OnUpdate();
                player.exRb.velocity = player.gravity.CurrentVelocity;


                if ((preViousX < bamili_x && player.transform.position.x >= bamili_x) || (preViousX > bamili_x && player.transform.position.x <= bamili_x))
                {
                    parent.TransitSubReady((int)SubStateId.Wait);
                }
                else
                {
                    Move.InputType type = default;
                    if (player.transform.position.x > bamili_x) type = Move.InputType.Left;
                    else if (player.transform.position.x < bamili_x) type = Move.InputType.Right;
                    player.move.OnUpdate(player.bottomHit.normal.Verticalize(), type);
                    Vector2 moveV = player.move.CurrentVelocity;
                    player.exRb.velocity += moveV;

                    if (moveV.x > 0)
                    {
                        player.TurnTo(true);
                    }
                    else if (moveV.x < 0)
                    {
                        player.TurnTo(false);
                    }

                    if (!player.bottomHit)
                    {
                        parent.TransitSubReady((int)SubStateId.Float);
                    }

                    preViousX = player.transform.position.x;
                }
            }
        }

        class Wait : ExRbSubState<StagePlayer, Wait, AutoMove>
        {
            int animationHash = Animator.StringToHash("Idle");

            protected override void Enter(StagePlayer player, AutoMove parent, int preId, int subId)
            {
                // 通知する
                player.ActionFinishNotify();

                player.bamili = null;

                player.MainAnimator.Play(animationHash);
            }

            protected override void OnBottomHitExit(StagePlayer player, AutoMove parent, RaycastHit2D hit)
            {
                parent.TransitSubReady((int)SubStateId.Float);
            }
        }
    }
    class DamagedState : ExRbState<StagePlayer, DamagedState>
    {
        int animationHash = Animator.StringToHash("Damaged");

        protected override void Enter(StagePlayer player, int preId, int subId)
        {
            player.MainAnimator.Play(animationHash);
            player.timer.Start(0.6f, 0.6f);
        }

        protected override void FixedUpdate(StagePlayer player)
        {
            player.exRb.velocity = ((player.IsRight) ? Vector2.left : Vector2.right) * 2;
        }

        protected override void Update(StagePlayer player)
        {
            player.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                player.m_mainStateMachine.TransitReady((int)Main_StateID.Standing);
            });
        }

        protected override void Exit(StagePlayer player, int nextId)
        {
            player.InvincibleState();
        }
    }
}
