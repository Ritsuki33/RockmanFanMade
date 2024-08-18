using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftControll : MonoBehaviour
{

    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] bool roundTrip = false;
    [SerializeField] float oneWayTime = 3.0f;

    float currentTime = 0;
    bool isReturn = false;
    bool isGoal=false;

    public event Action onGoalEvent = null;

    Rigidbody2D rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (!isGoal)
        {
           
            if (oneWayTime > 0)
            {
                Vector3 startPos = (!isReturn) ? start.position : end.position;
                Vector3 endPos = (!isReturn) ? end.position : start.position;


                Vector2 newPos = Vector3.Lerp(startPos, endPos, currentTime / oneWayTime);

                rb.velocity = (newPos - (Vector2)this.transform.position) / Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            if (currentTime == oneWayTime)
            {
                Goal();
            }
        }

     
        currentTime += Time.fixedDeltaTime;

        if (currentTime > oneWayTime)
        {
            currentTime = oneWayTime;
        }
    }

    private void Goal()
    {
        if (roundTrip)
        {
            currentTime = 0;
            if (isReturn) onGoalEvent?.Invoke();
            isReturn = !isReturn;
        }
        else
        {
            isGoal = true;
            onGoalEvent?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start.position, end.position); 
    }
}
