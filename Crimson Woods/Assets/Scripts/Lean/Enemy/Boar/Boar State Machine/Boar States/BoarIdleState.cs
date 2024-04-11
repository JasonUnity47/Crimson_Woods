using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoarIdleState : BoarState
{
    public BoarIdleState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName) : base(boar, boarStateMachine, boarStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
       
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        boarStateMachine.ChangeState(boar.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
