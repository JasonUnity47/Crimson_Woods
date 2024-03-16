using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1StateMachine
{
    public Boss1State CurrentState { get; private set; }

    public void Initialize(Boss1State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(Boss1State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
