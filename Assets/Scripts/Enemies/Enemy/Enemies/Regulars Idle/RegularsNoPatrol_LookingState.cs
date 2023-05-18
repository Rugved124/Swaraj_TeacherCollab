using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_LookingState : EnemyLookingState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;


    public RegularsNoPatrol_LookingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyLookingStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        SetLookingTime();
        regularsNoPatrolEnemy.LookAroundInIdle(lookingAngle, lookingTime);

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (regularsNoPatrolEnemy.SeeingPlayer())
        {
            stateMachine.ChangeState(regularsNoPatrolEnemy.searchingState);
            return;
        }

        if (regularsNoPatrolEnemy.hasFinishedLooking)
        {

            stateMachine.ChangeState(regularsNoPatrolEnemy.idleState);

        }

        if (regularsNoPatrolEnemy.enemyFOV.sawKill)
        {
            stateMachine.ChangeState(regularsNoPatrolEnemy.alarmState);
        }
    }

    private void SetLookingTime()
    {
        lookingAngle = stateData.lookingAngle;
        lookingTime = stateData.lookingTime;

    }
}
