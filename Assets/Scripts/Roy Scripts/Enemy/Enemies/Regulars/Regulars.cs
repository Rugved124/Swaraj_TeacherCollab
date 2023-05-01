using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars : BaseEnemy
{
    public Regulars_IdleState idleState;
    public Regulars_PatrolState patrolState;
    public Regulars_LookingState lookState;

    [SerializeField]
    private EnemyIdleStateData enemyIdleStateData;

    [SerializeField]
    private EnemyPatrolStateData enemyPatrolStateData;

    [SerializeField]
    private EnemyLookingStateData enemyLookingStateData;

    public override void Start()
    {
        base.Start();

        idleState = new Regulars_IdleState(enemyFSM, this, "idle", enemyIdleStateData, this);
        patrolState = new Regulars_PatrolState(enemyFSM, this, "move", enemyPatrolStateData, this);
        lookState = new Regulars_LookingState(enemyFSM, this, "idle", enemyLookingStateData, this);

        enemyFSM.Initialize(idleState);
    }
}
