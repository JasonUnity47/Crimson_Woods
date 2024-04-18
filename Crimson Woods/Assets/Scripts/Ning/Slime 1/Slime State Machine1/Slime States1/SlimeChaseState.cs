using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChaseState1 : SlimeState1
{
    // Declaration
    private Transform playerPos;
    public SlimeChaseState1(Slime1 slime1, SlimeStateMachine1 slimeStateMachine1, SlimeStats1 slimeStats1, string animName) : base(slime1, slimeStateMachine1, slimeStats1, animName)
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

        slime1.aiPath.destination = playerPos.position;



    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }
}
