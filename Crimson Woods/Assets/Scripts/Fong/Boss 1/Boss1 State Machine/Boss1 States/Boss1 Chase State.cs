using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss1ChaseState : Boss1State
{
    public Boss1ChaseState(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName) : base(boss1, boss1StateMachine, boss1Data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Let the enemy can move.
        boss1.aiPath.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Chase the player.
        boss1.aiPath.destination = boss1.playerPos.position;

        // Detect obstacle
        boss1.DetectObstacle();

        boss1.MeeleArea();

        // If no obstacles around the enemy then check whether the player is around the enemy.
        if (!boss1.hasObstacle)
        {
            boss1.DetectPlayer();
        }

        // IF detect player AND no obstacles AND haven't charged THEN enter SHOCK STATE
        if (boss1.isShocked && !boss1.hasCharged)
        {
            // The enemy should stop moving.
            boss1.aiPath.isStopped = true;

            boss1StateMachine.ChangeState(boss1.ShockState);
        }

        if (boss1.isMeeleAttack && !boss1.hasMeeleAttacked)
        {
            // The enemy should stop moving.
            boss1.aiPath.isStopped = true;

            boss1StateMachine.ChangeState(boss1.MeeleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
