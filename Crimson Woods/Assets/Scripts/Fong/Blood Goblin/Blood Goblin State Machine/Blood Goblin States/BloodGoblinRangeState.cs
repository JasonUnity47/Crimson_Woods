using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinRangeState : BloodGoblinState
{
    public BloodGoblinRangeState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
    {
    }

    public override void Enter()
    {
        // Trigger the throw attack animation.
        bloodGoblin.Anim.SetTrigger(animName);

        // If the enemy haven't throw the player before then throw the player.
        if (!bloodGoblin.hasThrowed)
        {
            bloodGoblin.ThrowPlayer();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Enemy was finished slash the player.
        bloodGoblin.FinishThrow();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // If the enemy has throwed the player then change to Idle State.
        if (bloodGoblin.hasThrowed)
        {
            bloodGoblinStateMachine.ChangeState(bloodGoblin.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
