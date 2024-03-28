using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarStateMachine
{
    public BoarState CurrentState { get; private set; }

    public void InitializeState(BoarState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BoarState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
