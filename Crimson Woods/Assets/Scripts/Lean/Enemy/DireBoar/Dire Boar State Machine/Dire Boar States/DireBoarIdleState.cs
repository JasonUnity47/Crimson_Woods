using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DireBoarIdleState : DireBoarState
{
    public DireBoarIdleState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
    {       
    }

    public override void Enter()
    {
        base.Enter();

        // Stop moving
        direBoar.Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Detect player
        direBoar.direBoarMovement.TargetInDistance();

        // IF detect player THEN change to CHASE STATE
        if (direBoar.direBoarMovement.isDetected)
        {
            direBoarStateMachine.ChangeState(direBoar.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
