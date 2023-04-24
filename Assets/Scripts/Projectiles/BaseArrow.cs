using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseArrow : MonoBehaviour
{
    public Vector3 startMarker; // the start position
    public Vector3 endMarker; // the end position
    public float height = 2f; // the height of the parabola
    public float targetSpeed = 1.0f; // the target speed of the movement
    public float minDistance = 0.1f; // the minimum distance between the start and end position
    private float startTime; // the time when the movement started
    private float journeyLength; // the distance between the start and end position
    private float journeyTime; // the time required to complete the journey at the target speed
    private float fracJourney;
    void Start()
    {
        startMarker = transform.position;
        // calculate the distance between the start and end position
        journeyLength = Vector3.Distance(startMarker, endMarker);
        // check if the start and end positions are too close
        if (journeyLength < minDistance)
        {
            // set the end position to be a certain distance away from the start position
            endMarker = startMarker + Vector3.right * minDistance;
            // recalculate the distance between the start and end position
            journeyLength = Vector3.Distance(startMarker, endMarker);
        }
        // calculate the time required to complete the journey at the target speed
        journeyTime = journeyLength / targetSpeed;
        // record the time when the movement started
        startTime = Time.time;
    }

    void Update()
    {
        if (fracJourney < 1.0f)
        {
            // calculate the distance covered so far
            float distCovered = (Time.time - startTime) * targetSpeed;
            // calculate the fraction of the journey completed
            fracJourney = distCovered / journeyLength;
            // calculate the position of the object using a parabolic path
            transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney) + Vector3.up * (Mathf.Sin(fracJourney * Mathf.PI) * height);
            // adjust the speed of the movement based on the remaining time
            float remainingTime = journeyTime - (Time.time - startTime);
        }
    }
}
