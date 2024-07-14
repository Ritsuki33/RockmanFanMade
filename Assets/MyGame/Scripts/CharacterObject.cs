using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterObject : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider = default;
    [SerializeField] 
    Rigidbody2D rb;

    protected ExpandRigidBody exRb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        exRb = GetComponent<ExpandRigidBody>();
        //velocityControll.Init(rb, boxCollider);
        OnAwake();
    }

    private void FixedUpdate()
    {
        rb.velocity=Vector2.zero;

        OnFixedUpdate();

        rb.velocity = exRb.CorrectVelocity(rb.velocity);
    }

    protected virtual void OnAwake(){}

    protected virtual void OnFixedUpdate() { }

    /// <summary>
    /// ���W�b�h�{�f�B�ւ̑��x�t��
    /// </summary>
    /// <param name="velocity"></param>
    protected void AddVelocity(Vector2 velocity)
    {
        rb.velocity += velocity;
    }

    /// <summary>
    /// �㉺���E�q�b�g���̃R�[���o�b�N�o�^
    /// </summary>
    /// <param name="createVelocity"></param>
    protected void AddOnHitEventCallback(IHitEvent createVelocity)
    {
        exRb.onHitBottomStay += createVelocity.OnBottomHitStay;
        exRb.onHitTopStay += createVelocity.OnTopHitStay;
        exRb.onHitLeftStay += createVelocity.OnLeftHitStay;
        exRb.onHitRightStay += createVelocity.OnRightHitStay;
        exRb.onHitBottomExit += createVelocity.OnBottomHitExit;
        exRb.onHitTopExit += createVelocity.OnTopHitExit;
        exRb.onHitRightExit += createVelocity.OnRightHitExit;
        exRb.onHitLeftExit += createVelocity.OnLeftHitExit;
    }
}
