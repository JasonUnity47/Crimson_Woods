using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SlimeDeadState : SlimeState
{

    public SlimeDeadState(Slime slime, SlimeStateMachine slimeStateMachine, SlimeStats slimeStats, string animName) : base(slime, slimeStateMachine, slimeStats, animName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        slime.lootBag.InstantiateLoot(slime.transform.position);

        slime.DestroyBody();
    }

    public override void Exit()
    {

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
