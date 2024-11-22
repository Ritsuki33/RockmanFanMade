using System;
using UnityEngine;

public class LiftBehavior : MonoBehaviour
{

    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    [SerializeField] bool roundTrip = false;
    [SerializeField] bool loop = false;
    [SerializeField] float oneWayTime = 3.0f;
    [SerializeField, Range(0, 1)] float startProgress = 0;
    [SerializeField] bool isStop = false;

    float currentTime = 0;
    bool isReturn = false;

    Transform Start => (!isReturn) ? pos1 : pos2;
    Transform End => (!isReturn) ? pos2 : pos1;

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


        Debug.Log(LineBehaviorHelper.GetStrobe(Start, End, currentTime / oneWayTime));
        rb.SetVelocty(LineBehaviorHelper.GetStrobe(Start, End, currentTime / oneWayTime));

        currentTime += Time.fixedDeltaTime;

        currentTime = Mathf.Clamp(currentTime, 0, oneWayTime);

        if (currentTime == oneWayTime)
        {
            if (roundTrip)
            {
                if (isReturn == false|| loop == true)
                {
                    currentTime = 0;
                    if (isReturn) onGoalEvent?.Invoke();
                    isReturn = !isReturn;
                }
            }
        }


        currentTime += Time.fixedDeltaTime;

        if (currentTime > oneWayTime)
        {
            currentTime = oneWayTime;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reverse();
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
        if (pos1 && pos2)
        {
            Gizmos.DrawLine(pos1.position, pos2.position);
        }

    }
}
