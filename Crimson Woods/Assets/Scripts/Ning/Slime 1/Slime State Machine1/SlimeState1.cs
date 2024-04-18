using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeState1 
{
    protected Slime1 slime1;
    protected SlimeStateMachine1 slimeStateMachine1;
    protected SlimeStats1 slimeStats1;
    private string animName;

    public SlimeState1(Slime1 slime1, SlimeStateMachine1 slimeStateMachine1, SlimeStats1 slimeStats1, string animName)
    {
        this.slime1 = slime1;
        this.slimeStateMachine1 = slimeStateMachine1;
        this.slimeStats1 = slimeStats1;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        slime1.Anim.SetBool(animName, true);
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
        slime1.Anim.SetBool(animName, false);
        Debug.Log("Exit " + animName);
    }
}
