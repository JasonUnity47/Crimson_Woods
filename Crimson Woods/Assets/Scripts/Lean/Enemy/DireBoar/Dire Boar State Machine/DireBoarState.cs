using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarState
{
    protected DireBoar direBoar;
    protected DireBoarStateMachine direBoarStateMachine;
    protected DireBoarStats direBoarStats;
    protected string animName;

    public DireBoarState(DireBoar direBoar, DireBoarStateMachine direBoarStateMachine, DireBoarStats direBoarStats, string animName)
    {
        this.direBoar = direBoar;
        this.direBoarStateMachine = direBoarStateMachine;
        this.direBoarStats = direBoarStats;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        direBoar.Anim.SetBool(animName, true);
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
        direBoar.Anim.SetBool(animName, false);
        Debug.Log("Exit " + animName);
    }
}
