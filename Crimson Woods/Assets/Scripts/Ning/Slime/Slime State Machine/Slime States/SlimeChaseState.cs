using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChaseState : SlimeState
{
    public SlimeChaseState(Slime slime, SlimeStateMachine slimeStateMachine, SlimeStats slimeStats, string animName) : base(slime, slimeStateMachine, slimeStats, animName)
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

        slime.slimeMovement.moveSpeed = slimeStats.moveSpeed;

        

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF detect player THEN chase player
        if (slime.slimeMovement.isDetected)
        {
            slime.slimeMovement.PathFollow();
        }
    }
}
