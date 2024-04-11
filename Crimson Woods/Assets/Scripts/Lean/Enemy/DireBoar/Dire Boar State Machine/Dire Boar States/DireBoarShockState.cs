using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarShockState : DireBoarState
{
    public DireBoarShockState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, string animName) : base(direBoar, direBoarStateMachine,  animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // If the enemy didn't prepare to charge the player before then start charging.
        if (!direBoar.isCharging)
        {
            // Get the target position once when enter this state.
            direBoar.lastTargetPosForCharge = direBoar.playerPos.position;

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
