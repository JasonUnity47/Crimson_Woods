using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinState
{
    protected Goblin goblin;
    protected GoblinStateMachine goblinStateMachine;
    protected GoblinStats goblinStats;
    private string animName;

    public GoblinState(Goblin goblin, GoblinStateMachine goblinStateMachine, GoblinStats goblinStats, string animName)
    {
        this.goblin = goblin;
        this.goblinStateMachine = goblinStateMachine;
        this.goblinStats = goblinStats;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        goblin.Anim.SetBool(animName, true);
        //Debug.Log("Enter " + animName);
    }

    public virtual void LogicalUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {
        goblin.Anim.SetBool(animName, false);
        //Debug.Log("Exit " + animName);
    }
}
