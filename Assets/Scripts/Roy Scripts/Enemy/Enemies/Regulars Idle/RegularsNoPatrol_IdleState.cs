using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_IdleState : EnemyIdleState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;

    public RegularsNoPatrol_IdleState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyIdleStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        regularsNoPatrolEnemy.HasFinishedLooking(false);
       

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log("No Patrol Idle");

        if (regularsNoPatrolEnemy.SeeingPlayer())
        {
            stateMachine.ChangeState(regularsNoPatrolEnemy.searchingState);
            return;
        }

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(regularsNoPatrolEnemy.lookState);
        }

    }
}
