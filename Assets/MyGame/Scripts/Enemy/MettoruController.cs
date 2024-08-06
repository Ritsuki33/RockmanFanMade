using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class MettoruController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Gravity gravity;
    [SerializeField] OnTheGround onTheGround;
    [SerializeField] Move move;
    private ExpandRigidBody exRb;
    StateMachine<MettoruController> stateMachine;

    enum StateID
    {
        Idle = 0,
        Hide,
        Appear,
    }

    private bool invincible = false;

    private Player Player => GameManager.Instance.Player;
    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;
    bool IsRight => this.transform.localScale.x > 0;

    bool defense = false;
    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        stateMachine = new StateMachine<MettoruController>();

        stateMachine.AddState((int)StateID.Idle, new Idle());
        stateMachine.AddState((int)StateID.Hide, new Hide());
        stateMachine.AddState((int)StateID.Appear, new Appear());

        stateMachine.TransitState((int)StateID.Idle);
    }


    private void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    private void Update()
    {
        stateMachine.Update(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster"))
        {
            if (invincible)
            {
                StartCoroutine(DefenseRockBuster());
            }
            else
            {
                Dead(collision);
            }
        }
    }

    IEnumerator DefenseRockBuster()
    {
        Debug.Log("Defense");
        defense = true;

        yield return new WaitForSeconds(2.0f);

        defense = false;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    private void Dead(Collider2D collision)
    {
        Debug.Log("Dead");

        var rockBuster = collision.gameObject.GetComponent<Projectile>();
        rockBuster?.Delete();

        var explode = ExplodePool.Pool.Get();
        explode.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラクターの振り向き
    /// </summary>
    private void TurnFace()
    {
        if (transform.position.x > Player.transform.position.x)
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
}
