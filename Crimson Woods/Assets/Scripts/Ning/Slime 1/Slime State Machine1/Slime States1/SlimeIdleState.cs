using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeIdleState1 : SlimeState1
{
    public SlimeIdleState1(Slime1 slime1, SlimeStateMachine1 slimeStateMachine1, SlimeStats1 slimeStats1, string animName) : base(slime1, slimeStateMachine1, slimeStats1, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        slime1.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        slimeStateMachine1.ChangeState(slime1.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
