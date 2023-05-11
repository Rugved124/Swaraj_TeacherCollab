using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PCCollisionManager PCCollisionManager;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory.Instance.AddArrow(); 

        Destroy(gameObject);

    }
}
