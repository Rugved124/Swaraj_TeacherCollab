using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : InteractiveObject
{
	public float radius = 10.0f;
	public LayerMask layerMask;

	public GameObject explosionFX;

	public override void OnHitByArrow()
	{

		// Check for colliders within the specified circle
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

		// Iterate over all colliders within the circle
		foreach (Collider2D collider in colliders)
		{
			// Check if the collider is on the enemy layer
			if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
			{
				if (collider.TryGetComponent(out BaseEnemy enemy))
				{
					enemy.OnHitByHazard();
				}
			}
			
			if (collider.gameObject.layer == LayerMask.NameToLayer("PC"))
			{
				collider.transform.root.GetComponent<PCController>().Die();
			}
			
			if (collider.gameObject.layer == LayerMask.NameToLayer("Crate"))
			{
				Destroy(collider.gameObject);
			}
		}

		explosionFX.SetActive(true);
		explosionFX.transform.parent = null;
		Destroy(gameObject);
	}

}

