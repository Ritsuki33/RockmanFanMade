using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player
{
    class Idle : ExRbState<Player>
    {
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }

        protected override void FixedUpdate(Player player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.TransitReady((int)StateID.Float);
            }
        }

        protected override void Update(Player player)
        {
            if (player.inputInfo.left)
            {
                player.TransitReady((int)StateID.Run);
            }
            else if (player.inputInfo.right)
            {
                player.TransitReady((int)StateID.Run);
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

            if (player.inputInfo.fire)
            {
                player.TransitReady((int)StateID.IdleFire);
            }
        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
        }
    }

    class Float : ExRbState<Player>
    {
        int animationHash = 0;
        public Float() { animationHash = Animator.StringToHash("Float"); }

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.onTheGround.Reset();
        }

        protected override void FixedUpdate(Player player)
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
                Vector3 localScale = player.transform.localScale;
                localScale.x = 1;
                player.transform.localScale = localScale;
            }
            else if (moveV.x < 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = -1;
                player.transform.localScale = localScale;
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                player.TransitReady((int)StateID.Climb);
            }
        }

        protected override void Update(Player player)
        {
            if (player.inputInfo.fire)
            {
                player.TransitReady((int)StateID.FloatBuster);
            }
        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
            player.TransitReady((int)StateID.Idle);
        }
    }

    class Run : ExRbState<Player>
    {
        int animationHash = 0;
        public Run() { animationHash = Animator.StringToHash("Run"); }

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }

        protected override void FixedUpdate(Player player)
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
                Vector3 localScale = player.transform.localScale;
                localScale.x = 1;
                player.transform.localScale = localScale;
            }
            else if (moveV.x < 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = -1;
                player.transform.localScale = localScale;
            }
            if (!player.onTheGround.Check())
            {
                player.TransitReady((int)StateID.Float);
            }
        }

        protected override void Update(Player player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.TransitReady((int)StateID.Idle);
            }
            else if (player.inputInfo.jump)
            {
                player.TransitReady((int)StateID.Jumping);
            }
            else if (player.inputInfo.fire)
            {
                player.TransitReady((int)StateID.RunBuster);
            }

        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
        }
    }

    class Jumping : ExRbState<Player>
    {
        int animationHash = 0;
        bool isJumping = true;
        public Jumping() { animationHash = Animator.StringToHash("Float"); }

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            if (preId != (int)StateID.JumpingBuster) player.jump.Init();
            player.onTheGround.Reset();
            isJumping = true;
        }

        protected override void FixedUpdate(Player player)
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
                Vector3 localScale = player.transform.localScale;
                localScale.x = 1;
                player.transform.localScale = localScale;
            }
            else if (moveV.x < 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = -1;
                player.transform.localScale = localScale;
            }

            player.exRb.velocity += player.jump.CurrentVelocity;

            if (player.jump.CurrentVelocity.sqrMagnitude <= 0.001f)
            {
                player.TransitReady((int)StateID.Float);
            }
        }

        protected override void Update(Player player)
        {
            if (player.inputInfo.fire)
            {
                player.TransitReady((int)StateID.JumpingBuster);
            }


            if (isJumping && !player.inputInfo.jumping)
            {
                player.jump.SetSpeed(player.jump.CurrentSpeed / 2);
                isJumping = false;
            }
        }

        protected override void Exit(Player player, int nextId)
        {
            if (nextId != (int)StateID.JumpingBuster) player.jump.SetSpeed(0); 
        }

        protected override void OnTopHitStay(Player player, RaycastHit2D hit)
        {
            player.jump.SetSpeed(0);
        }

        protected override void OnBottomHitEnter(Player player, RaycastHit2D hit)
        {
            player.jump.SetSpeed(0);
        }
    }

    class Climb : ExRbState<Player>
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
        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.animator.speed = 0;
            Vector2 pos = player.exRb.BoxColliderCenter;
            pos.x = player.bodyLadder.transform.position.x;
            player.exRb.SetPosition(pos);

            player.onTheGround.Reset();
            player.gravity.Reset();
        }

        protected override void FixedUpdate(Player player)
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

        protected override void Update(Player player)
        {

            if (player.bodyLadder == null)
            {
                player.TransitReady((int)StateID.Idle);
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
                player.TransitReady((int)StateID.Float);
            }
        }

        protected override void Exit(Player player,int nextId)
        {
            player.animator.speed = 1;
        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.TransitReady((int)StateID.Idle);
        }
    }

    class ClimbUp : ExRbState<Player>
    {
        int animationHash = 0;
        public ClimbUp() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            time = 0;
        }
        protected override void FixedUpdate(Player player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.exRb.SetPosition(ExpandRigidBody.PostionSetType.Bottom, player.bodyLadder.bounds.max.y);
                player.TransitReady((int)StateID.Idle);
            }
        }
    }

    class ClimbDown : ExRbState<Player>
    {
        int animationHash = 0;
        public ClimbDown() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            Vector2 nextPos = player.exRb.BoxColliderCenter;
            nextPos.x = player.bodyLadder.transform.position.x;
            nextPos.y = player.bodyLadder.bounds.max.y;
            player.exRb.SetPosition(nextPos);
            time = 0;
        }
        protected override void FixedUpdate(Player player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.TransitReady((int)StateID.Climb);
            }
        }
    }

    class IdleFire : ExRbState<Player>
    {

        int animationHash = 0;
        public IdleFire() { animationHash = Animator.StringToHash("Fire"); }

        float time = 0.15f;
        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.LaunchBaster();
            time = 0.15f;
        }

        protected override void FixedUpdate(Player player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.TransitReady((int)StateID.Float);
            }
        }

        protected override void Update(Player player)
        {
            if (player.inputInfo.jump)
            {
                player.TransitReady((int)StateID.Jumping);
            }

            if (time < 0)
            {
                player.TransitReady((int)StateID.Idle);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.15f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
        }
    }

    class RunBuster : ExRbState<Player>
    {
        int animationHash = 0;
        public RunBuster() { animationHash = Animator.StringToHash("RunBuster"); }
        float time = 0.3f;
        protected override void Enter(Player player, int preId)
        {
            time = 0.3f;
            player.animator.Play(animationHash);

            player.LaunchBaster();
        }

        protected override void FixedUpdate(Player player)
        {
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;
            player.move.UpdateVelocity(player.onTheGround.GroundHit.normal.Verticalize(), type);
            Vector2 moveV = player.move.CurrentVelocity;
            player.exRb.velocity = moveV;

            if (moveV.x > 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = 1;
                player.transform.localScale = localScale;
            }
            else if (moveV.x < 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = -1;
                player.transform.localScale = localScale;
            }

            if (!player.onTheGround.Check())
            {
                player.TransitReady((int)StateID.Float);
            }
        }

        protected override void Update(Player player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.TransitReady((int)StateID.Idle);
            }
            else if (player.inputInfo.jump)
            {
                player.TransitReady((int)StateID.Jumping);
            }

            if (time < 0)
            {
                player.TransitReady((int)StateID.Run);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.3f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
        }
    }

    class FloatBuster : ExRbState<Player>
    {
        int animationHash = 0;
        public FloatBuster() { animationHash = Animator.StringToHash("FloatBuster"); }
        float time = 0.15f;
        protected override void Enter(Player player, int preId)
        {
            time = 0.15f;
            player.animator.Play(animationHash);

            if (preId != 10) player.LaunchBaster();
        }

        protected override void FixedUpdate(Player player)
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
                Vector3 localScale = player.transform.localScale;
                localScale.x = 1;
                player.transform.localScale = localScale;
            }
            else if (moveV.x < 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = -1;
                player.transform.localScale = localScale;
            }
        }

        protected override void Update(Player player)
        {
            if (time < 0)
            {
                player.TransitReady((int)StateID.Float);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.15f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.gravity.OnGround(hit.normal);
            player.TransitReady((int)StateID.Idle);
        }
    }

    class JumpingBuster : ExRbState<Player>
    {
        int animationHash = 0;
        public JumpingBuster() { animationHash = Animator.StringToHash("FloatBuster"); }
        float time = 0.15f;

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.LaunchBaster();
            time = 0.15f;

        }

        protected override void FixedUpdate(Player player)
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
                Vector3 localScale = player.transform.localScale;
                localScale.x = 1;
                player.transform.localScale = localScale;
            }
            else if (moveV.x < 0)
            {
                Vector3 localScale = player.transform.localScale;
                localScale.x = -1;
                player.transform.localScale = localScale;
            }

            player.exRb.velocity += player.jump.CurrentVelocity;

            if (player.jump.CurrentVelocity.sqrMagnitude <= 0.001f)
            {
                player.TransitReady((int)StateID.FloatBuster);
            }
        }

        protected override void Update(Player player)
        {
            if (time < 0)
            {
                player.TransitReady((int)StateID.Jumping);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.15f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }
    }

    class Death : ExRbState<Player>
    {
        protected override void Enter(Player player, int preId)
        {
            player.gameObject.SetActive(false);
            EffectManager.Instance.PlayerDeathEffect.gameObject.transform.position = player.transform.position;
            EffectManager.Instance.PlayerDeathEffect.Play();
        }
    }

    class Transfer : ExRbState<Player>
    {
        int animationHash = 0;
        public Transfer() { animationHash = Animator.StringToHash("Transfer"); }

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }

        protected override void FixedUpdate(Player player)
        {
            player.exRb.velocity = Vector2.down * 13;

        }

        protected override void OnBottomHitStay(Player player, RaycastHit2D hit)
        {
            player.TransitReady((int)StateID.Transfered);
        }
    }

    class Transfered : ExRbState<Player>
    {
        int animationHash = 0;
        public Transfered() { animationHash = Animator.StringToHash("Transfered"); }

        protected override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }
    }
}
