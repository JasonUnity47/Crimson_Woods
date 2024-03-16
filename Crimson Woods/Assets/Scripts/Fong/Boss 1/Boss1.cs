using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public Boss1StateMachine StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = new Boss1StateMachine();
    }

    private void Start()
    {
        //TODO: Init statemachine
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}
