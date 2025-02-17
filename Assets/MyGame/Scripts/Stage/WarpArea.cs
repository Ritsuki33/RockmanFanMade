using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpArea : MonoBehaviour
{
    [SerializeField] ActionChainExecuter eventController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventController?.StartEvent();
    }
}
