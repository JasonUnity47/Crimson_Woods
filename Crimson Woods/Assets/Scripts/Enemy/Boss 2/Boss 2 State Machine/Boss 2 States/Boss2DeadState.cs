using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2DeadState : Boss2State
{
    private float dropTime;
    private int count = 0;
    private bool isFinished = false;

    public Boss2DeadState(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName) : base(boss2, boss2StateMachine, boss2Stats, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (count != boss2.lootCount)
        {
            if (dropTime > 0.05f)
            {
                dropTime = 0;
                // Drop items if the enemy is dead.
                boss2.lootBag.InstantiateLoot(boss2.transform.position);
                count++;
            }

            else
            {
                dropTime += Time.deltaTime;
            }
        }

        if (count == boss2.lootCount)
        {
            isFinished = true;
        }

        if (isFinished)
        {
            // Destroy the enemy after dead.
            boss2.DestroyBody();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
