using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public partial class MettoruBehavior : MonoBehaviour
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

    //[SerializeField] MaterialController materialController;
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

    //private PlayerBehavior Player => WorldManager.Instance.PlayerController;
    //private ExplodePool ExplodePool => EffectManager.Instance.ExplodePool;
    //private ProjectilePool MettoruFire => EffectManager.Instance.MettoruFirePool;
    bool IsRight => mettrou.IsRight;

    Coroutine defense = null;


    int isFadeColorID = Shader.PropertyToID("_IsFadeColor");
//    private void Awake()
//    {
//        exRb = GetComponent<ExpandRigidBody>();

//        AddState((int)StateID.Idle, new Idle());
//        AddState((int)StateID.Walk, new Walk());
//        AddState((int)StateID.Hide, new Hide());
//        AddState((int)StateID.Hiding, new Hiding());
//        AddState((int)StateID.Appear, new Appear());
//        AddState((int)StateID.LookIn, new LookIn());
//        AddState((int)StateID.Jump, new Jumping());
//        AddState((int)StateID.JumpFloating, new JumpFloating());

//        TransitReady((int)StateID.Hide);
//    }


//    IEnumerator DefenseRockBuster(ProjectileReusable projectile)
//    {
//        Vector2 reflection = projectile.CurVelocity;
//        float speed = projectile.CurSpeed;
//        reflection.x *= -1;
//        reflection = new Vector2(reflection.x, 0).normalized;
//        reflection += Vector2.up;
//        reflection = reflection.normalized;
//        projectile.Init(
//            0,
//            null
//,
//            (rb) =>
//            {
//                rb.velocity = reflection * speed;
//            });
//        yield return new WaitForSeconds(1f);

//        defense = null;
//    }

//    public void Init()
//    {
//        TransitReady((int)StateID.Idle, true);

//        materialController.SetFloat(isFadeColorID, 0);
//    }

//    /// <summary>
//    /// 弾をうつ
//    /// </summary>
//    public void Fire()
//    {
//        var fire = MettoruFire.Pool.Get();

//        Vector2 direction= IsRight ? Vector2.right : Vector2.left;
//        float speed = 5;
//        fire.transform.position = this.transform.position;
//        fire.GetComponent<ProjectileReusable>().Init(
//            1,
//            null,
//            (rb) =>
//            {
//                rb.velocity = direction * speed;
//            });
//    }

//    public void Defense(RockBusterDamage collision)
//    {
//        if (collision.baseDamageValue==1)
//        {
//            ReflectBuster(collision);
//        }
//        else if (collision.baseDamageValue > 1)
//        {
//            var rockBuster = collision.gameObject.GetComponent<ProjectileReusable>();
//            rockBuster.Delete();
//        }
//    }


//    public void ReflectBuster(RockBusterDamage collision)
//    {
//        if (defense != null) StopCoroutine(defense);
//        defense = StartCoroutine(DefenseRockBuster(collision.projectile));
//    }

//    /// <summary>
//    /// 死亡
//    /// </summary>
//    /// <param name="collision"></param>
//    private void Damaged(RockBusterDamage damage) => mettrou.Damaged(damage);

//    /// <summary>
//    /// ターゲットに振り向き
//    /// </summary>
//    private void TurnToTarget(Vector2 targetPos) => mettrou.TurnToTarget(targetPos);

//    /// <summary>
//    /// 振り向き
//    /// </summary>
//    private void TurnFace() => mettrou.TurnFace();

}
