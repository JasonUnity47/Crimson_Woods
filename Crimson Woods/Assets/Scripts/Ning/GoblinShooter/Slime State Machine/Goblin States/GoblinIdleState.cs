using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinIdleState : GoblinState
{
    public GoblinIdleState(Goblin goblin, GoblinStateMachine goblinStateMachine, GoblinStats goblinStats, string animName) : base(goblin, goblinStateMachine, goblinStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        goblin.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Detect player
        goblin.goblinMovement.TargetInDistance();

        // IF detect player THEN change to CHASE STATE
        if (goblin.goblinMovement.isDetected)
        {
           goblinStateMachine.ChangeState(goblin.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
