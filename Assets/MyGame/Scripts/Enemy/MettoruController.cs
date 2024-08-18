using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class MettoruController : StateMachine<MettoruController>
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

    private bool invincible = false;

    private Player Player => GameManager.Instance.Player;
    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;
    private BaseObjectPool MettoruFire => EffectManager.Instance.MettoruFirePool;
    bool IsRight => this.transform.localScale.x < 0;

    Coroutine defense = null;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster"))
        {
            var rockBuster = collision.gameObject.GetComponent<Projectile>();

            if (invincible)
            {
                if (defense != null) StopCoroutine(defense);
                defense=StartCoroutine(DefenseRockBuster(rockBuster));
            }
            else
            {
                Dead(rockBuster);
            }
        }
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

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    private void Dead(Projectile projectile)
    {
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
