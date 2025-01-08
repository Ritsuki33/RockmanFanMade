using System;
using UnityEngine;
using UnityEngine.Pool;

public enum ExplodeType
{
    Explode1,
    Explode2,
}

public class Explode : AnimObject, IPooledObject<Explode>
{
    [SerializeField] DamageBase damage = default;

    BoxCollider2D boxCollider;
    IObjectPool<Explode> pool = null;

    IObjectPool<Explode> IPooledObject<Explode>.Pool { get => pool; set => pool = value; }

    Action<Explode> _finishCallback = default;
    public enum Layer
    {
        PlayerAttack = 19,
        EnemyAttack = 20,
        None = 21,
    }

    protected override void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void OnUpdate()
    {
        if (!MainAnimator.IsPlayingCurrentAnimation())
        {
            _finishCallback?.Invoke(this);
            pool.Release(this);
            WorldManager.Instance.RemoveObject(this);
        }
    }
    void IPooledObject<Explode>.OnGet()
    {
        boxCollider.enabled = false;
    }

    public void Setup(Layer layer, int damageVal, Action<Explode> finishCallback)
    {
        boxCollider.enabled = true;

        gameObject.layer = (int)layer;
        damage.baseDamageValue = damageVal;
    }
}
