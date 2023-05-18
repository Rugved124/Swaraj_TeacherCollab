using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public FiniteStateMachine enemyFSM;

    public CharacterData characterData;

    public GameObject bloodVFX;

    public int facingDirection = 1;

    protected Rigidbody2D enemyRb;

    public Animator enemyAnim;

    protected SpriteRenderer enemySpriteRenderer;

    [HideInInspector]
    public EnemyFOV enemyFOV;

    Collider2D enemyCollider;

    private Vector2 updatedVelocity;

    [Header("Switch this on to make it waypoint based")]
    public bool isWayPointBased;

    public EnemyWaypointsData[] localWaypoints;
    Vector3[] globalWaypoints;

    int currentWaypoint = 0;
    int currentDirection = 1;

    [HideInInspector]
    public bool hasReachedNext;

    [HideInInspector]
    public bool hasFinishedLooking;

   int patrolCount;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;

    //private AudioSource enemyShooting;

    Quaternion originalFOVRotation;

    [HideInInspector]
    public AlertMeter alertMeter;
    float maxAlertLevel;
    float currentAlertLevel;
    float initialAlertLevel;
    bool initialAlert;

    [SerializeField]
    float increaseDuration = 1f;

    public float initialAlertFillAmount = 0.2f;
    float alertLevelIncreaseTime;

    bool isLookingDown = false;

    [HideInInspector]
    public bool resetVision;

    [HideInInspector]
    public bool isSeeingPlayer;

    [HideInInspector]
    public bool isAlarmed;

    public bool isCorpseAlarmed;

    public GameObject deadEnemyGO;

    public float dyingTime;
    public bool isDying;
    int dyingLayer;

    public AudioSource shootSound;
    public AudioSource sawKillSound;

    public virtual void Start()
    {
        isDying = false;
        enemyFOV = transform.Find("EnemyVision").GetComponent<EnemyFOV>();
        alertMeter = transform.Find("AlertMeter").GetComponent<AlertMeter>();
        dyingLayer = LayerMask.NameToLayer("Dying");

        enemyRb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        //enemyShooting = GetComponent<AudioSource>();

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

        enemyFOV.VisionInit(characterData.visionAngle, characterData.visionDistance, characterData.raycastCount, facingDirection);

        initialAlertLevel = 0;
        currentAlertLevel = initialAlertLevel;

        if (facingDirection == -1)
        {
            Flip();
        }
       

    }

    

    public virtual void Update()
    {
        enemyFSM.currentState.UpdateState();

        isSeeingPlayer = alertMeter.Alerted();
        isAlarmed = alertMeter.Alarmed();

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
        else
        {
            if (Time.time > alertLevelIncreaseTime)
            {
                alertLevelIncreaseTime = Time.time + increaseDuration;
                float newX = currentAlertLevel - initialAlertFillAmount;
                currentAlertLevel = Mathf.Clamp01(newX); // Clamp the value between 0 and 1
                alertMeter.SetScaleX(currentAlertLevel);
            }
        }


    }

    public bool SeeingPlayer()
    {
        return isSeeingPlayer;
    }

    public void LookAtPlayer()
    {
        enemyFOV.LookAtPlayer();
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
        enemyFOV.GetComponent<EnemyFOV>().SetFacingDirection(facingDirection);
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
        if (currentWaypoint + currentDirection >= globalWaypoints.Length)
        {
            currentDirection *= -1;
            currentWaypoint = globalWaypoints.Length - 2;
        }
        else if (currentWaypoint + currentDirection < 0)
        {
            currentDirection *= -1;
            currentWaypoint += currentDirection;
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
        
        Quaternion targetRotation = enemyFOV.transform.rotation * Quaternion.Euler(0f, 0f, -rotateAngle);
        StartCoroutine(LookDown(targetRotation, rotateTime));
    }

    IEnumerator LookDown(Quaternion targetRotation, float rotateTime)
    {
        isLookingDown = true;
        originalFOVRotation = enemyFOV.transform.rotation;
        float t = 0f;
        while (t < rotateTime)
        {
            t += Time.deltaTime;
            float fraction = Mathf.Clamp01(t / rotateTime);
            enemyFOV.transform.rotation = Quaternion.Lerp(originalFOVRotation, targetRotation, fraction);
            if (enemyFOV.sawPlayer)
            {
                isLookingDown = false;
                yield break; // exit the coroutine if player is seen
            }
            yield return null;
        }

        // If player is not seen, start looking up
        StartCoroutine(LookUp(rotateTime));
    }

    // Coroutine to look up
    IEnumerator LookUp(float rotateTime)
    {
        Quaternion targetRotation = originalFOVRotation; // rotate back to original rotation
        float t = 0f;
        while (t < rotateTime)
        {
            t += Time.deltaTime;
            float fraction = Mathf.Clamp01(t / rotateTime);
            enemyFOV.transform.rotation = Quaternion.Lerp(enemyFOV.transform.rotation, targetRotation, fraction);
            if (enemyFOV.sawPlayer)
            {
                yield break; // exit the coroutine if player is seen
            }
            yield return null;
        }

        isLookingDown = false;

        hasFinishedLooking = true;
    }

    public void ResetVision(Quaternion currentAngle, float rotateTime)
    {
        enemyFOV.transform.rotation = transform.rotation;
        //if (facingDirection == 1)
        //{
        //    enemyFOV.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        //}
        //else if (facingDirection == -1)
        //{
        //    enemyFOV.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
        //}
        
        resetVision = true;
    }

    public void SetDyingTime()
    {
       dyingTime = enemyAnim.GetCurrentAnimatorStateInfo(0).length;
    }

    public IEnumerator SpawnCorpse()
    {
        
        enemyRb.isKinematic = true;
        enemyRb.gravityScale = 0f;

        yield return new WaitForSeconds(1f);

        isDying = false;
        Instantiate(deadEnemyGO, transform.position - (new Vector3(0.75f,0f,0f)*facingDirection), transform.rotation);
        Destroy(gameObject);
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

    public virtual void OnHitByArrow(Arrow arrow)
    {
        //Rugved
        
        isDying = true;
        enemyFOV.SwitchOffLines();
        enemyFOV.enabled = false;
        alertMeter.enabled = false;
        bloodVFX.SetActive(true);
        bloodVFX.transform.parent = null;
    }

    public virtual void OnHitByHazard()
    {
        
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
