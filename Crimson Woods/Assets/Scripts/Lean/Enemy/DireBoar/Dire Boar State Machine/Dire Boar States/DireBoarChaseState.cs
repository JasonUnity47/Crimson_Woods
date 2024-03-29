using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarChaseState : DireBoarState
{
    public DireBoarChaseState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
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

        direBoar.direBoarMovement.moveSpeed = direBoarStats.moveSpeed;

        // Detect obstacle
        direBoar.DetectObstacle();

        // Detect player if no obstacle
        direBoar.ShockMotion();

        // IF detect player AND no obstacles AND haven't charged THEN enter SHOCK STATE
        if (direBoar.isShocked && !direBoar.hasObstacle && !direBoar.hasCharged)
        {
            direBoar.Rb.velocity = Vector2.zero; // Stop moving
            direBoarStateMachine.ChangeState(direBoar.ShockState);
        }

        
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF detect player THEN chase player
        if (direBoar.direBoarMovement.isDetected)
        {
            direBoar.direBoarMovement.PathFollow();
        }
    }
}
