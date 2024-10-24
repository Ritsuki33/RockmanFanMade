using System;
using System.Collections;
using UnityEngine;

public partial class PlayerController : ExRbStateMachine<PlayerController>
{
    [SerializeField] Player player;
    [SerializeField] LauncherController launcherController;
    [SerializeField] SpriteRenderer spriteRenderer;

    Gravity gravity;
    Move move;
    OnTheGround onTheGround;
    Animator animator;
    Jump jump;

    Collider2D bodyLadder = null;

    BoxCollider2D boxPhysicalCollider = null;
    bool isladderTop = false;

    GameMainManager.InputInfo inputInfo;

    private ExpandRigidBody exRb;

    Transform bamili = null;
    public bool IsRight => player.IsRight;

    Action actionFinishCallback = null;

    AmbiguousTimer timer=new AmbiguousTimer();

    bool invincible = false;

    public float CurrentHp => player.CurrentHp / player.MaxHp;
    enum StateID
    {
        Standing=0,
        Floating,
        Running,
        Climb,
        Jumping,
        ClimbUp,
        ClimbDown,
        Death,
        Transfer,
        Transfered,
        AutoMove,
        Repatriate,
        Damaged,
    }

    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        gravity = GetComponent<Gravity>();
        move = GetComponent<Move>();
        onTheGround = GetComponent<OnTheGround>();
        animator = GetComponent<Animator>();
        jump=GetComponent<Jump>();
        boxPhysicalCollider=GetComponent<BoxCollider2D>();

        AddState((int)StateID.Standing, new Standing());
        AddState((int)StateID.Floating, new Floating());
        AddState((int)StateID.Running, new Running());
        AddState((int)StateID.Climb, new Climb());
        AddState((int)StateID.Jumping, new Jumping());
        AddState((int)StateID.ClimbUp, new ClimbUp());
        AddState((int)StateID.ClimbDown, new ClimbDown());
        AddState((int)StateID.Death, new Death());
        AddState((int)StateID.Transfer, new Transfer());
        AddState((int)StateID.Transfered, new Transfered());
        AddState((int)StateID.AutoMove, new AutoMove());
        AddState((int)StateID.Repatriate, new Repatriation());
        AddState((int)StateID.Damaged, new DamagedState());
    }

    public void UpdateInput(GameMainManager.InputInfo input)
    {
        inputInfo = input;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("DeadZone"))
    //    {
    //        Dead();
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ladder"))
    //    {
            
    //        bodyLadder = collision;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ladder"))
    //    {
    //        bodyLadder = null;
    //    }
    //}

    public void Dead()
    {
        TransitReady((int)StateID.Death);
        GameMainManager.Instance.DeathNotification();
    }

    public void TransferedAnimationEnd()
    {
        TransitReady((int)StateID.Standing);
    }

    public void Prepare(Transform tranform)
    {
        this.gameObject.SetActive(false);
        this.transform.position = tranform.position;
    }

    public void TransferPlayer(Action actionFinishCallback = null)
    {
        this.gameObject.SetActive(true);
        this.actionFinishCallback = actionFinishCallback;
        TransitReady((int)StateID.Transfer);
    }

    public void RepatriatePlayer(Action actionFinishCallback = null)
    {
        this.actionFinishCallback = actionFinishCallback;
        TransitReady((int)StateID.Repatriate);
    }

    public void AutoMoveTowards(Transform bamili,Action actionFinishCallback)
    {
        this.bamili = bamili;
        this.actionFinishCallback = actionFinishCallback;
        this.TransitReady((int)StateID.AutoMove, true);
    }

    /// <summary>
    /// 入力の禁止
    /// </summary>
    /// <param name="actionFinishCallback"></param>
    public void InputProhibit(Action actionFinishCallback)
    {
        this.TransitReady((int)StateID.AutoMove, (int)AutoMove.SubStateId.Wait);
    }

    /// <summary>
    /// 入力を許可
    /// </summary>
    public void InputPermission()
    {
        this.TransitReady((int)StateID.Standing);
    }

    public void ActionFinishNotify()
    {
        actionFinishCallback?.Invoke();
        actionFinishCallback = null;
    }

    /// <summary>
    /// カメラのブレンディングによる強制移動
    /// </summary>
    /// <param name="actionFinishCallback"></param>
    public void PlayerForceMoveAccordingToCamera(Action actionFinishCallback)
    {
        StartCoroutine(PlayerForceMoveAccordingToCameraCo());
        actionFinishCallback?.Invoke();

        IEnumerator PlayerForceMoveAccordingToCameraCo()
        {
            this.enabled = false;

            while (!enabled)
            {
                this.transform.position += GameMainManager.Instance.MainCameraControll.DeltaMove * 0.08f;
                yield return null;
            }
        }
    }

    /// <summary>
    /// カメラのブレンディングによる強制移動 終了
    /// </summary>
    /// <param name="actionFinishCallback"></param>
    public void PlayerForceMoveAccordingToCameraEnd(Action actionFinishCallback)
    {
        this.enabled = true;
        actionFinishCallback?.Invoke();
    }


    public void SetHp(int hp) => player.SetHp(hp);

    public void Damaged(int val)
    {
        SetHp(player.CurrentHp - val);

        if (player.CurrentHp <= 0)
        {
            Dead();
        }
        else
        {
            TransitReady((int)StateID.Damaged);
        }
    }

    public void InvincibleState()
    {
        StartCoroutine(InvincibleStateCo());

        IEnumerator InvincibleStateCo()
        {
            invincible = true;
            int count = 5;

            for (int i = 0; i < count; i++)
            {
                spriteRenderer.enabled = false;

                yield return new WaitForSeconds(0.05f);

                spriteRenderer.enabled = true;

                yield return new WaitForSeconds(0.05f);
            }

            invincible = false;

            yield return null;
        }
    }
}
