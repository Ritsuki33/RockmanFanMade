using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : StateMachine<Player>
{
    Gravity gravity;
    Move move;
    OnTheGround onTheGround;
    Animator animator;
    Jump jump;
    //StateMachine<Player> stateMachine = new StateMachine<Player>();

    Collider2D bodyLadder = null;
    bool isladderTop = false;

    InputInfo inputInfo;

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

        AddState((int)StateID.Idle, new Idle());
        AddState((int)StateID.Float, new Float());
        AddState((int)StateID.Run, new Run());
        AddState((int)StateID.Climb, new Climb());
        AddState((int)StateID.Jumping, new Jumping());
        AddState((int)StateID.ClimbUp, new ClimbUp());
        AddState((int)StateID.ClimbDown, new ClimbDown());
        AddState((int)StateID.IdleFire, new IdleFire());
        AddState((int)StateID.RunBuster, new RunBuster());
        AddState((int)StateID.FloatBuster, new FloatBuster());
        AddState((int)StateID.JumpingBuster, new JumpingBuster());
        AddState((int)StateID.Death, new Death());
        AddState((int)StateID.Transfer, new Transfer());
        AddState((int)StateID.Transfered, new Transfered());
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
        this.enabled = false;
    }

    /// <summary>
    /// プレイヤーのポーズキャンセル（一つ前の状態に戻す）
    /// </summary>
    public void PlayerPuaseCancel()
    {
        this.enabled = true;
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
        TransitReady(11);
        GameManager.Instance.DeathNotification();
    }

    public void TransferedAnimationEnd()
    {
        TransitReady((int)StateID.Idle);
    }

    public void Prepare(Transform tranform)
    {
        this.gameObject.SetActive(false);
        this.transform.position = tranform.position;
    }

    public void TransferPlayer()
    {
        this.gameObject.SetActive(true);
        TransitReady((int)StateID.Transfer);
    }
}
