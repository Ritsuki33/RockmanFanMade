using UnityEngine;

public class Gravity : MonoBehaviour, IHitEvent
{
    [SerializeField] float speed = 1;
    [SerializeField] float masSpeed = 10;

    [SerializeField] float canStopSlope = 45;
    float currentSpeed = default;

    public Vector2 CurrentVelocity => Vector2.down * currentSpeed;

    Rigidbody2D rb = default;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //private void OnEnable()
    //{
    //    VelocityControll player = GetComponent<VelocityControll>();
    //    if (player)
    //    {
    //        player.onHitBottomStay += OnBottomHit;
    //    }
    //}

    //private void OnDisable()
    //{
    //    VelocityControll player = GetComponent<VelocityControll>();
    //    if (player) player.onHitBottomStay -= OnBottomHit;
    //}

    public Vector2 GetVelocity()
    {
        currentSpeed += speed;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, masSpeed);

        return Vector2.down * currentSpeed;
    }

    public void OnBottomHitStay(RaycastHit2D hit)
    {
        float angle = Vector2.Angle(Vector2.up, hit.normal);
        if (angle < canStopSlope)
        {
            currentSpeed = 0;
        }
    }

}
