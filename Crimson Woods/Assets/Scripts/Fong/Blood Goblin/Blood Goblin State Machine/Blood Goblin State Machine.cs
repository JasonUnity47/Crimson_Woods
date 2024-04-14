using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGoblinStateMachine
{
    public BloodGoblinState CurrentState { get; private set; }

    public void InitializeState(BloodGoblinState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BloodGoblinState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
