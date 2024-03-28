using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoarIdleState : BoarState
{
    public BoarIdleState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName) : base(boar, boarStateMachine, boarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        boar.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Detect player
        boar.boarMovement.TargetInDistance();

        // IF detect player THEN change to CHASE STATE
        if (boar.boarMovement.isDetected)
        {
            boarStateMachine.ChangeState(boar.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
