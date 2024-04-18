using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateMachine1
{
    public SlimeState1 CurrentState { get; private set; }

    public void InitializeState(SlimeState1 startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(SlimeState1 newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
