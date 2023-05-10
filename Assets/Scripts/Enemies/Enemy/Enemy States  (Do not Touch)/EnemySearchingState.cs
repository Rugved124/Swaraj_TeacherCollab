using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchingState : CharacterStates
{
    protected EnemySearchingStateData stateData;

    protected bool isSearchingTimeOver;
    protected float searchingTime = 2f;

    protected bool shoot;

    public EnemySearchingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemySearchingStateData stateData) : base(stateMachine, baseEnemy, animBoolName)
    {
        this.stateData = stateData;
       
    }

    public override void EnterState()
    {
        base.EnterState();
        

        baseEnemy.SetVelocity(0f);

    }

    public override void ExitState()
    {
        base.ExitState();
        isSearchingTimeOver = false;
        shoot = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!baseEnemy.isSeeingPlayer)
        {
            isSearchingTimeOver = true;
        }

        if (baseEnemy.isAlarmed)
        {
            shoot = true;
        }
        
    }


}
