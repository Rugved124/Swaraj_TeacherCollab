using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : CharacterStates
{
    protected EnemyIdleStateData stateData;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;

    protected float idleTime;

    public EnemyIdleState(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName, EnemyIdleStateData stateData) : base(stateMachine, baseEnemy, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void EnterState()
    {
        base.EnterState();
        baseEnemy.SetVelocity(0f);
        isIdleTimeOver = false;
        SetIdleTime();
    }

    public override void ExitState()
    {
        base.ExitState();

        if (flipAfterIdle)
        {
            baseEnemy.Flip();
            flipAfterIdle = false;
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }

    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetIdleTime()
    {
        idleTime = stateData.minIdleTime;
    }
}
