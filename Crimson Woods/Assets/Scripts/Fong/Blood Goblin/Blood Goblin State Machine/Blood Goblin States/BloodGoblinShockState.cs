using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinShockState : BloodGoblinState
{
    public BloodGoblinShockState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // If the enemy didn't prepare to charge the player before then start charging.
        if (!bloodGoblin.isCharging)
        {
            // Get the target position once when enter this state.
            bloodGoblin.lastTargetPosForCharge = bloodGoblin.playerPos.position;

            bloodGoblin.PrepareCharge();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Reset all value to false for next execution.
        bloodGoblin.isCharging = false;
        bloodGoblin.hasObstacle = false;
        bloodGoblin.isShocked = false;
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
