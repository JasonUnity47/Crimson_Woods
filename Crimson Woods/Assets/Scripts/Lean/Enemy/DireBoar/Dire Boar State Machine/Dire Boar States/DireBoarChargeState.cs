using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DireBoarChargeState : DireBoarState
{
    public DireBoarChargeState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, string animName) : base(direBoar, direBoarStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        // Enemy was finished charge the player.
        direBoar.FinishCharge();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // If the enemy haven't charged the player before then charge the player.
        if (!direBoar.hasCharged)
        {
            direBoar.ChargeAttack();
        }

        // IF reach to the end point THEN finish charged
        if (Vector2.Distance(direBoar.transform.position, direBoar.lastTargetPosForCharge) < direBoar.chargeDistance)
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
    }
}
