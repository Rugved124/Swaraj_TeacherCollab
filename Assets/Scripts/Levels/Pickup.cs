using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PCCollisionManager PCCollisionManager;
    private AudioSource arrowPickup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory.Instance.AddArrow(); 
        //arrowPickup.Play();
        Destroy(gameObject);

    }
}
