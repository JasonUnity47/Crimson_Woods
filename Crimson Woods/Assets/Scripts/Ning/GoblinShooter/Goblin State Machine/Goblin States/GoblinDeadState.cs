using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GoblinDeadState : GoblinState
{

    public GoblinDeadState(Goblin goblin, GoblinStateMachine goblinStateMachine, GoblinStats goblinStats, string animName) : base(goblin, goblinStateMachine, goblinStats, animName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        goblin.lootBag.InstantiateLoot(goblin.transform.position);

        goblin.DestroyBody();
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
