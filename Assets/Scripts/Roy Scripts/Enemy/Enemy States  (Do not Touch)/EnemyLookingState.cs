using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookingState : CharacterStates
{
    protected EnemyLookingStateData stateData;

    protected bool isLookingTimeOver;
    protected float lookingAngle;
    protected float lookingTime;

    public EnemyLookingState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyLookingStateData stateData) : base(stateMachine, baseEnemy, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void EnterState()
    {
        base.EnterState();
        isLookingTimeOver = false;
        SetLookingTime();

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (Time.time >= startTime + lookingTime)
        {
            isLookingTimeOver = true;
        }
    }

    private void SetLookingTime()
    {

        lookingAngle = baseEnemy.GetVisionRotation();
        lookingTime = baseEnemy.GetVisionTime();

    }
}
