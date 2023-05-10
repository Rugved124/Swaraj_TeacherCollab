using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PCCollisionManager PCCollisionManager;
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Destroy the pickup object
            Destroy(gameObject);

          // PCCollisionManager = ;
        }
    }
}
