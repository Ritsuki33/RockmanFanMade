using System.Collections;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    Gravity gravity;
    Move move;
    OnTheGround onTheGround;
    Animator animator;
    Jump jump;
    StateMachine<Player> stateMachine = new StateMachine<Player>();

    Collider2D bodyLadder = null;
    bool isladderTop = false;

    InputInfo inputInfo;
    bool isUpdate = true;
    protected ExpandRigidBody exRb;

    [SerializeField] GameObject launcher;
    [SerializeField] RockBuster rockBasterPrefab;

    bool IsRight => this.transform.localScale.x > 0;
    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        gravity = GetComponent<Gravity>();
        move = GetComponent<Move>();
        onTheGround = GetComponent<OnTheGround>();
        animator = GetComponent<Animator>();
        jump=GetComponent<Jump>();
        exRb.AddOnHitEventCallback(gravity);
        exRb.AddOnHitEventCallback(move);
        exRb.AddOnHitEventCallback(jump);
        exRb.AddOnHitEventCallback(onTheGround);

        stateMachine.AddState(0, new Idle());
        stateMachine.AddState(1, new Float());
        stateMachine.AddState(2, new Run());
        stateMachine.AddState(3, new Climb());
        stateMachine.AddState(4, new Jumping());
        stateMachine.AddState(5, new ClimbUp());
        stateMachine.AddState(6, new ClimbDown());
        stateMachine.AddState(7, new Fire());
        stateMachine.TransitState(this, 1);
    }

    private void FixedUpdate()
    {
        if(isUpdate) stateMachine.FixedUpdate(this);
    }

    private void Update()
    {
        if(isUpdate)stateMachine.Update(this);
    }

    public void UpdateInput(InputInfo input)
    {
        inputInfo = input;
    }

    /// <summary>
    /// プレイヤーのポーズ
    /// </summary>
    public void PlayerPause()
    {
        isUpdate = false;
    }

    /// <summary>
    /// プレイヤーのポーズキャンセル（一つ前の状態に戻す）
    /// </summary>
    public void PlayerPuaseCancel()
    {
        isUpdate = true;
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

    public void LaunchBaster()
    {
        var rockBaster = Instantiate<RockBuster>(rockBasterPrefab);
        rockBaster.Init((IsRight) ? Vector2.right : Vector2.left, launcher.transform.position);
    }
}
