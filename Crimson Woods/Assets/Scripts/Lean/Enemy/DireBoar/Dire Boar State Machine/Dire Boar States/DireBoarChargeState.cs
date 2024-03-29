using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DireBoarChargeState : DireBoarState
{
    public DireBoarChargeState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
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
        direBoar.FinishCharge();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // IF reach to the end point THEN finish charged
        if (Vector2.Distance(direBoar.transform.position, direBoar.lastTargetPosForCharge) < direBoar.direBoarMovement.chargeDistance)
        {
            direBoar.hasCharged = true;
        }

        // IF already charge player THEN change to IDLE STATE
        if (direBoar.hasCharged)
        {
            direBoarStateMachine.ChangeState(direBoar.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF enemy haven't charge player before THEN charge player
        if (!direBoar.hasCharged)
        {
            direBoar.ChargePlayer();
        }

    }
}
