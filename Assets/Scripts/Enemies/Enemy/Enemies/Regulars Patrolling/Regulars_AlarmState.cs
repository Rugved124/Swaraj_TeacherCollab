using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_AlarmState : EnemyAlarmState
{
    private Regulars regularsEnemy;

    public Regulars_AlarmState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyAlarmStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        regularsEnemy.SetVelocity(0f);

        if (!regularsEnemy.enemyFOV.sawKill)
        {
            regularsEnemy.shootSound.Play();
            PCController PC = GameObject.FindObjectOfType<PCController>();
            PC.Die();
        }
        else
        {
            regularsEnemy.sawKillSound.Play();
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
