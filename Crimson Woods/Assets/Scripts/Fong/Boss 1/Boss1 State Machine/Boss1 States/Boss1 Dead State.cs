using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss1DeadState : Boss1State
{
    // Declaration
    private float dropTime;
    private int count = 0;
    private bool isFinished = false;

    public Boss1DeadState(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName) : base(boss1, boss1StateMachine, boss1Data, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // If the loop count does not reach the desired count then keep dropping loot until meets the desired count.
        if (count != boss1.lootCount)
        {
            if (dropTime > 0.05f)
            {
                // Reset the drop time to continue dropping loots.
                dropTime = 0;

                // Drop items
                boss1.lootBag.InstantiateLoot(boss1.transform.position);
                count++;
            }

            else
            {
                dropTime += Time.deltaTime;
            }
        }

        // If the loot count reached the desired count then means the process is finished.
        if (count == boss1.lootCount)
        {
            isFinished = true;
        }

        // If the process is finsihed then the enemy will started to destroy the body.
        if (isFinished)
        {
            // Destroy the enemy after dead.
            boss1.DestroyBody();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
