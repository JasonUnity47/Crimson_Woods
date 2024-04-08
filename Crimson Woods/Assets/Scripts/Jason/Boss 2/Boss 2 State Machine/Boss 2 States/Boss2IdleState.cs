using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2IdleState : Boss2State
{
    public Boss2IdleState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        boss2.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Detect player
        //boss2.boss2Movement.TargetInDistance();

        // IF detect player THEN change to CHASE STATE
        //if (boss2.boss2Movement.isDetected)
        //{

        //}

        boss2StateMachine.ChangeState(boss2.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
