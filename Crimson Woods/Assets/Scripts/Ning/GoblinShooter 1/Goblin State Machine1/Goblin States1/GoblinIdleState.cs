using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinIdleState1 : GoblinState1
{
    public GoblinIdleState1(Goblin1 goblin1, GoblinStateMachine1 goblinStateMachine1, GoblinStats1 goblinStats1, string animName) : base(goblin1, goblinStateMachine1, goblinStats1, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        goblin1.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        goblinStateMachine1.ChangeState(goblin1.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
