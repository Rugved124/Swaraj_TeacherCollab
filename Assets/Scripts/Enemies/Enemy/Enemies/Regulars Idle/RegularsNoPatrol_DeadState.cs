using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_DeadState : EnemyDeadState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;

    CharacterStates previousState;

    Quaternion currentVisionAngle;

    public RegularsNoPatrol_DeadState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyDeadStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        regularsNoPatrolEnemy.SetVelocity(0f);
        regularsNoPatrolEnemy.SetDyingTime();
        regularsNoPatrolEnemy.StartCoroutine(regularsNoPatrolEnemy.SpawnCorpse());

    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void UpdateState()
    {
        base.UpdateState();

    }
}
