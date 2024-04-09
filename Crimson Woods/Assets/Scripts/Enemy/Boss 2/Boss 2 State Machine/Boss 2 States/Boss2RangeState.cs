using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2RangeState : Boss2State
{
    public Boss2RangeState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        // Trigger the slash attack animation.
        boss2.Anim.SetTrigger(animName);

        // If the enemy haven't slash the player before then slash the player.
        if (!boss2.hasSlashed)
        {
            boss2.SlashPlayer();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Enemy was finished slash the player.
        boss2.FinishSlash();

        boss2.playerController.moveSpeed = boss2.originalMovespeed;
        boss2.playerSprite.color = Color.white;
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // If the enemy has slashed the player then change to Idle State.
        if (boss2.hasSlashed)
        {
            boss2StateMachine.ChangeState(boss2.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
