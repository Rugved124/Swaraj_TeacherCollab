using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars_DeadState : EnemyDeadState
{
    private Regulars regularsEnemy;
    CharacterStates previousState;

    Quaternion currentVisionAngle;

    public Regulars_DeadState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyDeadStateData stateData, Regulars regularsEnemy) : base(stateMachine, baseEnemy, animBoolName, stateData)
    {
        this.regularsEnemy = regularsEnemy;
    }

    public override void EnterState()
    {
        base.EnterState();

        regularsEnemy.SetVelocity(0f);
        regularsEnemy.SetDyingTime();
        regularsEnemy.StartCoroutine(regularsEnemy.SpawnCorpse());
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
