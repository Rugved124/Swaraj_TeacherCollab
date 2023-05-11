using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : CharacterStates
{
    protected EnemyDeadStateData stateData;

    protected bool isSearchingTimeOver;
    protected float searchingTime = 2f;

    protected bool shoot;

    public EnemyDeadState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyDeadStateData stateData) : base(stateMachine, baseEnemy, animBoolName)
    {
        this.stateData = stateData;
       
    }

    public override void EnterState()
    {
        base.EnterState();
        

        baseEnemy.SetVelocity(0f);
        baseEnemy.SpawnCorpse();

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
