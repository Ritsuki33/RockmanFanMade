using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public partial class MettoruController : ExRbStateMachine<MettoruController>
{
    [SerializeField] Mettoru mettrou;
    [SerializeField] Animator _animator;
    [SerializeField] Gravity gravity;
    [SerializeField] OnTheGround onTheGround;
    [SerializeField] Move move;
    [SerializeField] RaycastSensor raycastSensor;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] JumpOverThere jumpOverThere;
    [SerializeField] Jump jump;
    [SerializeField] bool walk = false;

    [SerializeField] Transform jumpTarget;
    [SerializeField] MaterialController materialController;
    private ExpandRigidBody exRb;

    enum StateID
    {
        Idle = 0,
        Walk,
        Hide,
        Hiding,
        Appear,
        LookIn,
        Jump,
        JumpFloating,
    }

    //private bool invincible = false;

    private PlayerController Player => GameManager.Instance.Player;
    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;
    private BaseObjectPool MettoruFire => EffectManager.Instance.MettoruFirePool;
    bool IsRight => this.transform.localScale.x < 0;

    Coroutine defense = null;


    int isFadeColorID = Shader.PropertyToID("_IsFadeColor");
    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();

        AddState((int)StateID.Idle, new Idle());
        AddState((int)StateID.Walk, new Walk());
        AddState((int)StateID.Hide, new Hide());
        AddState((int)StateID.Hiding, new Hiding());
        AddState((int)StateID.Appear, new Appear());
        AddState((int)StateID.LookIn, new LookIn());
        AddState((int)StateID.Jump, new Jumping());
        AddState((int)StateID.JumpFloating, new JumpFloating());

        TransitReady((int)StateID.Hide);
    }


    IEnumerator DefenseRockBuster(Projectile projectile)
    {
        Vector2 reflection = projectile.CurVelocity;
        float speed = projectile.CurSpeed;
        reflection.x *= -1;
        reflection = new Vector2(reflection.x, 0).normalized;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        projectile.Init(
            0,
            (rb) =>
            {
                rb.velocity = reflection * speed;
            }
            );
        yield return new WaitForSeconds(1f);

        defense = null;
    }

    public void Init()
    {
        TransitReady((int)StateID.Idle, true);

        materialController.SetFloat(isFadeColorID, 0);
    }

    /// <summary>
    /// 弾をうつ
    /// </summary>
    public void Fire()
    {
        var fire = MettoruFire.Pool.Get();

        Vector2 direction= IsRight ? Vector2.right : Vector2.left;
        float speed = 5;
        fire.transform.position = this.transform.position;
        fire.GetComponent<Projectile>().Init(1,
            (rb) =>
            {
                rb.velocity = direction * speed;
            });
    }

    public void Defense(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster"))
        {
            ReflectBuster(collision);
        }
        else if (collision.gameObject.CompareTag("ChargeShot"))
        {
            var rockBuster = collision.gameObject.GetComponent<Projectile>();
            rockBuster.Delete();
        }
    }


    public void ReflectBuster(Collider2D collision)
    {
        var rockBuster = collision.gameObject.GetComponent<Projectile>();

        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(rockBuster));
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    private void Attacked(Collider2D collision) => mettrou.Attacked(collision);

    /// <summary>
    /// ターゲットに振り向き
    /// </summary>
    private void TurnToTarget(Vector2 targetPos) => mettrou.TurnToTarget(targetPos);

    /// <summary>
    /// 振り向き
    /// </summary>
    private void TurnFace() => mettrou.TurnFace();

}
