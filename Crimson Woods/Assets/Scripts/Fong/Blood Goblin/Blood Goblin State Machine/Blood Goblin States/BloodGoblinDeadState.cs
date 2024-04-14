using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinDeadState : BloodGoblinState
{
    // Declaration
    private float dropTime;
    private int count = 0;
    private bool isFinished = false;

    public BloodGoblinDeadState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
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

        // If the loop count does not reach the desired count then keep dropping loot until meets the desired count.
        if (count != bloodGoblin.lootCount)
        {
            if (dropTime > 0.05f)
            {
                // Reset the drop time to continue dropping loots.
                dropTime = 0;

                // Drop items
                bloodGoblin.lootBag.InstantiateLoot(bloodGoblin.transform.position);
                count++;
            }

            else
            {
                dropTime += Time.deltaTime;
            }
        }

        // If the loot count reached the desired count then means the process is finished.
        if (count == bloodGoblin.lootCount)
        {
            isFinished = true;
        }

        // If the process is finsihed then the enemy will started to destroy the body.
        if (isFinished)
        {
            // Destroy the enemy after dead.
            bloodGoblin.DestroyBody();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
