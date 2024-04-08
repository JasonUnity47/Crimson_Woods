using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2StateMachine
{
    public Boss2State CurrentState { get; private set; }

    public void InitializeState(Boss2State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(Boss2State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
