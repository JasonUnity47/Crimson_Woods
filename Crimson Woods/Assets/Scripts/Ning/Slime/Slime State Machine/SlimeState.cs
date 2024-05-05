using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeState 
{
    protected Slime slime;
    protected SlimeStateMachine slimeStateMachine;
    protected SlimeStats slimeStats;
    private string animName;

    public SlimeState(Slime slime, SlimeStateMachine slimeStateMachine, SlimeStats slimeStats, string animName)
    {
        this.slime = slime;
        this.slimeStateMachine = slimeStateMachine;
        this.slimeStats = slimeStats;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        slime.Anim.SetBool(animName, true);
    }

    public virtual void LogicalUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {
        slime.Anim.SetBool(animName, false);
    }
}
