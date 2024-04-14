using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinChargeState : BloodGoblinState
{
    public BloodGoblinChargeState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bloodGoblin.CreateDust();
    }

    public override void Exit()
    {
        base.Exit();

        // Enemy was finished charge the player.
        bloodGoblin.FinishCharge();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // If the enemy haven't charged the player before then charge the player.
        if (!bloodGoblin.hasCharged)
        {
            bloodGoblin.ChargeAttack();
        }

        // If the enemy reach to the end point then finished charged.
        if (Vector2.Distance(bloodGoblin.transform.position, bloodGoblin.lastTargetPosForCharge) < bloodGoblin.chargeDistance)
        {
            bloodGoblin.hasCharged = true;
        }

        // If the enemy has charged the player Then change to Idle State.
        if (bloodGoblin.hasCharged)
        {
            bloodGoblinStateMachine.ChangeState(bloodGoblin.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}