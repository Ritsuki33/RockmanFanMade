using System.Runtime.ConstrainedExecution;
using System.Threading;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public partial class PlayerController
{
    class Standing : ExRbState<PlayerController, Standing>
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

        protected override void Enter(PlayerController obj, int preId, int subId)
        {
            TransitSubReady((int)SubStateID.Basic);
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.TransitReady((int)StateID.Floating);
            }
        }

        protected override void Update(PlayerController player)
        {
            if (player.inputInfo.left)
            {
                player.TransitReady((int)StateID.Running);
            }
            else if (player.inputInfo.right)
            {
                player.TransitReady((int)StateID.Running);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                Vector2 pos = player.exRb.position;
                pos.y += 0.1f;
                player.exRb.SetPosition(pos);
                player.TransitReady((int)StateID.Climb);
            }
            else if (player.onTheGround.GroundHit && player.inputInfo.down)
            {
                if (player.onTheGround.GroundHit.collider.gameObject.CompareTag("Ladder"))
                {
                    player.bodyLadder = player.onTheGround.GroundHit.collider;
                    player.TransitReady((int)StateID.ClimbDown);
                }
            }

            if (player.inputInfo.jump)
            {
                player.TransitReady((int)StateID.Jumping);
            }
        }

        protected override void OnBottomHitStay(PlayerController player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
        }

        protected override void OnTriggerEnter(PlayerController player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        // ==========================================
        class Basic : ExRbSubState<PlayerController, Basic,Standing>
        {
            int animationHash = 0;
            public Basic() { animationHash = Animator.StringToHash("Idle"); }

            protected override void Enter(PlayerController player, Standing parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
            }

            protected override void Update(PlayerController player, Standing parent)
            {
                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<PlayerController, Shoot, Standing>
        {

            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("Fire"); }

            float time = 0.3f;

            protected override void Enter(PlayerController player, Standing parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
                time = 0.3f;
            }

            protected override void Update(PlayerController player, Standing parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Basic);
                }

                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { time = 0.3f; });
                time -= Time.deltaTime;
            }
        }
        // ==========================================
    }

    class Running: ExRbState<PlayerController,Running>
    {
        enum SubStateID{
            Run,
            Shoot,
        }
        public Running()
        {
            AddSubState((int)SubStateID.Run, new Run());
            AddSubState((int)SubStateID.Shoot, new Shoot());
        }

        protected override void Enter(PlayerController player, int preId, int subId)
        {
            TransitSubReady((int)SubStateID.Run);
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity += player.gravity.CurrentVelocity;
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.UpdateVelocity(player.onTheGround.GroundHit.normal.Verticalize(), type);
            Vector2 moveV = player.move.CurrentVelocity;
            player.exRb.velocity += moveV;

            if (moveV.x > 0)
            {
                player.player.TurnTo(true);
            }
            else if (moveV.x < 0)
            {
                player.player.TurnTo(false);

               
            }
            if (!player.onTheGround.Check())
            {
                player.TransitReady((int)StateID.Floating);
            }
        }

        protected override void Update(PlayerController player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.TransitReady((int)StateID.Standing);
            }
            else if (player.inputInfo.jump)
            {
                player.TransitReady((int)StateID.Jumping);
            }
        }

        protected override void OnBottomHitStay(PlayerController player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
        }

        protected override void OnTriggerEnter(PlayerController player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        // ============================================================
        class Run : ExRbSubState<PlayerController, Run, Running>
        {
            int animationHash = 0;
            public Run() { animationHash = Animator.StringToHash("Run"); }

            protected override void Enter(PlayerController player, Running parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
            }

            protected override void Update(PlayerController player, Running parent)
            {
                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<PlayerController, Shoot, Running>
        {
            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("RunBuster"); }
            float time = 0.3f;

            protected override void Enter(PlayerController player, Running parent, int preId, int subId)
            {
                time = 0.3f;
                player.animator.Play(animationHash);
            }

            protected override void Update(PlayerController player, Running parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Run);
                }

                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { time = 0.3f; });
                time -= Time.deltaTime;
            }
        }
    }

    class Floating : ExRbState<PlayerController, Floating>
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
        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.onTheGround.Reset();
            TransitSubReady((int)SubStateID.Basic);
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.UpdateVelocity(Vector2.right, type);
            Vector2 moveV = player.move.CurrentVelocity;
            player.exRb.velocity += moveV;

            if (moveV.x > 0)
            {
                player.player.TurnTo(true);
            }
            else if (moveV.x < 0)
            {
                player.player.TurnTo(false);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                player.TransitReady((int)StateID.Climb);
            }
        }

        protected override void Update(PlayerController ctr)
        {
            if(GameMainManager.Instance.MainCameraControll.CheckOutOfView(ctr.gameObject,out MainCameraControll.OutOfViewType outOfViewType))
            {
                if (outOfViewType == MainCameraControll.OutOfViewType.Bottom)
                {
                    ctr.Dead();
                }
            }
        }

        protected override void OnBottomHitStay(PlayerController player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
            player.TransitReady((int)StateID.Standing);
        }

        protected override void OnTriggerEnter(PlayerController player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        class Basic: ExRbSubState<PlayerController, Basic, Floating>
        {
            int animationHash = 0;
            public Basic() { animationHash = Animator.StringToHash("Float"); }

            protected override void Enter(PlayerController player, Floating parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
            }

            protected override void Update(PlayerController player, Floating parent)
            {
                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot: ExRbSubState<PlayerController, Shoot, Floating>
        {
            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("FloatBuster"); }
            float time = 0.3f;

            protected override void Enter(PlayerController player, Floating parent, int preId, int subId)
            {
                time = 0.3f;
                player.animator.Play(animationHash);
            }

            protected override void Update(PlayerController player, Floating parent)
            {
                if (time < 0)
                {
                    player.TransitReady((int)SubStateID.Shoot);
                }

                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { time = 0.15f; });

                time -= Time.deltaTime;
            }
        }
    }

    class Jumping : ExRbState<PlayerController, Jumping>
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

        protected override void Enter(PlayerController player, int preId, int subId)
        {
            TransitSubReady((int)SubStateID.Basic);
            player.onTheGround.Reset();
            player.jump.Init();
            isJumping = true;
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.jump.UpdateVelocity(player.gravity.GravityScale);
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.UpdateVelocity(Vector2.right, type);
            Vector2 moveV = player.move.CurrentVelocity;
            player.exRb.velocity = moveV;

            if (moveV.x > 0)
            {
                player.player.TurnTo(true);
            }
            else if (moveV.x < 0)
            {
                player.player.TurnTo(false);
            }

            player.exRb.velocity += player.jump.CurrentVelocity;

            if (player.jump.CurrentVelocity.sqrMagnitude <= 0.001f)
            {
                player.TransitReady((int)StateID.Floating);
            }
        }

        protected override void Update(PlayerController player)
        {
            if (isJumping && !player.inputInfo.jumping)
            {
                player.jump.SetSpeed(player.jump.CurrentSpeed / 2);
                isJumping = false;
            }
        }

        protected override void OnTopHitStay(PlayerController player, RaycastHit2D hit)
        {
            player.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(PlayerController player, RaycastHit2D hit)
        {
            player.jump.SetSpeed(0);
        }

        protected override void OnTriggerEnter(PlayerController player, DamageBase collision)
        {
            if (!player.invincible) player.Damaged(collision.baseDamageValue);
        }

        class Basic: ExRbSubState<PlayerController, Basic, Jumping>
        {
            int animationHash = 0;
            public Basic() { animationHash = Animator.StringToHash("Float"); }

            protected override void Enter(PlayerController player, Jumping parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
            }

            protected override void Update(PlayerController player, Jumping parent)
            {
                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { parent.TransitSubReady((int)SubStateID.Shoot); });
            }
        }

        class Shoot : ExRbSubState<PlayerController, Shoot, Jumping>
        {
            int animationHash = 0;
            public Shoot() { animationHash = Animator.StringToHash("FloatBuster"); }
            float time = 0.3f;

            protected override void Enter(PlayerController player, Jumping parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
                time = 0.3f;
            }

            protected override void Update(PlayerController player, Jumping parent)
            {
                if (time < 0)
                {
                    parent.TransitSubReady((int)SubStateID.Basic);
                }

                player.launcherController.LaunchTrigger(player.inputInfo.fire, () => { time = 0.3f; });

                time -= Time.deltaTime;
            }
        }
    }

    class Climb : ExRbState<PlayerController, Climb>
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
        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.animator.Play(animationHash);
            player.animator.speed = 0;
            Vector2 pos = player.exRb.BoxColliderCenter;
            pos.x = player.bodyLadder.transform.position.x;
            player.exRb.SetPosition(pos);

            player.onTheGround.Reset();
            player.gravity.Reset();
        }

        protected override void FixedUpdate(PlayerController player)
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

        protected override void Update(PlayerController player)
        {

            if (player.bodyLadder == null)
            {
                player.TransitReady((int)StateID.Standing);
                return;
            }

            player.isladderTop = player.transform.position.y > player.bodyLadder.bounds.max.y;
            if (player.isladderTop && player.inputInfo.up)
            {
                player.TransitReady((int)StateID.ClimbUp);
            }
            else if (player.inputInfo.down)
            {
                player.animator.speed = 1;
                input = Dir.Down;

            }
            else if (player.inputInfo.up)
            {
                player.animator.speed = 1;
                input = Dir.Up;
            }
            else
            {
                input = Dir.None;
                player.animator.speed = 0;
            }

            if (player.inputInfo.jump)
            {
                player.TransitReady((int)StateID.Floating);
            }
        }

        protected override void Exit(PlayerController player,int nextId)
        {
            player.animator.speed = 1;
        }

        protected override void OnBottomHitStay(PlayerController player, RaycastHit2D hit)
        {
            player.TransitReady((int)StateID.Standing);
        }
    }

    class ClimbUp : ExRbState<PlayerController, ClimbUp>
    {
        int animationHash = 0;
        public ClimbUp() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.animator.Play(animationHash);
            time = 0;
        }
        protected override void FixedUpdate(PlayerController player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.exRb.Bottom = player.bodyLadder.bounds.max.y;
                player.TransitReady((int)StateID.Standing);
            }
        }
    }

    class ClimbDown : ExRbState<PlayerController, ClimbDown>
    {
        int animationHash = 0;
        public ClimbDown() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.animator.Play(animationHash);
            Vector2 nextPos = player.exRb.BoxColliderCenter;
            nextPos.x = player.bodyLadder.transform.position.x;
            nextPos.y = player.bodyLadder.bounds.max.y;
            player.exRb.SetPosition(nextPos);
            time = 0;
        }
        protected override void FixedUpdate(PlayerController player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.TransitReady((int)StateID.Climb);
            }
        }
    }

    class Death : ExRbState<PlayerController, Death>
    {
        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.gameObject.SetActive(false);
            var deathEffect = EffectManager.Instance.DeathEffectPool.Pool.Get().GetComponent<ParticleSystem>();
            deathEffect.gameObject.transform.position = player.transform.position;

            deathEffect.Play();
        }
    }

    class Transfer : ExRbState<PlayerController, Transfer>
    {
        int animationHash = 0;
        public Transfer() { animationHash = Animator.StringToHash("Transfer"); }

        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.boxPhysicalCollider.enabled = false;
            player.animator.Play(animationHash);
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.exRb.velocity = Vector2.down * 26;
        }
        protected override void OnBottomHitStay(PlayerController player, RaycastHit2D hit)
        {
            player.TransitReady((int)StateID.Transfered);
        }

        protected override void OnTriggerEnter(PlayerController player, Collider2D collision)
        {
            player.boxPhysicalCollider.enabled = true;
        }
    }

    class Transfered : ExRbState<PlayerController, Transfered>
    {
        int animationHash = 0;
        public Transfered() { animationHash = Animator.StringToHash("Transfered"); }

        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.animator.Play(animationHash);
        }

        protected override void Update(PlayerController player)
        {
            if (!player.animator.IsPlayingCurrentAnimation(animationHash))
            {
                player.TransitReady((int)(StateID.Standing));
                player.ActionFinishNotify();
            }
        }
    }

    class Repatriation : ExRbState<PlayerController, Repatriation>
    {
        int animationHash = 0;
        public Repatriation() {
            animationHash = Animator.StringToHash("Repatriation");
            AddSubState(0, new Start());
            AddSubState(1, new Rise());

        }

        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.boxPhysicalCollider.enabled = false;
            player.animator.Play(animationHash);

            TransitSubReady(0);
        }

        class Start : ExRbSubState<PlayerController, Start, Repatriation>
        {
            protected override void Update(PlayerController player, Repatriation parent)
            {
                if (!player.animator.IsPlayingCurrentAnimation(parent.animationHash))
                {
                    parent.TransitSubReady(1);
                }
            }
        }

        class Rise : ExRbSubState<PlayerController, Rise, Repatriation>
        {
            protected override void FixedUpdate(PlayerController player, Repatriation parent)
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


    class AutoMove : ExRbState<PlayerController, AutoMove>
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
        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.onTheGround.Reset();

            if (subId >= 0)
            {
                this.TransitSubReady(subId);
            }
            else
            {
                var preStateId = (StateID)preId;

                if (preStateId == StateID.Standing || preStateId == StateID.Running)
                {
                    this.TransitSubReady((int)SubStateId.Run);
                }
                else
                {
                    this.TransitSubReady((int)SubStateId.Float);
                }
            }
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;
        }


        class Float : ExRbSubState<PlayerController, Float, AutoMove>
        {
            int animationHash = Animator.StringToHash("Float");
            bool isWait = false;

            protected override void Enter(PlayerController player, AutoMove parent, int preId, int subId)
            {
                player.animator.Play(animationHash);
                if (preId == (int)SubStateId.Wait)
                {
                    isWait = true;
                }
            }

            protected override void OnBottomHitStay(PlayerController player, AutoMove parent, RaycastHit2D hit)
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

        class Run : ExRbSubState<PlayerController, Run, AutoMove>
        {
            float bamili_x; // x軸だけ利用
            int animationHash = Animator.StringToHash("Run");

            float preViousX = 0;

            protected override void Enter(PlayerController player, AutoMove parent, int preId, int subId)
            {
                if (player.bamili)
                {
                    bamili_x = player.bamili.position.x;
                }
                preViousX = player.transform.position.x;

                player.animator.Play(animationHash);
            }

            protected override void OnBottomHitExit(PlayerController player, AutoMove parent, RaycastHit2D hit)
            {
                parent.TransitSubReady((int)SubStateId.Float);
            }

            protected override void FixedUpdate(PlayerController player, AutoMove parent)
            {
                player.gravity.UpdateVelocity();
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
                    player.move.UpdateVelocity(player.onTheGround.GroundHit.normal.Verticalize(), type);
                    Vector2 moveV = player.move.CurrentVelocity;
                    player.exRb.velocity += moveV;

                    if (moveV.x > 0)
                    {
                        player.player.TurnTo(true);
                    }
                    else if (moveV.x < 0)
                    {
                        player.player.TurnTo(false);
                    }

                    if (!player.onTheGround.Check())
                    {
                        parent.TransitSubReady((int)SubStateId.Float);
                    }

                    preViousX = player.transform.position.x;
                }
            }
        }

        class Wait : ExRbSubState<PlayerController, Wait, AutoMove> { 
            int animationHash = Animator.StringToHash("Idle");

            protected override void Enter(PlayerController player, AutoMove parent, int preId, int subId)
            {
                // 通知する
                player.ActionFinishNotify();

                player.bamili = null;

                player.animator.Play(animationHash);
            }

            protected override void OnBottomHitExit(PlayerController player, AutoMove parent, RaycastHit2D hit)
            {
                parent.TransitSubReady((int)SubStateId.Float);
            }
        }
    }
    class DamagedState : ExRbState<PlayerController, DamagedState>
    {
        int animationHash = Animator.StringToHash("Damaged");

        protected override void Enter(PlayerController player, int preId, int subId)
        {
            player.animator.Play(animationHash);
            player.timer.Start(0.6f, 0.6f);
        }

        protected override void FixedUpdate(PlayerController player)
        {
            player.exRb.velocity = ((player.IsRight) ? Vector2.left : Vector2.right) * 2;
        }

        protected override void Update(PlayerController player)
        {
            player.timer.MoveAheadTime(Time.deltaTime, () =>
            {
                player.TransitReady((int)StateID.Standing);
            });
        }

        protected override void Exit(PlayerController player, int nextId)
        {
            player.InvincibleState();
        }
    }
}
