using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2IdleState : Boss2State
{
    public Boss2IdleState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
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
        boss2StateMachine.ChangeState(boss2.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
