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

        boss2.DetectObstacle();

        boss2.ShockMotion();

        if (boss2.isShocked && !boss2.hasObstacle && !boss2.ChargeState.hasCharged)
        {
            boss2.Rb.velocity = Vector2.zero;
            boss2StateMachine.ChangeState(boss2.ShockState);
        }

        if (boss2.boss2Movement.isDetected)
        {
            boss2.boss2Movement.NearPlayer();
        }

        if (boss2.boss2Movement.isNearby)
        {
            boss2.boss2Movement.moveSpeed = 0;
            boss2.Rb.velocity = Vector2.zero;
        }

        else
        {
            boss2.boss2Movement.moveSpeed = boss2Stats.moveSpeed;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (boss2.boss2Movement.isDetected && !boss2.boss2Movement.isNearby)
        {
            boss2.boss2Movement.PathFollow();
        }
    }
}
