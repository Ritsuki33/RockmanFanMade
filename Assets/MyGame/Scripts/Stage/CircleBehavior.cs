using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float radius;
    [SerializeField] float oneWayTime;
    [SerializeField] bool loop;
    [SerializeField, Range(0, 1)] float startProgress = 0;
    [SerializeField] bool isReverse;
    [SerializeField] bool isStop = false;

    Rigidbody2D rb = null;

    float time = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        time = 0;
    }

    private void FixedUpdate()
    {
        if (isStop)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        time += ((!isReverse) ? 1 : -1) * Time.fixedDeltaTime;
        time %= oneWayTime;
        float progress = Mathf.PI * 2 * time / oneWayTime;
        rb.SetVelocty(CircleBehaviorHelper.GetStrobe(center.position, radius, progress));

        if (time == oneWayTime)
        {
            if (loop)
            {
                time = 0;
            }
        }
    }

    /// <summary>
    /// 逆移動を開始させる
    /// </summary>
    public void Reverse()
    {
        isReverse = !isReverse;
    }
}
