using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ShockState : Boss2State
{
    public Boss2ShockState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //if (!boss2.isCharging)
        //{
        //    // Get the target position ONCE
        //    boss2.lastTargetPosForCharge = boss2.boss2Movement.targetPos.position;

        //    boss2.PrepareCharge();
        //}
    }

    public override void Exit()
    {
        base.Exit();

        // Set all value to FALSE for next execution
        boss2.isCharging = false;
        boss2.hasObstacle = false;
        boss2.isShocked = false;
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
