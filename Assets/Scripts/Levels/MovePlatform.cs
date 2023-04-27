using System;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
	public enum WayPointType
	{
		ResetTo0,
		GoBack
	}

	[SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 5.0f;

    [SerializeField] private WayPointType type = WayPointType.GoBack;
    
    [SerializeField] private int currentWaypointIndex = 0;

    private bool reversed;

    void Update()
    {
        // Check if platform has reached current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            switch (type)
            {
                case WayPointType.ResetTo0:
                    // Set next waypoint as current waypoint
                    currentWaypointIndex = (currentWaypointIndex +1) % waypoints.Length;
                    break;
                case WayPointType.GoBack:
                    if (!reversed)
                    {
                        currentWaypointIndex++;
                        if (currentWaypointIndex == waypoints.Length)
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
            
        }

        // Move platform towards current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
    }
}