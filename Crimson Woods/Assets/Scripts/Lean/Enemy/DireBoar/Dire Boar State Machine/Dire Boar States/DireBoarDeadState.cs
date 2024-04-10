using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DireBoarDeadState : DireBoarState
{
    // Declaration
    private float dropTime;
    private int count = 0;
    private bool isFinished = false;

    public DireBoarDeadState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName) : base(direBoar, direBoarStateMachine, direBoarStats, animName)
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
        if (count != direBoar.lootCount)
        {
            if (dropTime > 0.05f)
            {
                // Reset the drop time to continue dropping loots.
                dropTime = 0;

                // Drop items
                direBoar.lootBag.InstantiateLoot(direBoar.transform.position);
                count++;
            }

            else
            {
                dropTime += Time.deltaTime;
            }
        }

        // If the loot count reached the desired count then means the process is finished.
        if (count == direBoar.lootCount)
        {
            isFinished = true;
        }

        // If the process is finsihed then the enemy will started to destroy the body.
        if (isFinished)
        {
            // Destroy the enemy after dead.
            direBoar.DestroyBody();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
