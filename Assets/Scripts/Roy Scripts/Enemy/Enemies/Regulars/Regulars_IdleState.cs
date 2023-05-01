using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_IdleState : EnemyIdleState
{
    private Regulars regularsEnemy;

    private int idleCounter;

    public Regulars_IdleState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyIdleStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        regularsEnemy.hasFinishedLooking = false;
        idleCounter += 1;

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (regularsEnemy.isWayPointBased)
        {
            if (stateMachine.CheckPreviousState() == regularsEnemy.lookState)
            {
                regularsEnemy.ModifyWaypoint();
                stateMachine.ChangeState(regularsEnemy.patrolState);
            }
            else
            {
                if (isIdleTimeOver)
                {
                    if (regularsEnemy.isWayPointBased)
                    {
                        regularsEnemy.hasReachedNext = false;

                        if (idleCounter != 1 && regularsEnemy.GetVisionRotation() > 0)
                        {
                            shouldLook = true;
                            stateMachine.ChangeState(regularsEnemy.lookState);
                        }
                        else
                        {
                            regularsEnemy.ModifyWaypoint();
                            stateMachine.ChangeState(regularsEnemy.patrolState);
                        }


                    }


                }
            }

            

        }

       
       
    }
}
