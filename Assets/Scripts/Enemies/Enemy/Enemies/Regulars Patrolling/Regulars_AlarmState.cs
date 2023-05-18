using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_AlarmState : EnemyAlarmState
{
    private Regulars regularsEnemy;
    GameManager gameManager;
    PCController PC;
    bool gameOver;

    public Regulars_AlarmState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyAlarmStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        regularsEnemy.SetVelocity(0f);
        PC = GameObject.FindObjectOfType<PCController>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        regularsEnemy.MuzzleFlashActive();

        if (!regularsEnemy.enemyFOV.sawKill)
        {
            regularsEnemy.shootSound.Play();
            
            PC.Die();
        }
        else
        {
            regularsEnemy.sawKillSound.Play();
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

        if (gameManager != null && (Time.time >= startTime + regularsEnemy.sawKillSound.clip.length) && gameOver)
        {
            gameManager.GameOver();
        }


    }
}
