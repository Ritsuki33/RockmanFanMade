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
    private ExpandRigidBody exRb;

    [SerializeField] GameObject launcher;

    [SerializeField] BaseObjectPool RockBusterPool => EffectManager.Instance.RockBusterPool;

    bool IsRight => this.transform.localScale.x > 0;

    enum StateID
    {
        Idle=0,
        Float,
        Run,
        Climb,
        Jumping,
        ClimbUp,
        ClimbDown,
        IdleFire,
        RunBuster,
        FloatBuster,
        JumpingBuster,
        Death,
        Transfer,
        Transfered,
    }
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

        stateMachine.AddState((int)StateID.Idle, new Idle());
        stateMachine.AddState((int)StateID.Float, new Float());
        stateMachine.AddState((int)StateID.Run, new Run());
        stateMachine.AddState((int)StateID.Climb, new Climb());
        stateMachine.AddState((int)StateID.Jumping, new Jumping());
        stateMachine.AddState((int)StateID.ClimbUp, new ClimbUp());
        stateMachine.AddState((int)StateID.ClimbDown, new ClimbDown());
        stateMachine.AddState((int)StateID.IdleFire, new IdleFire());
        stateMachine.AddState((int)StateID.RunBuster, new RunBuster());
        stateMachine.AddState((int)StateID.FloatBuster, new FloatBuster());
        stateMachine.AddState((int)StateID.JumpingBuster, new JumpingBuster());
        stateMachine.AddState((int)StateID.Death, new Death());
        stateMachine.AddState((int)StateID.Transfer, new Transfer());
        stateMachine.AddState((int)StateID.Transfered, new Transfered());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Dead();
        }
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
        var rockBaster = RockBusterPool.Pool.Get();

        rockBaster.GetComponent<Projectile>().Init((IsRight) ? Vector2.right : Vector2.left, launcher.transform.position, 8);
    }

    public void Dead()
    {
        stateMachine.TransitState(11);
        GameManager.Instance.DeathNotification();
    }

    public void TransferedAnimationEnd()
    {
        stateMachine.TransitState((int)StateID.Idle);
    }

    public void Prepare(Transform tranform)
    {
        this.gameObject.SetActive(false);
        this.transform.position = tranform.position;
    }

    public void TransferPlayer()
    {
        this.gameObject.SetActive(true);
        stateMachine.TransitState((int)StateID.Transfer);
    }
}
