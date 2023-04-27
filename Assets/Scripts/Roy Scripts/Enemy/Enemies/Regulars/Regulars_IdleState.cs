using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_IdleState : EnemyIdleState
{
    private Regulars regularsEnemy;

    public Regulars_IdleState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyIdleStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
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

        if (isIdleTimeOver)
        {
            if (regularsEnemy.isWayPointBased)
            {
                regularsEnemy.hasReachedNext = false;
                regularsEnemy.ModifyWaypoint();
            }
           
            stateMachine.ChangeState(regularsEnemy.patrolState);
        }
    }
}
