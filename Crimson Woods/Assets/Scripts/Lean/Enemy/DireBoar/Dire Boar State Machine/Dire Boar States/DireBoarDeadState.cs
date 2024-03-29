using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DireBoarDeadState : DireBoarState
{
    public DireBoarDeadState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        direBoar.lootBag.InstantiateLoot(direBoar.transform.position);

        direBoar.DestroyBody();
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
