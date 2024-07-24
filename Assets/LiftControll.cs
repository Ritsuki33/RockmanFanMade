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

    private void FixedUpdate()
    {
        if (!isGoal)
        {
            Vector3 startPos = (!isReturn) ? start.position : end.position;
            Vector3 endPos = (!isReturn) ? end.position : start.position;
            transform.position = Vector3.Lerp(startPos, endPos, currentTime / oneWayTime);

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
