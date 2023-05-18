using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlarmState : CharacterStates
{
    protected EnemyAlarmStateData stateData;


    public EnemyAlarmState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyAlarmStateData stateData) : base(stateMachine, baseEnemy, animBoolName)
    {
        this.stateData = stateData;

    }

    public override void EnterState()
    {
        base.EnterState();



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
