using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates
{
    protected FiniteStateMachine stateMachine;
    protected BaseEnemy baseEnemy;

    protected float startTime;

    protected string animBoolName;

    public CharacterStates(FiniteStateMachine stateMachine, BaseEnemy baseEnemy, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.baseEnemy = baseEnemy;
        this.animBoolName = animBoolName;
    }

    public virtual void EnterState()
    {
        startTime = Time.time;
        baseEnemy.enemyAnim.SetBool(animBoolName, true);
    }

    public virtual void ExitState() 
    {
        baseEnemy.enemyAnim.SetBool(animBoolName, false);
    }

    public virtual void UpdateState()
    {
        
    }
}
