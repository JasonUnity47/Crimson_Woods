using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ShockState : Boss2State
{
    public bool isCharging = false;

    public Boss2ShockState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!isCharging)
        {
            boss2.ChangeToChargeState();
        }
    }

    public override void Exit()
    {
        base.Exit();

        boss2.hasObstacle = false;
        boss2.isShocked = false;
        isCharging = false;
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
