using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BoarState
{
    public BoarChaseState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName) : base(boar, boarStateMachine, boarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        boar.boarMovement.moveSpeed = boarStats.moveSpeed;



    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF detect player THEN chase player
        if (boar.boarMovement.isDetected)
        {
            boar.boarMovement.PathFollow();
        }
    }
}
