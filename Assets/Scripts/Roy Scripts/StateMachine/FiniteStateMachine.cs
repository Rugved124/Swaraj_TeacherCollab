using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public CharacterStates currentState;

    public void Initialize(CharacterStates startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void ChangeState(CharacterStates newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }


}
