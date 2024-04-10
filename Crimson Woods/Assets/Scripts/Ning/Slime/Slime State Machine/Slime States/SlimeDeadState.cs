using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SlimeDeadState : SlimeState
{
    // Declaration
    private float dropTime;
    private int count = 0;
    private bool isFinished = false;

    public SlimeDeadState(Slime slime, SlimeStateMachine slimeStateMachine, SlimeStats slimeStats, string animName) : base(slime, slimeStateMachine, slimeStats, animName)
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
        if (count != slime.lootCount)
        {
            if (dropTime > 0.05f)
            {
                // Reset the drop time to continue dropping loots.
                dropTime = 0;

                // Drop items
                slime.lootBag.InstantiateLoot(slime.transform.position);
                count++;
            }

            else
            {
                dropTime += Time.deltaTime;
            }
        }

        // If the loot count reached the desired count then means the process is finished.
        if (count == slime.lootCount)
        {
            isFinished = true;
        }

        // If the process is finsihed then the enemy will started to destroy the body.
        if (isFinished)
        {
            // Destroy the enemy after dead.
            slime.DestroyBody();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
