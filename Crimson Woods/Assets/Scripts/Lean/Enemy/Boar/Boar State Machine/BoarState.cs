using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarState
{
    protected Boar boar;
    protected BoarStateMachine boarStateMachine;
    protected BoarStats boarStats;
    private string animName;

    public BoarState(Boar boar, BoarStateMachine boarStateMachine, BoarStats boarStats, string animName)
    {
        this.boar = boar;
        this.boarStateMachine = boarStateMachine;
        this.boarStats = boarStats;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        boar.Anim.SetBool(animName, true);
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
        boar.Anim.SetBool(animName, false);
        Debug.Log("Exit " + animName);
    }
}
