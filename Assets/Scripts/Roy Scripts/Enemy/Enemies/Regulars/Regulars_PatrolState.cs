using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_PatrolState : EnemyPatrolState
{
    private Regulars regularsEnemy;

    public Regulars_PatrolState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyPatrolStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
       
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log("Patrol State");

        if (regularsEnemy.isSeeingPlayer || regularsEnemy.isAlarmed)
        {
            stateMachine.ChangeState(regularsEnemy.searchingState);
            return;
        }

        if (regularsEnemy.hasReachedNext)
        {
            regularsEnemy.hasReachedNext = false;
            if (regularsEnemy.GetVisionRotation() > 0)
            {
                stateMachine.ChangeState(regularsEnemy.lookState);
            }
            else 
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
