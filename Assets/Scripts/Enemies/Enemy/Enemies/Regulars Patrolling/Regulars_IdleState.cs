using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_IdleState : EnemyIdleState
{
    private Regulars regularsEnemy;

    private int idleCounter;
    CharacterStates previousState;

    public Regulars_IdleState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyIdleStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        regularsEnemy.HasFinishedLooking(false);
        idleCounter += 1;
        previousState = stateMachine.CheckPreviousState();

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (regularsEnemy.SeeingPlayer())
        {
            stateMachine.ChangeState(regularsEnemy.searchingState);
            return;
        }

        if (previousState == regularsEnemy.patrolState)
        {
            if (isIdleTimeOver)
            {
                regularsEnemy.ModifyWaypoint();
                stateMachine.ChangeState(regularsEnemy.patrolState);
            }
        }
        else
        {

            regularsEnemy.ModifyWaypoint();
            stateMachine.ChangeState(regularsEnemy.patrolState);

        }

        if (regularsEnemy.enemyFOV.sawKill)
        {
            stateMachine.ChangeState(regularsEnemy.alarmState);
        }

    }
}
