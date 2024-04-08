using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ShockState : Boss2State
{
    public Boss2ShockState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // If the enemy didn't prepare to charge the player before then start charging.
        if (!boss2.isCharging)
        {
            // Get the target position once when enter this state.
            boss2.lastTargetPosForCharge = boss2.playerPos.position;

            boss2.PrepareCharge();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Reset all value to false for next execution.
        boss2.isCharging = false;
        boss2.hasObstacle = false;
        boss2.isShocked = false;
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
