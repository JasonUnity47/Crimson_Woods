using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinChaseState : GoblinState
{
    // Declaration
    private Transform playerPos;
    public GoblinChaseState(Goblin goblin, GoblinStateMachine goblinStateMachine, GoblinStats goblinStats, string animName) : base(goblin, goblinStateMachine, goblinStats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

       
        goblin.aiPath.destination = playerPos.position;


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }
}
