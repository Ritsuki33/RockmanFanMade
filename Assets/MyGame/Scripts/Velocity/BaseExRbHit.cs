using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseExRbHit : MonoBehaviour, IHitEvent
{
    [SerializeField] ExpandRigidBody exRb;
    protected virtual void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
    }

    protected virtual void OnEnable()
    {
        exRb = GetComponent<ExpandRigidBody>();
        exRb?.AddOnHitEventCallback(this);
    }


    protected virtual void OnDisable()
    {
        exRb = GetComponent<ExpandRigidBody>();
        exRb?.RemoveOnHitEventCallback(this);
    }

    void IHitEvent.BottomHitStay(RaycastHit2D hit) { OnBottomHitStay(hit); }
    void IHitEvent.TopHitStay(RaycastHit2D hit) { OnTopHitStay(hit); }
    void IHitEvent.LeftHitStay(RaycastHit2D hit) { OnLeftHitStay(hit); }
    void IHitEvent.RightHitStay(RaycastHit2D hit) { OnRightHitStay(hit); }
    void IHitEvent.BottomHitExit(RaycastHit2D hit) { OnBottomHitExit(hit); }
    void IHitEvent.TopHitExit(RaycastHit2D hit) { OnTopHitExit(hit); }
    void IHitEvent.LeftHitExit(RaycastHit2D hit) { OnLeftHitExit(hit); }
    void IHitEvent.RightHitExit(RaycastHit2D hit) { OnRightHitExit(hit); }

    protected virtual void OnBottomHitStay(RaycastHit2D hit) { }
    protected virtual void OnTopHitStay(RaycastHit2D hit) { }
    protected virtual void OnLeftHitStay(RaycastHit2D hit) { }
    protected virtual void OnRightHitStay(RaycastHit2D hit) { }
    protected virtual void OnBottomHitExit(RaycastHit2D hit) { }
    protected virtual void OnTopHitExit(RaycastHit2D hit) { }
    protected virtual void OnLeftHitExit(RaycastHit2D hit) { }
    protected virtual void OnRightHitExit(RaycastHit2D hit) { }

}
