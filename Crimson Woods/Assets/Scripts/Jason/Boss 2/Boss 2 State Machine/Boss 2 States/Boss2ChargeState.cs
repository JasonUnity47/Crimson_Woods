using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Boss2ChargeState : Boss2State
{
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

        // Finished charge player
        boss2.FinishCharge();

    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // IF reach to the end point THEN finish charged
        if (Vector2.Distance(boss2.transform.position, boss2.lastTargetPos) < boss2.boss2Movement.chargingDistance)
        {
            boss2.hasCharged = true;
        }

        // IF already charge player THEN change to IDLE STATE
        if (boss2.hasCharged)
        {
            boss2StateMachine.ChangeState(boss2.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF enemy haven't charge player before THEN charge player
        if (!boss2.hasCharged)
        {
            boss2.ChargePlayer();
        }

    }
}
