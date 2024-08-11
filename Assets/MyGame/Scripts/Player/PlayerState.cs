using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player
{
    class Idle : State<Player>
    {
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }

        public override void FixedUpdate(Player player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.stateMachine.TransitState((int)StateID.Float);
            }
        }

        public override void Update(Player player)
        {
            if (player.inputInfo.left)
            {
                player.stateMachine.TransitState((int)StateID.Run);
            }
            else if (player.inputInfo.right)
            {
                player.stateMachine.TransitState((int)StateID.Run);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                Vector2 pos = player.exRb.position;
                pos.y += 0.1f;
                player.exRb.SetPosition(pos);
                player.stateMachine.TransitState((int)StateID.Climb);
            }
            else if (player.onTheGround.GroundHit && player.inputInfo.down)
            {
                if (player.onTheGround.GroundHit.collider.gameObject.CompareTag("Ladder"))
                {
                    player.bodyLadder = player.onTheGround.GroundHit.collider;
                    player.stateMachine.TransitState((int)StateID.ClimbDown);
                }
            }


            if (player.inputInfo.jump)
            {
                player.stateMachine.TransitState((int)StateID.Jumping);
            }

            if (player.inputInfo.fire)
            {
                player.stateMachine.TransitState((int)StateID.IdleFire);
            }
        }
    }

    class Float : State<Player>
    {
        int animationHash = 0;
        public Float() { animationHash = Animator.StringToHash("Float"); }

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.onTheGround.Reset();
        }

        public override void FixedUpdate(Player player)
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
            if (player.onTheGround.CheckBottomHit())
            {
                player.stateMachine.TransitState((int)StateID.Idle);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                player.stateMachine.TransitState((int)StateID.Climb);
            }

        }

        public override void Update(Player player)
        {
            if (player.inputInfo.fire)
            {
                player.stateMachine.TransitState((int)StateID.FloatBuster);
            }
        }
    }

    class Run : State<Player>
    {
        int animationHash = 0;
        public Run() { animationHash = Animator.StringToHash("Run"); }

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }

        public override void FixedUpdate(Player player)
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
                player.stateMachine.TransitState((int)StateID.Float);
            }
        }

        public override void Update(Player player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.stateMachine.TransitState((int)StateID.Idle);
            }
            else if (player.inputInfo.jump)
            {
                player.stateMachine.TransitState((int)StateID.Jumping);
            }
            else if (player.inputInfo.fire)
            {
                player.stateMachine.TransitState((int)StateID.RunBuster);
            }

        }
    }

    class Jumping : State<Player>
    {
        int animationHash = 0;
        bool isJumping = true;
        public Jumping() { animationHash = Animator.StringToHash("Float"); }

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            if (preId != (int)StateID.JumpingBuster) player.jump.Init();
            player.onTheGround.Reset();
            isJumping = true;
        }

        public override void FixedUpdate(Player player)
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
                player.stateMachine.TransitState((int)StateID.Float);
            }
        }

        public override void Update(Player player)
        {
            if (player.inputInfo.fire)
            {
                player.stateMachine.TransitState((int)StateID.JumpingBuster);
            }


            if (isJumping && !player.inputInfo.jumping)
            {
                player.jump.SetSpeed(player.jump.CurrentSpeed / 2);
                isJumping = false;
            }
        }

        public override void Exit(Player player, int nextId)
        {
            if (nextId != (int)StateID.JumpingBuster) player.jump.SetSpeed(0); 
        }
    }

    class Climb : State<Player>
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
        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.animator.speed = 0;
            Vector2 pos = player.exRb.BoxColliderCenter;
            pos.x = player.bodyLadder.transform.position.x;
            player.exRb.SetPosition(pos);

            player.onTheGround.Reset();
            player.gravity.Reset();
        }

        public override void FixedUpdate(Player player)
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

        public override void Update(Player player)
        {

            if (player.bodyLadder == null)
            {
                player.stateMachine.TransitState((int)StateID.Idle);
                return;
            }

            if (player.onTheGround.CheckBottomHit())
            {
                player.stateMachine.TransitState((int)StateID.Idle);
                return;
            }

            player.isladderTop = player.transform.position.y > player.bodyLadder.bounds.max.y;
            if (player.isladderTop && player.inputInfo.up)
            {
                player.stateMachine.TransitState((int)StateID.ClimbUp);
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
                player.stateMachine.TransitState((int)StateID.Float);
            }
        }

        public override void Exit(Player player,int nextId)
        {
            player.animator.speed = 1;
        }
    }

    class ClimbUp : State<Player>
    {
        int animationHash = 0;
        public ClimbUp() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            time = 0;
        }
        public override void FixedUpdate(Player player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.exRb.SetPosition(ExpandRigidBody.PostionSetType.Bottom, player.bodyLadder.bounds.max.y);
                player.stateMachine.TransitState((int)StateID.Idle);
            }
        }
    }

    class ClimbDown : State<Player>
    {
        int animationHash = 0;
        public ClimbDown() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            Vector2 nextPos = player.exRb.BoxColliderCenter;
            nextPos.x = player.bodyLadder.transform.position.x;
            nextPos.y = player.bodyLadder.bounds.max.y;
            player.exRb.SetPosition(nextPos);
            time = 0;
        }
        public override void FixedUpdate(Player player)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                player.stateMachine.TransitState((int)StateID.Climb);
            }
        }
    }

    class IdleFire : State<Player>
    {

        int animationHash = 0;
        public IdleFire() { animationHash = Animator.StringToHash("Fire"); }

        float time = 0.15f;
        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.LaunchBaster();
            time = 0.15f;
        }


        public override void Update(Player player)
        {
            if (player.inputInfo.jump)
            {
                player.stateMachine.TransitState((int)StateID.Jumping);
            }

            if (time < 0)
            {
                player.stateMachine.TransitState((int)StateID.Idle);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.15f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }

        public override void FixedUpdate(Player player)
        {
            player.gravity.UpdateVelocity();
            player.exRb.velocity = player.gravity.CurrentVelocity;

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.stateMachine.TransitState((int)StateID.Float);
            }
        }

    }

    class RunBuster : State<Player>
    {
        int animationHash = 0;
        public RunBuster() { animationHash = Animator.StringToHash("RunBuster"); }
        float time = 0.3f;
        public override void Enter(Player player, int preId)
        {
            time = 0.3f;
            player.animator.Play(animationHash);

            player.LaunchBaster();
        }

        public override void FixedUpdate(Player player)
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
                player.stateMachine.TransitState((int)StateID.Float);
            }
        }

        public override void Update(Player player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.stateMachine.TransitState((int)StateID.Idle);
            }
            else if (player.inputInfo.jump)
            {
                player.stateMachine.TransitState((int)StateID.Jumping);
            }

            if (time < 0)
            {
                player.stateMachine.TransitState((int)StateID.Run);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.3f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }
    }

    class FloatBuster : State<Player>
    {
        int animationHash = 0;
        public FloatBuster() { animationHash = Animator.StringToHash("FloatBuster"); }
        float time = 0.15f;
        public override void Enter(Player player, int preId)
        {
            time = 0.15f;
            player.animator.Play(animationHash);

            if (preId != 10) player.LaunchBaster();
        }

        public override void FixedUpdate(Player player)
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
            if (player.onTheGround.CheckBottomHit())
            {
                player.stateMachine.TransitState((int)StateID.Idle);
            }
        }

        public override void Update(Player player)
        {
            if (time < 0)
            {
                player.stateMachine.TransitState((int)StateID.Float);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.15f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }
    }

    class JumpingBuster : State<Player>
    {
        int animationHash = 0;
        public JumpingBuster() { animationHash = Animator.StringToHash("FloatBuster"); }
        float time = 0.15f;

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
            player.LaunchBaster();
            time = 0.15f;

        }

        public override void FixedUpdate(Player player)
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
                player.stateMachine.TransitState((int)StateID.FloatBuster);
            }
        }

        public override void Update(Player player)
        {
            if (time < 0)
            {
                player.stateMachine.TransitState((int)StateID.Jumping);
            }
            else if (player.inputInfo.fire)
            {
                time = 0.15f;
                player.LaunchBaster();
            }
            time -= Time.deltaTime;
        }
    }

    class Death : State<Player>
    {
        public override void Enter(Player player, int preId)
        {
            player.gameObject.SetActive(false);
            EffectManager.Instance.PlayerDeathEffect.gameObject.transform.position = player.transform.position;
            EffectManager.Instance.PlayerDeathEffect.Play();
        }
    }

    class Transfer : State<Player>
    {
        int animationHash = 0;
        public Transfer() { animationHash = Animator.StringToHash("Transfer"); }

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }

        public override void FixedUpdate(Player player)
        {
            player.exRb.velocity = Vector2.down * 13;

            if (player.onTheGround.CheckBottomHit())
            {
                player.stateMachine.TransitState((int)StateID.Transfered);
            }
        }
    }

    class Transfered : State<Player>
    {
        int animationHash = 0;
        public Transfered() { animationHash = Animator.StringToHash("Transfered"); }

        public override void Enter(Player player, int preId)
        {
            player.animator.Play(animationHash);
        }
    }
}
