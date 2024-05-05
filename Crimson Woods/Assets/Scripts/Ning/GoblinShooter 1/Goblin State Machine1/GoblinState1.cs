using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinState1
{
    protected Goblin1 goblin1;
    protected GoblinStateMachine1 goblinStateMachine1;
    protected GoblinStats1 goblinStats1;
    private string animName;

    public GoblinState1(Goblin1 goblin1, GoblinStateMachine1 goblinStateMachine1, GoblinStats1 goblinStats1, string animName)
    {
        this.goblin1 = goblin1;
        this.goblinStateMachine1 = goblinStateMachine1;
        this.goblinStats1 = goblinStats1;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        goblin1.Anim.SetBool(animName, true);
    }

    public virtual void LogicalUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {
        goblin1.Anim.SetBool(animName, false);
    }
}
