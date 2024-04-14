using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BloodGoblinState
{
    protected BloodGoblin bloodGoblin;
    protected BloodGoblinStateMachine bloodGoblinStateMachine;
    protected string animName;

    public BloodGoblinState(BloodGoblin bloodGoblin, BloodGoblinStateMachine bloodGoblinStateMachine, string animName)
    {
        this.bloodGoblin = bloodGoblin;
        this.bloodGoblinStateMachine = bloodGoblinStateMachine;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        bloodGoblin.Anim.SetBool(animName, true);
        Debug.Log("Enter " + animName);
    }

    public virtual void LogicalUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Exit()
    {
        bloodGoblin.Anim.SetBool(animName, false);
        Debug.Log("Exit " + animName);
    }
}
