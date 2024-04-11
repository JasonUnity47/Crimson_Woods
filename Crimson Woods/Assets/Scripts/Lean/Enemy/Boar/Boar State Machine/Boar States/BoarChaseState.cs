using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BoarState
{
    private Transform playerPos;
    public BoarChaseState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName) : base(boar, boarStateMachine, boarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        boar.aiPath.destination = playerPos.position;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
