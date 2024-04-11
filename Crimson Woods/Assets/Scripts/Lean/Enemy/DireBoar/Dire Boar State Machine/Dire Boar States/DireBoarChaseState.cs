using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarChaseState : DireBoarState
{
    private Transform playerPos;
    public DireBoarChaseState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        direBoar.aiPath.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        direBoar.aiPath.destination = direBoar.playPos.position;

        // Detect obstacle
        direBoar.DetectObstacle();

        // Detect player if no obstacle
        direBoar.ShockMotion();

        if (!direBoar.hasObstacle)
        {
            direBoar.DetectPlayer();
        }


        // IF detect player AND no obstacles AND haven't charged THEN enter SHOCK STATE
        if (direBoar.isShocked && !direBoar.hasObstacle && !direBoar.hasCharged)
        {
            direBoar.aiPath.isStopped = true;
            direBoarStateMachine.ChangeState(direBoar.ShockState);
        }

        
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();  
    }
}
