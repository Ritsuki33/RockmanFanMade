using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : StageObject
{
    [SerializeField] EnemyData enemyData = default;

    private BaseObjectPool ExplodePool => EffectManager.Instance.ExplodePool;

    bool IsRight => this.transform.localScale.x < 0;
    private Material material;

    int hp = 0;
    public virtual void Init() {
        hp = (enemyData != null) ? enemyData.Hp : 3;
    }

    public void Attacked(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster") || collision.gameObject.CompareTag("ChargeShot"))
        {
            var projectile = collision.gameObject.GetComponent<Projectile>();
            hp -= projectile.AttackPower;
            if (hp==0)
            {
                Dead(projectile);
            }
            else
            {
                Damaged(projectile);
            }
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="collision"></param>
    public void Dead(Projectile projectile)
    {

        projectile?.Delete();

        var explode = ExplodePool.Pool.Get();
        explode.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラクターの振り向き
    /// </summary>
    public  void TurnToTarget(Vector2 targetPos)
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

    /// <summary>
    /// 振り返る
    /// </summary>
    public void TurnFace()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /// <summary>
    /// ダメージ演出
    /// </summary>
    /// <param name="collision"></param>
    public virtual void Damaged(Projectile projectile)
    {
        StartCoroutine(DamagedCo(projectile));

        IEnumerator DamagedCo(Projectile projectile)
        {
            projectile?.Delete();
            int count = 5;

            for (int i = 0; i < count; i++)
            {
                SetMaterialParam(ShaderPropertyId.IsFadeColorID, 1);

                yield return new WaitForSeconds(0.05f);

                SetMaterialParam(ShaderPropertyId.IsFadeColorID, 0);

                yield return new WaitForSeconds(0.05f);
            }
            yield return null;
        }
    }
}
