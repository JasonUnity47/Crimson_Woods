using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1MeeleAttackState : Boss1State
{
    public Boss1MeeleAttackState(Boss1 boss1, Boss1StateMachine boss1StateMachine, Boss1Data boss1Data, string animBoolName) : base(boss1, boss1StateMachine, boss1Data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        boss1.hasMeeleAttacked = true;
        boss1.IsMeeleAttacking();
    }

    public override void Exit()
    {
        base.Exit();

        boss1.FinishMeeleAttacked();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
