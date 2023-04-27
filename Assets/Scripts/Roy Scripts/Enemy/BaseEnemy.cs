using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public FiniteStateMachine enemyFSM;

    public CharacterData characterData;

    public int facingDirection;

    protected Rigidbody2D enemyRb;

    public Animator enemyAnim;

    protected SpriteRenderer enemySpriteRenderer;

    protected EnemyFOV enemyFOV;

    private Vector2 updatedVelocity;

    [Header("Switch this on to make it waypoint based")]
    public bool isWayPointBased;

    [Header("Make sure there are waypoints if ")]
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    int currentWaypoint = 0;
    int currentDirection = 1;

    [HideInInspector]
    public bool hasReachedNext;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;



    public virtual void Start()
    {
        facingDirection = 1;
        enemySpriteRenderer = transform.Find("Visuals").GetComponent<SpriteRenderer>();
        enemyAnim = transform.Find("Visuals").GetComponent<Animator>();
        enemyFOV = transform.Find("EnemyVision").GetComponent<EnemyFOV>();

        enemyRb = GetComponent<Rigidbody2D>();

        enemyFSM = new FiniteStateMachine();
        hasReachedNext = false;

        if (localWaypoints != null)
        {
            globalWaypoints = new Vector3[localWaypoints.Length];
            for (int i = 0; i < localWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i] + transform.position;
            }

            if (isWayPointBased)
            {
                transform.position = globalWaypoints[0];
            }
        }

        enemyFOV.VisionInit(characterData.visionAngle, characterData.visionDistance, characterData.raycastCount);
    }

    public virtual void Update()
    {
        enemyFSM.currentState.UpdateState();
    }

    public virtual void SetVelocity(float velocity)
    {
        updatedVelocity.Set(facingDirection * velocity, enemyRb.velocity.y);
        enemyRb.velocity = updatedVelocity;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, transform.right, characterData.wallCheckDistance, characterData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, characterData.ledgeCheckDistance, characterData.whatIsGround);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    public virtual void SetVelocityWaypoint(float speed)
    {
        if (!hasReachedNext)
        {
            if (currentWaypoint < globalWaypoints.Length)
            {
                Vector2 targetPosition = new Vector2(globalWaypoints[currentWaypoint].x, transform.position.y);
                Vector2 direction = targetPosition - enemyRb.position;
                enemyRb.velocity = direction.normalized * speed;

                if (Mathf.Abs(targetPosition.x - enemyRb.position.x) < 0.1f)
                {
                    hasReachedNext = true;
                }


            }
            else
            {
                currentDirection = -1;
                currentWaypoint = globalWaypoints.Length - 2;
            }


        }
      
    }

    public void ModifyWaypoint()
    {
        if (currentWaypoint + currentDirection >= globalWaypoints.Length || currentWaypoint + currentDirection < 0)
        {
            currentDirection *= -1;
            currentWaypoint = globalWaypoints.Length - 2;
        }
        else
        {
            currentWaypoint += currentDirection;
        }
    }

    public bool HasReachedEndWayPoint()
    {
        return(currentWaypoint == globalWaypoints.Length - 1 && currentDirection == 1 || currentWaypoint == 0 && currentDirection == -1);
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (!isWayPointBased)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * characterData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * characterData.ledgeCheckDistance));
        }
        else if (isWayPointBased)
        {
            if (localWaypoints != null)
            {
                float gizmoSize = 0.25f;
                Vector3 globalWaypointPos;

                for (int i = 0; i < localWaypoints.Length; i++)
                {
                    if (Application.isPlaying)
                    {
                        globalWaypointPos = globalWaypoints[i];
                    }
                    else
                    {
                        globalWaypointPos = localWaypoints[i] + transform.position;
                    }
                    

                    Gizmos.DrawLine(globalWaypointPos - Vector3.up * gizmoSize, globalWaypointPos + Vector3.up * gizmoSize);
                    Gizmos.DrawLine(globalWaypointPos - Vector3.left * gizmoSize, globalWaypointPos + Vector3.left * gizmoSize);
                }
            }
        }
       
    }
}
