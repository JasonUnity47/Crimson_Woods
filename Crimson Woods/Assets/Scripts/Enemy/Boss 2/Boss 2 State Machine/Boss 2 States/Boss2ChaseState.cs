using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ChaseState : Boss2State
{
    private float lastSpeed;

    public Boss2ChaseState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Let the enemy can move.
        boss2.aiPath.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Chase the player.
        boss2.aiPath.destination = boss2.playerPos.position;

        // Range Attack
        // Check whether the player is far away for the enemy to slash.
        boss2.FarPlayer();

        // Check whether the player is within the slash area to allow the enemy to slash.
        boss2.DetectSlash();

        // Melee Attack
        // Check whether there is any obstacle around the enemy.
        boss2.DetectObstacle();

        // If no obstacles around the enemy then check whether the player is around the enemy.
        if (!boss2.hasObstacle)
        {
            boss2.DetectPlayer();
        }

        // Result
        // If detect player && no obstacles && haven't charged then enter Shock State.
        if (boss2.isShocked && !boss2.hasObstacle && !boss2.hasCharged)
        {
            // The enemy should stop moving.
            boss2.aiPath.isStopped = true;

            boss2StateMachine.ChangeState(boss2.ShockState);
        }

        // If detect player && far enough && haven't slash then enter Prepare State.
        if (boss2.isPrepared && boss2.farEnough && !boss2.hasSlashed)
        {
            // The enemy should stop moving.
            boss2.aiPath.isStopped = true;

            boss2StateMachine.ChangeState(boss2.PrepareState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
