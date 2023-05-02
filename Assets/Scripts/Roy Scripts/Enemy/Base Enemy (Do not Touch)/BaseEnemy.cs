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

    public EnemyWaypointsData[] localWaypoints;
    public Vector3[] globalWaypoints;

    public int currentWaypoint = 0;
    public int currentDirection = 1;

    //[HideInInspector]
    public bool hasReachedNext;

    public bool hasFinishedLooking;

    public int patrolCount;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;

    Quaternion originalFOVRotation;


    public AlertMeter alertMeter;
    float maxAlertLevel;
    float currentAlertLevel;
    float initialAlertLevel;
    bool initialAlert;

    float increaseDuration = 1f;

    public float initialAlertFillAmount = 0.2f;
    float alertLevelIncreaseTime;

    public virtual void Start()
    {
        facingDirection = 1;
        enemySpriteRenderer = transform.Find("Visuals").GetComponent<SpriteRenderer>();
        enemyAnim = transform.Find("Visuals").GetComponent<Animator>();
        enemyFOV = transform.Find("EnemyVision").GetComponent<EnemyFOV>();
        alertMeter = transform.Find("AlertMeter").GetComponent<AlertMeter>();

        enemyRb = GetComponent<Rigidbody2D>();

        enemyFSM = new FiniteStateMachine();

        if (localWaypoints != null)
        {
            globalWaypoints = new Vector3[localWaypoints.Length];
            for (int i = 0; i < localWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i].localPosition + transform.position;
            }

            if (isWayPointBased)
            {
                transform.position = globalWaypoints[0];
            }
        }
        patrolCount = 0;
        hasReachedNext = false;

        enemyFOV.VisionInit(characterData.visionAngle, characterData.visionDistance, characterData.raycastCount);

        initialAlertLevel = 0;
        currentAlertLevel = initialAlertLevel;
    }

    public virtual void Update()
    {
        enemyFSM.currentState.UpdateState();

        if (enemyFOV.sawPlayer)
        {

            if (!initialAlert)
            {
                initialAlert = true;
                currentAlertLevel = initialAlertLevel + initialAlertFillAmount;
                alertMeter.SetScaleX(currentAlertLevel);
            }

            if (Time.time > alertLevelIncreaseTime)
            {
                alertLevelIncreaseTime = Time.time + increaseDuration;
                float newX = currentAlertLevel + initialAlertFillAmount;
                currentAlertLevel = Mathf.Clamp01(newX); // Clamp the value between 0 and 1
                alertMeter.SetScaleX(currentAlertLevel);
            }
        }


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

    public void LookAroundInIdle(float rotateAngle, float rotateTime)
    {
        originalFOVRotation = enemyFOV.transform.rotation;
        Quaternion targetRotation = enemyFOV.transform.rotation * Quaternion.Euler(0f, 0f, -rotateAngle);
        StartCoroutine(RotateVision(targetRotation, rotateTime));
    }

    IEnumerator RotateVision(Quaternion targetRotation, float rotateTime)
    {
        float t = 0f;
        while (t < rotateTime)
        {
            t += Time.deltaTime;
            float fraction = Mathf.Clamp01(t / rotateTime);
            enemyFOV.transform.rotation = Quaternion.Lerp(originalFOVRotation, targetRotation, fraction);
            yield return null;
        }

        yield return new WaitForSeconds(1f); // wait for 1 second

        t = 0f;
        while (t < rotateTime)
        {
            t += Time.deltaTime;
            float fraction = Mathf.Clamp01(t / rotateTime);
            enemyFOV.transform.rotation = Quaternion.Lerp(targetRotation, originalFOVRotation, fraction);
            yield return null;
        }

        yield return new WaitForSeconds(1f); // wait for 1 second

        transform.rotation = originalFOVRotation; // set rotation to original rotation
        hasFinishedLooking = true;
    }

    public float GetVisionRotation()
    {
        return localWaypoints[currentWaypoint].rotateAngle;
    }

    public float GetVisionTime()
    {
        return localWaypoints[currentWaypoint].rotateTime;
    }

    public void HasReachedNext(bool hasReached)
    {
        hasReachedNext = hasReached;
    }

    public void HasFinishedLooking(bool hasLooked)
    {
        hasFinishedLooking = hasLooked;
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
                        globalWaypointPos = localWaypoints[i].localPosition + transform.position;
                    }
                    

                    Gizmos.DrawLine(globalWaypointPos - Vector3.up * gizmoSize, globalWaypointPos + Vector3.up * gizmoSize);
                    Gizmos.DrawLine(globalWaypointPos - Vector3.left * gizmoSize, globalWaypointPos + Vector3.left * gizmoSize);
                }
            }
        }
       
    }
}
