using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss1ChargeState : Boss1State
{
    public Boss1ChargeState(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName) : base(boss1, boss1StateMachine, boss1Data, animBoolName)
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
        boss1.FinishCharge();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // IF reach to the end point THEN finish charged
        if (Vector2.Distance(boss1.transform.position, boss1.lastTargetPosForCharge) < boss1.boss1Movement.chargeDistance)
        {
            boss1.hasCharged = true;
        }

        // IF already charge player THEN change to IDLE STATE
        if (boss1.hasCharged)
        {
            boss1StateMachine.ChangeState(boss1.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // IF enemy haven't charge player before THEN charge player
        if (!boss1.hasCharged)
        {
            boss1.ChargePlayer();
        }

    }
}
