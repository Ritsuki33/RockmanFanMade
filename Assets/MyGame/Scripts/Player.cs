using UnityEngine;
using static Player;

public class Player : CharacterObject
{
    Gravity gravity;
    Move move;
    OnTheGround onTheGround;
    Animator animator;
    Jump jump;
    StateMachine<Player> stateMachine = new StateMachine<Player>();

    Collider2D bodyLadder = null;
    bool isladderTop = false;

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
            player.AddVelocity(player.gravity.GetVelocity());
            Vector2 moveV = player.move.GetVelocity(player.onTheGround.GroundHit.normal.Verticalize());

            var hitCheck = player.onTheGround.Check();
            if (!hitCheck)
            {
                player.stateMachine.TransitState(player, 1);
            }
        }

        public override void Update(Player player)
        {
            if (InputManager.Instance.GetInput(InputManager.InputType.Left))
            {
                player.stateMachine.TransitState(player, 2);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                player.stateMachine.TransitState(player, 2);
            }

            if (player.bodyLadder != null && Input.GetKey(KeyCode.UpArrow))
            {
                Vector2 pos = player.exRb.position;
                pos.y += 0.1f;
                player.exRb.SetPosition(pos);
                player.stateMachine.TransitState(player, 3);
            }
            else if (player.onTheGround.GroundHit && Input.GetKey(KeyCode.DownArrow))
            {
                if (player.onTheGround.GroundHit.collider.gameObject.CompareTag("Ladder"))
                {
                    player.bodyLadder = player.onTheGround.GroundHit.collider;
                    player.stateMachine.TransitState(player, 6);
                }
            }


            if (InputManager.Instance.GetInput(InputManager.InputType.Jump))
            {
                player.stateMachine.TransitState(player, 4);
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
            player.AddVelocity(player.gravity.GetVelocity());
            Vector2 moveV = player.move.GetVelocity(Vector2.right);
            player.AddVelocity(moveV);

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

            if (player.bodyLadder != null && Input.GetKey(KeyCode.UpArrow))
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
            Vector2 moveV = player.move.GetVelocity(player.onTheGround.GroundHit.normal.Verticalize());
            player.AddVelocity(moveV);

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
            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                player.stateMachine.TransitState(player, 0);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
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
            Vector2 moveV = player.move.GetVelocity(Vector2.right);
            player.AddVelocity(moveV);

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
            player.AddVelocity(jumpSpeed);

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

        enum InputType
        {
            Up,
            Down,
            None
        }
        InputType input = InputType.None;
        public override void Enter(int preId, Player player)
        {
            player.animator.Play(animationHash);
            player.animator.speed = 0;
            Vector2 pos= player.exRb.BoxColliderCenter;
            pos.x = player.bodyLadder.transform.position.x;
            player.exRb.SetPosition(pos);

            player.onTheGround.Reset();

        }

        public override void FixedUpdate(Player player)
        {
            switch (input)
            {
                case InputType.Up:
                player.AddVelocity(new Vector2(0, 5));
                    break;
                case InputType.Down:
                player.AddVelocity(new Vector2(0, -5));
                    break;
                case InputType.None:
                    break;
                default:
                    break;
            }

       
        }

        public override void Update(Player player)
        {

            if (player.bodyLadder==null)
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
            if (player.isladderTop && Input.GetKey(KeyCode.UpArrow))
            {
                player.stateMachine.TransitState(player, 5);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                player.animator.speed = 1;
                input= InputType.Down;

            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                player.animator.speed = 1;
                input = InputType.Up;
            }
            else
            {
                input= InputType.None;
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
    protected override void OnAwake()
    {
        gravity = GetComponent<Gravity>();
        move = GetComponent<Move>();
        onTheGround = GetComponent<OnTheGround>();
        animator = GetComponent<Animator>();
        jump=GetComponent<Jump>();
        AddOnHitEventCallback(gravity);
        AddOnHitEventCallback(move);
        AddOnHitEventCallback(jump);
        AddOnHitEventCallback(onTheGround);

        stateMachine.AddState(0, new Idle());
        stateMachine.AddState(1, new Float());
        stateMachine.AddState(2, new Run());
        stateMachine.AddState(3, new Climb());
        stateMachine.AddState(4, new Jumping());
        stateMachine.AddState(5, new ClimbUp());
        stateMachine.AddState(6, new ClimbDown());
        stateMachine.TransitState(this, 1);
    }

    protected override void OnFixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    private void Update()
    {
        stateMachine.Update(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            
            bodyLadder = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            bodyLadder = null;
        }
    }
}
