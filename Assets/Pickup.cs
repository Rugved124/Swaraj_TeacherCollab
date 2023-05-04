using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy the pickup object
            Destroy(gameObject);

            // Output a debug message to the console
            Debug.Log("You picked up something!");
        }
    }
}
