using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class MettoruController : ExRbStateMachine<MettoruController>
{
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
        projectile.DisableDamageDetection();
        Vector2 reflection = projectile.Direction;
        reflection.x *= -1;
        reflection += Vector2.up;
        reflection = reflection.normalized;
        projectile.ChangeDirection(reflection);
        yield return new WaitForSeconds(1f);

        defense = null;
    }

    /// <summary>
    /// 弾をうつ
    /// </summary>
    public void Fire()
    {
        var fire=MettoruFire.Pool.Get();

        fire.GetComponent<Projectile>().Init((IsRight) ? Vector2.right : Vector2.left, this.transform.position, 5);
    }

    public void ReflectBuster(Collider2D collision)
    {
        var rockBuster = collision.gameObject.GetComponent<Projectile>();

        if (defense != null) StopCoroutine(defense);
        defense = StartCoroutine(DefenseRockBuster(rockBuster));
    }

    public void Damaged(Collider2D collision)
    {
        StartCoroutine(DamagedCo(collision));

        IEnumerator DamagedCo(Collider2D collision)
        {
            var projectile = collision.gameObject.GetComponent<Projectile>();

            projectile?.Delete();
            int count = 5;

            for (int i = 0; i < count; i++)
            {
                materialController.SetFloat(isFadeColorID, 1);

                yield return new WaitForSeconds(0.05f);

                materialController.SetFloat(isFadeColorID, 0);

                yield return new WaitForSeconds(0.05f);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    private void Dead(Collider2D collision)
    {
        var projectile = collision.gameObject.GetComponent<Projectile>();

        projectile?.Delete();

        var explode = ExplodePool.Pool.Get();
        explode.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラクターの振り向き
    /// </summary>
    private void TurnToTarget(Vector2 targetPos)
    {
        if (transform.position.x > targetPos.x)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1;
            transform.localScale = localScale;
        }
        else
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1;
            transform.localScale = localScale;
        }
    }

    private void TurnFace()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
