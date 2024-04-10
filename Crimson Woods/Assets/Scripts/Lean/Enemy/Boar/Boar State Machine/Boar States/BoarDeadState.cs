using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoarDeadState : BoarState
{
    // Declaration
    private float dropTime;
    private int count = 0;
    private bool isFinished = false;
    public BoarDeadState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName) : base(boar, boarStateMachine, boarStats, animName)
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
        if (count != boar.lootCount)
        {
            if (dropTime > 0.05f)
            {
                // Reset the drop time to continue dropping loots.
                dropTime = 0;

                // Drop items
                boar.lootBag.InstantiateLoot(boar.transform.position);
                count++;
            }

            else
            {
                dropTime += Time.deltaTime;
            }
        }

        // If the loot count reached the desired count then means the process is finished.
        if (count == boar.lootCount)
        {
            isFinished = true;
        }

        // If the process is finsihed then the enemy will started to destroy the body.
        if (isFinished)
        {
            // Destroy the enemy after dead.
            boar.DestroyBody();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
