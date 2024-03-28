using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoarDeadState : BoarState
{

    public BoarDeadState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName) : base(boar, boarStateMachine, boarStats, animName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        boar.lootBag.InstantiateLoot(boar.transform.position);

        boar.DestroyBody();
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
