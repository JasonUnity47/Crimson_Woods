using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    public Boss2StateMachine boss2StateMachine { get; private set; }
    private Boss2Stats boss2Stats;

    public Boss2IdleState IdleState { get; private set; }
    public Boss2ChaseState ChaseState { get; private set; }

    public Boss2Movement boss2Movement { get; private set; }

    private void Awake()
    {
        boss2StateMachine = new Boss2StateMachine();

        boss2Stats = GetComponent<Boss2Stats>();

        IdleState = new Boss2IdleState(this, boss2StateMachine, boss2Stats, "IdleBool");
        ChaseState = new Boss2ChaseState(this, boss2StateMachine, boss2Stats, "ChaseBool");
    }

    private void Start()
    {
        boss2Movement = GetComponent<Boss2Movement>();

        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        boss2StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        boss2StateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        boss2StateMachine.CurrentState.PhysicsUpdate();
    }
}
