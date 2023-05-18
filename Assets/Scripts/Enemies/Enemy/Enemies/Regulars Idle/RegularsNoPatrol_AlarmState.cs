using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_AlarmState : EnemyAlarmState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;

    public RegularsNoPatrol_AlarmState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyAlarmStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        regularsNoPatrolEnemy.SetVelocity(0f);

        if (!regularsNoPatrolEnemy.enemyFOV.sawKill)
        {
            regularsNoPatrolEnemy.shootSound.Play();
            PCController PC = GameObject.FindObjectOfType<PCController>();
            PC.Die();
        }
        else
        {
            regularsNoPatrolEnemy.sawKillSound.Play();
            PCController PC = GameObject.FindObjectOfType<PCController>();
            PC.Die();
        }

       
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
