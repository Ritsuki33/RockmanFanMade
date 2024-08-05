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

    enum StateID
    {
        Idle = 0,
        Hide,
        Appear,
    }
    private void Awake()
    {
        exRb = GetComponent<ExpandRigidBody>();
        stateMachine = new StateMachine<MettoruController>();

        stateMachine.AddState((int)StateID.Idle, new Idle());
        stateMachine.AddState((int)StateID.Hide, new Hide());
        stateMachine.AddState((int)StateID.Appear, new Appear());

        stateMachine.TransitState((int)StateID.Appear);
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
