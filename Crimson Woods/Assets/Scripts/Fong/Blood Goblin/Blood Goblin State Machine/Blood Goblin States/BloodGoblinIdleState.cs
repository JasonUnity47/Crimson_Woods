using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinIdleState : BloodGoblinState
{
    public BloodGoblinIdleState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
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

        // When an enemy appears in the game, it will immediately detect the player and start chasing the player.
        bloodGoblinStateMachine.ChangeState(bloodGoblin.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
