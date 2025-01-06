using UnityEngine;
using UnityEngine.Pool;

public class Explode : AnimObject, IPooledObject<Explode>
{
    [SerializeField] DamageBase damage = default;
    Animator animator;

    BoxCollider2D boxCollider;
    IObjectPool<Explode> pool = null;

    IObjectPool<Explode> IPooledObject<Explode>.Pool { get => pool; set => pool = value; }

    public enum Layer
    {
        PlayerAttack = 19,
        EnemyAttack = 20,
        None = 21,
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void OnUpdate()
    {
        if (!animator.IsPlayingCurrentAnimation())
        {
            pool.Release(this);
        }
    }
    void IPooledObject<Explode>.OnGet()
    {
        boxCollider.enabled = false;
    }

    public void Init(Layer layer, int damageVal)
    {
        boxCollider.enabled = true;

        gameObject.layer = (int)layer;
        damage.baseDamageValue = damageVal;
    }
}
