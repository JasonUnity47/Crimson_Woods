using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChaseState : SlimeState
{
    // Declaration
    private Transform playerPos;
    public SlimeChaseState(Slime slime, SlimeStateMachine slimeStateMachine, SlimeStats slimeStats, string animName) : base(slime, slimeStateMachine, slimeStats, animName)
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

        slime.aiPath.destination = playerPos.position;



    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }
}
