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
        baseEnemy.SetVelocity(0f);
        SetLookingTime();
        regularsEnemy.HasReachedNext(false);
        regularsEnemy.LookAroundInIdle(lookingAngle,lookingTime);
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

        if (regularsEnemy.hasFinishedLooking)
        {
            if (regularsEnemy.HasReachedEndWayPoint())
            {

                regularsEnemy.idleState.SetFlipAfterIdle(true);
            }

            stateMachine.ChangeState(regularsEnemy.idleState);

        }

        if (regularsEnemy.enemyFOV.sawKill)
        {
            stateMachine.ChangeState(regularsEnemy.alarmState);
        }

    }

    private void SetLookingTime()
    {

        lookingAngle = regularsEnemy.GetVisionRotation();
        lookingTime = regularsEnemy.GetVisionTime();

    }
}
