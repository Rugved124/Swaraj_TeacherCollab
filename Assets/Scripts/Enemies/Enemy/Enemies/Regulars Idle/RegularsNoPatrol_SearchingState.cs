using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_SearchingState : EnemySearchingState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;

    CharacterStates previousState;

    Quaternion currentVisionAngle;

    public RegularsNoPatrol_SearchingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemySearchingStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        currentVisionAngle = regularsNoPatrolEnemy.enemyFOV.transform.rotation;
        previousState = stateMachine.CheckPreviousState();
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (isSearchingTimeOver && !shoot)
        {
            isSearchingTimeOver = false;

            regularsNoPatrolEnemy.ResetVision(currentVisionAngle, stateData.resetVisionAngleTime);

            if (regularsNoPatrolEnemy.resetVision)
            {
               
               stateMachine.ChangeState(regularsNoPatrolEnemy.idleState);
            }
        }
        else
        {
            regularsNoPatrolEnemy.LookAtPlayer();
        }

        if (shoot || regularsNoPatrolEnemy.enemyFOV.sawKill)
        {
            stateMachine.ChangeState(regularsNoPatrolEnemy.alarmState);
        }
        

    }
}
