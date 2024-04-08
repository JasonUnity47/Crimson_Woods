using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2DeadState : Boss2State
{
    public Boss2DeadState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Drop item if the enemy is dead.
        boss2.lootBag.InstantiateLoot(boss2.transform.position);

        // Destroy the enemy after dead.
        boss2.DestroyBody();
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
