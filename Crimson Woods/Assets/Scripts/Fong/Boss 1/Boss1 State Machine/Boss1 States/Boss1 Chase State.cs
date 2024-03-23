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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        boss1.boss1Movement.moveSpeed = boss1Data.moveSpeed;

        // Detect obstacle
        boss1.DetectObstacle();

        // Detect player if no obstacle
        boss1.ShockMotion();

        boss1.MeeleArea();

        // IF detect player AND no obstacles AND haven't charged THEN enter SHOCK STATE
        if (boss1.isShocked && !boss1.hasObstacle && !boss1.hasCharged)
        {
            boss1.Rb.velocity = Vector2.zero; // Stop moving
            boss1StateMachine.ChangeState(boss1.ShockState);
        }

        if (boss1.isMeeleAttack && !boss1.hasObstacle)
        {
            boss1.Rb.velocity = Vector2.zero; // Stop moving
            Debug.Log("fk");
            boss1StateMachine.ChangeState(boss1.MeeleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF detect player THEN chase player
        if (boss1.boss1Movement.isDetected)
        {
            boss1.boss1Movement.PathFollow();
        }
    }
}
