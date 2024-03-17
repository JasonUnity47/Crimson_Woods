using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss1IdleState : Boss1State
{
    public Boss1IdleState(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName) : base(boss1, boss1StateMachine, boss1Data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        boss1.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Detect player
        boss1.boss1Movement.TargetInDistance();

        // IF detect player THEN change to CHASE STATE
        if (boss1.boss1Movement.isDetected)
        {
            boss1StateMachine.ChangeState(boss1.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
