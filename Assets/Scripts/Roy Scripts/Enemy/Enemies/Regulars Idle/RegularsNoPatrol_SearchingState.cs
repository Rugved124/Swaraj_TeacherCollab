using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_SearchingState : EnemySearchingState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;

    public RegularsNoPatrol_SearchingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemySearchingStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }
}
