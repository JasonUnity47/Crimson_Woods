using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1State
{
    protected Boss1 boss1;
    protected Boss1StateMachine boss1StateMachine;
    protected Boss1Data boss1Data;

    protected float startTime;

    private string animBoolName;

    public Boss1State(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName)
    {
        this.boss1 = boss1;
        this.boss1StateMachine = boss1StateMachine;
        this.boss1Data = boss1Data;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoCheck();
        startTime = Time.time;
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }

    public virtual void DoCheck()
    {
        
    }



}
