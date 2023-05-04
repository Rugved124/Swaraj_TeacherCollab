using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_SearchingState : EnemySearchingState
{
    private Regulars regularsEnemy;
    CharacterStates previousState;

    Quaternion currentVisionAngle;

    public Regulars_SearchingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemySearchingStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        currentVisionAngle = regularsEnemy.enemyFOV.transform.rotation;
        previousState = stateMachine.CheckPreviousState();
    }

    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log("Searching State");


        if (isSearchingTimeOver)
        {
            isSearchingTimeOver = false;

            regularsEnemy.ResetVision(currentVisionAngle, stateData.resetVisionAngleTime);

            if (regularsEnemy.resetVision)
            {
                if (previousState == regularsEnemy.patrolState || previousState == regularsEnemy.idleState)
                {
                    stateMachine.ChangeState(previousState);
                }
                else if(previousState == regularsEnemy.lookState)
                {
                    if (regularsEnemy.HasReachedEndWayPoint())
                    {

                        regularsEnemy.idleState.SetFlipAfterIdle(true);
                    }

                    stateMachine.ChangeState(regularsEnemy.idleState);
                }
            }
        }
    }
}
