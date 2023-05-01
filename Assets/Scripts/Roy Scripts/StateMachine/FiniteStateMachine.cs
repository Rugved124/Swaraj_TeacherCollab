using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public CharacterStates currentState;
    public CharacterStates previousState;

    public void Initialize(CharacterStates startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void ChangeState(CharacterStates newState)
    {
        previousState = currentState;
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public CharacterStates CheckPreviousState()
    {
        return previousState;
    }


}
