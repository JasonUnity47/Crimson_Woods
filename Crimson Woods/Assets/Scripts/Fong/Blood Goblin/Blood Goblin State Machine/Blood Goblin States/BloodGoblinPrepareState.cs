using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinPrepareState : BloodGoblinState
{
    public BloodGoblinPrepareState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Get the target position once when enter this state.
        bloodGoblin.lastTargetPosForThrow = bloodGoblin.playerPos.position;

        // If the enemy didn't prepare to throw the player before then start throwing.
        if (!bloodGoblin.isThrowing)
        {
            bloodGoblin.PrepareThrow();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Reset all value to false for next execution.
        bloodGoblin.isThrowing = false;
        bloodGoblin.isPrepared = false;
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