using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarShockState : DireBoarState
{
    public DireBoarShockState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!direBoar.isCharging)
        {
            // Get the target position ONCE
            direBoar.lastTargetPosForCharge = direBoar.direBoarMovement.targetPos.position;

            direBoar.PrepareCharge();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Set all value to FALSE for next execution
        direBoar.isCharging = false;
        direBoar.hasObstacle = false;
        direBoar.isShocked = false;
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
