using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Regulars : BaseEnemy
{
    public Regulars_IdleState idleState;
    public Regulars_PatrolState patrolState;
    public Regulars_LookingState lookState;
    public Regulars_SearchingState searchingState;

    [SerializeField]
    private EnemyIdleStateData enemyIdleStateData;

    [SerializeField]
    private EnemyPatrolStateData enemyPatrolStateData;

    [SerializeField]
    private EnemyLookingStateData enemyLookingStateData;

    [SerializeField]
    private EnemySearchingStateData enemySearchingStateData;

    public override void Start()
    {
        base.Start();

        idleState = new Regulars_IdleState(enemyFSM, this, "idle", enemyIdleStateData, this);
        patrolState = new Regulars_PatrolState(enemyFSM, this, "move", enemyPatrolStateData, this);
        lookState = new Regulars_LookingState(enemyFSM, this, "idle", enemyLookingStateData, this);
        searchingState = new Regulars_SearchingState(enemyFSM, this, "aim", enemySearchingStateData, this);

        enemyFSM.Initialize(idleState);
    }

    public override void OnHitByArrow(Arrow arrow)
    {
        base.OnHitByArrow(arrow);

        Destroy(this.gameObject);
    }

    public override void OnHitByHazard()
    {

        Destroy(this.gameObject);
    }
}
