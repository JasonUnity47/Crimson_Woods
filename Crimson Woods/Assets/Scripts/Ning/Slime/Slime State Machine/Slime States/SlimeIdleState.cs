using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeIdleState : SlimeState
{
    public SlimeIdleState(Slime slime, SlimeStateMachine slimeStateMachine, SlimeStats slimeStats, string animName) : base(slime, slimeStateMachine, slimeStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        slime.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Detect player
        slime.slimeMovement.TargetInDistance();

        // IF detect player THEN change to CHASE STATE
        if (slime.slimeMovement.isDetected)
        {
            slimeStateMachine.ChangeState(slime.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
