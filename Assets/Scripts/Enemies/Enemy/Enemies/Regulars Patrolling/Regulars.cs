using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class Regulars : BaseEnemy
{
    public Regulars_IdleState idleState;
    public Regulars_PatrolState patrolState;
    public Regulars_LookingState lookState;
    public Regulars_SearchingState searchingState;
    public Regulars_DeadState deadState;
    public Regulars_AlarmState alarmState;

    [SerializeField]
    private EnemyIdleStateData enemyIdleStateData;

    [SerializeField]
    private EnemyPatrolStateData enemyPatrolStateData;

    [SerializeField]
    private EnemyLookingStateData enemyLookingStateData;

    [SerializeField]
    private EnemySearchingStateData enemySearchingStateData;

    [SerializeField]
    private EnemyDeadStateData enemyDeadStateData;

    [SerializeField]
    private EnemyAlarmStateData enemyAlarmStateData;

    public bool isDead { get; private set; }

    public override void Start()
    {
        base.Start();

        idleState = new Regulars_IdleState(enemyFSM, this, "idle", enemyIdleStateData, this);
        patrolState = new Regulars_PatrolState(enemyFSM, this, "move", enemyPatrolStateData, this);
        lookState = new Regulars_LookingState(enemyFSM, this, "idle", enemyLookingStateData, this);
        searchingState = new Regulars_SearchingState(enemyFSM, this, "aim", enemySearchingStateData, this);
        deadState = new Regulars_DeadState(enemyFSM, this, "dead", enemyDeadStateData, this);
        alarmState = new Regulars_AlarmState(enemyFSM, this, "shoot", enemyAlarmStateData, this);

        enemyFSM.Initialize(idleState);
    }

    public override void OnHitByArrow(Arrow arrow)
    {
        base.OnHitByArrow(arrow);
        gameObject.layer = 16;
        enemyFSM.ChangeState(deadState);

        //Destroy(this.gameObject);
    }

    public override void OnHitByHazard()
    {
        Destroy(this.gameObject);
    }
}
