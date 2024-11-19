using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBehavior : MonoBehaviour
{

    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] bool roundTrip = false;
    [SerializeField] float oneWayTime = 3.0f;
    [SerializeField, Range(0, 1)] float startProgress = 0;
    [SerializeField] bool isStop = false;

    float currentTime = 0;
    bool isReturn = false;
    bool isGoal = false;

    public event Action onGoalEvent = null;

    Rigidbody2D rb = null;

    public bool IsStop => isStop;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Init();
        StartMove();
    }

    private void FixedUpdate()
    {

        if (isStop)
        {
            rb.velocity = Vector2.zero;
            return;
        }

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

    public void Init()
    {
        currentTime = startProgress * oneWayTime;
        isReturn = false;
    }

    /// <summary>
    /// 開始地点を戻して再開
    /// </summary>
    public void StartMove()
    {
        isStop = false;
    }

    public void StopMove()
    {
        isStop = true;
    }
    

    /// <summary>
    /// 逆移動を開始させる
    /// </summary>
    public void Reverse()
    {
        isReturn = !isReturn;
        currentTime = oneWayTime - currentTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (start && end) Gizmos.DrawLine(start.position, end.position); 
    }
}
