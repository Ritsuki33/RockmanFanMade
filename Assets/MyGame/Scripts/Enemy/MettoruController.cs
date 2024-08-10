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
    [SerializeField] RaycastSensor raycastSensor;

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
    private BaseObjectPool MettoruFire => EffectManager.Instance.MettoruFirePool;
    bool IsRight => this.transform.localScale.x < 0;

    Coroutine defense = null;
    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        stateMachine = new StateMachine<MettoruController>();

        stateMachine.AddState((int)StateID.Idle, new Idle());
        stateMachine.AddState((int)StateID.Hide, new Hide());
        stateMachine.AddState((int)StateID.Appear, new Appear());

        stateMachine.TransitState((int)StateID.Hide);
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
    /// íeÇÇ§Ç¬
    /// </summary>
    public void Fire()
    {
        var fire=MettoruFire.Pool.Get();

        fire.GetComponent<Projectile>().Init((IsRight) ? Vector2.right : Vector2.left, this.transform.position, 5);
    }

    /// <summary>
    /// éÄñS
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
    /// ÉLÉÉÉâÉNÉ^Å[ÇÃêUÇËå¸Ç´
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
