using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStateMachine1
{
    public GoblinState1 CurrentState { get; private set; }

    public void InitializeState(GoblinState1 startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(GoblinState1 newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
