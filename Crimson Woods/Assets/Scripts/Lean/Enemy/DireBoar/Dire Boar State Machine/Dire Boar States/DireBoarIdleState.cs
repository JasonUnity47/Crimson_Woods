using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DireBoarIdleState : DireBoarState
{
    public DireBoarIdleState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, string animName) : base(direBoar, direBoarStateMachine, animName)
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

        // When an enemy appears in the game, it will immediately detect the player and start chasing the player.
        direBoarStateMachine.ChangeState(direBoar.ChaseState);
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
