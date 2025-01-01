using UnityEngine;

public class ExplodeController : Reusable
{
    [SerializeField] DamageBase damage = default;
    Animator animator;

    BoxCollider2D boxCollider;

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

    protected override void OnGet()
    {
        boxCollider.enabled = false;
    }

    public void Init(Layer layer,int damageVal)
    {
        boxCollider.enabled = true;

        gameObject.layer = (int)layer;
        damage.baseDamageValue = damageVal;
    }


    private void Update()
    {
        if (!animator.IsPlayingCurrentAnimation())
        {
            Pool.Release(this);
        }
    }
}
