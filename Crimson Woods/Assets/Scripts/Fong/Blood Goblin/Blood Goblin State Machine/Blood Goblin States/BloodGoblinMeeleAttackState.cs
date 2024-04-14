using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGoblinMeeleAttackState : BloodGoblinState
{
    public BloodGoblinMeeleAttackState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bloodGoblin.hasMeeleAttacked = true;
        bloodGoblin.IsMeeleAttacking();
    }

    public override void Exit()
    {
        base.Exit();

        bloodGoblin.FinishMeeleAttacked();
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
