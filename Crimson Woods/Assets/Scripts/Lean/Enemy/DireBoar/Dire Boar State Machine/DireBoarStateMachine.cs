using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBoarStateMachine
{
    public DireBoarState CurrentState { get; private set; }

    public void InitializeState(DireBoarState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(DireBoarState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
