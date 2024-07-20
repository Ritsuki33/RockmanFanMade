using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IHitEvent
{
    [SerializeField] float speed = 15;
    [SerializeField] float decrease = 1;

    float currentSpeed = 0;

    public void Init()
    {
        this.currentSpeed = speed;
    }

    public Vector2 GetVelocity()
    {
        currentSpeed -= decrease;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);

        return Vector2.up * currentSpeed;
    }


    public void OnBottomHitStay(RaycastHit2D hit)
    {
        currentSpeed = 0;
    }
}
