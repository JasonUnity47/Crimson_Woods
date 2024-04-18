using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinChaseState1 : GoblinState1
{
    // Declaration
    private Transform playerPos;
    public GoblinChaseState1(Goblin1 goblin1, GoblinStateMachine1 goblinStateMachine1, GoblinStats1 goblinStats1, string animName) : base(goblin1, goblinStateMachine1, goblinStats1, animName)
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

       
        goblin1.aiPath.destination = playerPos.position;


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }
}
