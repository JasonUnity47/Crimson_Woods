using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarChaseState : DireBoarState
{  
    public DireBoarChaseState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, string animName) : base(direBoar, direBoarStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Let the enemy can move.
        direBoar.aiPath.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Chase the player.
        direBoar.aiPath.destination = direBoar.playerPos.position;

        // Detect obstacle
        direBoar.DetectObstacle();

        // If no obstacles around the enemy then check whether the player is around the enemy.
        if (!direBoar.hasObstacle)
        {
            direBoar.DetectPlayer();
        }


        // IF detect player AND no obstacles AND haven't charged THEN enter SHOCK STATE
        if (direBoar.isShocked && !direBoar.hasObstacle && !direBoar.hasCharged)
        {
            // The enemy should stop moving.
            direBoar.aiPath.isStopped = true;

            direBoarStateMachine.ChangeState(direBoar.ShockState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();  
    }
}
