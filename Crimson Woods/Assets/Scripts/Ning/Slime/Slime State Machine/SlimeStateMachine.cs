using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateMachine 
{
    public SlimeState CurrentState { get; private set; }

    public void InitializeState(SlimeState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(SlimeState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
