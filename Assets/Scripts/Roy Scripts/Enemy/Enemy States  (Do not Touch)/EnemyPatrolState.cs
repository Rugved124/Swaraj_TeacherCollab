using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : CharacterStates
{
    protected EnemyPatrolStateData stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool hasReachedWayPoint;

    protected bool shouldLook;

    public EnemyPatrolState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyPatrolStateData stateData) : base(stateMachine, baseEnemy, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void EnterState()
    {
        base.EnterState();

        if (!baseEnemy.isWayPointBased)
        {
            baseEnemy.SetVelocity(stateData.movementSpeed);

            isDetectingLedge = baseEnemy.CheckGround();
            isDetectingWall = baseEnemy.CheckWall();
        }
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

       

        isDetectingLedge = baseEnemy.CheckGround();
        isDetectingWall = baseEnemy.CheckWall();

        if (baseEnemy.isWayPointBased)
        {
            if (!baseEnemy.hasReachedNext)
            {
                baseEnemy.SetVelocityWaypoint(stateData.movementSpeed);
            }
            
        }

    }

   
}
