using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2State
{
    protected Boss2 boss2;
    protected Boss2StateMachine boss2StateMachine;
    protected Boss2Stats boss2Stats;
    protected string animName;

    public Boss2State(Boss2 boss2, Boss2StateMachine boss2StateMachine, Boss2Stats boss2Stats, string animName)
    {
        this.boss2 = boss2;
        this.boss2StateMachine = boss2StateMachine;
        this.boss2Stats = boss2Stats;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        boss2.Anim.SetBool(animName, true);
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
        boss2.Anim.SetBool(animName, false);
        Debug.Log("Exit " + animName);
    }
}
