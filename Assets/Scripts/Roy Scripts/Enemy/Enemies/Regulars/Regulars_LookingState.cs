using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_LookingState : EnemyLookingState
{
    private Regulars regularsEnemy;

    public Regulars_LookingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyLookingStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        regularsEnemy.LookAroundInIdle(lookingAngle,lookingTime);
    }

    public override void ExitState()
    {
        base.ExitState();
        regularsEnemy.idleState.shouldLook = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log("Looking State");

        if (regularsEnemy.hasFinishedLooking)
        {
            stateMachine.ChangeState(regularsEnemy.idleState);

        }

    }
}
