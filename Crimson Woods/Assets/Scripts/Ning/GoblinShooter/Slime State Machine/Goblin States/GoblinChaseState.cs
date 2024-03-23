using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinChaseState : GoblinState
{
    public GoblinChaseState(Goblin goblin, GoblinStateMachine goblinStateMachine, GoblinStats goblinStats, string animName) : base(goblin, goblinStateMachine, goblinStats, animName)
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

        goblin.goblinMovement.moveSpeed = goblinStats.moveSpeed;

        

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF detect player THEN chase player
        if (goblin.goblinMovement.isDetected)
        {
            goblin.goblinMovement.PathFollow();
        }
    }
}
