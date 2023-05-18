using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol : BaseEnemy
{
    public RegularsNoPatrol_IdleState idleState;    
    public RegularsNoPatrol_LookingState lookState;
    public RegularsNoPatrol_SearchingState searchingState;
    public RegularsNoPatrol_DeadState deadState;
    public RegularsNoPatrol_AlarmState alarmState;

    [SerializeField]
    private EnemyIdleStateData enemyIdleStateData;

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

        idleState = new RegularsNoPatrol_IdleState(enemyFSM, this, "idle", enemyIdleStateData, this);
        lookState = new RegularsNoPatrol_LookingState(enemyFSM, this, "idle", enemyLookingStateData, this);
        searchingState = new RegularsNoPatrol_SearchingState(enemyFSM, this, "aim", enemySearchingStateData, this);
        deadState = new RegularsNoPatrol_DeadState(enemyFSM, this, "dead", enemyDeadStateData, this);
        alarmState = new RegularsNoPatrol_AlarmState(enemyFSM, this, "shoot", enemyAlarmStateData, this);

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
