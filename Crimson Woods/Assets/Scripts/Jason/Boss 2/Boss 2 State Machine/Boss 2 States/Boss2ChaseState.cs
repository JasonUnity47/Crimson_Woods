using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ChaseState : Boss2State
{
    public Boss2ChaseState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
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

        boss2.boss2Movement.moveSpeed = boss2Stats.moveSpeed;

        boss2.FarPlayer();

        boss2.PrepareMotion();

        // Detect obstacle
        boss2.DetectObstacle();

        // Detect player if no obstacle
        boss2.ShockMotion();

        // IF detect player AND no obstacles AND haven't charged THEN enter SHOCK STATE
        if (boss2.isShocked && !boss2.hasObstacle && !boss2.hasCharged)
        {
            boss2.Rb.velocity = Vector2.zero; // Stop moving
            boss2StateMachine.ChangeState(boss2.ShockState);
        }

        // IF detect player AND far enough AND haven't slash THEN enter PREPARE STATE
        if (boss2.isPrepared && boss2.farEnough && !boss2.hasSlashed)
        {
            boss2.Rb.velocity = Vector2.zero; // Stop moving
            boss2StateMachine.ChangeState(boss2.PrepareState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF detect player THEN chase player
        if (boss2.boss2Movement.isDetected)
        {
            boss2.boss2Movement.PathFollow();
        }
    }
}
