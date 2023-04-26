using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulars : BaseEnemy
{
    public Regulars_IdleState idleState;
    public Regulars_PatrolState patrolState;

    [SerializeField]
    private EnemyIdleStateData enemyIdleStateData;

    [SerializeField]
    private EnemyPatrolStateData enemyPatrolStateData;

    public override void Start()
    {
        base.Start();

        idleState = new Regulars_IdleState(enemyFSM, this, "idle", enemyIdleStateData, this);
        patrolState = new Regulars_PatrolState(enemyFSM, this, "move", enemyPatrolStateData, this);

        enemyFSM.Initialize(patrolState);
    }
}
