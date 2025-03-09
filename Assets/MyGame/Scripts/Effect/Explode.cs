using System;
using UnityEngine;
using UnityEngine.Pool;

public class Explode : AnimObject
{
    [SerializeField] DamageBase damage = default;

    [SerializeField] BoxCollider2D boxCollider;

    public enum Layer
    {
        PlayerAttack = 19,
        EnemyAttack = 20,
        None = 21,
    }

    protected override void Init()
    {
        AudioManager.Instance.PlaySe(SECueIDs.explosion);
    }

    protected override void OnUpdate()
    {
        if (!MainAnimator.IsPlayingCurrentAnimation())
        {
            Delete();
        }
    }

    public void Setup(Layer layer, Vector3 position, int damageVal)
    {
        boxCollider.enabled = true;

        gameObject.layer = (int)layer;
        boxCollider.gameObject.layer = (int)layer;
        this.transform.position = position;
        damage.baseDamageValue = damageVal;

        //_finishCallback = finishCallback;
    }

    protected override void OnReset()
    {
        Delete();
    }
}
