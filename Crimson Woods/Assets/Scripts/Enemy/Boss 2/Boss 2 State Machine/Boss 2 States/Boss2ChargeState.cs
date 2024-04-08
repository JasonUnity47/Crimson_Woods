using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Enemy was finished charge the player.
        boss2.FinishCharge();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // If the enemy haven't charged the player before then charge the player.
        if (!boss2.hasCharged)
        {
            boss2.ChargeAttack();
        }

        // If the enemy reach to the end point then finished charged.
        if (Vector2.Distance(boss2.transform.position, boss2.lastTargetPosForCharge) < boss2.chargeDistance)
        {
            boss2.hasCharged = true;
        }

        // If the enemy has charged the player Then change to Idle State.
        if (boss2.hasCharged)
        {
            boss2StateMachine.ChangeState(boss2.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
