using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Enums;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private Transform[] localWayPoints;
    [SerializeField] 
    private float speed = 5f;

    [SerializeField] 
    private float maxWaitTime = 2f;
    [SerializeField] 
    private EWayPointType type = EWayPointType.GoBack;
    
    private bool reversed;
    private Vector3[] globalWayPoints;
    private int currentWaypointIndex = 0;
    
    [SerializeField] 
    private float fieldOfViewAngle;
    [SerializeField] 
    private float maxDistance;
    [SerializeField] 
    private float alertIncreaseSpeed = 1f;
    [SerializeField] 
    private float alertDecreaseSpeed = 2f;
    [SerializeField] 
    private float chaseSpeed = 7.5f;
    
    private PC pc;
    private Rigidbody2D rb;
    private float alertMeter;
    private float waitTime;
    private bool waiting;
    private bool alerted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        globalWayPoints = new Vector3[localWayPoints.Length];
        for (int i = 0; i < globalWayPoints.Length; i++)
        {
            globalWayPoints[i] = localWayPoints[i].position;
        }
    }

    void Update()
    {
        Patrol();
        VisionCone();
    }
    
void Patrol()
{
    Vector2 targetPosition;

    if (alerted)
    {
        targetPosition = pc.transform.position;
    }
    else
    {
        // Check if platform has reached current waypoint
        if (Vector2.Distance(transform.position, globalWayPoints[currentWaypointIndex]) < 0.1f)
        {
            switch (type)
            {
                case EWayPointType.ResetTo0:
                    // Set next waypoint as current waypoint
                    currentWaypointIndex = (currentWaypointIndex +1) % globalWayPoints.Length;
                    break;
                case EWayPointType.GoBack:
                    if (!reversed)
                    {
                        currentWaypointIndex++;
                        if (currentWaypointIndex == globalWayPoints.Length)
                        {
                            currentWaypointIndex--;
                            reversed = true;
                        }
                    }
                    else
                    {
                        currentWaypointIndex--;
                        if (currentWaypointIndex < 0)
                        {
                            currentWaypointIndex++;
                            reversed = false;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            waiting = true;
        }

        targetPosition = globalWayPoints[currentWaypointIndex];
    }

    Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
    Vector2 direction = (targetPosition - position2D).normalized;

    // calculate velocity needed to reach target
    Vector2 velocity = direction * speed;

    if (waiting)
    {
        waitTime += Time.deltaTime;
        if (waitTime >= maxWaitTime)
        {
            waiting = false;
            waitTime = 0f;
        }
        else
        {
            velocity = Vector2.zero;
        }
    }

    rb.velocity = velocity;
    // set rotation towards the target
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
}



void VisionCone()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxDistance);


    foreach (Collider2D collider in colliders)
    {
        Vector2 direction = collider.transform.position - transform.position;
        float angleToCollider = Vector2.Angle(transform.right, direction);

        if (collider.GetComponent<PC>())
        {
            pc = collider.GetComponent<PC>();
            if (angleToCollider < fieldOfViewAngle * 0.5f && Vector2.Dot(transform.right, direction) > 0)
            {
                alertMeter += Time.deltaTime * alertIncreaseSpeed;
                if (alertMeter >= 1.0f)
                {
                    alerted = true;
                    Vector2 pcDirection = (pc.transform.position - transform.position).normalized;
                    rb.velocity = pcDirection * chaseSpeed;
                }
            }
            else
            {
                alertMeter -= Time.deltaTime * alertDecreaseSpeed;
                if (alertMeter <= 0.0f)
                {
                    alerted = false;
                }
            }
        }
    }
    alertMeter = Mathf.Clamp(alertMeter, 0f, 1f);
}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Vector3 coneDirection = transform.right;
        Vector3 coneLeftDirection = Quaternion.Euler(0, 0, fieldOfViewAngle * 0.5f) * coneDirection;
        Vector3 coneRightDirection = Quaternion.Euler(0, 0, -fieldOfViewAngle * 0.5f) * coneDirection;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + coneLeftDirection * maxDistance);
        Gizmos.DrawLine(transform.position, transform.position + coneRightDirection * maxDistance);
    }
}
