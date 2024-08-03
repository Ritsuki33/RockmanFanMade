using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class MettoruController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Gravity gravity;
    [SerializeField] OnTheGround onTheGround;
    [SerializeField] Move move;
    private ExpandRigidBody exRb;
    StateMachine<MettoruController> stateMachine;


    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        stateMachine = new StateMachine<MettoruController>();
    }


    private void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    private void Update()
    {
        stateMachine.Update(this);
    }
}
