using UnityEngine;

public class SpriteRotationBehavior : MonoBehaviour
{
    [SerializeField] Vector2 vector;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = vector;

        Vector2 direction = rb.velocity;
        if (direction.sqrMagnitude > 0.01f) // スプライトが動いている場合のみ回転を更新
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}