using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss1ShockState : Boss1State
{
    public Boss1ShockState(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName) : base(boss1, boss1StateMachine, boss1Data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!boss1.isCharging)
        {
            // Get the target position ONCE
            boss1.lastTargetPosForCharge = boss1.playerPos.position;
            boss1.PrepareCharge();
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Set all value to FALSE for next execution
        boss1.isCharging = false;
        boss1.hasObstacle = false;
        boss1.isShocked = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
