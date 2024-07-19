using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    class Idle : State<Player>
    {
        int animationHash = 0;
        public Idle() { animationHash = Animator.StringToHash("Idle"); }

        public override void Enter(int preId, Player player)
        {
            player.animator.Play(animationHash);
        }

        public override void FixedUpdate(Player player)
        {
            player.exRb.velocity = player.gravity.GetVelocity();

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.stateMachine.TransitState(player, 1);
            }
        }

        public override void Update(Player player)
        {
            if (player.inputInfo.left)
            {
                player.stateMachine.TransitState(player, 2);
            }
            else if (player.inputInfo.right)
            {
                player.stateMachine.TransitState(player, 2);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                Vector2 pos = player.exRb.position;
                pos.y += 0.1f;
                player.exRb.SetPosition(pos);
                player.stateMachine.TransitState(player, 3);
            }
            else if (player.onTheGround.GroundHit && player.inputInfo.down)
            {
                if (player.onTheGround.GroundHit.collider.gameObject.CompareTag("Ladder"))
                {
                    player.bodyLadder = player.onTheGround.GroundHit.collider;
                    player.stateMachine.TransitState(player, 6);
                }
            }


            if (player.inputInfo.jump)
            {
                player.stateMachine.TransitState(player, 4);
            }

            if (player.inputInfo.fire)
            {
                player.stateMachine.TransitState(player, 7);
            }
        }
    }

    class Float : State<Player>
    {
        int animationHash = 0;
        public Float() { animationHash = Animator.StringToHash("Float"); }

        public override void Enter(int preId, Player player)
        {
            player.animator.Play(animationHash);
            player.onTheGround.Reset();
        }

        public override void FixedUpdate(Player player)
        {
            player.exRb.velocity = player.gravity.GetVelocity();
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;

            Vector2 moveV = player.move.GetVelocity(Vector2.right, type);
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
                player.stateMachine.TransitState(player, 0);
            }

            if (player.bodyLadder != null && player.inputInfo.up)
            {
                player.stateMachine.TransitState(player, 3);
            }

        }
    }

    class Run : State<Player>
    {
        int animationHash = 0;
        public Run() { animationHash = Animator.StringToHash("Run"); }

        public override void Enter(int preId, Player player)
        {
            player.animator.Play(animationHash);
        }

        public override void FixedUpdate(Player player)
        {
            //player.AddVelocity(player.gravity.GetVelocity());
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;

            Vector2 moveV = player.move.GetVelocity(player.onTheGround.GroundHit.normal.Verticalize(), type);
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
                player.stateMachine.TransitState(player, 1);
            }


        }

        public override void Update(Player player)
        {
            if (!player.inputInfo.left && !player.inputInfo.right)
            {
                player.stateMachine.TransitState(player, 0);
            }
            else if (player.inputInfo.jump)
            {
                player.stateMachine.TransitState(player, 4);
            }
        }
    }

    public class Jumping : State<Player>
    {
        int animationHash = 0;
        public Jumping() { animationHash = Animator.StringToHash("Float"); }

        public override void Enter(int preId, Player player)
        {
            player.animator.Play(animationHash);
            player.jump.Init();
        }

        public override void FixedUpdate(Player player)
        {
            Move.InputType type = default;
            if (player.inputInfo.left == true) type = Move.InputType.Left;
            else if (player.inputInfo.right == true) type = Move.InputType.Right;

            Vector2 moveV = player.move.GetVelocity(Vector2.right, type);
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

            Vector2 jumpSpeed = player.jump.GetVelocity();
            player.exRb.velocity += jumpSpeed;

            if (jumpSpeed.sqrMagnitude <= 0.001f)
            {
                player.stateMachine.TransitState(player, 1);
            }
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
        public override void Enter(int preId, Player player)
        {
            player.animator.Play(animationHash);
            player.animator.speed = 0;
            Vector2 pos = player.exRb.BoxColliderCenter;
            pos.x = player.bodyLadder.transform.position.x;
            player.exRb.SetPosition(pos);

            player.onTheGround.Reset();

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
                player.stateMachine.TransitState(player, 0);
                return;
            }

            if (player.onTheGround.CheckBottomHit())
            {
                player.stateMachine.TransitState(player, 0);
                return;
            }

            player.isladderTop = player.transform.position.y > player.bodyLadder.bounds.max.y;
            if (player.isladderTop && player.inputInfo.up)
            {
                player.stateMachine.TransitState(player, 5);
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
        }

        public override void Exit(Player player)
        {
            player.animator.speed = 1;
        }
    }

    class ClimbUp : State<Player>
    {
        int animationHash = 0;
        public ClimbUp() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        public override void Enter(int preId, Player player)
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
                player.stateMachine.TransitState(player, 0);
            }
        }
    }

    class ClimbDown : State<Player>
    {
        int animationHash = 0;
        public ClimbDown() { animationHash = Animator.StringToHash("ClimbUp"); }

        float time = 0;
        public override void Enter(int preId, Player player)
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
                player.stateMachine.TransitState(player, 3);
            }
        }
    }

    class Fire : State<Player>
    {

        int animationHash = 0;
        public Fire() : base(false) { animationHash = Animator.StringToHash("Fire"); }

        public override IEnumerator EnterCoroutine(Player player)
        {
            player.animator.Play(animationHash);

            player.LaunchBaster();
            yield return new WaitForSeconds(0.15f);

            player.stateMachine.TransitState(player, 0);
        }

    }
}
