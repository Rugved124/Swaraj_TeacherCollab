using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularsNoPatrol : BaseEnemy
{
    public RegularsNoPatrol_IdleState idleState;    
    public RegularsNoPatrol_LookingState lookState;
    public RegularsNoPatrol_SearchingState searchingState;

    [SerializeField]
    private EnemyIdleStateData enemyIdleStateData;

    [SerializeField]
    private EnemyLookingStateData enemyLookingStateData;

    [SerializeField]
    private EnemySearchingStateData enemySearchingStateData;

    public override void Start()
    {
        base.Start();

        idleState = new RegularsNoPatrol_IdleState(enemyFSM, this, "idle", enemyIdleStateData, this);
        lookState = new RegularsNoPatrol_LookingState(enemyFSM, this, "idle", enemyLookingStateData, this);
        searchingState = new RegularsNoPatrol_SearchingState(enemyFSM, this, "aim", enemySearchingStateData, this);

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
