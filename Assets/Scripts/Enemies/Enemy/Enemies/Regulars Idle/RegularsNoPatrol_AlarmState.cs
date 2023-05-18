using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol_AlarmState : EnemyAlarmState
{
    private RegularsNoPatrol regularsNoPatrolEnemy;
    GameManager gameManager;
    PCController PC;
    bool gameOver;

    public RegularsNoPatrol_AlarmState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyAlarmStateData stateData, RegularsNoPatrol regularsNoPatrolEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsNoPatrolEnemy = regularsNoPatrolEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        regularsNoPatrolEnemy.SetVelocity(0f);
        PC = GameObject.FindObjectOfType<PCController>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (!regularsNoPatrolEnemy.enemyFOV.sawKill)
        {
            regularsNoPatrolEnemy.shootSound.Play();
            PC.Die();
        }
        else
        {
            regularsNoPatrolEnemy.sawKillSound.Play();
            gameOver = true;
        }

       
    }

    public override void ExitState()
    {
        base.ExitState();
        gameOver = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (gameManager != null && (Time.time >= startTime + regularsNoPatrolEnemy.sawKillSound.clip.length) && gameOver)
        {
            gameManager.GameOver();
        }

    }

}
