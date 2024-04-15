using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinChaseState : BloodGoblinState
{
    public BloodGoblinChaseState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName) : base(bloodGoblin, bloodGoblinStateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Let the enemy can move.
        bloodGoblin.aiPath.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // Chase the player.
        bloodGoblin.aiPath.destination = bloodGoblin.playerPos.position;

        // Range Attack
        // Check whether the player is far away for the enemy to throw.
        bloodGoblin.FarPlayer();

        // Check whether the player is within the slash area to allow the enemy to slash.
        bloodGoblin.DetectThrow();

        // Melee Attack
        // Check whether there is any obstacle around the enemy.
        bloodGoblin.DetectObstacle();


        // Check whether player is in meele range.
        bloodGoblin.MeeleArea();

        // If no obstacles around the enemy then check whether the player is around the enemy.
        if (!bloodGoblin.hasObstacle)
        {
            bloodGoblin.DetectPlayer();
        }

        // Result
        // If detect player && no obstacles && haven't charged then enter Shock State.
        if (bloodGoblin.isShocked && !bloodGoblin.hasObstacle && !bloodGoblin.hasCharged)
        {
            // The enemy should stop moving.
            bloodGoblin.aiPath.isStopped = true;

            bloodGoblinStateMachine.ChangeState(bloodGoblin.ShockState);
        }

        // If detect player && far enough && haven't throw then enter Prepare State.
        if (bloodGoblin.isPrepared && bloodGoblin.farEnough && !bloodGoblin.hasThrowed)
        {
            // The enemy should stop moving.
            bloodGoblin.aiPath.isStopped = true;

            bloodGoblinStateMachine.ChangeState(bloodGoblin.PrepareState);
        }

        if (bloodGoblin.isMeeleAttack && !bloodGoblin.hasMeeleAttacked)
        {
            // The enemy should stop moving.
            bloodGoblin.aiPath.isStopped = true;

            bloodGoblinStateMachine.ChangeState(bloodGoblin.MeeleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}