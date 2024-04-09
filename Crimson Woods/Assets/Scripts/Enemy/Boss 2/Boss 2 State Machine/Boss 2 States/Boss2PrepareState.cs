using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2PrepareState : Boss2State
{
    public Boss2PrepareState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Get the target position once when enter this state.
        boss2.lastTargetPosForSlash = boss2.playerPos.position;

        // Freeze the player.
        boss2.FreezePlayer();

        // If the enemy didn't prepare to slash the player before then start slashing.
        if (!boss2.isSlashing)
        {
            boss2.PrepareSlash();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Reset all value to false for next execution.
        boss2.isSlashing = false;
        boss2.isPrepared = false;
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
