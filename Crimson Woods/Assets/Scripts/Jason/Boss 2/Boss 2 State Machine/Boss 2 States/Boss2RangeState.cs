using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2RangeState : Boss2State
{
    private string animNameRange;

    public Boss2RangeState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
        animNameRange = animName;
    }

    public override void Enter()
    {
        boss2.Anim.SetTrigger(animNameRange);
        Debug.Log("Enter " + animNameRange);

        if (!boss2.hasSlashed)
        {
            boss2.BeforeSlash();
        }
    }

    public override void Exit()
    {
        base.Exit();

        boss2.FinishSlash();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (boss2.hasSlashed)
        {
            boss2StateMachine.ChangeState(boss2.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
