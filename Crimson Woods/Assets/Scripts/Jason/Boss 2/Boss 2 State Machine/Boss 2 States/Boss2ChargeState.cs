using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ChargeState : Boss2State
{
    public bool hasCharged = false;

    public Boss2ChargeState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        boss2.ChangeToIdleState();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (hasCharged)
        {
            boss2StateMachine.ChangeState(boss2.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!hasCharged)
        {
            boss2.ChargePlayer();
        }

    }
}
